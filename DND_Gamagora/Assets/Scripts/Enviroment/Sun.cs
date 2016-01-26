using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour
{
    private Camera cam;
    private float delta;

	// Use this for initialization
	void Awake()
    {
        FindTheOrthographicCamera();

        if(cam != null)
        {
            delta = transform.position.x - cam.transform.position.x;
        }
    }
	
	// Update is called once per frame
	void Update()
    {
        if (cam != null)
        {
            // Center the sun
            transform.position = new Vector3(cam.transform.position.x + delta, transform.position.y, transform.position.z);
        }
    }

    void FindTheOrthographicCamera()
    {
        if (cam == null)
        {
            Camera[] CameraList = FindObjectsOfType<Camera>();
            foreach (Camera child in CameraList)
            {
                if (child.orthographic == true)
                {
                    // Keep only first Orthographic Camera
                    cam = child;
                    break;
                }
            }
        }

    }
}
