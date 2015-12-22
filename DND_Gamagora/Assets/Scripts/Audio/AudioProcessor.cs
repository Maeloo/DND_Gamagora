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

public class AudioProcessor : MonoBehaviour
{
    private List<AudioCallbacks> callbacks;

    public int qSamples = 1024;  // array size
    public float refValue = 0.1f; // RMS value for 0 dB
    public float threshold = 0.02f;      // minimum amplitude to extract pitch
    float rmsValue;   // sound level - RMS
    float dbValue;    // sound level - dB
    float pitchValue; // sound pitch - Hz
 
    private float[] samples; // audio samples
    private float[] spectrum; // audio spectrum
    private int fSample;
    private float beatTime;

    private AudioSource audioSrc;

    // Use this for initialization
    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        samples = new float[qSamples];
        spectrum = new float[qSamples];
        fSample = AudioSettings.outputSampleRate;
        beatTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {   
        if (audioSrc.isPlaying)
        {
            beatTime += Time.deltaTime;
            AnalyzeSound();

            if (beatTime >= 1f / pitchValue)
            {
                if (callbacks != null)
                {
                    foreach (AudioCallbacks callback in callbacks)
                    {
                        callback.onOnbeatDetected();
                    }
                }
                beatTime = 0f;
            }
            
            if (callbacks != null)
            {
                foreach (AudioCallbacks callback in callbacks)
                {
                    callback.onData(spectrum, samples);
                }
            }

            Debug.Log("RMS: " + rmsValue.ToString("F2") +
         " (" + dbValue.ToString("F1") + " dB)\n" +
         "Pitch: " + pitchValue.ToString("F0") + " Hz");
        }
    }

    public void changeCameraColor()
    {
        //Debug.Log("camera");
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);

        Camera.main.clearFlags = CameraClearFlags.Color;
        Camera.main.backgroundColor = color;

        //camera.backgroundColor = color;
    }

    void AnalyzeSound()
    {
        audioSrc.GetOutputData(samples, 0); // Fill array with samples
        int i;
        float sum = 0f;

        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // Sum squared samples
        }

        rmsValue = Mathf.Sqrt(sum / qSamples); // RMS = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // Calculate dB
        if (dbValue < -160) dbValue = -160; // Clamp it to -160dB min
        
        // Get sound spectrum
        audioSrc.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
  
        float maxV = 0f;
        int maxN = 0;

        for (i = 0; i < qSamples; i++) // Find max 
        { 
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }

        float freqN = maxN; // Pass the index to a float variable
        if (maxN > 0 && maxN < qSamples - 1) // Interpolate index using neighbours
        { 
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (fSample / 2) / qSamples; // Convert index to frequency
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
        void onOnbeatDetected();
        void onData(float[] spectrum, float[] data);
    }
}


