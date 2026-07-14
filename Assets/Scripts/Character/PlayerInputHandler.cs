using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerInputHandler : MonoBehaviour
{
    private Locomotion locomotion;
    private AbilityHandler abilityHandler;

    [Header("Debug Testing")]
    [SerializeField] private StatusEffect testStatusEffect;
    [SerializeField] private int testStackAmount = 1;
    [SerializeField] private float testDamageAmount = 10f;
    [SerializeField] private bool enableDebugTesting;

    private void Awake()
    {
        if (gameObject.TryGetComponent<Locomotion>(out Locomotion motion))
        {
            locomotion = motion;
        }
        if (gameObject.TryGetComponent<AbilityHandler>(out AbilityHandler handler))
        {
            abilityHandler = handler;
        }
    }

    /// <summary>
    /// OnMove is called when the player provides movement input. It reads the input value as a Vector2 and passes it to the Locomotion component to handle character movement.
    /// </summary>
    /// <param name="context">The context of the input action, which contains information about the input event.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (locomotion != null)
        {
            locomotion.SetMovementInput(context.ReadValue<Vector2>());
        }
    }

    /// <summary>
    /// OnJump is called when the player provides a jump input, calling the JumpInput method in the Locomotion component.
    /// </summary>
    /// <param name="context">The context of the input action, which contains information about the input event.</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && locomotion != null)
        {
            locomotion.JumpInput();
        }
    }

    public void OnAbilityPrimary(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && abilityHandler != null)
        {
            abilityHandler.ActivateAbility(0);
        }
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && abilityHandler != null)
        {
            abilityHandler.ActivateAbility(1);
        }
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && abilityHandler != null)
        {
            abilityHandler.ActivateAbility(2);
        }
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && abilityHandler != null)
        {
            abilityHandler.ActivateAbility(3);
        }
    }

    public void OnAbility4(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && abilityHandler != null)
        {
            abilityHandler.ActivateAbility(4);
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
