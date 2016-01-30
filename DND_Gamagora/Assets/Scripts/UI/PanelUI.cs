using UnityEngine;
using System.Collections;

public class PanelUI : MonoBehaviour {

    public Vector3 fromPosition;
    public Vector3 showPosition;
    public Vector3 hidePosition;
    public float time;
    public bool hideOnStart;

    private bool _displayed;
    public bool isDisplayed
    {
        get { return _displayed; }
    }
    
    void Start () {
        if (hideOnStart)
            transform.localPosition = fromPosition;
        else
            _displayed = true;
	}
	
	
    public void display()
    {
        _displayed = true;

        transform.localPosition = fromPosition;

        iTween.MoveTo(gameObject, iTween.Hash(
            "time", time,
            "islocal", true,
            "position", showPosition,
            "easetype", iTween.EaseType.easeInOutExpo));
    }


    public void hide()
    {
        _displayed = false;

        iTween.MoveTo(gameObject, iTween.Hash(
            "time", time,
            "islocal", true,
            "position", hidePosition,
            "easetype", iTween.EaseType.easeInOutExpo));
    }
}
