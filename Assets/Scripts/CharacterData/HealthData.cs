using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "Scriptable Objects/HealthData")]
public class HealthData : ScriptableObject
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth;

    public int GetMaxHealth => maxHealth;
}
