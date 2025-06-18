using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class LoadSave : MonoBehaviour
{
    private string saveFolder;

    void Start()
    {
        saveFolder = Application.persistentDataPath;
    }

    public void ButtonContinue()
    {
        int latestDay = GetLatestSavedDay();

        if (latestDay > 0)
        {
            PlayerPrefs.SetInt("SavedDay", latestDay); // Truyền dữ liệu qua scene khác
            SceneManager.LoadScene("Son");
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy file save nào!");
        }
    }

    private int GetLatestSavedDay()
    {
        if (!Directory.Exists(saveFolder))
            return 0;

        string[] files = Directory.GetFiles(saveFolder, "Save_Day*.txt");

        if (files.Length == 0)
            return 0;

        int maxDay = 0;

        foreach (var file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file); // "Save_Day3"
            string[] parts = fileName.Split('_');

            if (parts.Length >= 2 && parts[1].StartsWith("Day"))
            {
                string dayPart = parts[1].Replace("Day", "");
                if (int.TryParse(dayPart, out int dayNumber))
                {
                    maxDay = Mathf.Max(maxDay, dayNumber);
                }
            }
        }

        return maxDay;
    }
}
