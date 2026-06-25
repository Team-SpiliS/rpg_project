using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [Header("Слайдеры")]
    public Slider mainSlider;
    public Slider drainSlider;

    [Header("Настройки эффекта")]
    public float drainSpeed = 2f;
    private HealthComponent health;

    [Tooltip("Множитель скорости стекания ХП")]
    [SerializeField] private float drainMultiplier = 0.5f;

    void Awake()
    {
        health = GetComponent<HealthComponent>();
    }

    void OnEnable()
    {
        if (health == null)
        {
            health = GetComponent<HealthComponent>();
        }

        if (health != null)
        {
            health.OnHealthChanged += UpdateHealthBar;
            ResetVisuals();
        }
    }

    void OnDisable()
    {
        if (health != null)
        {
            health.OnHealthChanged -= UpdateHealthBar;
        }
    }

    public void ResetVisuals()
    {
        if (health == null)
        {
            health = GetComponent<HealthComponent>();
        }

        if (health == null) return;

        float max = health.GetMaxHealth();
        float cur = health.GetCurrentHealth();

        if (mainSlider != null)
        {
            mainSlider.maxValue = max;
            mainSlider.value = cur;
        }

        if (drainSlider != null)
        {
            drainSlider.maxValue = max;
            drainSlider.value = cur;
        }
    }

    void Update()
    {
        if (mainSlider == null || drainSlider == null) return;

        if (drainSlider.value > mainSlider.value)
        {
            drainSlider.value -= drainSpeed * Time.deltaTime * (drainSlider.maxValue * drainMultiplier);
        }
    }

    void UpdateHealthBar(int current, int max)
    {
        if (mainSlider != null)
        {
            mainSlider.maxValue = max;
            mainSlider.value = current;
        }

        if (drainSlider != null)
        {
            drainSlider.maxValue = max;
        }
    }
}
