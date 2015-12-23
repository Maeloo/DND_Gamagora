using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

    private bool aroundCenter = false;
    private Vector3 center = Vector3.zero;
    private float angle = .0f;

    public Vector3 rotationsPerSecond = new Vector3 ( 0f, 0f, 0f );

    void Update ( ) {
        float delta = Time.deltaTime;
        delta *= Mathf.Rad2Deg * Mathf.PI * 2f;
        Quaternion offset = Quaternion.Euler ( rotationsPerSecond * delta );

        if ( !aroundCenter )
            transform.rotation = transform.rotation * offset;
        else
            transform.Rotate ( center, angle );
    }

    
    public void enableAroundCenter ( ) {
        MeshFilter mf = GetComponent<MeshFilter> ( );

        foreach ( Vector3 vertex in mf.mesh.vertices )
            center += vertex;

        center /= mf.mesh.vertexCount;

        aroundCenter = true;
        angle = Random.RandomRange ( -.2f, .2f );
    }
}
