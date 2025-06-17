using UnityEngine;
using UnityEngine.UI;

public class OxygenSensorTrigger : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float oxygenDepletionRate = 10f;


    [Header("Sensor Reference")]
    public WaterSensor sensor; // Script phụ gắn lên thanh sensor

    void Start()
    {
        currentOxygen = maxOxygen;
        
    }

    void Update()
    {
        if (sensor == null) return;

        if (sensor.isTouchingWater)
        {
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;
            currentOxygen = Mathf.Max(currentOxygen, 0f);
        }
        else
        {
            // Hồi oxy từ từ khi lên bờ
            currentOxygen += (oxygenDepletionRate / 2f) * Time.deltaTime;
            currentOxygen = Mathf.Min(currentOxygen, maxOxygen);
        }

        if (currentOxygen <= 0f)
        {
            Debug.Log("Hết oxy!");
            // TODO: mất máu hoặc chết
        }
        Debug.Log(sensor.isTouchingWater);
    }



}
