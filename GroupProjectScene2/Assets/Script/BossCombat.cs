using UnityEngine;

public class BossCombat : MonoBehaviour
{
    private Animator animator;
    public float attackCooldown = 2f;
    public float attackRange = 3f;
    public Transform player;
    private bool canAttack = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange && canAttack)
        {
            Attack();
        }
    }

    void Attack()
    {
        canAttack = false;
        int attackType = Random.Range(1, 4); 
        animator.SetTrigger("Attack" + attackType);

        Invoke("ResetAttack", attackCooldown);
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}
