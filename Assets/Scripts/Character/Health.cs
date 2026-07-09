using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthData healthData;

    private int currentHealth;
    private int greyHealth;
    private int deathHealth;

    private StatusEffectManager statusEffectManager;
    private StatModifier DamageResistanceModifier = new StatModifier(0f, 1f);

    private void Awake()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        currentHealth = healthData.GetMaxHealth;
        greyHealth = healthData.GetMaxHealth;
        deathHealth = 0;

        if (TryGetComponent<StatusEffectManager>(out StatusEffectManager manager))
        {
            statusEffectManager = manager;
        }
    }

    /// <summary>
    /// UpdateModifiers grabs the current modifiers from the StatusEffectManager and applies them to the character's movement parameters.
    /// </summary>
    private void UpdateModifiers()
    {
        if (statusEffectManager != null)
        {
            DamageResistanceModifier = statusEffectManager.GetFinalModifier(CharacterAttribute.DamageResistance);
        }
    }

    /// <summary>
    /// TakeDamage applies damage to the character's health, randomly rounding to an integer value. Grey health is also reduced by a fraction of the damage taken. The damage is modified by the character's damage taken multiplier. Returns if this damage kills the character.
    /// </summary>
    /// <param name="damage">The amount of damage to apply to the character's health.</param>
    public bool TakeDamage(float damage)
    {
        UpdateModifiers();
        float damageToTake = (damage - DamageResistanceModifier.FlatBonus) / DamageResistanceModifier.MultiplierBonus;
        int effectiveDamage = Mathf.FloorToInt(damageToTake);
        int effectiveGreyDamage = Mathf.FloorToInt(damageToTake / 10);
        float randomDamage = damage - effectiveDamage;
        float randomGreyDamage = (damage / 10) - effectiveGreyDamage;

        if (Random.value < randomDamage)
        {
            effectiveDamage++;
        }
        if (Random.value < randomGreyDamage)
        {
            effectiveGreyDamage++;
        }

        ChangeCurrentHealth(-effectiveDamage);
        ChangeGreyHealth(-effectiveGreyDamage);
        Debug.Log($"Damage Taken: {effectiveDamage}, Grey Damage Taken: {effectiveGreyDamage}, Current Health: {currentHealth}, Grey Health: {greyHealth}");
        return CheckDeath();
    }

    /// <summary>
    /// Heal restores health to the character, randomly rounding to an integer value. The healing amount is applied to the character's current health.
    /// </summary>
    /// <param name="healAmount">The amount of health to restore to the character.</param>
    public void Heal(float healAmount)
    {
        int effectiveHeal = Mathf.FloorToInt(healAmount);
        float randomHeal = healAmount - effectiveHeal;
        if (Random.value < randomHeal)
        {
            effectiveHeal++;
        }
        ChangeCurrentHealth(effectiveHeal);
    }

    /// <summary>
    /// HealGrey restores grey health to the character, randomly rounding to an integer value. The healing amount is applied to the character's grey health.
    /// </summary>
    /// <param name="healAmount">The amount of grey health to restore to the character.</param>
    public void HealGrey(float healAmount)
    {
        int effectiveHeal = Mathf.FloorToInt(healAmount);
        float randomHeal = healAmount - effectiveHeal;
        if (Random.value < randomHeal)
        {
            effectiveHeal++;
        }
        ChangeGreyHealth(effectiveHeal);
    }

    /// <summary>
    /// CheckDeath checks if the character's current health is less than or equal to the death health. If so, it calls the Die method to handle the character's death logic.
    /// </summary>
    private bool CheckDeath()
    {
        if (currentHealth <= deathHealth)
        {
            Die();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Die handles the character's death logic.
    /// </summary>
    private void Die()
    {
        // Implement death logic here, such as playing a death animation, disabling controls, etc.
        Debug.Log("Character has died.");
    }

    /// <summary>
    /// ChangeDeathHealth modifies the death health value by a specified amount. The death health is clamped between 0 and the character's maximum health.
    /// </summary>
    /// <param name="amount">The amount to change the death health by. Can be positive or negative.</param>
    public void ChangeDeathHealth(int amount)
    {
        deathHealth += amount;
        deathHealth = Mathf.Clamp(deathHealth, 0, healthData.GetMaxHealth);
    }

    /// <summary>
    /// ChangeCurrentHealth modifies the current health value by a specified amount. The current health is clamped between 0 and the grey health value.
    /// </summary>
    /// <param name="amount">The amount to change the current health by. Can be positive or negative.</param>
    private void ChangeCurrentHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, greyHealth);
    }

    /// <summary>
    /// ChangeGreyHealth modifies the grey health value by a specified amount. The grey health is clamped between 0 and the character's maximum health.
    /// </summary>
    /// <param name="amount">The amount to change the grey health by. Can be positive or negative.</param>
    private void ChangeGreyHealth(int amount)
    {
        greyHealth += amount;
        greyHealth = Mathf.Clamp(greyHealth, 0, healthData.GetMaxHealth);
    }

    /// <summary>
    /// SetDamageResistanceModifier applies a <see cref="StatModifier"/> to the character's damage resistance.
    /// </summary>
    /// <param name="modifier">The <see cref="StatModifier"/> containing the flat and multiplier bonuses to apply to damage resistance.</param>
    public void SetDamageResistanceModifier(StatModifier modifier)
    {
        DamageResistanceModifier = modifier;
    }
}
