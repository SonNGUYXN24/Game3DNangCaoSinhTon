using UnityEngine;
using UnityEngine.UI;

public class SaveTrigger : MonoBehaviour
{
    public GameObject promptUI;  // Kéo UI "Nh?n E ?? l?u game" vào ?ây
    private bool isPlayerInRange = false;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            SaveGame();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    void SaveGame()
    {
        Debug.Log("Game ?ã ???c l?u!");
        // Thêm code l?u game ? ?ây, ví d? PlayerPrefs ho?c file
    }
}
