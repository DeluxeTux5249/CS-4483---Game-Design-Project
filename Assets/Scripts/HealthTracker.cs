using UnityEngine;

public class HealthTracker : MonoBehaviour
{

    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; 
    }

    // returns true if creature dies, false otherwise.
    public bool GiveDamage(int dmgReceived)
    {
        currentHealth -= dmgReceived;

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
    }

    // logic to resolve on death
    private void Die() { 
        gameObject.SetActive(false);
    }
}
