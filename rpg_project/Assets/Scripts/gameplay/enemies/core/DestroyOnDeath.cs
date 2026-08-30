using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    public float delay = 5f;

    private HealthComponent health;

    void Start()
    {
        health = GetComponentInChildren<HealthComponent>();

        if (health != null)
            health.OnDeath += HandleDeath;
    }

    void OnDestroy()
    {
        if (health != null)
            health.OnDeath -= HandleDeath;
    }

    void HandleDeath()
    {
        if (!gameObject.CompareTag("Player"))
        {
            Destroy(transform.root.gameObject, delay);
        }
    }

}