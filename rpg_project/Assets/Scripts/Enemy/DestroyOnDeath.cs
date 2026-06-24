using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    public float delay = 5f;

    void Start()
    {
        HealthComponent health = GetComponentInChildren<HealthComponent>();
        if (health != null)
        {
            health.OnDeath += HandleDeath;
        }
    }

    void HandleDeath()
    {
        if (!gameObject.CompareTag("Player"))
        {
            Destroy(transform.root.gameObject, delay);
        }
    }
}