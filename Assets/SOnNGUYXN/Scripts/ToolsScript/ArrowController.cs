using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public bool isFireArrow = false;
    public GameObject flameVFX;
    private bool hasHit = false;

    private void Start()
    {
        if (flameVFX != null) flameVFX.SetActive(false);
    }

    private void Update()
    {
        // Nếu người chơi nhấn E => kích hoạt FireArrow
        if (Input.GetKeyDown(KeyCode.E) && !isFireArrow)
        {
            ActivateFireArrow();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemies"))
        {
            hasHit = true;

            int damage = isFireArrow ? 50 : 20;
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject, 5f);
        }
    }

    public void ActivateFireArrow()
    {
        isFireArrow = true;
        if (flameVFX != null) flameVFX.SetActive(true);
    }
}
