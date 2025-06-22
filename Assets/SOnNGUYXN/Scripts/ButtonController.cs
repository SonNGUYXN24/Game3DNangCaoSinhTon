using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private const string sceneName = "Son";

    // Gọi khi ấn nút "Play" (reset dữ liệu)
    public void OnPlayButton()
    {
        Debug.Log("🆕 Play - Resetting save data...");
        PlayerPrefs.DeleteAll(); // Xoá toàn bộ dữ liệu đã lưu
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneName);
    }

    // Gọi khi ấn nút "Continue" (load lại)
    public void OnContinueButton()
    {
        if (PlayerPrefs.HasKey("SavedDay"))
        {
            Debug.Log("▶ Continue - Loading from saved day: " + PlayerPrefs.GetInt("SavedDay"));
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("❌ Không có dữ liệu lưu! Vui lòng chơi mới.");
            // Có thể hiện thông báo trong UI tại đây nếu muốn
        }
    }
}
