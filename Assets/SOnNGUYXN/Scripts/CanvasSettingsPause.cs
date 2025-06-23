using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasSettingsPause : MonoBehaviour
{
    [Header("Canvas Settings")]
    public GameObject canvasSettings;

    private bool isActive = false;

    void Start()
    {
        if (canvasSettings != null)
            canvasSettings.SetActive(false);
    }

    void Update()
    {
        // Bật/tắt CanvasSettings khi nhấn ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            canvasSettings.SetActive(isActive);
        }

        if (isActive && Input.GetKeyDown(KeyCode.Return))
        {
            // ✅ Unlock & show cursor trước khi rời scene góc nhìn thứ nhất
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("MainMenu");
        }

    }
}
