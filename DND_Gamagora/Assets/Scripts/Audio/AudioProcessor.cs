/*
 * Copyright (c) 2015 Allan Pichardo
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AudioProcessor : Singleton<AudioProcessor>
{
    protected AudioProcessor() { }

    public float C; // Constant for average sensibility
    public float VarianceMin; // Variance minimum to detect a beat
    public float AmplitudeMultiplier; // Multiplier for FFT amplitude computing
    public float RefValue; // RMS value for 0 dB
    public float Threshold; // Minimum amplitude to extract pitch
    public int Samples;  // Spectrum array size
    public int FftSamples; // Buffer subbands
    // Constants based on Samples and FftSamples for w computing
    // w1 = A + B (~= 2)
    // FftSamples * B + A * FftSamples * (FftSamples - 1) * 0.5 = Samples
    public float A; // 0.44307692307f
    public float B; // 1.6f

    public float RmsValue { get; private set; } // Sound level - RMS
    public float DbValue { get; private set; } // Sound level - dB
    public float PitchValue { get; private set; } // Sound pitch - Hz
    public float InstantEnergy { get; private set; } // Instant sound energy
    [Obsolete]
    public float Variance { get; private set; } // Variance of energies
    
    private const int EnergiesLength = 43; // Energies array size

    
    private float[] samples; // Audio samples
    private float[] spectrum; // Audio spectrum
    private float[] energies_s; // Current energies for all subbands
    private List<float[]> energies; // Energy subband history
    [Obsolete]
    private float[] energies_a; // Local energies history
    private int fSample; // Sampling rate (22.05, 44.1 kHz...)

    private List<AudioCallbacks> callbacks;
    private AudioSource audioSrc;

    private bool rewind;
    // Music time at last checkpoint
    private float rewind_to;
    private AudioClip rewind_sound;
    private int key_sound_rewind;

    // Use this for initialization
    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        samples = new float[Samples];
        spectrum = new float[Samples];
        fSample = AudioSettings.outputSampleRate;

        // === Old algorithm ===
        //energies_a = new float[EnergiesLength];
        //for (int i = 0; i < EnergiesLength; i++)
        //    energies_a[i] = 0f;

        energies_s = new float[FftSamples];
        for (int i = 0; i < FftSamples; i++)
            energies_s[i] = 0f;

        energies = new List<float[]>(FftSamples);
        for (int i = 0; i < FftSamples; i++)
        {
            energies.Add(new float[EnergiesLength]);
            for (int j = 0; j < EnergiesLength; j++)
                energies[i][j] = 0f;
        }

        rewind = false;

    }

    // Update is called once per frame
    void Update()
    {   
        if (audioSrc.isPlaying && !rewind && !GameManager.Instance.Pause)
        {
            AnalyzeSound();
         //   Debug.Log("RMS: " + rmsValue.ToString("F2") +
         //" (" + dbValue.ToString("F1") + " dB)\n" +
         //"Pitch: " + pitchValue.ToString("F0") + " Hz");
        }
    }

    // Debug
    public void changeCameraColor(Color c)
    {
        Camera.main.clearFlags = CameraClearFlags.Color;
        Camera.main.backgroundColor = c;
    }

    public void RewindSound(float music_position)
    {
        if(!rewind)
        {
            PauseMusic();
            Hashtable param = new Hashtable();
            param.Add("starttime", 1f);
            key_sound_rewind = SceneAudioManager.Instance.playAudio(Game.Audio_Type.Rewind, param);
           
            rewind_to = music_position;
            rewind = true;
        }
    }

    // Play music after rewind
    public void PlayMusic()
    {
        if (rewind)
        {
            audioSrc.time = rewind_to;
            SceneAudioManager.Instance.stop(key_sound_rewind);
            audioSrc.Play();
            rewind = false;
        }
        else
            audioSrc.Play();
    }

    public void PauseMusic()
    {
        //audioSrc.Pause();
    }

    // Current music time in seconds
    public float GetMusicCurrentTime()
    {
        if(audioSrc != null)
            return audioSrc.time;

        return 0f;
    }

    // Music length in seconds
    public float GetMusicLength()
    {
        return audioSrc.clip.length;
    }

    void AnalyzeSound()
    {
        audioSrc.GetOutputData(samples, 0); // Fill array with samples
        int i;
        float sum = 0f;

        for (i = 0; i < Samples; i++)
        {
            sum += samples[i] * samples[i]; // Sum squared samples
        }

        InstantEnergy = sum;

        // === Old algorithm ===
        //float localEnergy = GetAverageLocalEnergy();
        //Variance = GetVariance(localEnergy);
        //C = (-0.0025714f * Variance) + 1.5142857f;
        //ShiftEnergies();
        //energies_a[0] = InstantEnergy;

        RmsValue = Mathf.Sqrt(InstantEnergy * (1f / Samples)); // RMS = square root of average
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // Calculate dB

        if (DbValue < -160)
            DbValue = -160; // Clamp it to -160dB min
        
        // Get sound spectrum
        audioSrc.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // Get amplitudes
        float[] fft = GetFftAmplitudes();
        // FFT sampling
        ComputeEnergySubband(fft);

        float[] average_energy = new float[FftSamples];
        float[] list_variance = new float[FftSamples];

        int wi = 0;
        int w = 1;
        int counter = 0;
        // For each samples
        for (i = 0; i < FftSamples; i++)
        {
            // Average energy for a frequency band (based on last 43 energies)
            float energy_i = GetAverageEnergyForBand(i);
            average_energy[i] = energy_i;

            // Shift energy history on the right
            ShiftEnergyForBand(i);
            // New energy in first position
            energies[i][0] = energies_s[i];

            // Current variance for a band
            float variance = GetVarianceForBand(i, energy_i);
            list_variance[i] = variance;

            if (w == 1)
            {
                wi = (int)GetW(i + 1);
                w = wi;
                counter++;
            }
            else
                w--;

            // Check for a beat
            if (energies_s[i] > C * energy_i && variance > VarianceMin)
            {
                if (callbacks != null)
                {
                    foreach (AudioCallbacks callback in callbacks)
                    {
                        callback.onBeatDetected();

                        if(counter == 1)
                            callback.onBeatLow1(energies_s[i], energy_i, variance, wi);
                        else if (counter == 2)
                            callback.onBeatLow2(energies_s[i], energy_i, variance, wi);
                        else if (counter == 3)
                            callback.onBeatLow3(energies_s[i], energy_i, variance, wi);
                        else if (counter == 4)
                            callback.onBeatLow4(energies_s[i], energy_i, variance, wi);
                        else if (counter == 5)
                            callback.onBeatMedium1(energies_s[i], energy_i, variance, wi);
                        else if (counter == 6)
                            callback.onBeatMedium2(energies_s[i], energy_i, variance, wi);
                        else if (counter == 7)
                            callback.onBeatMedium3(energies_s[i], energy_i, variance, wi);
                        else if (counter == 8)
                            callback.onBeatHigh1(energies_s[i], energy_i, variance, wi);
                        else if (counter == 9)
                            callback.onBeatHigh2(energies_s[i], energy_i, variance, wi);
                    }
                }
            }
        }

        float maxV = 0f;
        int maxN = 0;

        for (i = 0; i < Samples; i++) // Find max 
        { 
            if (spectrum[i] > maxV && spectrum[i] > Threshold)
            {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }

        float freqN = maxN; // Pass the index to a float variable

        if (maxN > 0 && maxN < Samples - 1) // Interpolate index using neighbours
        { 
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }

        PitchValue = freqN * (fSample * 0.5f) * (1f / Samples); // Convert index to frequency

        // Callback on datas
        if (callbacks != null)
        {
            foreach (AudioCallbacks callback in callbacks)
            {
                callback.onData(spectrum, energies_s, average_energy, list_variance);
            }
        }
    }

    [Obsolete("GetAverageLocalEnergy is deprecated, please use GetAverageEnergyForBand instead.")]
    public float GetAverageLocalEnergy()
    {
        float e = 0f;

        for (int i = 0; i < EnergiesLength; i++)
        {
            e += energies_a[i] * energies_a[i];
        }

        return e / EnergiesLength;
    }

    [Obsolete("GetVariance is deprecated, please use GetVarianceForBand instead.")]
    private float GetVariance(float local_energy)
    {
        float v = 0f;

        for (int i = 0; i < EnergiesLength; i++)
        {
            v += (energies_a[i] - local_energy) * (energies_a[i] - local_energy);
        }

        return v / EnergiesLength;
    }

    [Obsolete("ShiftEnergies is deprecated, please use ShiftEnergyForBand instead.")]
    private void ShiftEnergies()
    {
        for (int i = EnergiesLength - 1; i >= 1; i--)
            energies_a[i] = energies_a[i - 1];   
    }

    public float[] GetFftAmplitudes()
    {
        float[] fft = new float[Samples];

        for (int i = 0; i < Samples; i++)
            fft[i] = spectrum[i] * AmplitudeMultiplier * spectrum[i] * AmplitudeMultiplier;

        return fft;
    }

    private void ComputeEnergySubband(float[] amplitudes)
    {
        for (int i = 0; i < FftSamples; i++)
        {
            float sum = 0f;
            float wi = GetW(i);
            float wj = 0f;

            for (int j = 1; j < i - 1; j++)
            {
                wj += GetW(j);
            }

            for(float wk = wj; wk < (wj + wi); wk++)
            {
                sum += amplitudes[(int)wk];
            }
     
            energies_s[i] = sum * wi * (1f / Samples);
        }
    }

    private float GetAverageEnergyForBand(int i)
    {
        float e = 0f;

        for (int k = 0; k < EnergiesLength; k++)
            e += energies[i][k];

        return e * (1f / EnergiesLength);
    }

    private float GetW(int i)
    {
        return A * i + B;
    }

    private float GetFreq(int i)
    {
        if (i < Samples * 0.5)
            return (i * fSample) * (1f / Samples);
        return ((Samples - i) * fSample) * (1f / Samples);
    }

    private float GetVarianceForBand(int i, float e)
    {
        float v = 0f;

        for (int k = 0; k < EnergiesLength; k++)
            v += (energies[i][k] - e) * (energies[i][k] - e);

        return v / EnergiesLength;
    }

    private void ShiftEnergyForBand(int i)
    {
        for (int k = EnergiesLength - 1; k >= 1; k--)
            energies[i][k] = energies[i][k - 1];
    }

    public void addAudioCallback(AudioCallbacks callback)
    {
        if (callbacks == null)
            callbacks = new List<AudioCallbacks>();

        callbacks.Add(callback);
    }

    public void removeAudioCallback(AudioCallbacks callback)
    {
        callbacks.Remove(callback);
    }

    public interface AudioCallbacks
    {
        void onBeatLow1(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatLow2(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatLow3(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatLow4(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatMedium1(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatMedium2(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatMedium3(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatHigh1(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatHigh2(float energy, float average_energy, float radiance, int frequency_size);
        void onBeatDetected();
        void onData(float[] spectrum, float[] energy, float[] average_energy, float[] variance);
    }
}


