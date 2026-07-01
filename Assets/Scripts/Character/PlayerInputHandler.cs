using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Locomotion locomotion;

    private void Awake()
    {
        locomotion = GetComponent<Locomotion>();
    }

    /// <summary>
    /// OnMove is called when the player provides movement input. It reads the input value as a Vector2 and passes it to the Locomotion component to handle character movement.
    /// </summary>
    /// <param name="context">The context of the input action, which contains information about the input event.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        locomotion.SetMovementInput(context.ReadValue<Vector2>());
    }

    /// <summary>
    /// OnJump is called when the player provides a jump input, calling the JumpInput method in the Locomotion component.
    /// </summary>
    /// <param name="context">The context of the input action, which contains information about the input event.</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            locomotion.JumpInput();
        }
    }
}
