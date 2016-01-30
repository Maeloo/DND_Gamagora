using UnityEngine;
using System.Collections;

public class AutoFade : MonoBehaviour {

    [SerializeField]
    float fadeValue;
    [SerializeField]
    float fadeTime;
    [SerializeField]
    float fadeDuration;
    [SerializeField]
    bool autoDestruct;

    private float   enableTime;
    private bool    enableFade = false;

    void OnEnable ( ) {
        enableTime = Time.time;
        enableFade = true;
    }


    void Update ( ) {
        if ( enableFade && Time.time - enableTime > fadeTime ) {
            enabled = false;
            iTween.FadeTo ( gameObject, fadeValue, fadeDuration );

            if ( autoDestruct )
                Invoke ( "destroy", fadeDuration );
        }
    }


    void destroy ( ) {
        DestroyImmediate ( gameObject );
    }

}
