using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatistics", menuName = "Scriptable Objects/CharacterStatistics")]
public class CharacterStatistics : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private float damageTakenMultiplier;

    public float GetMovementSpeed => movementSpeed;
    public float GetGroundAcceleration => groundAcceleration;
    public float GetAirAcceleration => airAcceleration;
    public float GetJumpForce => jumpForce;
    public float GetGravity => gravity;
    public float GetRotationSpeed => rotationSpeed;
    public float GetMaxHealth => maxHealth;
    public float GetCurrentHealth => currentHealth;
    public float GetDamageTakenMultiplier => damageTakenMultiplier;
}
