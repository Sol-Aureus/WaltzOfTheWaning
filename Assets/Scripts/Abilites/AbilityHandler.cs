using UnityEngine;

public class AbilityHandler : MonoBehaviour
{

    [SerializeField] private AbilitySetData[] abilitySets;
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

        currentSet = 0;
        isCooldown = true;
        isTicking = true;
    }

    private void Update()
    {
        if (isCooldown)
        {
            UpdateModifiers();
            float clockSpeed = Time.deltaTime * ((1f + CooldownModifier.FlatBonus) * CooldownModifier.MultiplierBonus);

            foreach (AbilitySetData set in abilitySets)
            {
                if (set != null)
                {
                    foreach (Ability ability in set.GetAbility)
                    {
                        if (ability != null)
                        {
                            ability.TickCooldown(clockSpeed);
                        }
                    }
                }
            }
        }

        if (isTicking)
        {
            foreach(Ability ability in abilitySets[currentSet].GetAbility)
            {
                if (ability != null)
                {
                    ability.TickAbility(Time.deltaTime);
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
    /// ActivateAbility calls the Activate function in the specified ability in the current set
    /// </summary>
    /// <param name="abilityId">The index for the ability to activate</param>
    public void ActivateAbility(int abilityId)
    {
        AbilitySetData activeSet = abilitySets[currentSet];
        bool abilityUsed = false;

        if (abilityId >= 0 && abilityId < activeSet.GetAbility.Length)
        {
            Ability ability = activeSet.GetAbility[abilityId];
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
}
