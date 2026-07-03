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
    private int currentHealth;
    private int greyHealth;
    private int deathHealth;
    private float damageTakenMultiplier;

    [Header("Ability Settings")]
    [SerializeField] private CharacterAbilities[] abilitySets;

    public float GetMovementSpeed => movementSpeed;
    public float GetGroundAcceleration => groundAcceleration;
    public float GetAirAcceleration => airAcceleration;
    public float GetJumpForce => jumpForce;
    public float GetGravity => gravity;
    public float GetRotationSpeed => rotationSpeed;
    public float GetMaxHealth => maxHealth;
    public float GetCurrentHealth => currentHealth;
    public float GetGreyHealth => greyHealth;
    public float GetDeathHealth => deathHealth;
    public float GetDamageTakenMultiplier => damageTakenMultiplier;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Initialize sets the current health, grey health, and damage taken multiplier to their default values.
    /// </summary>
    public void Initialize()
    {
        currentHealth = maxHealth;
        greyHealth = maxHealth;
        deathHealth = 0;
        damageTakenMultiplier = 1f;
    }

    /// <summary>
    /// MultiplyMovementSpeed multiplies movement speed by a specified value.
    /// </summary>
    /// <param name="multiplier">The multiplier to multiply movement speed by.</param>
    public void MultiplyMovementSpeed(float multiplier)
    {
        movementSpeed *= multiplier;
    }

    /// <summary>
    /// MultiplyMovementSpeed multiplies both ground and air acceleration by a specified value.
    /// </summary>
    /// <param name="multiplier">The multiplier to multiply ground and air acceleration by.</param>
    public void MultiplyAcceleration(float multiplier)
    {
        groundAcceleration *= multiplier;
        airAcceleration *= multiplier;
    }

    /// <summary>
    /// MultiplyMovementSpeed multiplies the jump force by a specified value.
    /// </summary>
    /// <param name="multiplier">The multiplier to multiply the jump force by.</param>
    public void MultiplyJumpForce(float multiplier)
    {
        jumpForce *= multiplier;
    }

    /// <summary>
    /// MultiplyMovementSpeed multiplies gravity by a specified value.
    /// </summary>
    /// <param name="multiplier">The multiplier to multiply gravity by.</param>
    public void MultiplyGravity(float multiplier)
    {
        gravity *= multiplier;
    }

    /// <summary>
    /// ResetHealth resets the current health and grey health to the maximum health value.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        greyHealth = maxHealth;
    }

    /// <summary>
    /// SetCurrentHealth sets the current health of the character, clamped between 0 and greyHealth.
    /// </summary>
    /// <param name="value">The new health value to set.</param>
    public void SetCurrentHealth(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, greyHealth);
    }

    /// <summary>
    /// ChangeCurrentHealth modifies the current health of the character by a specified value.
    /// </summary>
    /// <param name="value">The amount to change the current health by (can be positive or negative).</param>
    public void ChangeCurrentHealth(int value)
    {
        SetCurrentHealth(currentHealth + value);
    }

    /// <summary>
    /// SetGreyHealth sets the grey health of the character, clamped between 0 and maxHealth.
    /// </summary>
    /// <param name="value">The new health value to set.</param>
    public void SetGreyHealth(int value)
    {
        greyHealth = Mathf.Clamp(value, 0, maxHealth);
    }

    /// <summary>
    /// ChangeGreyHealth modifies the grey health of the character by a specified value.
    /// </summary>
    /// <param name="value">The amount to change the grey health by (can be positive or negative).</param>
    public void ChangeGreyHealth(int value)
    {
        SetGreyHealth(greyHealth + value);
    }

    /// <summary>
    /// SetDeathHealth sets the death health of the character to a specified value.
    /// </summary>
    /// <param name="value">The new death health value to set.</param>
    public void SetDeathHealth(int value)
    {
        deathHealth = Mathf.Clamp(value, 0, maxHealth);
    }

    /// <summary>
    /// ChangeDeathHealth modifies the death health of the character by a specified value.
    /// </summary>
    /// <param name="value">The amount to change the death health by (can be positive or negative).</param>
    public void ChangeDeathHealth(int value)
    {
        SetDeathHealth(deathHealth + value);
    }

    /// <summary>
    /// MultiplyDamageTakenMultiplier multiplies the damage taken multiplier by a specified value.
    /// </summary>
    /// <param name="multiplier">The multiplier to multiply the damage taken multiplier by.</param>
    public void MultiplyDamageTakenMultiplier(float multiplier)
    {
        damageTakenMultiplier *= multiplier;
    }
}
