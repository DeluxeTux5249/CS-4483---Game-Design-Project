using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public float cooldown = 1.5f;
    public float knockbackForce = 50f;
    public Transform attackPoint;
    public float weaponRange = 1f;
    public LayerMask enemyLayer;
    public int damage = 1;
    public float stunTime = 0.5f;
    public float knockTime = 0.5f;
    private float timer;

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if(timer <= 0)
        {
            animator.SetBool("isAttacking", true);

            timer = cooldown;
        }
        animator.SetBool("isAttacking", true);
    }

    public void DealDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);
        if (hitColliders.Length > 0)
        {
            hitColliders[0].GetComponent<EnemyHealth>().ChangeHealth(-damage);
            hitColliders[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, knockTime, stunTime);
        }

    }

    public void finishAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}
