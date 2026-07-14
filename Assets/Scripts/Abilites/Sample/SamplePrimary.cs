using UnityEngine;

[CreateAssetMenu(fileName = "SamplePrimary", menuName = "Abilities/SamplePrimary")]
public class SamplePrimary : Ability
{
    private int damage = 10;

    protected override void OnEnable()
    {
        AbilityId = "SamplePrimary";
        cooldownSeconds = 2.5f;
        soulCost = 0;
        qiThreshold = 0;
        cooldownBehaviour = CooldownBehavior.Cooldown;
        base.OnEnable();
    }

    protected override void UseAbility(GameObject caster)
    {
        if (caster.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage((damage + DamageModifier.FlatBonus) * DamageModifier.MultiplierBonus);
        }
    }
}
