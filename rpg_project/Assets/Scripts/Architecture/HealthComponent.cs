using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

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

        BossEnemy boss = transform.root.GetComponentInChildren<BossEnemy>();

        if (boss != null && boss.isInvulnerable)
        {
            return;
        }

        PlayerCombat playerCombat = GetComponent<PlayerCombat>();
        if (playerCombat != null && playerCombat.IsBlocking)
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

    public void LoadHealth(int health)
    {
        currentHealth = health;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}