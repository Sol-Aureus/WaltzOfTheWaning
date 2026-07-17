using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySetData", menuName = "Scriptable Objects/AbilitySetData")]
public class AbilitySetData : ScriptableObject
{
    [Header("Ability Settings")]
    [SerializeField] private Ability[] abilitySet;

    public Ability[] GetAbility => abilitySet;
}
