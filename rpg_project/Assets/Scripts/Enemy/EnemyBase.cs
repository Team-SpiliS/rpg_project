using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    public float detectionRange = 10f;

    protected NavMeshAgent agent;
    protected Transform player;
    protected HealthComponent playerHealth;
    protected Animator animator;

    protected bool isPlayerDead = false;
    protected bool isDead = false; 

    protected virtual void Start()
    {
        Transform root = transform.root;
        agent = root.GetComponentInChildren<NavMeshAgent>();
        animator = root.GetComponentInChildren<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p)
        {
            player = p.transform;
            playerHealth = p.GetComponent<HealthComponent>();

            if (playerHealth != null)
            {
                playerHealth.OnDeath += HandlePlayerDeath;
            }
        }

        HealthComponent myHealth = root.GetComponentInChildren<HealthComponent>();
        if (myHealth != null)
        {
            myHealth.OnDeath += HandleOwnDeath;
        }
        else
        {
            Debug.LogError($"[EnemyBase] На {root.name} не найден HealthComponent!");
        }
    }

    private void HandlePlayerDeath()
    {
        isPlayerDead = true;
        StopMoving();
    }

    private void HandleOwnDeath()
    {
        isDead = true; 
        StopMoving();

        if (agent != null) agent.enabled = false;

        Collider[] colliders = transform.root.GetComponentsInChildren<Collider>();
        foreach (var col in colliders) col.enabled = false;

        this.enabled = false;
    }

    protected void LookAtPlayer()
    {
        if (player == null || isPlayerDead || isDead) return; 

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);
    }

    protected void StopMoving()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true; 
            agent.ResetPath();
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    protected virtual void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandlePlayerDeath;
        }

        HealthComponent myHealth = transform.root.GetComponentInChildren<HealthComponent>();
        if (myHealth != null)
        {
            myHealth.OnDeath -= HandleOwnDeath;
        }
    }
}