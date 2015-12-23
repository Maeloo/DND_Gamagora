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
using System;
using System.Collections;

/*
 * Make your class implement the interface AudioProcessor.AudioCallbaks
 */
public class AudioManager : MonoBehaviour, AudioProcessor.AudioCallbacks
{
    public GameObject box1;
    public GameObject box2;
    public GameObject box3;
    public GameObject box4;
    public GameObject box5;
    public GameObject box6;
    public GameObject box7;
    public GameObject box8;
    public GameObject box9;

    private AudioProcessor processor;
    private EnemyManager enemiesSpawner;
    private Vector3 currentPos;

    void Awake()
    {
        processor = FindObjectOfType<AudioProcessor>();
        processor.addAudioCallback(this);

        enemiesSpawner = EnemyManager.Instance;
        currentPos = Camera.main.transform.position;
    }
    
    void Update()
    {
        
    }

    // This event will be called every time a beat is detected.
    // Change the threshold parameter in the inspector to adjust the sensitivity
    public void onBeatDetected()
    {
        //float r = UnityEngine.Random.Range(0f, 1f);
        //float g = UnityEngine.Random.Range(0f, 1f);
        //float b = UnityEngine.Random.Range(0f, 1f);
        //Color color = new Color(r, g, b);
        //processor.changeCameraColor(color);
    }

    // This event will be called every frame while music is playing
    public void onData(float[] spectrum, float[] energy, float[] average_energy, float[] variance)
    {
        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i * 0.015f + 4f, 0f, 0);
            Vector3 end = new Vector3(i * 0.015f + 4f, 20f * spectrum[i], 0);
            Debug.DrawLine(start, end, Color.yellow);
        }

        for (int i = 0; i < energy.Length; ++i)
        {
            Vector3 start = new Vector3(i * 0.065f - 10f, 0f, 0);
            Vector3 end = new Vector3(i * 0.065f - 10f, 0.1f * energy[i], 0);
            Debug.DrawLine(start, end, Color.red);
        }

        for (int i = 0; i < average_energy.Length; ++i)
        {
            Vector3 start = new Vector3(i * 0.065f - 5f, 0f, 0);
            Vector3 end = new Vector3(i * 0.065f - 5f, 0.1f * average_energy[i], 0);
            Debug.DrawLine(start, end, Color.red);
        }

        for (int i = 0; i < variance.Length; ++i)
        {
            Vector3 start = new Vector3(i * 0.065f, 0f, 0);
            Vector3 end = new Vector3(i * 0.065f, 0.0005f * variance[i], 0);
            Debug.DrawLine(start, end, Color.yellow);
        }
    }

    public void onBeatLow1(float energy, float average_energy, float radiance, int frequency_size)
    {
        box1.GetComponent<Renderer>().material.color = Color.red;
        StartCoroutine(StopColor(box1));
    }

    public void onBeatLow2(float energy, float average_energy, float radiance, int frequency_size)
    {
        box2.GetComponent<Renderer>().material.color = Color.yellow;
        StartCoroutine(StopColor(box2));
    }

    public void onBeatLow3(float energy, float average_energy, float radiance, int frequency_size)
    {
        box3.GetComponent<Renderer>().material.color = Color.blue;
        StartCoroutine(StopColor(box3));
    }

    public void onBeatLow4(float energy, float average_energy, float radiance, int frequency_size)
    {
        box4.GetComponent<Renderer>().material.color = Color.green;
        StartCoroutine(StopColor(box4));
    }

    public void onBeatMedium1(float energy, float average_energy, float radiance, int frequency_size)
    {
        box5.GetComponent<Renderer>().material.color = Color.black;
        StartCoroutine(StopColor(box5));
    }

    public void onBeatMedium2(float energy, float average_energy, float radiance, int frequency_size)
    {
        box6.GetComponent<Renderer>().material.color = Color.cyan;
        StartCoroutine(StopColor(box6));
    }

    public void onBeatMedium3(float energy, float average_energy, float radiance, int frequency_size)
    {
        box7.GetComponent<Renderer>().material.color = Color.magenta;
        StartCoroutine(StopColor(box7));
    }

    public void onBeatHigh1(float energy, float average_energy, float radiance, int frequency_size)
    {
        box8.GetComponent<Renderer>().material.color = (Color.red + Color.green) * 0.5f;
        StartCoroutine(StopColor(box8));
    }

    public void onBeatHigh2(float energy, float average_energy, float radiance, int frequency_size)
    {
        box9.GetComponent<Renderer>().material.color = Color.white;
        StartCoroutine(StopColor(box9));
    }

    public IEnumerator StopColor(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        obj.GetComponent<Renderer>().material.color = Color.grey;
    }
}
