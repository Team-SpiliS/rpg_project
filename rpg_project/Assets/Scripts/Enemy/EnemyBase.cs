using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    public float detectionRange = 10f; 

    protected NavMeshAgent agent;
    protected Transform player;
    protected HealthComponent playerHealth;
    protected Animator animator;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) { player = p.transform; playerHealth = p.GetComponent<HealthComponent>(); }
    }

    protected void LookAtPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);
    }
}