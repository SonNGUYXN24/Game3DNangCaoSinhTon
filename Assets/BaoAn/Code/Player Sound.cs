using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepSound;
    public AudioClip jumpSound;

    public void PlayFootstep()
    {
        if (footstepSound != null && audioSource != null)
            audioSource.PlayOneShot(footstepSound);
    }

    public void PlayJump()
    {
        if (jumpSound != null && audioSource != null)
            audioSource.PlayOneShot(jumpSound);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayJump(); 
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayFootstep(); 
        }
    }

}
