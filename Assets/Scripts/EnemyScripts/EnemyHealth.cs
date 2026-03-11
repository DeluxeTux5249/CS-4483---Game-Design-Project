using UnityEngine;

public class EnemyHealth : MonoBehaviour

{
    [SerializeField] private HealthBar healthBar;
    public int currentHealth;
    public int maxHealth;
    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }

    }

    public void ChangeHealth(int amount)
    {
        if (healthBar != null) healthBar.SetHealth(currentHealth);
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
            if (healthBar != null) healthBar.SetHealth(currentHealth);
        }
        else if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    } 


}
