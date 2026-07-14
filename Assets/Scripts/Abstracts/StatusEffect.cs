using UnityEngine;

public enum StackingBehavior
{
    DecrementOnTimeout, // Stacks will decrease by a certain amount when the effect times out, until it reaches 0 stacks and is removed
    RefreshOnStack, // Stacks will refresh the duration of the effect when a new stack is applied, and will follow decrementing behavior on timeout
    IndividualTimeouts // Each stack will have its own timeout, and will be removed individually when it times out
}

public abstract class StatusEffect : ScriptableObject
{
    public string EffectId { get; protected set; }
    public int maxStacks { get; protected set; } // If 0, then the effect can stack infinitely
    public float maxDurationSeconds { get; protected set; } // If 0, then the effect lasts indefinitely
    public float tickIntervalSeconds { get; protected set; } // If 0, then the effect does not tick
    public StackingBehavior stackingBehavior { get; protected set; }
    public int decrementStacksOnTimeout { get; protected set; } // If 0, then the effect will remove all stacks on timeout

    public virtual void OnApply(StatusEffectManager manager) { }
    public virtual void OnStackCountChanged(StatusEffectManager manager, int currentStacks) { }
    public virtual void OnTick(StatusEffectManager manager, float deltaTime, int currentStacks) { }
    public virtual void OnRemove(StatusEffectManager manager) { }
}
