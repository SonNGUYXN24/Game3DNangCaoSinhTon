using UnityEngine;

public class RockHitCounter : MonoBehaviour
{
    public int hitCount = 0;

    public void Hit()
    {
        hitCount++;
    }
}
