using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Параметры ИИ")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float fleeHealthThreshold = 25f;

    [Header("Имена анимаций")]
    public EnemyAnimationData animData;

    [Header("Компоненты")]
    public NavMeshAgent agent;
    public Animator animator;
    public HealthComponent myHealth;

    public Transform player { get; private set; }
    public HealthComponent playerHealth { get; private set; }
    public IGameSettings gameSettings { get; private set; }

    public EnemyStateMachine StateMachine { get; private set; }

    [HideInInspector] public bool wasHitByPlayer = false;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine();

        Transform root = transform.root;
        agent = root.GetComponentInChildren<NavMeshAgent>();
        animator = root.GetComponentInChildren<Animator>();
        myHealth = root.GetComponentInChildren<HealthComponent>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p)
        {
            player = p.transform;
            playerHealth = p.GetComponent<HealthComponent>();
        }
    }

    public virtual AbstractEnemyState CreateAttackState()
    {
        return new EnemyAttackState(this);
    }

    protected virtual void Start()
    {
        gameSettings = ServiceLocator.Get<IGameSettings>();

        if (myHealth != null)
        {
            myHealth.OnDeath += HandleDeath;
            myHealth.OnTakeDamage += (amount) => wasHitByPlayer = true;
        }
        StateMachine.Initialize(new EnemyIdleState(this));
    }

    protected virtual void Update()
    {
        if (isDead) return;
        StateMachine.CurrentState?.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        if (isDead) return;
        StateMachine.CurrentState?.PhysicsUpdate();
    }

    public void RotateTowardsPlayer()
    {
        if (player == null) return;
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);
    }

    private void HandleDeath()
    {
        isDead = true;
        if (agent != null) agent.enabled = false;
        Collider[] colliders = transform.root.GetComponentsInChildren<Collider>();
        foreach (var col in colliders) col.enabled = false;
        this.enabled = false;
    }
}