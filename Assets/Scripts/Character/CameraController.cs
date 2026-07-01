using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 followOffset;
    [SerializeField] private float followDistance;
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private float followSpeed;
    [SerializeField] private float cameraSensitivty;
    [SerializeField] private float pitchLimit;

    private Vector3 defaultPosition;
    private float rotationY;
    private float rotationX;

    private Vector2 lookInput;

    private void Awake()
    {
        defaultPosition = cameraObject.transform.localPosition;
        defaultPosition.z = -followDistance; // Set the default position based on follow distance
    }

    private void Update()
    {
        HandleCameraRotation();
        HandleCameraMovement();
        HandleCameraCollision();
    }

    private void HandleCameraRotation()
    {
        rotationY -= lookInput.y * cameraSensitivty * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -pitchLimit, pitchLimit);

        rotationX += lookInput.x * cameraSensitivty * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
    }

    private void HandleCameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.position + followOffset, followSpeed * Time.deltaTime);
    }

    private void HandleCameraCollision()
    {
        cameraObject.transform.localPosition = CheckForObstructions();
    }

    private Vector3 CheckForObstructions()
    {
        Vector3 desiredCameraPosition = defaultPosition;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.forward, out hit, followDistance, ~ignoreMask))
        {
            desiredCameraPosition = transform.InverseTransformPoint(hit.point);
            desiredCameraPosition.z += 0.5f; // Add a small offset to avoid clipping
        }
        return desiredCameraPosition;
    }

    public void SetLookInput(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}
