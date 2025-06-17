using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;              
    public Transform playerBody;           
    public CharacterController controller; 

    [Header("Animation Settings")]
    public float smoothSpeed = 5f;

    private float currentSpeed = 0f;
    private bool wasGrounded = true;

    void Start()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!controller && playerBody != null)
            controller = playerBody.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!controller || !animator) return;

        
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;
        float speed = horizontalVelocity.magnitude;

        
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * smoothSpeed);
        animator.SetFloat("Speed", currentSpeed);

        
        bool isGrounded = controller.isGrounded;

       
        if (!isGrounded && wasGrounded)
        {
            animator.SetTrigger("Jump");
        }

        wasGrounded = isGrounded;
    }
}
