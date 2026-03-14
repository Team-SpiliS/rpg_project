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
        agent.stoppingDistance = attackRange - 0.5f;
    }

    void Update()
    {
        if (!player || isAttacking) return;
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            agent.SetDestination(player.position); 
            if (animator) animator.SetFloat("Speed", agent.velocity.magnitude);

            if (dist <= attackRange && Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackRoutine());
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else if (animator) animator.SetFloat("Speed", 0);
    }

    void Attack()
    {
        LookAtPlayer();
        if (animator) animator.SetTrigger("Attack");
        if (playerHealth) playerHealth.TakeDamage(damage, DamageType.Physical);
    }
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        agent.isStopped = true; 

        LookAtPlayer();

        if (animator) animator.SetTrigger("Attack");

        yield return new WaitForSeconds(damageDelay);

        float currentDist = Vector3.Distance(transform.position, player.position);
        if (currentDist <= attackRange + 0.5f)
        {
            if (playerHealth != null)
                playerHealth.TakeDamage(damage, DamageType.Physical);
        }

        yield return new WaitForSeconds(0.5f);

        agent.isStopped = false;
        isAttacking = false;
    }
}