using UnityEngine;

public class SleepNode : BTNodes
{
    private Animator animator;
    private Transform player;
    private float wakeRange;

    public SleepNode(Animator animator, Transform player, float wakeRange)
    {
        this.animator = animator;
        this.player = player;
        this.wakeRange = wakeRange;
    }

    public override State Evaluate()
    {
        float dist = Vector3.Distance(player.position, animator.transform.position);
        if (dist < wakeRange)
        {
            animator.SetBool("IsSleep", false);
            return State.Success; // chuyển sang node tiếp theo
        }

        animator.SetBool("IsSleep", true);
        return State.Running; // vẫn còn ngủ
    }
}
