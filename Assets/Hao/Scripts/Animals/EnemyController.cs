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

    [Header("Máu của Enemy")]
    public int maxHP = 200;
    public int currentHP = 200;
    public float regenDelay = 5f; // thời gian chờ hồi máu sau khi bị tấn công
    public float regenRate = 2f;  // 2 HP mỗi giây

    private float timeSinceLastHit = 0f;

    private BTNodes root;

    void Start()
    {
        if (animator == null) Debug.LogError("Thiếu Animator!");
        if (player == null) Debug.LogError("Thiếu Player!");
        if (characterController == null) characterController = GetComponent<CharacterController>();

        root = BuildBehaviorTree();

        currentHP = maxHP;
    }

    void Update()
    {
        root?.Evaluate();

        // Hồi máu nếu đủ điều kiện
        if (currentHP < maxHP)
        {
            timeSinceLastHit += Time.deltaTime;

            if (timeSinceLastHit >= regenDelay)
            {
                currentHP += Mathf.FloorToInt(regenRate * Time.deltaTime);
                currentHP = Mathf.Min(currentHP, maxHP);
            }
        }
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

    // ✅ Hàm gọi từ bên ngoài khi Enemy bị tấn công
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);
        timeSinceLastHit = 0f;

        Debug.Log($"{gameObject.name} bị tấn công, còn lại {currentHP} HP");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} đã chết.");
        animator.SetTrigger("Die"); // nếu có animation Die
        // Có thể thêm: Disable AI, xóa object sau thời gian, rớt vật phẩm, v.v.
        Destroy(gameObject, 2f);
    }
}
