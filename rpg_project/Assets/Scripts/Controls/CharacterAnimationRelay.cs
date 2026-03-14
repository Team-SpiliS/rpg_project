using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimationRelay : MonoBehaviour
{
    private Animator animator;
    private HealthComponent health;
    private NavMeshAgent agent; 

    void Start()
    {
        animator = GetComponent<Animator>(); 
        health = GetComponent<HealthComponent>();
        if (health == null) health = GetComponentInChildren<HealthComponent>();
        agent = GetComponent<NavMeshAgent>();


        if (health != null)
        {
            health.OnTakeDamage += PlayHitAnimation;
            health.OnDeath += PlayDeathAnimation;
        }
    }

    void PlayHitAnimation()
    {
        if (animator != null) animator.SetTrigger("Hit");
    }

    void PlayDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsDead", true);

            animator.SetFloat("Speed", 0f);
        }
        if (agent != null) agent.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this && script != health && script != animator)
                script.enabled = false;
        }

        if (!gameObject.CompareTag("Player"))
        {
            Destroy(transform.root.gameObject, 5f);
        }
    }
}