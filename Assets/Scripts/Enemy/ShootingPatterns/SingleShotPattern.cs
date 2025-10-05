using UnityEngine;

public class SingleShotPattern : MonoBehaviour, IShootingPattern
{
    [Header("Bullet Settings")]
    [SerializeField] private float bulletSpeed = 8f;

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        BulletController bullet = PoolManager.Instance.GetEnemyBullet();
        bullet.transform.position = position;
        bullet.Initialize(BulletType.EnemyBullet, bulletSpeed, 0f); // straight down
    }

    public int GetBulletCount() => 1;
}
