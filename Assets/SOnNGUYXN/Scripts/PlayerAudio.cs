using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioSource runSource;
    [SerializeField] private AudioSource jumpSource;

    [SerializeField] private PlayerMovement movement;

    private bool wasGroundedLastFrame = true;

    void Update()
    {
        HandleFootsteps();
        HandleJumpSound();
    }

    void HandleFootsteps()
    {
        bool isGrounded = movement.IsGrounded();
        bool isMoving = movement.IsMoving();
        bool isSprinting = movement.IsSprinting();

        if (isGrounded && isMoving)
        {
            if (isSprinting)
            {
                if (!runSource.isPlaying)
                {
                    walkSource.Stop();
                    runSource.loop = true;
                    runSource.Play();
                }
            }
            else
            {
                if (!walkSource.isPlaying)
                {
                    runSource.Stop();
                    walkSource.loop = true;
                    walkSource.Play();
                }
            }
        }
        else
        {
            if (walkSource.isPlaying) walkSource.Stop();
            if (runSource.isPlaying) runSource.Stop();
        }
    }

    void HandleJumpSound()
    {
        bool isGrounded = movement.IsGrounded();

        if (!isGrounded && wasGroundedLastFrame)
        {
            jumpSource.PlayOneShot(jumpSource.clip);
        }

        wasGroundedLastFrame = isGrounded;
    }
}
