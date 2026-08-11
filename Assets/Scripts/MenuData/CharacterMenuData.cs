using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMenuData", menuName = "Scriptable Objects/CharacterMenuData")]
public class CharacterMenuData : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField] private string characterTitle;
    [SerializeField] private string characterOverview;
    [SerializeField] private Sprite characterPortrait;
    [SerializeField] private string[] characterKeywords;

    [SerializeField] private MovementData movementData;
    [SerializeField] private HealthData healthData;
    [SerializeField] private AbilityDescriptionData abilityDescriptionData;

    [CreateProperty] public int health => healthData.GetMaxHealth;
    [CreateProperty] public float movementSpeed => movementData.GetMovementSpeed;
    [CreateProperty] public float jumpVelocity => movementData.GetJumpVelocity;

    [CreateProperty]
    public string GetStatistics => $"Health: {health}\n" +
               $"Speed: {movementSpeed}\n" +
               $"Jump: {jumpVelocity}";

    [CreateProperty] public string primary => characterKeywords[0];
    [CreateProperty] public string secondary => characterKeywords[1];
    [CreateProperty] public string tertiary => characterKeywords[2];

    [CreateProperty]
    public string GetCharacterKeywords => $"{primary}\n" +
               $"{secondary}\n" +
               $"{tertiary}";

    public string GetCharacterTitle => characterTitle;
    public Sprite GetCharacterPortrait => characterPortrait;
}