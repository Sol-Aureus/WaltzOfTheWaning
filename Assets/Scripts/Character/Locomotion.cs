using UnityEngine;

public class Locomotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementData movementData;
    [SerializeField] private Transform cameraTransform;
    private CharacterController characterController;

    private float currentMovementSpeed;
    private float currentGroundAcceleration;
    private float currentAirAcceleration;
    private float currentJumpVelocity;
    private float currentGravity;
    private float currentRotationSpeed;

    private StatModifier MovementSpeedModifier = new StatModifier(0f, 1f);
    private StatModifier AccelerationModifier = new StatModifier(0f, 1f);
    private StatModifier JumpVelocityModifier = new StatModifier(0f, 1f);
    private StatModifier GravityModifier = new StatModifier(0f, 1f);
    private StatModifier RotationSpeedModifier = new StatModifier(0f, 1f);


    private Vector2 movementInput;
    private Vector3 moveDirection;
    
    private Vector3 currentVelocity;
    private float verticalVelocity;

    private float coyoteTimer = 0;
    private float jumpBufferTimer = 0;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        InitializeData();
    }

    private void Update()
    {
        CalculateMoveDirection();
        HandleMovement();
        HandleJump();
        CountDownTimers();
    }

    /// <summary>
    /// InitializeData initializes the movement parameters from the MovementData ScriptableObject, setting the current movement speed, acceleration, jump force, gravity, and rotation speed for the character.
    /// </summary>
    private void InitializeData()
    {
        currentMovementSpeed = movementData.GetMovementSpeed;
        currentGroundAcceleration = movementData.GetGroundAcceleration;
        currentAirAcceleration = movementData.GetAirAcceleration;
        currentJumpVelocity = movementData.GetJumpVelocity;
        currentGravity = movementData.GetGravity;
        currentRotationSpeed = movementData.GetRotationSpeed;
    }

    /// <summary>
    /// CalculateMoveDirection computes the desired movement direction based on the character's input and the camera's orientation.
    /// </summary>
    private void CalculateMoveDirection()
    {
        // Calculate raw direction based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Flatten camera vectors so looking down doesn't make the player slow down
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = forward * movementInput.y + right * movementInput.x;
        moveDirection.Normalize();

        if (movementInput.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (currentRotationSpeed + RotationSpeedModifier.FlatBonus) * RotationSpeedModifier.MultiplierBonus * Time.deltaTime);
        }
    }

    /// <summary>
    /// HandleMovement calculates the desired velocity based on player input and applies it to the CharacterController, while also considering gravity and jump mechanics.
    /// </summary>
    private void HandleMovement()
    {
        // 1. Calculate the velocity the player wants to achieve
        Vector3 targetVelocity = moveDirection * (currentMovementSpeed + MovementSpeedModifier.FlatBonus) * MovementSpeedModifier.MultiplierBonus;

        // 2. Select acceleration based on ground status
        float acceleration = characterController.isGrounded ? (currentGroundAcceleration + AccelerationModifier.FlatBonus) * AccelerationModifier.MultiplierBonus : (currentAirAcceleration + AccelerationModifier.FlatBonus) * AccelerationModifier.MultiplierBonus;

        // 3. Smoothly accelerate currentVelocity towards targetVelocity
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

        // 4. Combine horizontal acceleration with vertical physics (Gravity placeholder)
        Vector3 finalMotion = currentVelocity;

        if (!characterController.isGrounded)
        {
            verticalVelocity -= (currentGravity + GravityModifier.FlatBonus) * GravityModifier.MultiplierBonus * Time.deltaTime;
        }
        else
        {
            coyoteTimer = 0.2f;
        }

        finalMotion.y = verticalVelocity;

        // 5. Move the controller using our calculated velocity
        characterController.Move(finalMotion * Time.deltaTime);
    }

    /// <summary>
    /// HandleJump handles the jump action by applying the jump force if the jump buffer and coyote time conditions are met.
    /// </summary>
    /// <remarks>This method allows for a jump to occur within a short window after leaving the ground (coyote
    /// time) or after the jump input is buffered (jump buffer). Both timers are reset upon a successful
    /// jump.</remarks>
    private void HandleJump()
    {
        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            verticalVelocity = (currentJumpVelocity + JumpVelocityModifier.FlatBonus) * JumpVelocityModifier.MultiplierBonus;
            jumpBufferTimer = 0;
            coyoteTimer = 0;
        }
    }

    /// <summary>
    /// CountDownTimers decrements the coyote time and jump buffer timers each frame.
    /// </summary>
    private void CountDownTimers()
    {
        coyoteTimer -= Time.deltaTime;
        jumpBufferTimer -= Time.deltaTime;
    }

    /// <summary>
    /// SetMovementInput sets the movement input direction for the character.
    /// </summary>
    /// <param name="inputDirection">A <see cref="Vector2"/> representing the direction of movement.</param>
    public void SetMovementInput(Vector2 inputDirection)
    {
        movementInput = inputDirection;
    }

    /// <summary>
    /// JumpInput buffers a jump input.
    /// </summary>
    public void JumpInput()
    {
        jumpBufferTimer = 0.2f;
    }

    /// <summary>
    /// SetMovementSpeedModifier applies a <see cref="StatModifier"/> to the character's movement speed, allowing for both flat and multiplier bonuses to be set.
    /// </summary>
    /// <param name="modifier">The <see cref="StatModifier"/> containing the flat and multiplier bonuses to apply to the movement speed.</param>
    public void SetMovementSpeedModifier(StatModifier modifier)
    {
        MovementSpeedModifier = modifier;
    }

    /// <summary>
    /// SetAccelerationModifier applies a <see cref="StatModifier"/> to the character's acceleration (ground and air).
    /// </summary>
    /// <param name="modifier">The <see cref="StatModifier"/> containing the flat and multiplier bonuses to apply to acceleration.</param>
    public void SetAccelerationModifier(StatModifier modifier)
    {
        AccelerationModifier = modifier;
    }

    /// <summary>
    /// SetJumpVelocityModifier applies a <see cref="StatModifier"/> to the character's jump velocity.
    /// </summary>
    /// <param name="modifier">The <see cref="StatModifier"/> containing the flat and multiplier bonuses to apply to jump velocity.</param>
    public void SetJumpVelocityModifier(StatModifier modifier)
    {
        JumpVelocityModifier = modifier;
    }

    /// <summary>
    /// SetGravityModifier applies a <see cref="StatModifier"/> to the character's gravity.
    /// </summary>
    /// <param name="modifier">The <see cref="StatModifier"/> containing the flat and multiplier bonuses to apply to gravity.</param>
    public void SetGravityModifier(StatModifier modifier)
    {
        GravityModifier = modifier;
    }

    /// <summary>
    /// SetRotationSpeedModifier applies a <see cref="StatModifier"/> to the character's rotation speed.
    /// </summary>
    /// <param name="modifier">The <see cref="StatModifier"/> containing the flat and multiplier bonuses to apply to rotation speed.</param>
    public void SetRotationSpeedModifier(StatModifier modifier)
    {
        RotationSpeedModifier = modifier;
    }
}
