using UnityEngine;
using UnityEngine.InputSystem;

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

    // Ввод
    private PlayerControls controls;
    private Vector2 moveInput;

    void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (Camera.main != null) mainCamera = Camera.main.transform;
    }

    void Update()
    {
        Move();
        ApplyGravity();
    }

    private void Move()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        float currentSpeed = 0f;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            currentSpeed = walkSpeed;

            if (Keyboard.current != null && Keyboard.current.shiftKey.isPressed)
            {
                currentSpeed = runSpeed;
            }

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
}