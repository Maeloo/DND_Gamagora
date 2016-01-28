using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using Game;

public class SceneAudioManager : Singleton<SceneAudioManager>
{
    protected SceneAudioManager() { }

    public AudioMixerGroup masterMixerGroup;

    private Dictionary<Audio_Type, AudioData> AudioDatas;
    private Dictionary<int, AudioPSource> AudioPlaying;
    private Pool<AudioPSource> AudioPool;

    void Awake( )
    {
        AudioPool       = new Pool<AudioPSource> ( null, 64, 256 );
        if (AudioDatas == null)
            AudioDatas = new Dictionary<Audio_Type, AudioData>();
        AudioPlaying    = new Dictionary<int, AudioPSource> ( );

        if ( masterMixerGroup == null ) {
            AudioMixer masterMixer = ( Resources.Load ( "Audio/MasterMixer" ) as AudioMixer );

            if ( masterMixer != null )
                masterMixerGroup = masterMixer.FindMatchingGroups ( "Master" )[0];
        }
        
    }


    public void registerAudioData ( Audio_Type type, AudioData data ) {
        //if (AudioDatas == null)
        //    AudioDatas = new Dictionary<Audio_Type, AudioData>();

        if (!AudioDatas.ContainsKey(type))
            AudioDatas.Add(type, data);
    }


    public AudioMixerGroup getMixerType ( Audio_Type type ) {
        AudioData datas;
        if ( AudioDatas.TryGetValue ( type, out datas ) ) {
            return datas.defaultMixerGroup;
        }

        return null;
    }


    public void setGlobalVolume ( float volume ) {
        if ( masterMixerGroup != null ) {
            masterMixerGroup.audioMixer.SetFloat ( "master_volume", volume );
        }
    }


    public void setGlobalPitch ( float pitch ) {
        if ( masterMixerGroup != null ) {
            masterMixerGroup.audioMixer.SetFloat ( "master_pitch", pitch );
        }
    }


    public void move ( int key, float time, Vector3 position ) {
        AudioPSource source;
        if ( AudioPlaying.TryGetValue ( key, out source ) ) {
            source.Move ( time, position );
        }
    }


    public void moveAll ( int key, float time, Vector3 position ) {
        foreach ( KeyValuePair<int, AudioPSource> entry in AudioPlaying ) {
            entry.Value.Move ( time, position );
        }
    }


    public void fade ( int key, float time, float volume ) {
        volume = Mathf.Clamp ( volume, .0f, 1.0f );

        AudioPSource source;
        if ( AudioPlaying.TryGetValue ( key, out source ) ) {
            source.Fade ( time, volume );
        }
    }


    public void fadeAll ( int key, float time, float volume ) {
        volume = Mathf.Clamp ( volume, .0f, 1.0f );

        foreach ( KeyValuePair<int, AudioPSource> entry in AudioPlaying ) {
            entry.Value.Fade ( time, volume );
        }
    }


    public void pitch ( int key, float time, float pitch ) {
        pitch = Mathf.Clamp ( pitch, -3.0f, 3.0f );

        AudioPSource source;
        if ( AudioPlaying.TryGetValue ( key, out source ) ) {
            source.Pitch ( time, pitch );
        }
    }


    public void pitchAll ( int key, float time, float pitch ) {
        pitch = Mathf.Clamp ( pitch, -3.0f, 3.0f );

        foreach ( KeyValuePair<int, AudioPSource> entry in AudioPlaying ) {
            entry.Value.Pitch ( time, pitch );
        }
    }


