using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum BearType
    {
        SleepingBear,
        PatrollingBear
    }

   
    [Header("Cấu hình loại gấu")]
    public BearType bearType = BearType.SleepingBear;

    [Header("Tham chiếu")]
    public Transform player;
    public Animator animator;
    public CharacterController characterController;

    [Header("Cấu hình hành vi")]
    public float khoangcachphathien = 5f;
    public float attackRange = 2f;
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;

    private BTNodes root;

    void Start()
    {
        if (animator == null) Debug.LogError("Thiếu Animator!");
        if (player == null) Debug.LogError("Thiếu Player!");
        if (characterController == null) characterController = GetComponent<CharacterController>();

        root = BuildBehaviorTree();
    }

    void Update()
    {
        root?.Evaluate();
    }

    private BTNodes BuildBehaviorTree()
    {
        var detectPlayer = new PlayerDetectedNode(transform, player, khoangcachphathien);

        var attackSequence = new Sequence(new List<BTNodes>
    {
        new IsInAttackRangeNode(transform, player, attackRange),
        new AttackNode(animator, transform, player, attackRange)
    });

        var chaseSequence = new Sequence(new List<BTNodes>
    {
        new InvertNode(new IsInAttackRangeNode(transform, player, attackRange)),
        new ChaseNode(animator, transform, player, characterController)
    });

        var engageSequence = new Sequence(new List<BTNodes>
    {
        detectPlayer,
        new BuffNode(animator),
        new Selector(new List<BTNodes> { attackSequence, chaseSequence })
    });

        BTNodes defaultBehavior = bearType == BearType.SleepingBear
            ? new SleepNode(animator, player, khoangcachphathien)
            : new PatrolNode(transform, patrolPoints, patrolSpeed, animator);

        return new Selector(new List<BTNodes>
    {
        engageSequence,
        defaultBehavior
    });
    }
}