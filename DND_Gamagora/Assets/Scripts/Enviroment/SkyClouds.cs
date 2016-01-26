using UnityEngine;
using System.Collections;

public class SkyClouds : MonoBehaviour
{
    [SerializeField]
    private bool ScaleSprite = false;

    [SerializeField]
    private int index = 0;

    private Camera cam;
    private float width;
    private Vector3 left;
    private Vector3 right;
    private float vertExtent;
    private float horzExtent;

    // Use this for initialization
    void Awake()
    {
        FindTheOrthographicCamera();

        if (cam != null)
        {
            if (ScaleSprite)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Vector3 size = GetComponentInChildren<Renderer>().bounds.size;
       
                // Strength this gameObject to fit the camera view
                this.transform.localScale = new Vector3(
                    horzExtent / size.x,
                    transform.localScale.y,
                    1);
            }
            
            if(index == 0)
                transform.position = new Vector3(cam.transform.position.x, transform.position.y, transform.position.z);
            else if(index == 1)
                transform.position = new Vector3(cam.transform.position.x + width, transform.position.y, transform.position.z);
        }
    }
	
	// Update is called once per frame
	void Update()
    {
        // Calculate Left/Right most position at the edge of camera view
        if (cam != null)
        {
            if ((cam.transform.position.x - transform.position.x) > width)
                transform.position = new Vector3(transform.position.x + width * 2f, transform.position.y, transform.position.z);
            else if((transform.position.x - cam.transform.position.x) > width)
                transform.position = new Vector3(transform.position.x - width * 2f, transform.position.y, transform.position.z);
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

        if (cam != null)
        {
            vertExtent = cam.orthographicSize * 2f;
            horzExtent = vertExtent * Screen.width / Screen.height;
            // Calculations assume cam is position at the origin
            float minX = horzExtent - cam.transform.position.x * 0.5f;
            float maxX = cam.transform.position.x * 0.5f - horzExtent;
            float minY = vertExtent - cam.transform.position.y * 0.5f;
            float maxY = cam.transform.position.y * 0.5f - vertExtent;

            width = (minX - maxX) * 0.5f;
        }
    }
}
