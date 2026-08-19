using UnityEngine;

[CreateAssetMenu(fileName = "SampleSelfGreyHealing", menuName = "Abilities/SampleSelfGreyHealing")]
public class SampleSelfGreyHealing : Ability
{
    [SerializeField] private int healing;

    protected override void UseAbility(GameObject caster)
    {
        if (caster.TryGetComponent<Health>(out Health health))
        {
            health.HealGrey(healing);
        }
    }
}
