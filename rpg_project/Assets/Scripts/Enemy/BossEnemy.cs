using UnityEngine;

public class BossEnemy : EnemyBase
{
    [Header("Баланс Босса")]
    public int heavyDamage = 40;
    public int magicDamage = 25;
    public float stunDamageThreshold = 50f;

    [Header("Ссылки для магии")]
    public GameObject projectilePrefab;
    public Transform shootPoint;

    [HideInInspector] public bool isPhaseTwo = false;
    [HideInInspector] public bool isInvulnerable = false;

    private float _damageTakenRecently = 0;
    private float _lastDamageTime = 0;
    private float _stunResetTime = 3f;

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(new EnemyIdleState(this));

        if (myHealth != null)
            myHealth.OnTakeDamage += HandleBossDamage;
    }

    private void HandleBossDamage(int amount)
    {
        if (myHealth.GetCurrentHealth() <= 0) return;

        if (StateMachine.CurrentState is EnemyChaseState && StateMachine.CurrentState is not BossChaseState)
        {
            StateMachine.ChangeState(new BossChaseState(this));
        }

        base.Update();

        wasHitByPlayer = true; 

        if (!isPhaseTwo && myHealth.GetCurrentHealth() <= myHealth.GetMaxHealth() * 0.5f)
        {
            isPhaseTwo = true;
            _damageTakenRecently = 0; 
            StateMachine.ChangeState(new BossTauntState(this));
            return;
        }

        if (StateMachine.CurrentState is BossStunState || StateMachine.CurrentState is BossTauntState) return;

        if (Time.time > _lastDamageTime + _stunResetTime) _damageTakenRecently = 0;
        _damageTakenRecently += amount;
        _lastDamageTime = Time.time;

        if (_damageTakenRecently >= stunDamageThreshold)
        {
            _damageTakenRecently = 0;
            StateMachine.ChangeState(new BossStunState(this));
        }
    }

    protected override void Update()
    {
        if (isDead || myHealth.GetCurrentHealth() <= 0) return;

        base.Update();
    }

    public override AbstractEnemyState CreateAttackState()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange + 1.5f) return new BossRangedState(this);
        return (Random.value > 0.6f) ? new BossHeavyAttackState(this) : new BossLightAttackState(this);
    }
}