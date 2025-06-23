using UnityEngine;

public class AttackNode : BTNodes
{
    private Animator animator;
    private Transform enemy;
    private Transform player;
    private float attackRange;
    private float cooldown;
    private float lastAttackTime;

    private int numberOfAttacks;

    public AttackNode(Animator animator, Transform enemy, Transform player, float attackRange = 4f, float cooldown = 1f, int numberOfAttacks = 4)
    {
        this.animator = animator;
        this.enemy = enemy;
        this.player = player;
        this.attackRange = attackRange;
        this.cooldown = cooldown;
        this.numberOfAttacks = numberOfAttacks;
        lastAttackTime = -Mathf.Infinity;
    }

    public override State Evaluate()
    {
        float distance = Vector3.Distance(enemy.position, player.position);

        if (distance > attackRange)
        {
            
            return State.Failure; // quá xa => không đánh nữa
        }

        

        if (Time.time - lastAttackTime >= cooldown)
        {
            int randomAttack = Random.Range(1, numberOfAttacks + 1);
            string triggerName = $"Attack{randomAttack}";

            animator.SetTrigger(triggerName);
            

            lastAttackTime = Time.time;
        }

        return State.Running;
    }

    public override void Reset()
    {
        animator.SetBool("IsAttacking", false);
    }
}
