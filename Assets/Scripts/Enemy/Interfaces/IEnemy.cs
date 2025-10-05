using UnityEngine;

public interface IEnemy
{
    void Initialize();
    void Activate();
    void Deactivate();
    void ReturnToPool();
    EnemyType GetEnemyType();
    Transform GetTransform();
}
