using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 5f;
    private float verticalVelocity = 0f;
    public float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform cameraTransform;

    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    private float cameraPitch = 0f;
    private float cameraRoll = 0f;

    [Header("Tilt Settings")]
    public float tiltFrequency = 5f;
    public float tiltAmplitude = 3f;
    private float tiltTimer = 0f;

    [Header("Bob Settings")]
    public float bobFrequency = 12f;
    public float bobAmplitude = 0.05f;
    private float bobTimer = 0f;
    private Vector3 originalCamLocalPos;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction lookAction;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        lookAction = playerInput.actions["Look"];
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        lookAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
        lookAction.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!cameraHolder) Debug.LogError("Camera Holder not assigned!");
        if (!cameraTransform) Debug.LogError("Camera Transform not assigned!");

        originalCamLocalPos = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        ApplyCameraTiltAndBob();
    }

    public bool IsGrounded() => controller.isGrounded;

    public bool IsMoving() => moveAction.ReadValue<Vector2>().magnitude > 0.1f;

    public bool IsSprinting() => sprintAction.ReadValue<float>() > 0f;

    void HandleMovement()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        float x = inputVector.x;
        float z = inputVector.y;

        float currentSpeed = IsSprinting() ? sprintSpeed : moveSpeed;
        Vector3 move = transform.right * x + transform.forward * z;

        if (IsGrounded() && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (jumpAction.triggered && IsGrounded())
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 velocity = Vector3.up * verticalVelocity;

        controller.Move((move * currentSpeed + velocity) * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * mouseSensitivity;
        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        cameraHolder.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void ApplyCameraTiltAndBob()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        bool isMoving = inputVector.magnitude > 0.1f;

        if (isMoving)
        {
            tiltTimer += Time.deltaTime * tiltFrequency;
            cameraRoll = Mathf.Sin(tiltTimer) * tiltAmplitude;
        }
        else
        {
            cameraRoll = Mathf.Lerp(cameraRoll, 0f, Time.deltaTime * 5f);
            tiltTimer = 0f;
        }

        Vector3 bobOffset = Vector3.zero;
        if (isMoving && IsSprinting())
        {
            bobTimer += Time.deltaTime * bobFrequency;
            bobOffset.y = Mathf.Sin(bobTimer) * bobAmplitude;
        }
        else
        {
            bobTimer = 0f;
        }

        cameraTransform.localRotation = Quaternion.Euler(0f, 0f, cameraRoll);
        cameraTransform.localPosition = originalCamLocalPos + bobOffset;
    }
}
