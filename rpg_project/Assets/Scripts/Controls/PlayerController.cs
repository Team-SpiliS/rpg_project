using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
   public float moveSpeed = 5f;
   public float mouseSensivity = 0.2f;
   public Transform playerCamera;

   private CharacterController controller;
   private float xRotation = 0f;

   private PlayerControls controls;
   private Vector2 moveInput;
   private Vector2 lookInput;

   void Awake()
   {
      controls = new PlayerControls(); //Инициализация управления
   }
   
   private void OnEnable() => controls.Enable();   // Включение ввода
   private void OnDisable() => controls.Disable(); // Выключение при деактивации игрока
   
   void Start()
   {
      controller = GetComponent<CharacterController>();
      Cursor.lockState = CursorLockMode.Locked; //Блокировка курсора по центру экрана и его скрытие
   }

   void Update()
   {
      moveInput = controls.Player.Move.ReadValue<Vector2>();
      lookInput = controls.Player.Look.ReadValue<Vector2>();
      
      float mouseX = Input.GetAxis("MouseX") * mouseSensivity;
      float mouseY = Input.GetAxis("MouseY") *  mouseSensivity;
      
      xRotation -= mouseY; 
      xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Ограничение наклона камеры по вертикали
      Debug.Log(xRotation);
      
      playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //Вращение камеры по горизонтали
      transform.Rotate(Vector3.up * mouseX); //Вращение игрока вместе с камерой
      
      Vector3 move = transform.right * moveSpeed * Time.deltaTime; //Управление игроком относительно камеры
      controller.Move(move * moveSpeed * Time.deltaTime);
   }
}