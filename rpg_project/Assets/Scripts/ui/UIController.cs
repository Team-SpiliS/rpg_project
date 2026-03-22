using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System; 

public class UIController : MonoBehaviour
{
    [Header("Здоровье Игрока")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    [Header("Кулдаун Магии")]
    public Image magicCooldownMask;

    [Header("Экран Смерти")]
    public GameObject gameOverPanel;

    private HealthComponent playerHealth;
    private PlayerCombat playerCombat;

    void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthComponent>();
            playerCombat = player.GetComponent<PlayerCombat>();

            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged += UpdateHealthUI;
                playerHealth.OnDeath += ShowGameOverScreen;

                UpdateHealthUI(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
            }
        }
    }

    void Update()
    {
        if (playerCombat != null && magicCooldownMask != null)
        {
            magicCooldownMask.fillAmount = playerCombat.GetMagicCooldownNormalized();
        }
    }

    void UpdateHealthUI(int current, int max)
    {
        if (healthSlider)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }

        if (healthText)
        {
            healthText.text = $"{current} / {max}";
        }
    }

    void ShowGameOverScreen()
    {
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}