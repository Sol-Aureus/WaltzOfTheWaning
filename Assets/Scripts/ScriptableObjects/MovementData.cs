using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Scriptable Objects/MovementData")]
public class MovementData : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed;

    public float GetMovementSpeed => movementSpeed;
    public float GetGroundAcceleration => groundAcceleration;
    public float GetAirAcceleration => airAcceleration;
    public float GetJumpVelocity => jumpVelocity;
    public float GetGravity => gravity;
    public float GetRotationSpeed => rotationSpeed;

}
