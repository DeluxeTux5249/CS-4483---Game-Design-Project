using UnityEngine;

public class HealthTracker : MonoBehaviour
{

    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;
    [SerializeField] private HealthBar healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null )
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    // returns true if creature dies, false otherwise.
    public bool GiveDamage(int dmgReceived)
    {
        currentHealth -= dmgReceived;
        
        if (healthBar != null) healthBar.SetHealth(currentHealth);

        // if you run out of health, you die.
        if (currentHealth <= 0) {
            Die();
            return true;
        } else return false;
    }

    public void GiveHealth(int hpReceived)
    {
        currentHealth += hpReceived;

        // can't heal beyond max
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthBar != null) healthBar.SetHealth(currentHealth);
    }

    // logic to resolve on death
    private void Die() { 
        gameObject.SetActive(false);
    }
}
