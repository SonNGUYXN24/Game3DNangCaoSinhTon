using UnityEngine;

public class ChaseNode : BTNodes
{
    private Animator animator;
    private Transform enemy;
    private Transform player;
    private CharacterController controller;

    private float chaseRange;
    private float moveSpeed;

    public ChaseNode(Animator animator, Transform enemy, Transform player, CharacterController controller, float chaseRange = 10f, float moveSpeed = 3f)
    {
        this.animator = animator;
        this.enemy = enemy;
        this.player = player;
        this.controller = controller;
        this.chaseRange = chaseRange;
        this.moveSpeed = moveSpeed;
    }

    public override State Evaluate()
    {
        float distance = Vector3.Distance(player.position, enemy.position);

        if (distance > chaseRange)
        {
            animator.SetBool("IsRunning", false);
            return State.Failure;
        }

        // Xoay mặt về phía player
        Vector3 direction = (player.position - enemy.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemy.rotation = Quaternion.Slerp(enemy.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Di chuyển bằng CharacterController
        Vector3 move = direction * moveSpeed;
        controller.Move(move * Time.deltaTime);

        animator.SetBool("IsRunning", true);
        return State.Running;
    }

    public override void Reset()
    {
        animator.SetBool("IsRunning", false);
    }
}
