using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject gameOverPanel;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateScore(int kills)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {kills}";
    }

    public void UpdatePlayerHealth(int health)
    {
        if (healthText != null)
            healthText.text = $"Health: {health}";
    }
    public void ShowGameOverUI()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}
