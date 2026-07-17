using UnityEngine;

public class AbilityHandler : MonoBehaviour
{

    [SerializeField] private AbilitySetData[] sourceAbilitySets;
    private Ability[][] runtimeAbilitySets;
    private int currentSet;
    private bool isCooldown;
    private bool isTicking;

    private StatusEffectManager statusEffectManager;
    private StatModifier CooldownModifier = new StatModifier(0f, 1f);

    private void Awake()
    {
        if (TryGetComponent<StatusEffectManager>(out StatusEffectManager manager))
        {
            statusEffectManager = manager;
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] AbilityHandler is missing a StatusEffectManager on its hierarchy!", this);
        }

        if (sourceAbilitySets != null)
        {
            runtimeAbilitySets = new Ability[sourceAbilitySets.Length][];

            for (int i = 0; i < sourceAbilitySets.Length; i++)
            {
                int abilityCount = sourceAbilitySets[i].GetAbility.Length;
                runtimeAbilitySets[i] = new Ability[abilityCount];

                for (int j = 0; j < abilityCount; j++)
                {
                    runtimeAbilitySets[i][j] = Instantiate(sourceAbilitySets[i].GetAbility[j]);
                }
            }
        }

        currentSet = 0;
        isCooldown = true;
        isTicking = true;
    }

    private void Update()
    {
        if (runtimeAbilitySets != null)
        {
            if (isCooldown)
            {
                UpdateModifiers();
                float clockSpeed = Time.deltaTime * ((1f + CooldownModifier.FlatBonus) * CooldownModifier.MultiplierBonus);
                for (int i = 0; i < runtimeAbilitySets.Length; i++)
                {
                    for (int j = 0; j < runtimeAbilitySets[i].Length; j++)
                    {
                        runtimeAbilitySets[i][j].TickCooldown(clockSpeed);
                    }
                }
            }
            if (isTicking)
            {
                if (currentSet >= 0 && currentSet < runtimeAbilitySets.Length)
                {
                    for (int j = 0; j < runtimeAbilitySets[currentSet].Length; j++)
                    {
                        runtimeAbilitySets[currentSet][j].TickAbility(Time.deltaTime);
                    }
                }
                else
                {
                    Debug.LogError($"[{gameObject.name}] AbilityHandler currentSet is out of range!", this);
                }
            }
        }
    }

    /// <summary>
    /// UpdateModifiers grabs the current modifiers from the StatusEffectManager and applies them to the character's attack parameters.
    /// </summary>
    private void UpdateModifiers()
    {
        if (statusEffectManager != null)
        {
            CooldownModifier = statusEffectManager.GetFinalModifier(CharacterAttribute.CooldownReduction);
        }
    }

    /// <summary>
    /// ActivateAbility calls the Activate function in the specified ability in the current set.
    /// </summary>
    /// <param name="abilityId">The index for the ability to activate</param>
    public void ActivateAbility(int abilityId)
    {
        Ability[] activeSet = runtimeAbilitySets[currentSet];
        bool abilityUsed = false;

        if (abilityId >= 0 && abilityId < activeSet.Length)
        {
            Ability ability = activeSet[abilityId];
            if (ability != null && ability.Activate(gameObject))
            {
                abilityUsed = true;
            }
        }
        else
        {
            Debug.Log($"Ability {abilityId} is out of range.");
        }

        if (abilityUsed)
        {
            Debug.Log($"Ability {abilityId} activated.");
        }
        else
        {
            Debug.Log($"Ability {abilityId} did not activate.");
        }
    }

    /// <summary>
    /// CycleAbilitySet cycles the current set to the next once, looping back to the starting set once it reaches the end.
    /// </summary>
    public void CycleAbilitySet()
    {
        if (runtimeAbilitySets == null || runtimeAbilitySets.Length <= 1) return;

        currentSet = (currentSet + 1) % runtimeAbilitySets.Length;
    }
}
