using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;


    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
        GameEvents.OnPlayerHit += TakeDamage;
    }
    private void Start()
    {
        UIManager.Instance.UpdatePlayerHealth(currentHealth);
    }
    private void OnDestroy()
    {
        GameEvents.OnPlayerHit -= TakeDamage;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(currentHealth);
        currentHealth -= damage;
        UIManager.Instance.UpdatePlayerHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        gameObject.SetActive(false);
        UIManager.Instance.ShowGameOverUI();
    }
   

    public int GetHealth() => currentHealth;
}
