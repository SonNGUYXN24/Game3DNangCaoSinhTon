using UnityEngine;

public class IsInAttackRangeNode : BTNodes
{
    private Transform enemy;
    private Transform player;
    private float range;

    public IsInAttackRangeNode(Transform enemy, Transform player, float range)
    {
        this.enemy = enemy;
        this.player = player;
        this.range = range;
    }

    public override State Evaluate()
    {
        float distance = Vector3.Distance(enemy.position, player.position);
        return distance <= range ? State.Success : State.Failure;
    }
}
