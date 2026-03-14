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

    void Start()
    {
        health = GetComponent<HealthComponent>();

        if (health != null)
        {
            health.OnHealthChanged += UpdateHealthBar;

            float max = health.GetMaxHealth();
            float cur = health.GetCurrentHealth();

            mainSlider.maxValue = max;
            mainSlider.value = cur;

            drainSlider.maxValue = max;
            drainSlider.value = cur;
        }
    }

    void Update()
    {
        if (drainSlider.value > mainSlider.value)
        {
            drainSlider.value -= drainSpeed * Time.deltaTime * (drainSlider.maxValue * 0.5f);
        }
    }

    void UpdateHealthBar(int current, int max)
    {
        mainSlider.maxValue = max;
        mainSlider.value = current;

        drainSlider.maxValue = max;
    }
}