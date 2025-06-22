using UnityEngine;

public class TreeHitCounter : MonoBehaviour
{
    public int hitCount = 0;

    public void Hit()
    {
        hitCount++;
    }
}
