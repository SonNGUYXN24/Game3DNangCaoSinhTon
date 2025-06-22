using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Time Reference")]
    public GameTime gameTime;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // nếu bạn muốn giữ qua scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveProgress()
    {
        StartCoroutine(SaveGameRoutine());
    }

    private IEnumerator SaveGameRoutine()
    {
        // Fade màn hình đen
        if (fadeCanvas != null)
        {
            yield return FadeIn();
        }

        // Chuyển thời gian thành 9h sáng ngày hôm sau
        float nineAMPercent = 9f / 24f;
        gameTime.currentTimeOfDay = nineAMPercent;
        gameTime.SetTimeTo(nineAMPercent); // cần thêm hàm SetTimeTo trong GameTime
        Debug.Log($"💾 Game saved at Day {gameTime.currentDay} - 09:00");

        PlayerPrefs.SetInt("SavedDay", gameTime.currentDay);
        PlayerPrefs.Save();

        yield return new WaitForSeconds(1f);

        if (fadeCanvas != null)
        {
            yield return FadeOut();
        }
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = 0;
    }
}
