using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Player bị trúng đòn! Máu còn: " + health);
        if (health <= 0)
        {
            Debug.Log("Player đã chết!");
        }
    }
}
