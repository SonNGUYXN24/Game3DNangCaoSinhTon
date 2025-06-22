using UnityEngine;
using UnityEngine.UI;

public class SaveTrigger : MonoBehaviour
{
    public GameObject promptUI;
    private bool isPlayerInRange = false;

    private void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TrySave();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    private void TrySave()
    {
        float hour = GameManager.Instance.gameTime.currentTimeOfDay * 24f;

        // Thời gian hợp lệ: từ 19h00 → 2h00
        if (hour >= 19f || hour <= 2f)
        {
            GameManager.Instance.SaveProgress();
        }
        else
        {
            Debug.Log("⛔ Không thể ngủ ngoài giờ từ 19:00 đến 02:00!");
        }
    }
}
