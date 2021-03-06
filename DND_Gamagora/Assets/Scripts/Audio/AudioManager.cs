﻿/*
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
using Game;
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

    private Transform player;

    private AudioProcessor processor;
    private EnemyManager enemiesSpawner;
    private TerrainManager platformSpawn;

    void Awake()
    {
        processor = FindObjectOfType<AudioProcessor>();
        processor.addAudioCallback(this);

        enemiesSpawner = EnemyManager.Instance;
        platformSpawn = TerrainManager.Instance;

        player = LoadCharacter.Instance.GetCharacter().transform;

        _lastShoot = Time.time;
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
        //for (int i = 0; i < spectrum.Length; ++i)
        //{
        //    Vector3 start = new Vector3(i * 0.015f + 4f, 0f, 0);
        //    Vector3 end = new Vector3(i * 0.015f + 4f, 20f * spectrum[i], 0);
        //    Debug.DrawLine(start, end, Color.yellow);
        //}

        //for (int i = 0; i < energy.Length; ++i)
        //{
        //    Vector3 start = new Vector3(i * 0.065f - 10f, 0f, 0);
        //    Vector3 end = new Vector3(i * 0.065f - 10f, 0.1f * energy[i], 0);
        //    Debug.DrawLine(start, end, Color.red);
        //}

        //for (int i = 0; i < average_energy.Length; ++i)
        //{
        //    Vector3 start = new Vector3(i * 0.065f - 5f, 0f, 0);
        //    Vector3 end = new Vector3(i * 0.065f - 5f, 0.1f * average_energy[i], 0);
        //    Debug.DrawLine(start, end, Color.red);
        //}

        //for (int i = 0; i < variance.Length; ++i)
        //{
        //    Vector3 start = new Vector3(i * 0.065f, 0f, 0);
        //    Vector3 end = new Vector3(i * 0.065f, 0.0005f * variance[i], 0);
        //    Debug.DrawLine(start, end, Color.yellow);
        //}
    }

    protected int minSkipBeatLow1 = 20; // Reglage Higklight Tribe
    protected int countTimeBeatLow1 = 0;
    public void onBeatLow1(float energy, float average_energy, float radiance, int frequency_size)
    {
        //box1.GetComponent<Renderer>().material.color = Color.red;
        //StartCoroutine(StopColor(box1));

        countTimeBeatLow1++;
        if (countTimeBeatLow1 >= minSkipBeatLow1)
        {
            if (TerrainManager.Instance.makeCurrentClassicPlatformFall())
                countTimeBeatLow1 = 0;
        }

        BonusManager.Instance.SpawnBonus(Type_Bonus.Special);
    }

    protected float _lastShoot;
    public void onBeatLow2(float energy, float average_energy, float radiance, int frequency_size)
    {
        //box2.GetComponent<Renderer>().material.color = Color.yellow;
        //StartCoroutine(StopColor(box2));

        ShootFireball(energy);
    }

    private void ShootFireball(float energy)
    {
        bool shot = false;
        for (int i = 0; i < enemiesSpawner.fireballs.Count; i++)
        {
            if (enemiesSpawner.fireballs[i] == null)
            {
                continue;
            }

            if (Time.time - _lastShoot > 1.0f)
            {
                enemiesSpawner.fireballs[i].shoot();
                shot = true;
            }

            iTween.Stop(enemiesSpawner.fireballs[i].gameObject);

            float scale = Mathf.Min(2.5f + energy * .004f, 6f);

            iTween.ScaleTo(enemiesSpawner.fireballs[i].gameObject, iTween.Hash(
                "time", .1f,
                "scale", new Vector3(scale, scale, scale),
                "easetype", iTween.EaseType.easeInOutExpo));

            iTween.ScaleTo(enemiesSpawner.fireballs[i].gameObject, iTween.Hash(
                "delay", .1f,
                "time", .2f,
                "scale", new Vector3(2.0f, 2.0f, 2.0f),
                "easetype", iTween.EaseType.easeInOutExpo));
        }

        if (shot)
            _lastShoot = Time.time;
    }

    protected int minSkipBeatLow3 = 20; // Reglage Higklight Tribe
    protected int countTimeBeatLow3 = 0;
    public void onBeatLow3(float energy, float average_energy, float radiance, int frequency_size)
    {
        //box3.GetComponent<Renderer>().material.color = Color.blue;
        //StartCoroutine(StopColor(box3));

        countTimeBeatLow3++;
        if (countTimeBeatLow3 >= minSkipBeatLow3)
        {
            for (int i = 0; i < enemiesSpawner.Shooters.Count; i++)
            {
                if (enemiesSpawner.Shooters[i] == null)
                {
                    continue;
                }

                if (enemiesSpawner.Shooters[i].transform.position.x > player.position.x)
                {
                    enemiesSpawner.Shooters[i].shoot();
                }
            }

            countTimeBeatLow3 = 0;
        }
         
    }

    protected int minSkipBeatLow4 = 60; // Reglage Higklight Tribe
    protected int countTimeBeatLow4 = 0;
    public void onBeatLow4(float energy, float average_energy, float radiance, int frequency_size)
    {
        //box4.GetComponent<Renderer>().material.color = Color.green;
        //StartCoroutine(StopColor(box4));

        countTimeBeatLow4++;
        if (countTimeBeatLow4 >= minSkipBeatLow4)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand <= 0.5)
            {
                platformSpawn.SpawnPlatformHight(Type_Platform.Hight);
            }
            else
            {
                platformSpawn.SpawnPlatformTnt(enemiesSpawner,player);
            }
            countTimeBeatLow4 = 0;
        }
    }

    public void onBeatMedium1(float energy, float average_energy, float radiance, int frequency_size)
    {
        //box5.GetComponent<Renderer>().material.color = Color.black;
        //StartCoroutine(StopColor(box5));

        BonusManager.Instance.SpawnBonus(Type_Bonus.Note);
    }

    public void onBeatMedium2(float energy, float average_energy, float radiance, int frequency_size)
    {
        //box6.GetComponent<Renderer>().material.color = Color.cyan;
        //StartCoroutine(StopColor(box6));

        BonusManager.Instance.SpawnBonus(Type_Bonus.Heart);
    }

    protected int minSkipBeatMedium3 = 50;
    protected int countTimeBeatMedium3 = 0;
    public void onBeatMedium3(float energy, float average_energy, float radiance, int frequency_size)
    {
        float rand = UnityEngine.Random.Range(0f, 1f);

        if (rand <= 0.3f)
            BonusManager.Instance.SpawnBonus(Type_Bonus.Invincibility);
        else if (rand <= 0.6f)
            BonusManager.Instance.SpawnBonus(Type_Bonus.Power);

        countTimeBeatMedium3++;
        if (countTimeBeatMedium3 >= minSkipBeatMedium3)
        {
            float rand2 = UnityEngine.Random.Range(0f, 1f);
            if (rand2 <= 0.1f)
                TerrainManager.Instance.makeCurrentClassicPlatformFall();
            else if (rand2 <= 0.3f)
                ShootFireball(energy);

            countTimeBeatMedium3 = 0;
        }
    }

    protected int minSkipBeatHigh1 = 50;
    protected int countTimeBeatHigh1 = 0;
    public void onBeatHigh1(float energy, float average_energy, float radiance, int frequency_size)
    {
        float rand = UnityEngine.Random.Range(0f, 1f);

        if (rand <= 0.25f)
            BonusManager.Instance.SpawnBonus(Type_Bonus.Special);
        else if (rand <= 0.5f)
            BonusManager.Instance.SpawnBonus(Type_Bonus.Invincibility);
        else if (rand <= 0.75f)
            BonusManager.Instance.SpawnBonus(Type_Bonus.Power);

        countTimeBeatHigh1++;
        if (countTimeBeatHigh1 >= minSkipBeatHigh1)
        {
            float rand2 = UnityEngine.Random.Range(0f, 1f);

            if (rand2 <= 0.5f)
            {
                for (int i = 0; i < enemiesSpawner.Shooters.Count; i++)
                {
                    if (enemiesSpawner.Shooters[i] == null)
                    {
                        continue;
                    }

                    if (enemiesSpawner.Shooters[i].transform.position.x > player.position.x)
                    {
                        enemiesSpawner.Shooters[i].shoot();
                    }
                }
            }

            countTimeBeatHigh1 = 0;
        }
    }

    protected int minSkipBeatHigh2 = 50;
    protected int countTimeBeatHigh2 = 0;
    public void onBeatHigh2(float energy, float average_energy, float radiance, int frequency_size)
    {

        countTimeBeatHigh2++;
        if (countTimeBeatHigh2 >= minSkipBeatHigh2)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand <= 0.4)
            {
                platformSpawn.SpawnPlatformHight(Type_Platform.Hight);
            }
            else if (rand <= 0.8)
            {
                platformSpawn.SpawnPlatformTnt(enemiesSpawner, player);
            }
            else
            {
                BonusManager.Instance.SpawnBonus(Type_Bonus.Invincibility);
            }
            countTimeBeatHigh2 = 0;
        }
    }

    public IEnumerator StopColor(GameObject obj)
    {
        yield return new WaitForSeconds(0.05f);
        obj.GetComponent<Renderer>().material.color = Color.grey;
    }
}
