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
    private float _stunResetTime = 3f;

    protected override void Awake()
    {
        StateMachine = new BossStateMachine(this);

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        if (myHealth != null)
            myHealth.OnTakeDamage += RegisterDamage;

        StateMachine.Initialize(new BossIdleState(this));
    }

    private void OnDestroy()
    {
        if (myHealth != null)
            myHealth.OnTakeDamage -= RegisterDamage;
    }

    public void RegisterDamage(int amount)
    {
        wasHitByPlayer = true;

        if (Time.time > _lastDamageTime + _stunResetTime)
            damageTakenRecently = 0;

        damageTakenRecently += amount;
        _lastDamageTime = Time.time;
    }

    public void ResetStunMeter()
    {
        damageTakenRecently = 0;
    }

    protected override void Update()
    {
        if (isDead || (myHealth != null && myHealth.GetCurrentHealth() <= 0)) return;

        base.Update();
    }

    public override AbstractEnemyState CreateAttackState()
    {
        return (AbstractEnemyState)StateMachine.CreateAttackState();
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
}