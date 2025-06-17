using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FallScreamCharacterController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip screamClip;
    public float fallHeightThreshold = 3f;

    private CharacterController controller;
    private float fallStartY;
    private bool isFalling = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // N?u nhân v?t ?ang r?i (không ch?m ??t và t?c ?? Y âm)
        if (!controller.isGrounded && !isFalling && controller.velocity.y < -0.1f)
        {
            isFalling = true;
            fallStartY = transform.position.y;
        }

        // N?u v?a ch?m ??t sau khi r?i
        if (controller.isGrounded && isFalling)
        {
            float fallDistance = fallStartY - transform.position.y;

            if (fallDistance >= fallHeightThreshold)
            {
                audioSource.PlayOneShot(screamClip);
                // Debug.Log("La lên vì r?i t? cao " + fallDistance + "m");
            }

            isFalling = false;
        }
    }
}
