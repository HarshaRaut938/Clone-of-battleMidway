using UnityEngine;

public class FifthShotPattern : MonoBehaviour, IShootingPattern
{
    [Header("Bullet Settings")]
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float spreadAngle = 30f;

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        ShootBullet(position, 0f);                 // Center
        ShootBullet(position, -spreadAngle);       // Left outer
        ShootBullet(position, -spreadAngle / 2f);  // Left inner
        ShootBullet(position, spreadAngle / 2f);   // Right inner
        ShootBullet(position, spreadAngle);        // Right outer
    }

    private void ShootBullet(Vector3 position, float angle)
    {
        BulletController bullet = PoolManager.Instance.GetEnemyBullet();
        bullet.transform.position = position;
        bullet.Initialize(BulletType.EnemyBullet, bulletSpeed, angle);
        bullet.OnSpawn();
    }

    public int GetBulletCount() => 5;
}
