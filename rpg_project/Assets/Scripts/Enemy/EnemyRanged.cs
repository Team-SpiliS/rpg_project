using UnityEngine;

public class EnemyRanged : EnemyBase
{
    [Header("Дистанции боя")]
    public float stopDistance = 8f;
    public float retreatDistance = 5f;

    [Header("Настройки магии")]
    public int magicDamage = 25;
    public float fireRate = 3f;
    public GameObject projectilePrefab;
    public Transform shootPoint;

    private float nextFireTime;

    void Update()
    {
        if (isDead) return;

        if (isPlayerDead || !player) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            HandleMovement(dist);

            if (dist <= detectionRange && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        else if (animator) animator.SetFloat("Speed", 0);
    }

    void HandleMovement(float dist)
    {
        if (dist < retreatDistance)
        {
            Vector3 dirToPlayer = transform.position - player.position;
            Vector3 retreatPos = transform.position + dirToPlayer.normalized * 3f;
            if (agent != null && agent.isOnNavMesh) agent.SetDestination(retreatPos);
        }
        else if (dist > stopDistance)
        {
            if (agent != null && agent.isOnNavMesh) agent.SetDestination(player.position);
        }
        else
        {
            if (agent != null && agent.isOnNavMesh) agent.ResetPath();
        }

        if (animator && agent != null) animator.SetFloat("Speed", agent.velocity.magnitude);
        LookAtPlayer();
    }

    void Shoot()
    {
        if (animator) animator.SetTrigger("Attack");

        if (projectilePrefab && shootPoint)
        {
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, transform.rotation);
            Vector3 targetPoint = player.position + Vector3.up * 1.2f;
            proj.transform.LookAt(targetPoint);

            if (proj.TryGetComponent(out MagicProjectile magic))
            {
                magic.Setup(magicDamage, "Enemy");
            }
        }
    }
}