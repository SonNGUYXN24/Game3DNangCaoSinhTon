using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class MetalAxe : MonoBehaviour
{
    [Header("Axe Settings")]
    public Vector3 swingRotation = new Vector3(-90f, 0f, 0f); // góc bổ
    public float swingDuration = 0.1f;

    [Header("Inventory")]
    public Inventory inventory;
    public ItemData.ItemType woodItemType;

    [Header("UI Popup")]
    public GameObject popupUI;
    public Image popupIcon;
    public TMP_Text popupText;
    public Sprite woodIcon;
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
            StartCoroutine(SwingAxe());
        }
    }

    private IEnumerator SwingAxe()
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
        if (other.CompareTag("Tree") && other.gameObject.layer == LayerMask.NameToLayer("Tree"))
        {
            int amount = Random.Range(1, 11);
            inventory.AddItem(woodItemType, amount);
            ShowPopup(amount);

            TreeHitCounter treeHit = other.GetComponent<TreeHitCounter>();
            if (treeHit == null)
                treeHit = other.gameObject.AddComponent<TreeHitCounter>();

            treeHit.Hit();

            if (treeHit.hitCount >= 7)
                Destroy(other.gameObject);

            Debug.Log("Bổ trúng cây! +" + amount + " Wood");
        }
    }

    private void ShowPopup(int amount)
    {
        popupUI.SetActive(true);

        popupIcon.sprite = woodIcon;
        popupText.text = "+" + amount;

        popupGroup.alpha = 1f;
        popupGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            popupUI.SetActive(false);
            popupGroup.alpha = 1f;
        });
    }
}
