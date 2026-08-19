using UnityEngine;

[CreateAssetMenu(fileName = "SampleStatus", menuName = "Abilities/SampleStatus")]
public class SampleStatus : Ability
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private int stackCount;

    protected override void UseAbility(GameObject caster)
    {
        if (caster.TryGetComponent<StatusEffectManager>(out StatusEffectManager manager))
        {
            manager.ApplyEffect(statusEffect, stackCount);
        }
    }
}
