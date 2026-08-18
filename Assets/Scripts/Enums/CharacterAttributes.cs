using UnityEngine;

public enum CharacterAttribute
{
    MovementSpeed,
    Acceleration,
    Gravity,
    JumpVelocity,
    RotationSpeed,
    DamageResistance,
    DamageMultiplier,
    CooldownReduction
}

/// <summary>
/// StatModifier contains both the flat and multiplicative modifiers for attributes
/// </summary>
public struct StatModifier
{
    public float FlatBonus;
    public float MultiplierBonus;

    public StatModifier(float flat, float mult)
    {
        FlatBonus = flat;
        MultiplierBonus = mult;
    }

    /// <summary>
    /// EvaluateModifier calculates the final value after applying the flat and multiplicative bonuses. If bypassNegation is true, it ensures that the flat bonus is not negative and the multiplier is not less than 1, effectively ignoring any resistances.
    /// </summary>
    /// <param name="baseValue">The incoming base value</param>
    /// <param name="bypassNegation">Weather or not the negative bonususes will be ignored</param>
    /// <returns>The new value</returns>
    public float EvaluateModifier(float baseValue, bool bypassNegation)
    {
        float flat = bypassNegation ? Mathf.Max(0f, FlatBonus) : FlatBonus;
        float mult = bypassNegation ? Mathf.Max(1f, MultiplierBonus) : MultiplierBonus;
        return (baseValue + flat) * mult;
    }
}

/// <summary>
/// ReturnDamage contains the damage the hit did, the health the character has remaining, the max health of the character, and wheather or not the hit killed
/// </summary>
public struct ReturnDamage
{
    public int DamageTaken;
    public int HealthRemaining;
    public int MaxHealth;
    public bool HitKilled;

    public ReturnDamage(int damage, int health, int max, bool killed)
    {
        DamageTaken = damage;
        HealthRemaining = health;
        MaxHealth = max;
        HitKilled = killed;
    }
}