using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float shootRate = 0.5f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform firePoint;

    [Header("Power-Up Settings")]
    [SerializeField] private int killsPerLevel = 10;
    [SerializeField] private float horizontalSpacing = 0.4f;

    private float lastShotTime;
    private bool isShooting = false;
    private int currentKillCount = 0;
    private int currentShootLevel = 1;

    private void Awake()
    {
        if (firePoint == null)
            firePoint = transform;
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += RegisterEnemyKill;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= RegisterEnemyKill;
    }

    private void Start()
    {
        StartShooting();
    }

    private void Update()
    {
        if (!isShooting) return;

        if (Time.time - lastShotTime > shootRate)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    private void Shoot()
    {
        int bulletCount = currentShootLevel;
        float totalWidth = (bulletCount - 1) * horizontalSpacing;
        float startOffset = -totalWidth / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 offset = firePoint.right * (startOffset + i * horizontalSpacing);
            Vector3 spawnPos = firePoint.position + offset;

            BulletController bullet = PoolManager.Instance.GetPlayerBullet();
            bullet.transform.position = spawnPos;
            bullet.transform.rotation = firePoint.rotation;
            bullet.Initialize(BulletType.PlayerBullet, BulletDirection.Up, bulletSpeed);
        }
    }

    #region Power-Up Logic
    public void RegisterEnemyKill()
    {
        currentKillCount++;
        UIManager.Instance.UpdateScore(currentKillCount);

        int newLevel = (currentKillCount / killsPerLevel) + 1;
        newLevel = Mathf.Clamp(newLevel, 1, 3);

        if (newLevel != currentShootLevel)
        {
            currentShootLevel = newLevel;
        }
    }
    #endregion

    #region Controls
    public void StartShooting() => isShooting = true;
    #endregion
}
