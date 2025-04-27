using UnityEngine;

public class Health //: NON-MonoBehaviour
{
    // OOP health to be used for player and enemies
    // health for playable objects is set here with constructors



    private float currentHealth;
    private float maxHealth;
    private float healthRegenRate;

    public Health(float _maxHealth, float _healthRegenRate, float _currentHealth = 100)
    {
        currentHealth = _currentHealth; // OOP: enscapulation 
        maxHealth = _maxHealth;
        healthRegenRate = _healthRegenRate;
    }

    public Health()
    {
        // base constructor
        // allows other objects to use Health class without needing to set values - default values eg 0
    }

    public void AddHealth(float value)
    {
        currentHealth += value;
        // add health to the current health
        // we could use neg values to deduct health
    }

    public void DeductHealth(float value)
    {
        currentHealth -= value;
        // deduct health from the current health
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
