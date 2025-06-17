using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MusicController : MonoBehaviour
{
    [Header("Danh sách nhạc")]
    public List<GameObject> musicObjects;

    [Header("Text hiển thị tên nhạc")]
    public TextMeshProUGUI musicNameText;

    [Header("Tốc độ scroll (giây cho mỗi chiều)")]
    public float scrollDuration = 5f;

    [Header("Điểm bắt đầu và kết thúc (UI)")]
    public RectTransform startPoint;
    public RectTransform endPoint;

    private RectTransform textRect;
    private int currentIndex = 0;
    private Tween scrollTween;

    void Start()
    {
        textRect = musicNameText.GetComponent<RectTransform>();

        // Dừng toàn bộ nhạc ban đầu
        foreach (var obj in musicObjects)
            obj.GetComponent<AudioSource>().Stop();

        PlayMusic(currentIndex);
    }

    void Update()
    {
        AudioSource currentAudio = musicObjects[currentIndex].GetComponent<AudioSource>();
        if (!currentAudio.isPlaying && currentAudio.time > 0.1f)
        {
            NextSong();
        }
    }

    public void NextSong()
    {
        musicObjects[currentIndex].GetComponent<AudioSource>().Stop();
        currentIndex = (currentIndex + 1) % musicObjects.Count;
        PlayMusic(currentIndex);
    }

    public void PreviousSong()
    {
        musicObjects[currentIndex].GetComponent<AudioSource>().Stop();
        currentIndex = (currentIndex - 1 + musicObjects.Count) % musicObjects.Count;
        PlayMusic(currentIndex);
    }

    void PlayMusic(int index)
    {
        // Dừng toàn bộ nhạc
        foreach (var obj in musicObjects)
            obj.GetComponent<AudioSource>().Stop();

        // Phát bài mới
        musicObjects[index].GetComponent<AudioSource>().Play();

        // Đặt tên bài hát
        musicNameText.text = musicObjects[index].name;

        // Force layout để cập nhật lại kích thước
        Canvas.ForceUpdateCanvases();

        // Reset vị trí và tween
        if (scrollTween != null && scrollTween.IsActive())
            scrollTween.Kill();

        StartScrollAnimation();
    }

    void StartScrollAnimation()
    {
        // Đặt chữ tại điểm Start
        textRect.anchoredPosition = startPoint.anchoredPosition;

        // Tạo tween lặp vô hạn từ Start -> End và ngược lại
        scrollTween = textRect.DOAnchorPos(endPoint.anchoredPosition, scrollDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
