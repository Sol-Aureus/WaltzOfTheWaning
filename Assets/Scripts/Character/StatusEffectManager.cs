using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    // Master registry linking an Effect ID string to its live stopwatch tracker
    private Dictionary<string, ActiveEffect> activeEffects = new Dictionary<string, ActiveEffect>();

    // Cached list to avoid collection modification exceptions during Update loops
    private List<string> effectsToRemove = new List<string>();

    public void ApplyEffect(StatusEffect blueprint, int stackCount)
    {
        if (!activeEffects.TryGetValue(blueprint.EffectId, out ActiveEffect targetEffect))
        {
            // Brand new effect group setup
            targetEffect = new ActiveEffect(blueprint, gameObject);
            activeEffects.Add(blueprint.EffectId, targetEffect);
            blueprint.OnApply(gameObject); // Run setup logic once
        }

        // Whether it was just created or already existed, add the correct amount of stacks!
        targetEffect.AddStack(stackCount);
    }

    private void Update()
    {
        effectsToRemove.Clear();

        // Tick every active effect running on this specific character
        foreach (var kvp in activeEffects)
        {
            ActiveEffect effect = kvp.Value;
            effect.UpdateTimers(Time.deltaTime);

            if (effect.IsExpired)
            {
                effect.Blueprint.OnRemove(gameObject); // Run cleanup logic
                effectsToRemove.Add(kvp.Key); // Mark for deletion
            }
        }

        // Safely purge expired effects outside the dictionary loop
        foreach (string key in effectsToRemove)
        {
            activeEffects.Remove(key);
        }
    }

    // Master Dictionary structure: Dictionary<Attribute, Dictionary<SourceID, Modifier>>
    private Dictionary<CharacterAttribute, Dictionary<string, StatModifier>> masterModifiers
        = new Dictionary<CharacterAttribute, Dictionary<string, StatModifier>>();

    /// <summary>
    /// SetAttributeModifier allows a status effect to register its specific modifier for a given <see cref="CharacterAttribute"/>. It uses the source ID (typically the status effect's unique identifier) to ensure that multiple effects can modify the same attribute without conflict. After setting the modifier, it recalculates and applies the final aggregated modifier for that attribute.
    /// </summary>
    /// <param name="attribute">The <see cref="CharacterAttribute"/> to modify.</param>
    /// <param name="sourceId">The unique identifier of the source (e.g., status effect) that is providing the modifier.</param>
    /// <param name="modifier">The <see cref="StatModifier"/> that defines the flat and multiplier bonuses to apply.</param>
    public void AddAttributeModifier(CharacterAttribute attribute, string sourceId, StatModifier modifier)
    {
        // 1. If the "drawer" for this attribute doesn't exist yet, build it
        if (!masterModifiers.ContainsKey(attribute))
        {
            masterModifiers[attribute] = new Dictionary<string, StatModifier>();
        }

        // 2. File the multiplier away under the status effect's unique ID
        masterModifiers[attribute][sourceId] = modifier;

        // 3. Update the specific component that cares about this stat!
        ApplyFinalAttribute(attribute);
    }

    /// <summary>
    /// RemoveAttributeModifier removes a specific modifier for a given <see cref="CharacterAttribute"/> based on the source ID (typically the status effect's unique identifier). After removal, it recalculates and applies the final aggregated modifier for that attribute.
    /// </summary>
    /// <param name="attribute">The <see cref="CharacterAttribute"/> to remove the modifier from.</param>
    /// <param name="sourceId">The unique identifier of the source (e.g., status effect) that provided the modifier.</param>
    public void RemoveAttributeModifier(CharacterAttribute attribute, string sourceId)
    {
        if (masterModifiers.ContainsKey(attribute) && masterModifiers[attribute].ContainsKey(sourceId))
        {
            masterModifiers[attribute].Remove(sourceId);
            ApplyFinalAttribute(attribute);
        }
    }

    /// <summary>
    /// ApplyFinalAttribute aggregates all active modifiers for a specific <see cref="CharacterAttribute"/> and applies the final result to the relevant component (e.g., Locomotion or Health).
    /// </summary>
    /// <param name="attribute">The <see cref="CharacterAttribute"/> to apply the final aggregated modifier for.</param>
    private void ApplyFinalAttribute(CharacterAttribute attribute)
    {
        StatModifier finalModifier;
        finalModifier.FlatBonus = 0f;
        finalModifier.MultiplierBonus = 1f;

        // Multiply all active values inside this specific attribute's drawer
        if (masterModifiers.ContainsKey(attribute))
        {
            foreach (StatModifier modifier in masterModifiers[attribute].Values)
            {
                finalModifier.FlatBonus += modifier.FlatBonus;
                finalModifier.MultiplierBonus *= modifier.MultiplierBonus;
            }
        }

        switch (attribute)
        {
            case CharacterAttribute.MovementSpeed:
                if (TryGetComponent<Locomotion>(out Locomotion locomotion))
                    locomotion.SetMovementSpeedModifier(finalModifier);
                break;

            case CharacterAttribute.Acceleration:
                if (TryGetComponent<Locomotion>(out Locomotion locomotion2))
                    locomotion2.SetAccelerationModifier(finalModifier);
                break;

            case CharacterAttribute.Gravity:
                if (TryGetComponent<Locomotion>(out Locomotion locomotion3))
                    locomotion3.SetGravityModifier(finalModifier);
                break;

            case CharacterAttribute.JumpVelocity:
                if (TryGetComponent<Locomotion>(out Locomotion locomotion4))
                    locomotion4.SetJumpVelocityModifier(finalModifier);
                break;

            case CharacterAttribute.RotationSpeed:
                if (TryGetComponent<Locomotion>(out Locomotion locomotion5))
                    locomotion5.SetRotationSpeedModifier(finalModifier);
                break;

            case CharacterAttribute.DamageResistance:
                if (TryGetComponent<Health>(out Health health))
                    health.SetDamageResistanceModifier(finalModifier);
                break;
        }
    }
}