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

    private StatusEffectManager statusEffectManager;

    private StatModifier MovementSpeedModifier = new StatModifier(0f, 1f);
    private StatModifier AccelerationModifier = new StatModifier(0f, 1f);
    private StatModifier JumpVelocityModifier = new StatModifier(0f, 1f);
    private StatModifier GravityModifier = new StatModifier(0f, 1f);
    private StatModifier RotationSpeedModifier = new StatModifier(0f, 1f);

    private enum MovementState
    {
        Grounded,
        Sliding,
        Falling
    }

    private MovementState currentState;

    private Vector2 movementInput;
    private Vector3 moveDirection;
    private Vector3 groundNormal;
    private float slopeAngle;
    
    private Vector3 currentVelocity;
    private float verticalVelocity;
    private float currentSlideSpeed;
    private float currentLateralSpeed;

    private float coyoteTimer = 0;
    private float jumpBufferTimer = 0;
    private bool isJumping = false;
    private bool isSliding = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        InitializeData();
    }

    private void Update()
    {
        UpdateModifiers();
        HandleJump();
        CalculateMoveDirection();
        UpdateMovementState();
        HandleMovement();
        CountDownTimers();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        groundNormal = hit.normal;
        Debug.DrawRay(gameObject.transform.position + new Vector3 (0, 0.25f, 0), groundNormal * 2f, new Color (0,1,0));
        Debug.DrawRay(gameObject.transform.position + new Vector3(0, 0.25f, 0), Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized * 2f, new Color(0, 0, 1));
    }

    /// <summary>
    /// UpdateMovementState sets the charcter's current state
    /// </summary>
    private void UpdateMovementState()
    {
        slopeAngle = Vector3.Angle(groundNormal, Vector3.up);

        if (!characterController.isGrounded || isJumping)
        {
            currentState = MovementState.Falling;
        }
        else if (slopeAngle >= characterController.slopeLimit - 0.1f)
        {
            currentState = MovementState.Sliding;
        }
        else
        {
            currentState = MovementState.Grounded;
        }
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

        if (TryGetComponent<StatusEffectManager>(out StatusEffectManager manager))
        {
            statusEffectManager = manager;
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] Health is missing a StatusEffectManager on its hierarchy!", this);
        }
    }

    /// <summary>
    /// UpdateModifiers grabs the current modifiers from the StatusEffectManager and applies them to the character's movement parameters.
    /// </summary>
    private void UpdateModifiers()
    {
        if (statusEffectManager != null)
        {
            MovementSpeedModifier = statusEffectManager.GetFinalModifier(CharacterAttribute.MovementSpeed);
            AccelerationModifier = statusEffectManager.GetFinalModifier(CharacterAttribute.Acceleration);
            JumpVelocityModifier = statusEffectManager.GetFinalModifier(CharacterAttribute.JumpVelocity);
            GravityModifier = statusEffectManager.GetFinalModifier(CharacterAttribute.Gravity);
            RotationSpeedModifier = statusEffectManager.GetFinalModifier(CharacterAttribute.RotationSpeed);
        }
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
        CalculateHorizontalVelocity();
        Vector3 finalMotion = currentVelocity;

        // Modifies the motion based on the current MovementState
        switch (currentState)
        {
            case MovementState.Grounded:
                isSliding = false;
                ApplyGroundState();
                finalMotion.y = verticalVelocity;
                break;

            case MovementState.Sliding:
                Vector3 downSlopeDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized;
                if (!isSliding)
                {
                    currentSlideSpeed = 0;
                    isSliding = true;
                }

                Vector3 slideForce = CalculateSlideVelcocity(downSlopeDirection);
                Vector3 lateralMovement = CalculateLateralSlideVelocity(downSlopeDirection);

                finalMotion = slideForce + lateralMovement;
                verticalVelocity = finalMotion.y;
                break;

            case MovementState.Falling:
                isSliding = false;
                ApplyGravity();
                finalMotion.y = verticalVelocity;
                break;
        }

        characterController.Move(finalMotion * Time.deltaTime);
    }

    /// <summary>
    /// CalculateHorizontalVelocity uses the movement direction and acceleration to change the character's horizontal velocity
    /// </summary>
    private void CalculateHorizontalVelocity()
    {
        Vector3 targetVelocity = moveDirection * (currentMovementSpeed + MovementSpeedModifier.FlatBonus) * MovementSpeedModifier.MultiplierBonus;

        float acceleration;
        if (currentState == MovementState.Grounded) {
            acceleration = (currentGroundAcceleration + AccelerationModifier.FlatBonus) * AccelerationModifier.MultiplierBonus;
        }
        else
        {
            acceleration = (currentAirAcceleration + AccelerationModifier.FlatBonus) * AccelerationModifier.MultiplierBonus;
        } 

        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
    }

    /// <summary>
    /// ApplyGroundState resets the coyote timer and applies a constant grounding force to the character
    /// </summary>
    private void ApplyGroundState()
    {
        coyoteTimer = 0.2f;
        verticalVelocity = -(currentGravity + GravityModifier.FlatBonus) * GravityModifier.MultiplierBonus;
    }

    /// <summary>
    /// CalculateSlideVelcocity uses the angle of the slope to determine the direction and magnitude of gravity to apply
    /// </summary>
    /// <param name="slopeDirection">The tangent vector down the slope</param>
    /// <returns>The slide velocity</returns>
    private Vector3 CalculateSlideVelcocity(Vector3 slopeDirection)
    {
        float slopeRadians = slopeAngle * Mathf.Deg2Rad;
        float angleGravityMultiplier = Mathf.Sin(slopeRadians);
        currentSlideSpeed += (currentGravity + GravityModifier.FlatBonus) * GravityModifier.MultiplierBonus * angleGravityMultiplier * Time.deltaTime;

        Vector3 slideForce = slopeDirection * currentSlideSpeed;
        Vector3 downwardStickyForce = Vector3.down * (currentGravity + GravityModifier.FlatBonus) * GravityModifier.MultiplierBonus;

        return slideForce + downwardStickyForce;
    }

    /// <summary>
    /// CalculateLateralSlideVelocity uses the character's input to move along the 
    /// </summary>
    /// <param name="slopeDirection">The tangent vector down the slope</param>
    /// <returns>The lateral slide velocity</returns>
    private Vector3 CalculateLateralSlideVelocity(Vector3 slopeDirection)
    {
        Vector3 lateralDir = Vector3.Cross(slopeDirection, groundNormal).normalized;

        float lateralInput = Vector3.Dot(moveDirection, lateralDir);

        float targetLateralSpeed = lateralInput * (currentMovementSpeed + MovementSpeedModifier.FlatBonus) * MovementSpeedModifier.MultiplierBonus;

        currentLateralSpeed = Mathf.MoveTowards(currentLateralSpeed, targetLateralSpeed, (currentAirAcceleration + AccelerationModifier.FlatBonus) * AccelerationModifier.MultiplierBonus * Time.deltaTime);

        Vector3 lateralMovement = lateralDir * currentLateralSpeed;

        return lateralMovement;
    }

    /// <summary>
    /// ApplyGravity causes the character to fall
    /// </summary>
    private void ApplyGravity()
    {
        verticalVelocity -= (currentGravity + GravityModifier.FlatBonus) * GravityModifier.MultiplierBonus * Time.deltaTime;
        isJumping = false;
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
            isJumping = true;
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
}
