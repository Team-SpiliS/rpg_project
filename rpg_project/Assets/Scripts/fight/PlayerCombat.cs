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
    public Transform attackPoint;

    [Header("Магическая атака (ПКМ)")]
    public GameObject magicPrefab;
    public int magicDamage = 30;
    public float magicCooldown = 2f;
    [Tooltip("Задержка перед вылетом шара (для синхронизации с анимацией)")]
    public float magicSpawnDelay = 0.5f;

    private float nextMeleeTime = 0f;
    private float nextMagicTime = 0f;
    private float lastMeleeClickTime = 0f;
    private int comboStep = 0;

    private Animator animator;

    public float MagicCooldownPercentage => Mathf.Clamp01((nextMagicTime - Time.time) / magicCooldown);

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMeleeInput();
        HandleMagicInput();
    }

    private void HandleMeleeInput()
    {
        if (Time.time - lastMeleeClickTime > comboResetTime) comboStep = 0;

        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextMeleeTime)
        {
            MeleeAttack();
            lastMeleeClickTime = Time.time;
            nextMeleeTime = Time.time + meleeCooldown;
        }
    }

    private void HandleMagicInput()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && Time.time >= nextMagicTime)
        {
            StartCoroutine(MagicAttackRoutine());
            nextMagicTime = Time.time + magicCooldown;
        }
    }

    private void MeleeAttack()
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
    }

    private IEnumerator MagicAttackRoutine()
    {
        animator.SetTrigger("Magic");

        yield return new WaitForSeconds(magicSpawnDelay);

        if (magicPrefab != null && attackPoint != null)
        {
            Instantiate(magicPrefab, attackPoint.position, transform.rotation);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}