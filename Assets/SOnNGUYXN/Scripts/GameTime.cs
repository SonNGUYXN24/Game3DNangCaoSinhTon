using UnityEngine;
using TMPro;

public class GameTime : MonoBehaviour
{
    public float secondsInFullDay = 3600f; // 2 giờ ngoài đời = 1 ngày game
    [Range(0, 1)] public float currentTimeOfDay;
    public float timeMultiplier = 1f;
    public Light directionalLight;
    public Gradient lightColor; // Dùng chỉnh màu sáng theo thời gian
    public AnimationCurve lightIntensity;
    public TextMeshProUGUI timeText;

    private float time; // tổng số giây trong game từ khi bắt đầu
    public int currentDay;

    private float startHour = 7f; // 7 giờ sáng

    void Start()
    {
        // Tính thời gian bắt đầu tương ứng với 7h sáng
        float startPercent = startHour / 24f;
        time = startPercent * secondsInFullDay;
    }

    void Update()
    {
        time += Time.deltaTime * timeMultiplier;
        currentTimeOfDay = (time % secondsInFullDay) / secondsInFullDay;
        currentDay = Mathf.FloorToInt(time / secondsInFullDay) + 1;

        UpdateLighting(currentTimeOfDay);
        UpdateClockDisplay();
    }

    void UpdateLighting(float timePercent)
    {
        // Góc mặt trời quay theo thời gian trong ngày, 0% = 90° (Đông), 50% = 270° (Tây)
        float angle = timePercent * 360f + 90f;
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(angle, 170f, 0));
        directionalLight.color = lightColor.Evaluate(timePercent);
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);
    }

    void UpdateClockDisplay()
    {
        float totalMinutes = currentTimeOfDay * 24f * 60f;
        int hours = Mathf.FloorToInt(totalMinutes / 60f);
        int minutes = Mathf.FloorToInt(totalMinutes % 60f);

        string timeStr = $"{hours:00}:{minutes:00}";
        timeText.text = $"Day {currentDay} - {timeStr}";
    }
}
