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