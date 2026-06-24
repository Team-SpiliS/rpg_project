using UnityEngine;

public class CharacterAnimationRelay : MonoBehaviour
{
    private Animator animator;
    private HealthComponent health;

    void Awake()
    {
        Transform root = transform.root;
        health = root.GetComponentInChildren<HealthComponent>();
        animator = root.GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        if (health != null)
        {
            health.OnTakeDamage += HandleTakeDamage;
            health.OnDeath += PlayDeathAnimation;
        }
    }

    void OnDisable()
    {
        if (health != null)
        {
            health.OnTakeDamage -= HandleTakeDamage;
            health.OnDeath -= PlayDeathAnimation;
        }
    }

    void HandleTakeDamage(int amount)
    {
        if (health != null && health.GetCurrentHealth() <= 0) return;

        PlayHitAnimation();
    }

    void PlayHitAnimation()
    {
        if (animator == null) return;

        if (HasParameter("Hit"))
        {
            animator.SetTrigger("Hit");
        }
    }

    void PlayDeathAnimation()
    {
        if (animator == null) return;

        if (HasParameter("IsDead"))
        {
            animator.SetBool("IsDead", true);
        }

        if (HasParameter("Speed"))
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private bool HasParameter(string paramName)
    {
        if (animator == null) return false;
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }

    void PlayShieldHitAnimation()
    {
        if (animator != null) animator.CrossFade("HitShield", 0.05f);
    }
}
