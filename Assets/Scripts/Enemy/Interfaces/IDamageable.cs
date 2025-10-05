public interface IDamageable
{
    void TakeDamage(int damage = 1);
    bool CanBeDamagedBy(BulletType bulletType);
}
