using UnityEngine;

public enum StackingBehavior
{
    DecrementOnTimeout, // Stacks will decrease by a certain amount when the effect times out, until it reaches 0 stacks and is removed
    RefreshOnStack, // Stacks will refresh the duration of the effect when a new stack is applied, and will follow decrementing behavior on timeout
    IndividualTimeouts // Each stack will have its own timeout, and will be removed individually when it times out
}

public abstract class StatusEffect : ScriptableObject
{
    [field: SerializeField]
    public string EffectId { get; protected set; }
    [field: SerializeField]
    public int maxStacks { get; protected set; } // If 0, then the effect can stack infinitely
    [field: SerializeField]
    public float maxDurationSeconds { get; protected set; } // If 0, then the effect lasts indefinitely
    [field: SerializeField]
    public float tickIntervalSeconds { get; protected set; } // If 0, then the effect does not tick
    [field: SerializeField]
    public StackingBehavior stackingBehavior { get; protected set; }
    [field: SerializeField]
    public int decrementStacksOnTimeout { get; protected set; } // If 0, then the effect will remove all stacks on timeout

    /// <summary>
    /// OnApply is called once when a new status of this type is applied.
    /// </summary>
    /// <param name="manager">The StatusEffectManager of the target</param>
    public virtual void OnApply(StatusEffectManager manager) { }

    /// <summary>
    /// OnStackCountChanged is called after the stack count for this status has changed.
    /// </summary>
    /// <param name="manager">The StatusEffectManager of the target</param>
    /// <param name="currentStacks">The current number of stacks</param>
    public virtual void OnStackCountChanged(StatusEffectManager manager, int currentStacks) { }

    /// <summary>
    /// OnTick is called every time the the tick interval has been reached.
    /// </summary>
    /// <param name="manager">The StatusEffectManager of the target</param>
    /// <param name="deltaTime">The time between ticks</param>
    /// <param name="currentStacks">The current number of stacks</param>
    public virtual void OnTick(StatusEffectManager manager, float deltaTime, int currentStacks) { }

    /// <summary>
    /// OnRemove is called once when the final stack of the status has expired or is removed.
    /// </summary>
    /// <param name="manager">The StatusEffectManager of the target</param>
    public virtual void OnRemove(StatusEffectManager manager) { }
}
