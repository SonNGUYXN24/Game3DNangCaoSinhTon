using UnityEngine;

public class SimpleSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip soundClip;

    void Start()
    {
        audioSource.PlayOneShot(soundClip);
    }
}
