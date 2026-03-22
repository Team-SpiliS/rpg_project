using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("Физическая атака (ЛКМ)")]
    public int physicalDamage = 25;
    public float attackRange = 1.5f;
    public float meleeCooldown = 0.5f;
    public float comboResetTime = 1.0f;
    public float meleeDamageDelay = 0.3f;
    public Transform attackPoint;

    [Header("Магическая атака (ПКМ)")]
    public GameObject magicPrefab;
    public int magicDamage = 30;
    public float magicCooldown = 2f;
    [Tooltip("Задержка перед вылетом шара")]
    public float magicSpawnDelay = 0.5f;

    private float nextMeleeTime = 0f;
    private float nextMagicTime = 0f;
    private float lastMeleeClickTime = 0f;
    private int comboStep = 0;
    private Coroutine currentAttackCoroutine;
    private HealthComponent health;

    private Animator animator;

    public float MagicCooldownPercentage => Mathf.Clamp01((nextMagicTime - Time.time) / magicCooldown);

    void Start()
    {
        animator = GetComponent<Animator>();

        health = GetComponent<HealthComponent>();
        if (health != null)
        {
            health.OnTakeDamage += InterruptAttack;
        }
    }

    void InterruptAttack()
    {
        if (currentAttackCoroutine != null)
        {
            StopCoroutine(currentAttackCoroutine);
            currentAttackCoroutine = null;
        }
    }

    void Update()
    {
        HandleMeleeInput();
        HandleMagicInput();
    }

    private void HandleMeleeInput()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Magic")) return;

        if (Time.time - lastMeleeClickTime > comboResetTime) comboStep = 0;

        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextMeleeTime)
        {
            currentAttackCoroutine = StartCoroutine(MeleeAttack());
            lastMeleeClickTime = Time.time;
            nextMeleeTime = Time.time + meleeCooldown;
        }
    }

    private void HandleMagicInput()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")) return;

        if (Mouse.current.rightButton.wasPressedThisFrame && Time.time >= nextMagicTime)
        {
            currentAttackCoroutine = StartCoroutine(MagicAttackRoutine());
            nextMagicTime = Time.time + magicCooldown;
        }
    }

    private IEnumerator MeleeAttack()
    {
        if (comboStep == 0)
        {
            animator.SetTrigger("Attack1");
            comboStep = 1;
        }
        else
        {
            animator.SetTrigger("Attack2");
            comboStep = 0;
        }

        yield return new WaitForSeconds(meleeDamageDelay);

        Vector3 hitPosition = attackPoint != null ? attackPoint.position : transform.position + transform.forward * 1f;
        Collider[] hitEnemies = Physics.OverlapSphere(hitPosition, attackRange);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.gameObject == gameObject) continue;
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(physicalDamage, DamageType.Physical);
            }
        }

        currentAttackCoroutine = null;
    }

    private IEnumerator MagicAttackRoutine()
    {
        animator.SetTrigger("Magic");

        yield return new WaitForSeconds(magicSpawnDelay);

        if (magicPrefab != null && attackPoint != null)
        {
            GameObject proj = Instantiate(magicPrefab, attackPoint.position, transform.rotation);

            if (proj.TryGetComponent(out MagicProjectile magic))
            {
                magic.Setup(magicDamage, "Player");
            }
        }

        currentAttackCoroutine = null;
    }


    public float GetMagicCooldownNormalized()
    {
        if (Time.time >= nextMagicTime) return 0f; 

        float timeRemaining = nextMagicTime - Time.time;
        return timeRemaining / magicCooldown;
    }
}