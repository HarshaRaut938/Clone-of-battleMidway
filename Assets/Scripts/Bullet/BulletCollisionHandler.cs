using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
    private BulletController bullet;

    private void Awake()
    {
        bullet = GetComponent<BulletController>();
        if (bullet == null)
            Debug.LogError("BulletController missing on bullet prefab");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!bullet.IsActive) return;
        if (bullet.GetBulletType() == BulletType.PlayerBullet && other.TryGetComponent(out EnemyController enemy))
        {
            enemy.TakeDamage(1);             
            bullet.OnDespawn();             
            GameEvents.OnEnemyKilled();       
        }
        else if (bullet.GetBulletType() == BulletType.EnemyBullet && other.TryGetComponent(out PlayerHealth player))
        {
            player.TakeDamage(1);
            bullet.OnDespawn();              
        }
    }
}
