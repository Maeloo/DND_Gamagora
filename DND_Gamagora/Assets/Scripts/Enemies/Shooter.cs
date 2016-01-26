using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

    public Bullet bulletPrefab;


    protected Pool<Bullet> _bullets;

    protected Transform _player;


    public void spawn(Vector3 position, Transform player)
    {
        if (_bullets == null)
            _bullets = new Pool<Bullet>(bulletPrefab, 8, 16);

        _player = player;

        transform.position = position;
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
