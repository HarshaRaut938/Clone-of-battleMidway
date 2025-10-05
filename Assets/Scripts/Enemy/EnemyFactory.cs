using UnityEngine;

public static class EnemyFactory
{
    public static EnemyController CreateEnemy(EnemyType type, Vector3 position)
    {
        EnemyController enemy = PoolManager.Instance.GetEnemy(type);
        enemy.transform.position = position;
        enemy.Initialize(); 
        return enemy;
    }
    public static void DestroyEnemy(EnemyController enemy)
    {
        if (enemy == null) return;
        enemy.Deactivate();
        PoolManager.Instance.ReturnEnemy(enemy);
    }
}
