using UnityEngine;

public class TripleShotPattern : MonoBehaviour, IShootingPattern
{
    [Header("Bullet Settings")]
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float spreadAngle = 25f;

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        ShootBullet(position, 0f);                // Center
        ShootBullet(position, -spreadAngle);      // Left
        ShootBullet(position, spreadAngle);       // Right
    }

    private void ShootBullet(Vector3 position, float angle)
    {
        BulletController bullet = PoolManager.Instance.GetEnemyBullet();
        bullet.transform.position = position;
        bullet.Initialize(BulletType.EnemyBullet, bulletSpeed, angle);
        bullet.OnSpawn();
    }

    public int GetBulletCount() => 3;
}
