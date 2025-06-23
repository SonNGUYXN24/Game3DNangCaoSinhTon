using UnityEngine;

public class GravityHandler : MonoBehaviour
{
    public float gravity = -9.81f;
    private CharacterController controller;
    private float verticalVelocity;

    
void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // giữ dính đất
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
        controller.Move(gravityMove * Time.deltaTime);
    }
}