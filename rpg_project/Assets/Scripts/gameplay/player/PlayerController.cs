using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки скорости")]
    public float walkSpeed = 3f;
    public float runSpeed = 7f;

    [Header("Настройки поворота")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [Header("Гравитация")]
    public float gravity = -9.81f;
    private Vector3 velocity;

    private CharacterController controller;
    private Transform mainCamera;
    private Animator animator;
    private PlayerCombat combat;

    private IPlayerInputService input;
    private Vector2 moveInput;

    private bool isDead = false;

    void Awake()
    {
        input = ServiceLocator.Get<IPlayerInputService>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        combat = GetComponent<PlayerCombat>();

        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }

        HealthComponent health = GetComponent<HealthComponent>();
        if (health != null)
        {
            health.OnDeath += HandleDeath;
        }
    }

    void HandleDeath()
    {
        isDead = true;

        if (animator != null) animator.SetFloat("Speed", 0);
    }

    void Update()
    {
        if (isDead) return;
        Move();
        ApplyGravity();
    }

    private void Move()
    {
        if (combat != null && combat.IsBlocking)
        {
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            return;
        }

        moveInput = input.Controls.Player.Move.ReadValue<Vector2>();

        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        float currentSpeed = 0f;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                mainCamera.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir =
                Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            bool isRunning = input.Controls.Player.Run.IsPressed();

            currentSpeed = isRunning ? runSpeed : walkSpeed;

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnDestroy()
    {
        HealthComponent health = GetComponent<HealthComponent>();

        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }
    }
}