using UnityEngine;
using System.Collections;

public class Accessibility2 : MonoBehaviour {
    [SerializeField]
    private Movement m;
    [SerializeField]
    private float _deadZone = 0.5f;
    public float sensitivity = 1.5f;
    // Update is called once per frame
    void Update()
    {

        Vector2 currentMousePosition = Input.mousePosition; 
        // Calculer les differences de position entre deux frames
        Vector2 deltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (Input.touchCount > 0)
        {
            deltaPosition = Input.touches[0].deltaPosition*sensitivity;
        }

        // TODO Sauter 
        // DeltaPosition est en haut
        if (deltaPosition.y > _deadZone)
        {
            Debug.Log("jump");
            m.Jump();
        }

        // TODO Glisser
        // DeltaPosition est en bas
        if (deltaPosition.y < -_deadZone)
        {
            Debug.Log("slide");
            m.Slide();
        }

        // TODO Accelerer
        // DeltaPosition est en droite
        if (deltaPosition.x > _deadZone)
        {
            Debug.Log("sprint");
            m.Sprint();
        }

        // TODO Freinner
        // DeltaPosition est en gauche
        if (deltaPosition.x < -_deadZone)
        {
            Debug.Log("slow");
            m.SlowDown();
        }
    }
}