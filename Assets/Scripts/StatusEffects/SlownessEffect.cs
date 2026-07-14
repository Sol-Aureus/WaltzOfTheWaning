using UnityEngine;

[CreateAssetMenu(fileName = "SlownessEffect", menuName = "Status Effects/Slowness")]
public class SlownessEffect : StatusEffect
{
    private void OnEnable()
    {
        EffectId = "TwinSouls_Slowness";
        maxStacks = 6;
        maxDurationSeconds = 1.5f;
        tickIntervalSeconds = 0f;
        stackingBehavior = StackingBehavior.RefreshOnStack;
        decrementStacksOnTimeout = 2;
    }

    public override void OnStackCountChanged(StatusEffectManager manager, int currentStacks)
    {
        // 2. Use the struct constructor you built to pack the math cleanly
        float multiplier = Mathf.Pow(0.85f, currentStacks); // Each stack reduces speed by 15%
        StatModifier speedModifier = new StatModifier(0f, multiplier);

        // 3. Ask this specific character's manager to update our modifier drawer
        manager.AddAttributeModifier(CharacterAttribute.MovementSpeed, EffectId, speedModifier);
    }

    public override void OnRemove(StatusEffectManager manager)
    {
        // 2. Tell the manager to completely erase our slowness records from the speed drawer
        manager.RemoveAttributeModifier(CharacterAttribute.MovementSpeed, EffectId);
    }
}
