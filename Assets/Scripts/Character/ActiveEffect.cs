using System.Collections.Generic;
using UnityEngine;

public class ActiveEffect
{
    public StatusEffect Blueprint { get; private set; }
    public int CurrentStacks { get; private set; }

    private List<float> individualTimers = new List<float>();

    private float masterTimer;

    private float tickTimer;
    private StatusEffectManager targetManager;

    public ActiveEffect(StatusEffect blueprint, StatusEffectManager manager)
    {
        Blueprint = blueprint;
        targetManager = manager;
        CurrentStacks = 0;
        tickTimer = 0f;
    }

    /// <summary>
    /// AddStack increases the number of stacks by the stackSize.
    /// </summary>
    /// <param name="stackSize">The number of stacks to add</param>
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
            if (CurrentStacks == 1)
            {
                masterTimer = Blueprint.maxDurationSeconds;
            }
        }
    }

    /// <summary>
    /// UpdateTimers progresses the tick timer, calling OnTick every time the threshold is reached.
    /// </summary>
    /// <param name="deltaTime">The time between calls</param>
    public void UpdateTimers(float deltaTime)
    {
        if (Blueprint.tickIntervalSeconds > 0f && CurrentStacks > 0)
        {
            tickTimer += deltaTime;
            if (tickTimer >= Blueprint.tickIntervalSeconds)
            {
                Blueprint.OnTick(targetManager, tickTimer, CurrentStacks);
                tickTimer -= Blueprint.tickIntervalSeconds;
            }
        }

        if (Blueprint.stackingBehavior == StackingBehavior.IndividualTimeouts)
        {
            int previoiusStacks = CurrentStacks;
            for (int i = individualTimers.Count - 1; i >= 0; i--)
            {
                individualTimers[i] -= deltaTime;
                if (individualTimers[i] <= 0f)
                {
                    individualTimers.RemoveAt(i);
                    CurrentStacks--;
                }
            }
            if (previoiusStacks != CurrentStacks && CurrentStacks > 0)
            {
                Blueprint.OnStackCountChanged(targetManager, CurrentStacks);
            }
        }
        else
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
                            masterTimer = Blueprint.maxDurationSeconds;
                            Blueprint.OnStackCountChanged(targetManager, CurrentStacks);
                        }
                    }
                    else
                    {
                        CurrentStacks = 0;
                    }
                }
            }
        }
    }

    public bool IsExpired => CurrentStacks <= 0;
}