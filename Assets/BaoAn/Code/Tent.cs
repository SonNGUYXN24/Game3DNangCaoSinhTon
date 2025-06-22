using UnityEngine;
using UnityEngine.UI;

public class SaveTrigger : MonoBehaviour
{
    public GameObject promptUI;  // K�o UI "Nh?n E ?? l?u game" v�o ?�y
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
        Debug.Log("Game ?� ???c l?u!");
        // Th�m code l?u game ? ?�y, v� d? PlayerPrefs ho?c file
    }
}
