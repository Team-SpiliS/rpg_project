using UnityEngine;

public class BossEnemy : EnemyBase
{
    [Header("Boss Settings")]
    public int heavyDamage = 40;
    public int magicDamage = 25;
    public float stunDamageThreshold = 50f;

    [Header("References")]
    public GameObject projectilePrefab;
    public Transform shootPoint;

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
}