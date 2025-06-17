using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public GameTime gameTime;
    public GameObject rainObject;

    private int lastCheckedDay = -1;
    private int lastRainDay = -2;
    private bool isRaining = false;
    private float rainTimer = 0f;
    public float rainDuration = 1800f; // 30 phút game = 1800s game time

    void Start()
    {
        // Luôn tắt hiệu ứng mưa khi bắt đầu game
        rainObject.SetActive(false);
    }

    void Update()
    {
        int currentDay = gameTime.currentDay;

        if (!isRaining)
        {
            // Kiểm tra 1 lần duy nhất mỗi ngày
            if (currentDay != lastCheckedDay)
            {
                lastCheckedDay = currentDay;

                // Không mưa 2 ngày liên tiếp
                if (currentDay - lastRainDay >= 2)
                {
                    // Tỉ lệ 10% mưa mỗi ngày (có thể thay đổi)
                    if (Random.value < 0.1f)
                    {
                        StartRain();
                        lastRainDay = currentDay;
                    }
                }
            }
        }
        else
        {
            // Đang mưa → đếm thời gian
            rainTimer += Time.deltaTime * gameTime.timeMultiplier;

            if (rainTimer >= rainDuration)
            {
                StopRain();
            }
        }
    }

    void StartRain()
    {
        isRaining = true;
        rainTimer = 0f;
        rainObject.SetActive(true);
    }

    void StopRain()
    {
        isRaining = false;
        rainObject.SetActive(false);
    }
}
