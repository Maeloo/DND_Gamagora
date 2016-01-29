using UnityEngine;
using System.Collections;

public class CrazyFireball : MonoBehaviour
{
    public Bullet bulletPrefab;

    protected Pool<Bullet> _bullets;


    public void init()
    {
        reset();

        if(_bullets == null)
        {
            _bullets = new Pool<Bullet>(bulletPrefab, 4, 8);
            _bullets.automaticReuseUnavailables = true;
        }            
    }

    public void spawn(Vector3 position)
    {
        reset();

        transform.position = position;

        iTween.ScaleTo(gameObject, iTween.Hash(
            "time", .5f,
            "scale", new Vector3(3.0f, 3.0f, 3.0f),
            "easetype", iTween.EaseType.easeInOutExpo));
    }

    void reset ()
    {
        transform.localScale = Vector3.zero;
    }

    public void shoot()
    {
        Bullet bullet;

        if (_bullets.GetAvailable(false, out bullet))
        {
            bullet.shoot(transform.position, -Vector3.up);
        }
    }

    public void releaseBullets()
    {
        for (int i = _bullets.usedObjects.Count - 1; i >= 0; i--)
        {
            _bullets.usedObjects[i].Release();
        }
    }

}
