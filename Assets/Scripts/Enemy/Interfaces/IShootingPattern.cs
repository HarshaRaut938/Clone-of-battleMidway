using UnityEngine;

public interface IShootingPattern
{
    void Shoot(Vector3 position, Quaternion rotation);
    int GetBulletCount();
}
