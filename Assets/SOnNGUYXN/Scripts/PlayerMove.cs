using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

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
    [SerializeField] private PlayerAnimatorController playerAnimatorController;
    [SerializeField] public StartusPlayer playerStats; // Thêm tham chiếu tới hệ thống chỉ số

    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    private float cameraPitch = 0f;

    [Header("Tilt & Bob Settings")]
    public float tiltFrequency = 5f, tiltAmplitude = 3f;
    public float bobFrequency = 12f, bobAmplitude = 0.05f;
    private float tiltTimer = 0f, bobTimer = 0f;
    private Vector3 originalCamLocalPos;

    private PlayerInput playerInput;
    private InputAction moveAction, jumpAction, sprintAction, lookAction;

    private float staminaDrainTimer = 0f;
    private float staminaRegenTimer = 0f;
    private float healthRegenTimer = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        lookAction = playerInput.actions["Look"];
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalCamLocalPos = cameraTransform.localPosition;

        if (!playerStats) Debug.LogError("PlayerStats (StartusPlayer) not assigned!");
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        ApplyCameraTiltAndBob();
        HandleStamina();
        HandleHealthRegen();
    }

    void HandleMovement()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        float x = inputVector.x, z = inputVector.y;

        float currentSpeed = moveSpeed;
        bool isSprinting = IsSprinting() && playerStats.CurrentStamina > 20;

        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
            staminaDrainTimer += Time.deltaTime;
            if (staminaDrainTimer >= 0.1f)
            {
                playerStats.ChangeStamina(-1);
                staminaDrainTimer = 0f;
            }
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if (IsGrounded() && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (jumpAction.triggered && IsGrounded() && playerStats.CurrentStamina >= 20)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            playerStats.ChangeStamina(-20);
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 velocity = Vector3.up * verticalVelocity;
        controller.Move((move * currentSpeed + velocity) * Time.deltaTime);
    }

    void HandleStamina()
    {
        // Hồi stamina nếu không chạy hoặc đang đứng yên
        if (!IsSprinting())
        {
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer >= 0.1f)
            {
                playerStats.ChangeStamina(+1);
                staminaRegenTimer = 0f;
            }
        }
    }

    void HandleHealthRegen()
    {
        if (playerStats.CurrentHP < playerStats.MaxHP)
        {
            healthRegenTimer += Time.deltaTime;
            if (healthRegenTimer >= 5f)
            {
                playerStats.ChangeHP(+1);
                healthRegenTimer = 0f;
            }
        }
    }

    void HandleMouseLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * mouseSensitivity;
        float mouseX = lookInput.x, mouseY = lookInput.y;

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
            cameraTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(tiltTimer) * tiltAmplitude);

            if (IsSprinting())
            {
                bobTimer += Time.deltaTime * bobFrequency;
                cameraTransform.localPosition = originalCamLocalPos + new Vector3(0f, Mathf.Sin(bobTimer) * bobAmplitude, 0f);
            }
        }
        else
        {
            cameraTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cameraTransform.localPosition = originalCamLocalPos;
            bobTimer = 0f;
        }
    }

    public bool IsGrounded() => controller.isGrounded;

    public bool IsSprinting() => sprintAction.ReadValue<float>() > 0f;

    internal bool IsMoving()
    {
        throw new NotImplementedException();
    }
}
