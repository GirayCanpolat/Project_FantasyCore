using UnityEngine;
using UnityEngine.InputSystem; // For new input system.

public class PlayerMovement : MonoBehaviour
{
    //Speed, jump force.
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float gravity = -9.81f;

    //For Camera reference, directions.
    [SerializeField] private Transform cam;

    //Other variables.
    private CharacterController controller;     //Unity reference.
    private PlayerControls controls;            //Input Actions class.
    private Vector2 moveInput;                  //WASD inputs.
    private Vector3 playerVelocity;             //Movement + gravity.
    private bool isGrounded;                    //Ground check for player jump.


    private void Awake()
    {
        controls = new PlayerControls();     //For creating the Input Actions class.
    }

    private void OnEnable()
    {
        controls.Enable();  //Active

        // Move (Vector2)
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Jump (Button)
        controls.Player.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        controls.Disable(); //Disable
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>(); //Take Unity component
    }

    private void Update()
    {
        // Karakter yerde mi?
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f; // yere sabitlenmiş hissi

        // Girdi yönünü oluştur
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Kamera yönüne göre hareketi döndür
        move = Quaternion.Euler(0, cam.eulerAngles.y, 0) * move;

        // Hareketi uygula
        controller.Move(move * Time.deltaTime * moveSpeed);

        // Yerçekimi uygula
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    
    private void Jump()
    {
        if (isGrounded)
            playerVelocity.y = jumpForce;
    }
        
}
