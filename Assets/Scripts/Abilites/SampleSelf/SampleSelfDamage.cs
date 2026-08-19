using UnityEngine;

[CreateAssetMenu(fileName = "SampleSelfDamage", menuName = "Abilities/SampleSelfDamage")]
public class SampleSelfDamage : Ability
{
    [SerializeField] private int damage;
    [SerializeField] private DamageType damageType;

    protected override void UseAbility(GameObject caster)
    {
        if (caster.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(DamageModifier.EvaluateModifier(damage, false), damageType);
        }
    }
}
