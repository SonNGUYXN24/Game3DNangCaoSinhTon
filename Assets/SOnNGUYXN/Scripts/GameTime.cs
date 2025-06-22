using UnityEngine;
using TMPro;

public class GameTime : MonoBehaviour
{
    public float secondsInFullDay = 3600f; // 2 giờ ngoài đời = 1 ngày game
    [Range(0, 1)] public float currentTimeOfDay;
    public float timeMultiplier = 1f;
    public Light directionalLight;
    public Gradient lightColor; // Đổi màu mặt trời theo thời gian
    public AnimationCurve lightIntensity;
    public AnimationCurve skyboxExposure; // <-- Thêm curve để điều chỉnh độ sáng Skybox
    public TextMeshProUGUI timeText;

    private float time;
    public int currentDay;
    private float startHour = 7f;

    private Material skyboxMaterial;

    void Start()
    {
        float startPercent = startHour / 24f;
        time = startPercent * secondsInFullDay;

        // Tạo bản sao Skybox để thay đổi thông số riêng không ảnh hưởng global
        if (RenderSettings.skybox != null)
        {
            skyboxMaterial = new Material(RenderSettings.skybox);
            RenderSettings.skybox = skyboxMaterial;
        }
    }

    void Update()
    {
        time += Time.deltaTime * timeMultiplier;
        currentTimeOfDay = (time % secondsInFullDay) / secondsInFullDay;
        currentDay = Mathf.FloorToInt(time / secondsInFullDay) + 1;

        UpdateLighting(currentTimeOfDay);
        UpdateClockDisplay();
    }
    public void SetTimeTo(float timePercent)
{
    time = timePercent * secondsInFullDay + (currentDay * secondsInFullDay);
}

    void UpdateLighting(float timePercent)
    {
        // Di chuyển mặt trời
        float angle = timePercent * 360f + 90f;
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(angle, 170f, 0));
        directionalLight.color = lightColor.Evaluate(timePercent);
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);

        // Điều chỉnh Skybox Exposure nếu có
        if (skyboxMaterial != null && skyboxMaterial.HasProperty("_Exposure"))
        {
            float exposure = skyboxExposure.Evaluate(timePercent);
            skyboxMaterial.SetFloat("_Exposure", exposure);
        }

        // Cập nhật ánh sáng môi trường (ambient)
        RenderSettings.ambientIntensity = lightIntensity.Evaluate(timePercent);
    }

    void UpdateClockDisplay()
    {
        float totalMinutes = currentTimeOfDay * 24f * 60f;
        int hours = Mathf.FloorToInt(totalMinutes / 60f);
        int minutes = Mathf.FloorToInt(totalMinutes % 60f);
        timeText.text = $"Day {currentDay} - {hours:00}:{minutes:00}";
    }
}
