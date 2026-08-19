using UnityEngine;

[CreateAssetMenu(fileName = "SampleSelfHealing", menuName = "Abilities/SampleSelfHealing")]
public class SampleSelfHealing : Ability
{
    [SerializeField] private int healing;

    protected override void UseAbility(GameObject caster)
    {
        if (caster.TryGetComponent<Health>(out Health health))
        {
            health.Heal(healing);
        }
    }
}
