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