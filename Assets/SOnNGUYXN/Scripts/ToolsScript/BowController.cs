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
    [Range(0f, 30f)] public float arcAngle = 10f; // góc nghiêng vòng cung khi bắn

    [Header("Inventory")]
    public Inventory inventory;
    public ItemData.ItemType arrowItemType = ItemData.ItemType.Arrow;
    public TMP_Text arrowCountText;
    public int initialArrowCount = 200;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip bowShootClip;

    private float holdTimer = 0f;
    private bool isHolding = false;
    private GameObject currentArrow;

    // Xác định đầu và đuôi mũi tên nếu cần
    private Transform arrowHead;
    private Transform arrowTail;

    void Start()
    {
        if (inventory.GetItemQuantity(arrowItemType) <= 0)
        {
            inventory.AddItem(arrowItemType, initialArrowCount);
        }

        UpdateArrowUI();
    }

    void Update()
    {
        if (inventory.GetItemQuantity(arrowItemType) <= 0)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
            holdTimer = 0f;

            // Spawn mũi tên và giữ nguyên rotation gốc của Prefab
            currentArrow = Instantiate(arrowPrefab);
            currentArrow.transform.position = arrowSpawnPoint.position;
            currentArrow.transform.rotation = arrowPrefab.transform.rotation; // rotation gốc của prefab
            currentArrow.transform.SetParent(arrowSpawnPoint, true); // giữ rotation

            currentArrow.transform.localPosition = Vector3.zero;

            // Lấy các Transform Head & Tail nếu có
            arrowHead = currentArrow.transform.Find("Head");
            arrowTail = currentArrow.transform.Find("Tail");
        }

        if (Input.GetMouseButton(0) && isHolding)
        {
            holdTimer += Time.deltaTime;
            holdTimer = Mathf.Clamp(holdTimer, 0, maxHoldTime);

            float t = holdTimer / maxHoldTime;
            // ✅ Kéo mũi tên về trục Z cục bộ của arrowSpawnPoint
            currentArrow.transform.localPosition = new Vector3(0f, 0f, -Mathf.Lerp(0f, 0.5f, t));
        }



        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            float powerRatio = holdTimer / maxHoldTime;
            float force = Mathf.Lerp(minShootForce, maxShootForce, powerRatio);

            FireArrow(force);

            inventory.RemoveItem(arrowItemType, 1);
            UpdateArrowUI();

            if (bowShootClip) audioSource.PlayOneShot(bowShootClip);

            isHolding = false;
            holdTimer = 0f;
        }
    }

    private void FireArrow(float force)
    {
        currentArrow.transform.SetParent(null);
        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // ✅ Đảm bảo mũi tên bay theo đúng chiều forward của nó
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
