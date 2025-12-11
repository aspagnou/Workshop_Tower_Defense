using UnityEngine;

public class Nexus : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            NexusDeath();
        }
    }

    public void NexusDamaged(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
    }

    public void NexusDeath()
    {
        Destroy(gameObject);
    }
}