    /*
     * Hashtable parameters :
     *  loop : boolean
     *  delayed : boolean
     *  delayedtime : float
     *  priority : int
     *  volume : float
     *  pitch : float
     *  panStereo : float
     *  spatialBlend : float
     *  reverbZoneMix : float
     *  dopplerLevel : float
     *  spread : float
     *  rolloffMode : AudioRolloffMode
     *  minDistance : float
     *  maxDistance : float
     *  position : Vector3
     *  autoRelease : boolean
     *  starttime : float
     */
    public int playAudio ( Audio_Type type, Hashtable param = null ) {
        AudioSource source;
        int key;

        if ( GetSource ( type, out source, out key ) ) {
            if ( param == null )
                param = new Hashtable ( );

            bool delayed = param.ContainsKey ( "delayed" ) ? ( bool ) param["delayed"] : false;

            source.loop             = param.ContainsKey ( "loop" ) ? ( bool ) param["loop"] : false;
            source.priority         = param.ContainsKey ( "priority" ) ? ( int ) param["priority"] : 128;
            source.volume           = param.ContainsKey ( "volume" ) ? ( float ) param["volume"] : 1.0f;
            source.pitch            = param.ContainsKey ( "pitch" ) ? ( float ) param["pitch"] : 1.0f;
            source.panStereo        = param.ContainsKey ( "panStereo" ) ? ( float ) param["panStereo"] : .0f;
            source.spatialBlend     = param.ContainsKey ( "spatialBlend" ) ? ( float ) param["spatialBlend"] : .0f;
            source.reverbZoneMix    = param.ContainsKey ( "reverbZoneMix" ) ? ( float ) param["reverbZoneMix"] : .45f;
            source.dopplerLevel     = param.ContainsKey ( "dopplerLevel" ) ? ( float ) param["dopplerLevel"] : 1.0f;
            source.spread           = param.ContainsKey ( "spread" ) ? ( float ) param["spread"] : .0f;
            source.rolloffMode      = param.ContainsKey ( "rolloffMode" ) ? ( AudioRolloffMode ) param["rolloffMode"] : AudioRolloffMode.Logarithmic;
            source.minDistance      = param.ContainsKey ( "minDistance" ) ? ( float ) param["minDistance"] : 1.0f;
            source.maxDistance      = param.ContainsKey ( "maxDistance" ) ? ( float ) param["maxDistance"] : 500.0f;

            source.transform.position = param.ContainsKey ( "position" ) ? ( Vector3 ) param["position"] : Vector3.zero;
            source.time = param.ContainsKey("starttime") ? (float)param["starttime"] : .0f;

            float time = param.ContainsKey ( "delayedtime" ) ? ( float ) param["delayedtime"] : .0f;
            if ( delayed ) {
                source.PlayDelayed ( time );
            } else {
                source.Play ( );
            }

            if ( !source.loop && param.ContainsKey ( "autoRelease" ) ? ( bool ) param["autoRelease"] : true ) {
                StartCoroutine ( autoRelease ( key, time ) );
            }
        } else {
            Destroy ( source );
        }

        return key;
    }


    public IEnumerator autoRelease ( int key, float delay ) {
        yield return new WaitForSeconds ( delay );

        AudioPSource psource;
        if ( AudioPlaying.TryGetValue ( key, out psource ) ) {
            AudioSource source = psource.audio_source;
            while ( source.time < source.clip.length ) {
                yield return new WaitForSeconds ( .1f );
            }
        }

        stop ( key );
    }


    public void pause ( int key, bool mute = true ) {
        AudioPSource source;
        if ( AudioPlaying.TryGetValue ( key, out source ) ) {
            source.audio_source.Pause ( );
        }
    }


    public void pauseAll ( bool mute = true ) {
        foreach ( KeyValuePair<int, AudioPSource> entry in AudioPlaying ) {
            entry.Value.audio_source.Pause ( );
        }
    }


    public IEnumerator stopDelayed ( int key, float time ) {
        yield return new WaitForSeconds ( time + 1.0f );
        stop ( key );
    }


    public IEnumerator stopDelayed ( float time ) {
        yield return new WaitForSeconds ( time + 1.0f );
        stopAll ( );
    }


    public void stop ( int key ) {
        AudioPSource source;
        if ( AudioPlaying.TryGetValue ( key, out source ) ) {
            source.audio_source.Stop ( );
            source.Release ( );

            AudioPlaying.Remove ( key );
        }
    }


    public void stopAll ( ) {
        foreach ( KeyValuePair<int, AudioPSource> entry in AudioPlaying ) {
            entry.Value.audio_source.Stop ( );
            entry.Value.Release ( );
        }
    }


    public void mute ( int key, bool mute = true ) {
        AudioPSource source;
        if ( AudioPlaying.TryGetValue ( key, out source ) ) {
            source.audio_source.mute = mute;
        }
    }


    public void muteAll ( bool mute = true ) {
        foreach ( KeyValuePair<int, AudioPSource> entry in AudioPlaying ) {
            entry.Value.audio_source.mute = mute;
        }
    }


    private bool GetSource ( Audio_Type type, out AudioSource source, out int key ) {
        AudioData data;

        if ( AudioDatas.TryGetValue ( type, out data ) ) {
            AudioPSource psource;
            if ( AudioPool.GetAvailable ( true, out psource ) ) {
                key = 0;
                while ( AudioPlaying.ContainsKey ( key ) ) {
                    key++;
                }

                AudioPlaying.Add ( key, psource );

                source = psource.audio_source;

                source.clip = data.getClip ( );

                if ( data.defaultMixerGroup != null )
                    source.outputAudioMixerGroup = data.defaultMixerGroup;
                else
                    source.outputAudioMixerGroup = masterMixerGroup;

                return true;
            } else {
                Destroy ( psource );
            }
        } else {
            Destroy ( data );
        }

        source = new AudioSource ( );
        key = -1;

        return false;
    }

}
