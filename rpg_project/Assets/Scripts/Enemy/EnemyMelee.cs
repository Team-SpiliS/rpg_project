using UnityEngine;
using System.Collections;

public class EnemyMelee : EnemyBase
{
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 20;
    public float damageDelay = 0.5f;

    private float nextAttackTime;
    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
        if (agent != null) agent.stoppingDistance = attackRange - 0.5f;
    }

    void Update()
    {
        if (isDead) return;

        if (isPlayerDead || !player || isAttacking) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            if (agent != null && agent.isOnNavMesh) agent.SetDestination(player.position);
            if (animator) animator.SetFloat("Speed", agent.velocity.magnitude);

            if (dist <= attackRange && Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackRoutine());
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else if (animator) animator.SetFloat("Speed", 0);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        if (agent != null) agent.isStopped = true;

        LookAtPlayer();

        if (animator) animator.SetTrigger("Attack");

        yield return new WaitForSeconds(damageDelay);

        if (!isDead && !isPlayerDead && playerHealth != null)
        {
            float currentDist = Vector3.Distance(transform.position, player.position);
            if (currentDist <= attackRange + 0.5f)
            {
                playerHealth.TakeDamage(damage, DamageType.Physical);
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (agent != null && agent.isOnNavMesh) agent.isStopped = false;
        isAttacking = false;
    }
}