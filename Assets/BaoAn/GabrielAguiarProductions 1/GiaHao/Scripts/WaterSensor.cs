using UnityEngine;

public class WaterSensor : MonoBehaviour
{
    [HideInInspector]
    public bool isTouchingWater = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UnderWater")) // hoặc check layer
        {
            isTouchingWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UnderWater"))
        {
            isTouchingWater = false;
        }
    }
}
