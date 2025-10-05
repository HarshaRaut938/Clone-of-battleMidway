using UnityEngine;

public class BulletController : MonoBehaviour, IPoolable
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private BulletType bulletType;

    private Vector3 moveDirection;
    private bool isActive = false;
    private Camera mainCamera;

    public bool IsActive => isActive;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!isActive) return;
        transform.position += moveDirection * speed * Time.deltaTime;
        CheckBounds();
    }

    #region Initialize Methods
    public void Initialize(BulletType type, BulletDirection direction, float bulletSpeed)
    {
        bulletType = type;
        speed = bulletSpeed;
        moveDirection = direction switch
        {
            BulletDirection.Up => Vector3.up,
            BulletDirection.Down => Vector3.down,
            _ => Vector3.up
        };

        transform.rotation = direction == BulletDirection.Up ? Quaternion.identity : Quaternion.Euler(0, 0, 180);

        OnSpawn();
    }

    public void Initialize(BulletType type, float bulletSpeed, float angle)
    {
        bulletType = type;
        speed = bulletSpeed;
        moveDirection = Quaternion.Euler(0, 0, angle) * Vector3.down;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        OnSpawn(); 
    }

    #endregion

    #region Pool Handling

    public void OnSpawn() => isActive = true;
    public void OnDespawn() => isActive = false;

    private void CheckBounds()
    {
        if (!mainCamera) return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        if (screenPos.y < -50f || screenPos.y > Screen.height + 50f ||
            screenPos.x < -50f || screenPos.x > Screen.width + 50f)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        OnDespawn();

        if (bulletType == BulletType.PlayerBullet)
            PoolManager.Instance.ReturnPlayerBullet(this);
        else
            PoolManager.Instance.ReturnEnemyBullet(this);
    }

    #endregion

    #region Collision Handling

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (bulletType == BulletType.PlayerBullet && other.TryGetComponent(out EnemyController enemy))
        {
            EnemyFactory.DestroyEnemy(enemy);
            GameEvents.OnEnemyKilled();
            ReturnToPool();
        }
        else if (bulletType == BulletType.EnemyBullet && other.TryGetComponent(out PlayerHealth player))
        {
            player.TakeDamage(1);
            ReturnToPool();
        }
    }

    #endregion

    public BulletType GetBulletType() => bulletType;
}
