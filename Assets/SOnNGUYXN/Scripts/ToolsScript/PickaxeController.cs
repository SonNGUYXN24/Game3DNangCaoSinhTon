using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class PickaxeController : MonoBehaviour
{
    [Header("Pickaxe Settings")]
    public Vector3 swingRotation = new Vector3(-90f, 0f, 0f); // Góc xoay khi bổ
    public float swingDuration = 0.1f;

    [Header("Inventory")]
    public Inventory inventory;
    public ItemData.ItemType rockItemType;

    [Header("UI Popup")]
    public GameObject popupUI; // UI có sẵn trên Canvas
    public Image popupIcon;
    public TMP_Text popupText;
    public Sprite rockIcon;
    public CanvasGroup popupGroup;


    private Quaternion originalRotation;
    private bool isSwinging = false;

    private void Start()
    {
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(SwingPickaxe());
        }
    }

    private IEnumerator SwingPickaxe()
    {
        isSwinging = true;

        Quaternion targetRotation = Quaternion.Euler(swingRotation);
        float elapsed = 0f;

        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(originalRotation, targetRotation, elapsed / swingDuration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(targetRotation, originalRotation, elapsed / swingDuration);
            yield return null;
        }

        isSwinging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocks") && other.gameObject.layer == LayerMask.NameToLayer("Rocks"))
        {
            int amount = Random.Range(1, 11); // Ngẫu nhiên 1 - 10
            inventory.AddItem(rockItemType, amount);
            ShowPopup(amount);

            RockHitCounter rockHit = other.GetComponent<RockHitCounter>();
            if (rockHit == null)
                rockHit = other.gameObject.AddComponent<RockHitCounter>();

            rockHit.Hit();

            if (rockHit.hitCount >= 7)
                Destroy(other.gameObject);

            Debug.Log("Bổ trúng đá! +" + amount + " Rock");
        }
    }

    private void ShowPopup(int amount)
    {
        popupUI.SetActive(true);

        popupIcon.sprite = rockIcon;
        popupText.text = "+" + amount;

        popupGroup.alpha = 1f;
        popupGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            popupUI.SetActive(false);
            popupGroup.alpha = 1f; // reset lại alpha
        });
    }

}
