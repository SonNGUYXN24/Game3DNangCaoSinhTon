using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Weapon References")]
    public GameObject pickaxe;
    public GameObject metalAxe;
    public GameObject bow;

    [Header("Hand Positions")]
    public Transform hand;      // Đã chứa Pickaxe & MetalAxe
    public Transform handBow;   // Đã chứa Bow

    [Header("Camera Reference")]
    public Transform cameraTransform;

    private int currentWeaponIndex = 0;

    void Start()
    {
        DisableAllWeapons();

        // Gắn camera làm cha để luôn theo góc nhìn (chỉ cần 1 lần)
        if (hand && hand.parent != cameraTransform)
            hand.SetParent(cameraTransform, true);

        if (handBow && handBow.parent != cameraTransform)
            handBow.SetParent(cameraTransform, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ToggleWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ToggleWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ToggleWeapon(3);
    }

    private void ToggleWeapon(int weaponIndex)
    {
        if (currentWeaponIndex == weaponIndex)
        {
            DisableAllWeapons();
            currentWeaponIndex = 0;
            return;
        }

        DisableAllWeapons();
        currentWeaponIndex = weaponIndex;

        // ❌ KHÔNG cần SetParent nữa vì đã gắn sẵn trong hierarchy
        switch (weaponIndex)
        {
            case 1:
                if (pickaxe) pickaxe.SetActive(true);
                break;
            case 2:
                if (metalAxe) metalAxe.SetActive(true);
                break;
            case 3:
                if (bow) bow.SetActive(true);
                break;
        }
    }

    private void DisableAllWeapons()
    {
        if (pickaxe) pickaxe.SetActive(false);
        if (metalAxe) metalAxe.SetActive(false);
        if (bow) bow.SetActive(false);
    }
}
