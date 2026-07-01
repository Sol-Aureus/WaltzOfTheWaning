using UnityEngine;

public class Locomotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterStatistics characterStatistics;
    [SerializeField] private Transform cameraTransform;
    private CharacterController characterController;

    private Vector2 movementInput;
    private Vector3 moveDirection;
    
    private Vector3 currentVelocity;
    private float verticalVelocity;

    private float coyoteTimer = 0;
    private float jumpBufferTimer = 0;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CalculateMoveDirection();
        HandleMovement();
        HandleJump();
        CountDownTimers();
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

        if (movementInput.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, characterStatistics.GetRotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// HandleMovement calculates the desired velocity based on player input and applies it to the CharacterController, while also considering gravity and jump mechanics.
    /// </summary>
    private void HandleMovement()
    {
        // 1. Calculate the velocity the player wants to achieve
        Vector3 targetVelocity = moveDirection * characterStatistics.GetMovementSpeed;

        // 2. Select acceleration based on ground status
        float acceleration = characterController.isGrounded ? characterStatistics.GetGroundAcceleration : characterStatistics.GetAirAcceleration;

        // 3. Smoothly accelerate currentVelocity towards targetVelocity
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

        // 4. Combine horizontal acceleration with vertical physics (Gravity placeholder)
        Vector3 finalMotion = currentVelocity;

        if (!characterController.isGrounded)
        {
            verticalVelocity += characterStatistics.GetGravity * Time.deltaTime;
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
            verticalVelocity = characterStatistics.GetJumpForce;
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
}
