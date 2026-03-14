using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [Header("Настройки здоровья")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnTakeDamage;
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, DamageType type)
    {
        if (currentHealth <= 0) return; 

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 

        Debug.Log($"{gameObject.name} получил {amount} урона типа {type}. Текущее ХП: {currentHealth}");

        OnTakeDamage?.Invoke();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} умер!");
        OnDeath?.Invoke();
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}