using UnityEngine;

public class BearPatrol : MonoBehaviour
{
    public Transform[] waypoints;      // Các điểm tuần tra
    public float speed = 3f;           // Tốc độ di chuyển
    public float stopDistance = 0.2f;  // Khoảng cách để coi như đã tới điểm

    private int currentIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // Di chuyển gấu tới điểm tiếp theo
        transform.position += direction * speed * Time.deltaTime;

        // Nếu tới gần waypoint, chuyển sang điểm tiếp theo
        if (Vector3.Distance(transform.position, target.position) < stopDistance)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;  // Tuần hoàn
        }

        // Quay mặt gấu về hướng di chuyển
        if (direction.x != 0 || direction.z != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 5f * Time.deltaTime);
        }
    }
}
