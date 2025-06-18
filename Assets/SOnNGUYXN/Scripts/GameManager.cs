using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameTime gameTime; // Kéo GameTime vào đây trong Inspector

    private int lastSavedDay = 0;

    void Start()
    {
        if (gameTime == null)
        {
            Debug.LogError("GameTime chưa được gán trong GameManager!");
        }

        lastSavedDay = gameTime.currentDay;
    }

    void Update()
    {
        int currentDay = gameTime.currentDay;

        if (currentDay > lastSavedDay)
        {
            SaveGame(currentDay - 1); // Lưu lại ngày vừa kết thúc
            lastSavedDay = currentDay;
        }
    }

    void SaveGame(int dayToSave)
    {
        // Ví dụ lưu file đơn giản
        string saveData = $"Game saved at end of Day {dayToSave} - {System.DateTime.Now}";
        string filePath = Application.persistentDataPath + $"/Save_Day{dayToSave}.txt";

        try
        {
            File.WriteAllText(filePath, saveData);
            Debug.Log($"✅ Game đã được lưu: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Lỗi khi lưu game: {e.Message}");
        }
    }
}
