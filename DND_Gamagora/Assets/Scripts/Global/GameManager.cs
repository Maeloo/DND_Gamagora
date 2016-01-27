﻿using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    public bool Pause { get; private set; }

    private SceneAudioManager audio_manager;
    private AudioProcessor audio_process;

	// Use this for initialization
	void Awake ()
    {
        audio_manager = SceneAudioManager.Instance;
        audio_process = AudioProcessor.Instance;
        Pause = false;
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void SetPause(bool pause)
    {
        Pause = pause;
        if (pause)
            audio_process.PauseMusic();
        else
            audio_process.PlayMusic();
    }
}