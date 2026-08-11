using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDescriptionData", menuName = "Scriptable Objects/AbilityDescriptionData")]
public class AbilityDescriptionData : ScriptableObject
{
    [SerializeField] private string abilityName;
    [SerializeField] private string abilityDescription;
    [SerializeField] private string abilityCooldown;
    [SerializeField] private string upgrade1Description;
    [SerializeField] private string upgrade2Description;
    [SerializeField] private string upgrade3Description;
}
