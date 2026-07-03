using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAbilities", menuName = "Scriptable Objects/CharacterAbilities")]
public class CharacterAbilities : ScriptableObject
{
    [Header("Abilities")]
    [SerializeField] private GameObject[] abilities;
}
