using UnityEngine;
using TMPro;

public class BowController : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float maxHoldTime = 3f;
    public float maxShootForce = 50f;
    public float minShootForce = 5f;

    [Header("Inventory")]
    public Inventory inventory;
    public ItemData.ItemType arrowItemType = ItemData.ItemType.Arrow;
    public TMP_Text arrowCountText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip bowShootClip;

    private float holdTimer = 0f;
    private bool isHolding = false;
    private GameObject currentArrow;

    void Start()
    {
        UpdateArrowUI();
    }

    void Update()
    {
        // Nếu không còn mũi tên thì không làm gì
        if (inventory.GetItemQuantity(arrowItemType) <= 0)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // Bắt đầu giữ chuột → tạo mũi tên, đặt ở vị trí gốc
            isHolding = true;
            holdTimer = 0f;

            currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            currentArrow.transform.SetParent(arrowSpawnPoint);
            currentArrow.transform.localPosition = Vector3.zero;
            currentArrow.transform.localRotation = Quaternion.identity;
        }

        if (Input.GetMouseButton(0) && isHolding)
        {
            holdTimer += Time.deltaTime;
            holdTimer = Mathf.Clamp(holdTimer, 0, maxHoldTime);

            // Kéo mũi tên lùi về phía sau nhẹ (hiệu ứng)
            float t = holdTimer / maxHoldTime;
            currentArrow.transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, -0.5f), t);
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            // Bắn mũi tên
            float powerRatio = holdTimer / maxHoldTime;
            float force = Mathf.Lerp(minShootForce, maxShootForce, powerRatio);

            FireArrow(force);

            // Cập nhật Inventory
            inventory.RemoveItem(arrowItemType, 1);
            UpdateArrowUI();

            // Âm thanh
            if (bowShootClip) audioSource.PlayOneShot(bowShootClip);

            // Reset
            isHolding = false;
            holdTimer = 0f;
        }
    }

    private void FireArrow(float force)
    {
        currentArrow.transform.SetParent(null); // tách khỏi Bow
        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = currentArrow.transform.forward * force;
        }
        else
        {
            Debug.LogWarning("Arrow prefab is missing Rigidbody!");
        }

        currentArrow = null;
    }

    private void UpdateArrowUI()
    {
        int count = inventory.GetItemQuantity(arrowItemType);
        if (arrowCountText != null)
        {
            arrowCountText.text = "Arrows: " + count;
        }
    }
}
