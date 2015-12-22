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

/*
 * Make your class implement the interface AudioProcessor.AudioCallbaks
 */
public class AudioManager : MonoBehaviour, AudioProcessor.AudioCallbacks
{
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
    public void onOnbeatDetected()
    {
        processor.changeCameraColor();
    }

    // This event will be called every frame while music is playing
    public void onData(float[] spectrum, float[] data)
    {
        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i * 0.1f, -1f, 0);
            Vector3 end = new Vector3(i * 0.1f, -1f + 50f * spectrum[i], 0);
            Debug.DrawLine(start, end, Color.yellow);

            start = new Vector3(i * 0.01f, 1f, 0);
            end = new Vector3(i * 0.01f, 1f + 10f * data[i], 0);
            Debug.DrawLine(start, end, Color.red);
        }
    }
}
