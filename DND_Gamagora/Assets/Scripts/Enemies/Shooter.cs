using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

    public Bullet bulletPrefab;

    protected Pool<Bullet> _bullets;

    protected Transform _player;

    public void init()
    {
        if (_bullets == null)
        {
            _bullets = new Pool<Bullet>(bulletPrefab, 4, 8);
            _bullets.automaticReuseUnavailables = true;
        }            
    }

    public void spawn(Vector3 position, Transform player)
    {
        //if (_bullets == null)
        //    _bullets = new Pool<Bullet>(bulletPrefab, 8, 16);

        _player = player;

        transform.position = position;

        GetComponent<BoxCollider2D>().size = new Vector2(1.0f, 2.0f);
        GetComponent<BoxCollider2D>().offset = new Vector2(.0f, -.7f);
    }
	
	
	public void shoot()
    {
        Bullet bullet;

        if(_bullets.GetAvailable(false, out bullet))
        {
            bullet.shoot(transform.position, (_player.position - transform.position).normalized);
        }
    }
}
