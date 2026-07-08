using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Locomotion locomotion;

    [Header("Debug Testing")]
    [SerializeField] private StatusEffect testStatusEffect;
    [SerializeField] private int testStackAmount = 1;
    [SerializeField] private float testDamageAmount = 10f;
    [SerializeField] private bool enableDebugTesting;

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

    public void OnTestEffect(InputAction.CallbackContext context)
    {
        if (!enableDebugTesting)
            return;

        if (context.ReadValue<float>() > 0)
        {
            if (testStatusEffect != null)
            {
                // Find the StatusEffectManager on the player and apply the effect
                if (TryGetComponent<StatusEffectManager>(out StatusEffectManager manager))
                {
                    manager.ApplyEffect(testStatusEffect, testStackAmount);
                    Debug.Log($"Debug applied {testStackAmount} stack(s) of {testStatusEffect.EffectId} to player.");
                }
            }
        }
    }

    public void OnTestDamage(InputAction.CallbackContext context)
    {
        if (!enableDebugTesting)
            return;

        if (context.ReadValue<float>() > 0)
        {
            // Find the StatusEffectManager on the player and apply the effect
            if (TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(10f);
                Debug.Log($"Debug applied 10 damage to player.");
            }
        }
    }
}
