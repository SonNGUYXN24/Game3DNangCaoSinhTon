using UnityEngine;

public class PlayerDetectedNode : BTNodes
{
    private Transform enemy;
    private Transform player;
    private float detectRange;

    public PlayerDetectedNode(Transform enemy, Transform player, float detectRange)
    {
        this.enemy = enemy;
        this.player = player;
        this.detectRange = detectRange;
    }

    public override State Evaluate()
    {
        float distance = Vector3.Distance(enemy.position, player.position);
        return distance <= detectRange ? State.Success : State.Failure;
    }
}