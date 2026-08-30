using UnityEngine;

public class BossEnemy : EnemyBase
{
    [Header("Boss Settings")]
    public int heavyDamage = 40;
    public int magicDamage = 25;
    public float stunDamageThreshold = 50f;

    [HideInInspector] public bool isPhaseTwo = false;
    [HideInInspector] public bool isInvulnerable = false;
    [HideInInspector] public float damageTakenRecently = 0;

    private float _lastDamageTime = 0;
    private float _stunResetTime = 5f;

    public event System.Action OnPhaseChanged;
    public event System.Action OnStunTriggered;

    public override string SaveId => "Boss";
    public override bool CanReturnToPool => false;
    public override bool CountsForKillReward => false;
    public override int ScoreReward => 100;


    protected override void Awake()
    {
        StateMachine = new BossStateMachine(this);

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(CreateIdleState());
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (myHealth != null)
        {
            myHealth.OnTakeDamage += RegisterDamage;
        }
    }

    protected override void OnDisable()
    {
        if (myHealth != null)
        {
            myHealth.OnTakeDamage -= RegisterDamage;
        }

        base.OnDisable();
    }

    public void RegisterDamage(int amount)
    {
        if (myHealth == null || myHealth.GetCurrentHealth() <= 0) return;

        wasHitByPlayer = true;

        if (Time.time > _lastDamageTime + _stunResetTime)
            ResetStunMeter();

        damageTakenRecently += amount;

        if (myHealth.GetCurrentHealth() <= myHealth.GetMaxHealth() * 0.5f && !isPhaseTwo)
        {
            isPhaseTwo = true;
            OnPhaseChanged?.Invoke();
            return;
        }
        if (damageTakenRecently >= stunDamageThreshold)
        {
            OnStunTriggered?.Invoke();
            return;
        }
        _lastDamageTime = Time.time;
    }

    public void ResetStunMeter()
    {
        damageTakenRecently = 0;
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
        if (myHealth != null)
        {
            myHealth.SetInvulnerable(value);
        }
    }

    protected override void Update()
    {
        if (isDead || (myHealth != null && myHealth.GetCurrentHealth() <= 0)) return;

        base.Update();
    }


    public void PlayMeleeHitEffects()
    {
        if (elementConfig == null) return;

        GameObject vfxPrefab = null;

        if (weaponConfig.weaponVisualIndex == 0)
        {
            vfxPrefab = elementConfig.vfx_Sword1;
        }
        else
        {
            vfxPrefab = elementConfig.vfx_Sword2; 
        }

        if (vfxPrefab != null)
        {
            GameObject vfx = Instantiate(vfxPrefab, CurrentShootPoint.position, Quaternion.identity);

            Destroy(vfx, 2f); 
        }
    }

    public override IEnemyState CreateIdleState()
    {
        return new BossIdleState(this);
    }

    public override IEnemyState CreateChaseState()
    {
        return new BossChaseState(this);
    }

    public override IEnemyState CreateAttackState()
    {
        return CreateLightAttackState();
    }

    public IEnemyState CreateLightAttackState()
    {
        return new BossLightAttackState(this);
    }

    public IEnemyState CreateHeavyAttackState()
    {
        return new BossHeavyAttackState(this);
    }

    public IEnemyState CreateRangedState()
    {
        return new BossRangedState(this);
    }

    public IEnemyState CreateStunState()
    {
        return new BossStunState(this);
    }

    public IEnemyState CreateTauntState()
    {
        return new BossTauntState(this);
    }

}
