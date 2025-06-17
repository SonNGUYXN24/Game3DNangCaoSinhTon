using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootstep : MonoBehaviour
{
    public AudioSource footstepAudioSource;
    public AudioClip footstepClip;
    public float footstepInterval = 0.5f; 

    private CharacterController controller;
    private float footstepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        footstepAudioSource.clip = footstepClip;
        footstepAudioSource.loop = false; 
    }

    void Update()
    {
        
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                footstepAudioSource.Play();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; 
        }
    }
}