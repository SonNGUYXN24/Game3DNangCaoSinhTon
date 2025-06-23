using UnityEngine;

public class BuffNode : BTNodes
{
    private Animator animator;
    private float buffDuration = 1.5f;
    private float timer = 0f;
    private bool isBuffing = false;

    public BuffNode(Animator animator, float buffDuration = 1.5f)
    {
        this.animator = animator;
        this.buffDuration = buffDuration;
    }

    public override State Evaluate()
    {
        if (!isBuffing)
        {
            animator.SetTrigger("Buff");
            timer = 0f;
            isBuffing = true;
        }

        timer += Time.deltaTime;

        if (timer >= buffDuration)
        {
            return State.Success;
        }

        return State.Running;
    }

    public override void Reset()
    {
        timer = 0f;
        isBuffing = false;
    }
}
