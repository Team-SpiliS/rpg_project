using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isInvulnerable;

    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnTakeDamage;
    public event Action OnDeath;
    public event Action OnBlockHit;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, DamageType type)
    {
        if (currentHealth <= 0) return;

        if (isInvulnerable)
        {
            return;
        }

        IDamageBlocker damageBlocker = GetComponent<IDamageBlocker>();
        if (damageBlocker != null && damageBlocker.CanBlockDamage(type))
        {
            OnBlockHit?.Invoke(); 
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);


        OnTakeDamage?.Invoke(amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsInvulnerable => isInvulnerable;

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

    public void LoadHealth(int health)
    {
        currentHealth = health;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
