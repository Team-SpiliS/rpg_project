using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System; 

public class UIController : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    public Image magicCooldownMask;

    public GameObject gameOverPanel;

    private HealthComponent playerHealth;
    private PlayerCombat playerCombat;
    private bool _isSubscribed;

    void OnEnable()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        BindPlayer();
    }

    void OnDisable()
    {
        UnbindPlayer();
    }

    private void BindPlayer()
    {
        if (_isSubscribed) return;

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
                _isSubscribed = true;
            }
        }
    }

    private void UnbindPlayer()
    {
        if (!_isSubscribed || playerHealth == null) return;

        playerHealth.OnHealthChanged -= UpdateHealthUI;
        playerHealth.OnDeath -= ShowGameOverScreen;
        _isSubscribed = false;
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
