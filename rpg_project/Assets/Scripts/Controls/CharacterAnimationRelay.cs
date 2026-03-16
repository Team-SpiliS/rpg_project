using UnityEngine;

public class CharacterAnimationRelay : MonoBehaviour
{
    private Animator animator;
    private HealthComponent health;

    void Start()
    {
        Transform root = transform.root;

        health = root.GetComponentInChildren<HealthComponent>();
        animator = root.GetComponentInChildren<Animator>();

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
    }
}