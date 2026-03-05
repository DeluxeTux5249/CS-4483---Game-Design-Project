using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public float cooldown = 1.5f;
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

    public void finishAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}
