using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class PickaxeController : MonoBehaviour
{
    [Header("Pickaxe Settings")]
    public Vector3 swingRotation = new Vector3(-90f, 0f, 0f);
    public float swingDuration = 0.1f;

    [Header("Inventory")]
    public Inventory inventory;
    public ItemData.ItemType rockItemType;

    [Header("UI Popup")]
    public GameObject popupUI;
    public Image popupIcon;
    public TMP_Text popupText;
    public Sprite rockIcon;
    public CanvasGroup popupGroup;

    [Header("Raycast Reference")]
    public Raycast raycastScript;

    [Header("Collider")]
    public Collider pickaxeCollider; // ← Gắn collider của pickaxe vào đây

    private Quaternion originalRotation;
    private bool isSwinging = false;

    private void Start()
    {
        originalRotation = transform.localRotation;
        if (pickaxeCollider) pickaxeCollider.enabled = false;
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
        if (pickaxeCollider) pickaxeCollider.enabled = true;

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

        if (pickaxeCollider) pickaxeCollider.enabled = false;
        isSwinging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocks"))
        {
            int amount = Random.Range(1, 11);
            inventory.AddItem(rockItemType, amount);
            ShowPopup(amount);

            if (raycastScript) raycastScript.UpdateRockUI();

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
            popupGroup.alpha = 1f;
        });
    }
}
