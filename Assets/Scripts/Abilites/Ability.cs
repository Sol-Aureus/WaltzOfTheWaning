using UnityEngine;
public enum CooldownBehavior
{
    Cooldown, // The ability will go on cooldown when it is used
    Passive, // The ability can always be active, or be used automatically when a condition is met, going on cooldown afterwards
    Cost // The ability takes souls to recharge as well as a required Qi threshold to use
}

public abstract class Ability : ScriptableObject
{
    public string AbilityId { get; protected set; }
    public float cooldownSeconds { get; protected set; } 
    public int soulCost { get; protected set; }
    public float qiThreshold { get; protected set; }
    public CooldownBehavior cooldownBehaviour { get; protected set; }
    protected float cooldownTimer { get; set; }
    protected int soulCount { get; set; }
    protected bool canUseAbility { get; set; }

    protected StatModifier DamageModifier = new StatModifier(0f, 1f);

    protected virtual void OnEnable()
    {
        cooldownTimer = 0;
        canUseAbility = true;
    }

    /// <summary>
    /// Activate starts the ability function and resets the cooldown
    /// </summary>
    /// <returns>Returns true if the ability successfully activated</returns>
    public bool Activate(GameObject caster)
    {
        if (!IsReady())
        {
            return false;
        }

        if (caster.TryGetComponent<StatusEffectManager>(out StatusEffectManager manager))
        {
            DamageModifier = manager.GetFinalModifier(CharacterAttribute.DamageMultiplier);
        }

        UseAbility(caster);
        cooldownTimer -= cooldownSeconds;
        soulCount = 0;
        return true;
    }

    /// <summary>
    /// UseAbility runs the logic for the ability functions
    /// </summary>
    protected abstract void UseAbility(GameObject caster);

    /// <summary>
    /// TickCooldown processes the cooldown timer
    /// </summary>
    /// <param name="detlaTime">The delta time</param>
    public virtual void TickCooldown(float detlaTime)
    {
        if (!IsCooledDown())
        {
            cooldownTimer += detlaTime;
        }
    }

    /// <summary>
    /// TickAbility processes any ability timers or processes
    /// </summary>
    /// <param name="detlaTime">The delta time</param>
    public virtual void TickAbility(float detlaTime) { }

    /// <summary>
    /// IsReady determines if the ability is able to be used
    /// </summary>
    /// <returns>If the ability can be used at the moment or not</returns>
    public bool IsReady()
    {
        if (canUseAbility)
        {
            return IsCooledDown();
        }
       
        return false;
    }

    /// <summary>
    /// IsCooledDown determines if the ability's cooldown is finished
    /// </summary>
    /// <returns>If the ability cooldown is finished or not</returns>
    public bool IsCooledDown()
    {
        if (cooldownBehaviour == CooldownBehavior.Cooldown || cooldownBehaviour == CooldownBehavior.Passive)
        {
            if (cooldownTimer >= cooldownSeconds)
            {
                return true;
            }
        }
        else if (cooldownBehaviour == CooldownBehavior.Cost)
        {
            // Add proper Qi and Soul checks
            return true;
        }

        return false;
    }
}
