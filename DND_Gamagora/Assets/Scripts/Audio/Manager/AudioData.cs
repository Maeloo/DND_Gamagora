using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using Game;

public class AudioData : MonoBehaviour {

    [SerializeField]
    AudioClip[] AudioClips;

    public AudioMixerGroup defaultMixerGroup;

    public Audio_Type type;
    
    public bool playRandom;

    private int _lastPlayed;


    void Awake ( ) {
        _lastPlayed = 0;

        SceneAudioManager.Instance.registerAudioData ( type, this );
    }


    public AudioClip getClip ( ) {
        AudioClip clip;

        if ( playRandom ) {
            clip = AudioClips[( int ) Mathf.Floor ( Random.value * AudioClips.Length )];
        } else {
            int idx = _lastPlayed = _lastPlayed == AudioClips.Length - 1 ? 0 : _lastPlayed + 1; 
            clip = AudioClips[idx];
        }

        return clip;
    }

}
