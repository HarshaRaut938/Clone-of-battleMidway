using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSize = 20;

    [Header("Enemy Prefabs")]
    [SerializeField] private EnemyController[] enemyPrefabs;

    [Header("Bullet Prefabs")]
    [SerializeField] private BulletController playerBulletPrefab;
    [SerializeField] private BulletController enemyBulletPrefab;

    [Header("Pool Parents")]
    [SerializeField] private Transform enemyPoolParent;
    [SerializeField] private Transform bulletPoolParent;

    private Dictionary<EnemyType, ObjectPool<EnemyController>> enemyPools;
    private ObjectPool<BulletController> playerBulletPool;
    private ObjectPool<BulletController> enemyBulletPool;

    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        enemyPools = new Dictionary<EnemyType, ObjectPool<EnemyController>>();
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            EnemyType type = (EnemyType)i;
            enemyPools[type] = new ObjectPool<EnemyController>(enemyPrefabs[i], enemyPoolParent, initialPoolSize);
        }

        playerBulletPool = new ObjectPool<BulletController>(playerBulletPrefab, bulletPoolParent, initialPoolSize);
        enemyBulletPool = new ObjectPool<BulletController>(enemyBulletPrefab, bulletPoolParent, initialPoolSize);
    }

    #region Enemy Pool Methods
    public EnemyController GetEnemy(EnemyType type)
    {
        if (enemyPools.ContainsKey(type))
            return enemyPools[type].Get();
        return null;
    }

    public void ReturnEnemy(EnemyController enemy)
    {
        if (enemy == null) return;

        enemy.Deactivate();
        if (enemyPools.ContainsKey(enemy.GetEnemyType()))
            enemyPools[enemy.GetEnemyType()].Return(enemy);
    }
    #endregion

    #region Bullet Pool Methods
    public BulletController GetPlayerBullet() => playerBulletPool.Get();
    public BulletController GetEnemyBullet() => enemyBulletPool.Get();

    public void ReturnPlayerBullet(BulletController bullet) => playerBulletPool.Return(bullet);
    public void ReturnEnemyBullet(BulletController bullet) => enemyBulletPool.Return(bullet);
    #endregion
}
