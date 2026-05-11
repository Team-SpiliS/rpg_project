using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float fleeHealthThreshold = 25f;

    public NavMeshAgent agent;
    public Animator animator;
    public EnemyAnimationData animData;
    public HealthComponent myHealth;

    [Header("Visual References")]
    public GameObject[] visualWeapons;
    public Transform[] weaponShootPoints;

    [Header("Configs")]
    public WeaponConfigSO weaponConfig;
    public ElementConfigSO elementConfig;

    public EnemyFactorySO originFactory;

    public Transform player { get; private set; }
    public HealthComponent playerHealth { get; private set; }
    public IGameSettings gameSettings { get; private set; }

    public EnemyStateMachine StateMachine { get; protected set; }

    [HideInInspector] public bool wasHitByPlayer = false;
    protected bool isDead = false;

    protected virtual void Awake()
    {
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
        ApplyVisuals();
    }

    protected virtual void Start()
    {
        gameSettings = ServiceLocator.Get<IGameSettings>();

        if (myHealth != null)
        {
            myHealth.OnDeath += HandleDeath;
            myHealth.OnTakeDamage += (amount) => wasHitByPlayer = true;
        }
    }

    protected virtual void Update()
    {
        if (!isDead) StateMachine?.CurrentState?.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        if (!isDead) StateMachine?.CurrentState?.PhysicsUpdate();
    }

    public void RotateTowardsPlayer()
    {
        if (player == null) return;
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);
    }

    public abstract AbstractEnemyState CreateAttackState();

    private void HandleDeath()
    {
        isDead = true;
        if (agent != null) agent.enabled = false;
        this.enabled = false;
    }


    public virtual void ApplyVisuals()
    {
        foreach (var w in visualWeapons) w.SetActive(false);

        if (weaponConfig != null && visualWeapons.Length > weaponConfig.weaponVisualIndex)
        {
            visualWeapons[weaponConfig.weaponVisualIndex].SetActive(true);
        }
    }


    public virtual void ResetEnemy()
    {
        isDead = false;
        wasHitByPlayer = false;
        if (agent != null) agent.enabled = true;
        this.enabled = true;

        myHealth.LoadHealth(myHealth.GetMaxHealth());

        var healthUI = GetComponentInChildren<EnemyHealthUI>();
        if (healthUI != null)
        {
            healthUI.ResetVisuals();
        }

        StateMachine.ChangeState(new EnemyIdleState(this));
    }

    public Transform CurrentShootPoint
    {
        get
        {
            if (weaponShootPoints != null &&
                weaponShootPoints.Length > weaponConfig.weaponVisualIndex &&
                weaponShootPoints[weaponConfig.weaponVisualIndex] != null)
            {
                return weaponShootPoints[weaponConfig.weaponVisualIndex];
            }
            return transform;
        }
    }

    public GameObject GetCurrentProjectile()
    {
        GameObject projectile = weaponConfig.projectilePrefab;

        if (elementConfig != null && elementConfig.projectileOverride != null)
        {
            projectile = elementConfig.projectileOverride;
        }

        return projectile;
    }
}