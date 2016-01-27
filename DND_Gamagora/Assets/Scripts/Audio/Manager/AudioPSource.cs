using UnityEngine;
using System.Collections;

public class AudioPSource : MonoBehaviour, Poolable<AudioPSource> {

    static int num = 0;

    static AudioPSource SCreate ( ) {
        GameObject obj = new GameObject ( );
        obj.name = "audio_source_" + num.ToString ( "000" );
        obj.transform.SetParent ( SceneAudioManager.Instance.transform );
        
        AudioPSource self = obj.AddComponent<AudioPSource> ( );
        self.audio_source = obj.AddComponent<AudioSource> ( );

        ++num;

        return self;
    }


    protected Pool<AudioPSource> _pool;

    public AudioSource audio_source;


    public AudioPSource Create ( ) {
        return AudioPSource.SCreate ( );
    }
    

    public void Register ( UnityEngine.Object pool ) {
        _pool = pool as Pool<AudioPSource>;
    }


    public bool IsReady ( ) {
        return audio_source.enabled;
    }


    public void Duplicate ( AudioPSource a_template ) {
        if ( a_template != null )
            audio_source = a_template.audio_source;
    }


    public void Release ( ) {
        _pool.onRelease ( this );        
    }

    float debug_time;

    protected bool fading;
    protected float fade;
    protected float fadeTime;
    protected float currentFadeTime;
    
   
    public void Fade ( float time, float volume ) {
        fade            = volume;
        fadeTime        = time == .0f ? .0f : 1.0f / ( time * time * 10.0f );
        currentFadeTime = .0f;
        fading          = true;

        debug_time = Time.time;
    }


    protected bool pitching;
    protected float pitch;
    protected float pitchTime;
    protected float currentPitchTime;

    public void Pitch ( float time, float volume ) {
        pitch               = volume;
        pitchTime           = time == .0f ? .0f : 1.0f / ( time * time * 5.5f );
        currentPitchTime    = .0f;
        pitching            = true;

        debug_time = Time.time;
    }

    
    protected bool moving;
    protected Vector3 move;
    protected float moveTime;
    protected float currentMoveTime;


    public void Move ( float time, Vector3 position ) {
        move = position;
        moveTime = time;
        moving = true;
        currentMoveTime = .0f;

        debug_time = Time.time;
    }


    void Update ( ) {
        if ( fading ) {
            currentFadeTime += Time.deltaTime;

            if ( Mathf.Abs ( fade - audio_source.volume ) < 0.05f ) {
                fading = false;
                currentFadeTime = fadeTime;
                audio_source.volume = fade;

                Debug.Log ( Time.time - debug_time );
            }

            audio_source.volume = Mathf.Lerp ( audio_source.volume, fade, currentFadeTime * fadeTime );
        }

        if ( pitching ) {
            currentPitchTime += Time.deltaTime;

            if ( Mathf.Abs ( pitch - audio_source.pitch ) < 0.01f ) {
                pitching = false;
                currentPitchTime = fadeTime;
                audio_source.pitch = pitch;

                Debug.Log ( Time.time - debug_time );
            }

            audio_source.pitch = Mathf.Lerp ( audio_source.pitch, pitch, currentPitchTime * pitchTime );
        }

        if ( moving ) {
            if ( currentMoveTime <= moveTime ) {
                currentMoveTime += Time.deltaTime;
                transform.position = Vector3.Lerp ( transform.position, move, currentMoveTime / moveTime );
            } else {
                transform.position = move;
                moving = false;

                Debug.Log ( Time.time - debug_time );
            }
        }
    }

}
