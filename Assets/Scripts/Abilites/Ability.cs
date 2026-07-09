using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public string EffectId { get; protected set; }
    public int maxStacks { get; protected set; } // If 0, then the effect can stack infinitely
    public float maxDurationSeconds { get; protected set; } // If 0, then the effect lasts indefinitely
    public float tickIntervalSeconds { get; protected set; } // If 0, then the effect does not tick
    public StackingBehavior stackingBehavior { get; protected set; }
    public int decrementStacksOnTimeout { get; protected set; } // If 0, then the effect will remove all stacks on timeout

    public virtual void OnApply(GameObject target) { }
    public virtual void OnStackCountChanged(GameObject target, int currentStacks) { }
    public virtual void OnTick(GameObject target, float deltaTime, int currentStacks) { }
    public virtual void OnRemove(GameObject target) { }
}
