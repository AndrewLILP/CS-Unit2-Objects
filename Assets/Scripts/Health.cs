using UnityEngine;

public class Health
{
    // ==================== EXISTING FIELDS (Pre-Story 2.2) ====================
    private float currentHealth;
    private float maxHealth;
    private float healthRegenRate;
    // ==========================================================================

    // ==================== EXISTING CONSTRUCTOR - ENHANCED FOR Story 2.2 ====================
    public Health(float _maxHealth, float _healthRegenRate, float _currentHealth = 100)
    {
        maxHealth = _maxHealth; // Enhanced: ensure maxHealth is stored
        healthRegenRate = _healthRegenRate;
        currentHealth = _currentHealth;
    }

    public Health()
    {
        // Default constructor - allows Health class usage without parameters
        maxHealth = 100f;
        healthRegenRate = 0f;
        currentHealth = 100f;
    }
    // =====================================================================================

    // ==================== EXISTING METHOD - ENHANCED FOR Story 2.2 ====================
    public void RegenHealth()
    {
        AddHealth(healthRegenRate * Time.deltaTime);
    }
    // ===================================================================================

    // ==================== EXISTING METHOD - ENHANCED FOR Story 2.2 ====================
    public void AddHealth(float value)
    {
        currentHealth += value;

        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        // Prevent health from exceeding maximum
        if (maxHealth > 0)
        {
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
        // ======================================================================================
    }
    // ===================================================================================

    // ==================== EXISTING METHOD - ENHANCED FOR Story 2.2 ====================
    public void DeductHealth(float value)
    {
        currentHealth -= value;

        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        // Prevent health from going below zero
        currentHealth = Mathf.Max(0, currentHealth);
        // ======================================================================================
    }
    // ===================================================================================

    // ==================== EXISTING METHOD (Pre-Story 2.2) ====================
    public float GetHealth()
    {
        return currentHealth;
    }
    // =====================================================================

    // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        if (maxHealth <= 0) return 0f;
        return currentHealth / maxHealth;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public bool IsFullHealth()
    {
        return currentHealth >= maxHealth;
    }

    public bool IsCriticalHealth(float criticalThreshold = 20f)
    {
        return currentHealth <= criticalThreshold;
    }
    // ======================================================================================
}