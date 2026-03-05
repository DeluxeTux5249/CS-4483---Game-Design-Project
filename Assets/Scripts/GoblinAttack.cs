using UnityEngine;

public class GoblinAttack : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange = 0.8f;
    public LayerMask playerLayer;

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        if (hits.Length > 0)
        {
            hits[0].GetComponent<HealthTracker>().GiveDamage(damage);
        }
    }
}
