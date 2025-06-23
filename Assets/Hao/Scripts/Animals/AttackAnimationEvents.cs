using UnityEngine;

public class AttackAnimationEvents : MonoBehaviour
{
    public GameObject hitboxLeftHand;
    public GameObject hitboxRightHand;
    public GameObject hitboxHead;

    public void EnableLeftHandHitbox() => hitboxLeftHand.SetActive(true);
    public void DisableLeftHandHitbox() => hitboxLeftHand.SetActive(false);

    public void EnableRightHandHitbox() => hitboxRightHand.SetActive(true);
    public void DisableRightHandHitbox() => hitboxRightHand.SetActive(false);

    public void EnableHeadHitbox() => hitboxHead.SetActive(true);
    public void DisableHeadHitbox() => hitboxHead.SetActive(false);
}
    