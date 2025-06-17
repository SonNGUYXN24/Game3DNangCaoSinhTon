using UnityEngine;

public class BearSpawner : MonoBehaviour
{
    public GameObject bearPrefab; // Prefab con gấu
    public int numberOfBears = 5; // Số lượng gấu muốn sinh
    public float spawnRange = 20f; // Phạm vi sinh

    void Start()
    {
        for (int i = 0; i < numberOfBears; i++)
        {
            SpawnBear();
        }
    }

    void SpawnBear()
    {
        // Vị trí ngẫu nhiên quanh vị trí của Spawner
        Vector3 randomPos = new Vector3(
            transform.position.x + Random.Range(-spawnRange, spawnRange),
            transform.position.y,
            transform.position.z + Random.Range(-spawnRange, spawnRange)
        );

        Instantiate(bearPrefab, randomPos, Quaternion.identity);
    }
}
