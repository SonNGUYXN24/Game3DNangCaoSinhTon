using UnityEngine;

public class PatrolNode : BTNodes
{
    private Transform[] waypoints;
    private int currentIndex = 0;
    private float reachDistance = 0.5f;
    private float moveSpeed;
    private Transform enemy;
    private Animator animator;

    public PatrolNode(Transform enemy, Transform[] waypoints, float moveSpeed, Animator animator)
    {
        this.enemy = enemy;
        this.waypoints = waypoints;
        this.moveSpeed = moveSpeed;
        this.animator = animator;
    }

    public override State Evaluate()
    {
        if (waypoints.Length == 0) return State.Failure;

        Transform target = waypoints[currentIndex];
        Vector3 direction = (target.position - enemy.position).normalized;
        direction.y = 0;

        enemy.rotation = Quaternion.Slerp(enemy.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        enemy.position += direction * moveSpeed * Time.deltaTime;

        animator.SetBool("IsWalking", true);

        if (Vector3.Distance(enemy.position, target.position) < reachDistance)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }

        return State.Running;
    }

    public override void Reset()
    {
        animator.SetBool("IsWalking", false);
    }
}
