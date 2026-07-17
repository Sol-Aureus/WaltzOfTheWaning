using UnityEngine;

[CreateAssetMenu(fileName = "SlownessEffect", menuName = "Status Effects/Slowness")]
public class SlownessEffect : StatusEffect
{
    public override void OnStackCountChanged(StatusEffectManager manager, int currentStacks)
    {
        float multiplier = Mathf.Pow(0.85f, currentStacks);
        StatModifier speedModifier = new StatModifier(0f, multiplier);

        manager.AddAttributeModifier(CharacterAttribute.MovementSpeed, EffectId, speedModifier);
    }

    public override void OnRemove(StatusEffectManager manager)
    {
        manager.RemoveAttributeModifier(CharacterAttribute.MovementSpeed, EffectId);
    }
}
