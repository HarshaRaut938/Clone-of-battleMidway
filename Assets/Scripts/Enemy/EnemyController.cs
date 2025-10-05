using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Enemy Settings")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Shooting Settings")]
    [SerializeField] private float shootInterval = 2f;
    private float lastShootTime;

    private IShootingPattern shootingPattern;
    private bool isActive = false;

    public EnemyType GetEnemyType() => enemyType;

    private void Awake()
    {
        shootingPattern = GetComponent<IShootingPattern>();
    }

    private void Update()
    {
        if (!isActive) return;

        MoveDown();
        HandleShooting();

        if (transform.position.y < -10f)
        {
            EnemyFactory.DestroyEnemy(this);
        }
    }

    private void MoveDown()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    private void HandleShooting()
    {
        if (shootingPattern == null) return;

        if (Time.time - lastShootTime >= shootInterval)
        {
            shootingPattern.Shoot(transform.position, transform.rotation);
            lastShootTime = Time.time;
        }
    }
    public void TakeDamage(int damage = 1)
    {
        if (!isActive) return;

        isActive = false;
        gameObject.SetActive(false);
        GameEvents.OnEnemyKilled?.Invoke();
    }

    public bool CanBeDamagedBy(BulletType bulletType) => bulletType == BulletType.PlayerBullet;

    #region Pooling / Activation
    public void Initialize()
    {
        isActive = true;
        gameObject.SetActive(true);
        lastShootTime = Time.time; 
    }

    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
    #endregion
}
