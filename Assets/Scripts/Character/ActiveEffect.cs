using System.Collections.Generic;
using UnityEngine;

public class ActiveEffect
{
    // Reference back to our read-only rulebook
    public StatusEffect Blueprint { get; private set; }
    public int CurrentStacks { get; private set; }

    // Option A: For IndividualTimeouts (The "Scooter" list)
    private List<float> individualTimers = new List<float>();

    // Option B: For Decrement / Refresh behaviors (The "Uber" master clock)
    private float masterTimer;

    // Internal ticker to know when to run OnTick (e.g., every 0.33 seconds)
    private float tickTimer;
    private StatusEffectManager targetManager;

    public ActiveEffect(StatusEffect blueprint, StatusEffectManager manager)
    {
        Blueprint = blueprint;
        targetManager = manager;
        CurrentStacks = 0;
        tickTimer = 0f;
    }

    public void AddStack(int stackSize)
    {
        if (Blueprint.maxStacks > 0 && CurrentStacks >= Blueprint.maxStacks)
        {
            if (Blueprint.stackingBehavior == StackingBehavior.RefreshOnStack)
            {
                masterTimer = Blueprint.maxDurationSeconds;
            }
            return;
        }

        if (Blueprint.maxStacks > 0 && CurrentStacks + stackSize > Blueprint.maxStacks)
        {
            stackSize = Blueprint.maxStacks - CurrentStacks;
        }
        CurrentStacks += stackSize;
        Blueprint.OnStackCountChanged(targetManager, CurrentStacks);

        if (Blueprint.stackingBehavior == StackingBehavior.IndividualTimeouts)
        {
            for (int i = 0; i < stackSize; i++)
                individualTimers.Add(Blueprint.maxDurationSeconds);
        }
        else if (Blueprint.stackingBehavior == StackingBehavior.RefreshOnStack)
        {
            masterTimer = Blueprint.maxDurationSeconds;
        }
        else if (Blueprint.stackingBehavior == StackingBehavior.DecrementOnTimeout)
        {
            // Only start the clock if this is the very first stack entering the group!
            if (CurrentStacks == 1)
            {
                masterTimer = Blueprint.maxDurationSeconds;
            }
        }
    }

    public void UpdateTimers(float deltaTime)
    {
        // 1. MANAGE ACTION TICKS (e.g. Pyre Fist's Burn interval)
        if (Blueprint.tickIntervalSeconds > 0f && CurrentStacks > 0)
        {
            tickTimer += deltaTime;
            if (tickTimer >= Blueprint.tickIntervalSeconds)
            {
                // Trigger the actual gameplay logic rule defined in your blueprint!
                Blueprint.OnTick(targetManager, tickTimer, CurrentStacks);
                tickTimer -= Blueprint.tickIntervalSeconds; // Reset tick intervals
            }
        }

        // 2. MANAGE EXPIRATION CLOCKS
        if (Blueprint.stackingBehavior == StackingBehavior.IndividualTimeouts)
        {
            int previoiusStacks = CurrentStacks;
            // Loop backward through the list so we can safely remove elements while iterating
            for (int i = individualTimers.Count - 1; i >= 0; i--)
            {
                individualTimers[i] -= deltaTime;
                if (individualTimers[i] <= 0f)
                {
                    individualTimers.RemoveAt(i);
                    CurrentStacks--; // This individual stack timed out!
                }
            }
            if (previoiusStacks != CurrentStacks && CurrentStacks > 0)
            {
                Blueprint.OnStackCountChanged(targetManager, CurrentStacks);
            }
        }
        else // Master timer approach
        {
            if (CurrentStacks > 0)
            {
                masterTimer -= deltaTime;
                if (masterTimer <= 0f)
                {
                    if (Blueprint.decrementStacksOnTimeout != 0)
                    {
                        CurrentStacks -= Blueprint.decrementStacksOnTimeout;

                        if (CurrentStacks > 0)
                        {
                            masterTimer = Blueprint.maxDurationSeconds; // Reset clock for next decay
                            Blueprint.OnStackCountChanged(targetManager, CurrentStacks);
                        }
                    }
                    else
                    {
                        CurrentStacks = 0; // Wipe all stacks instantly if decrement is 0
                    }
                }
            }
        }
    }

    public bool IsExpired => CurrentStacks <= 0;
}