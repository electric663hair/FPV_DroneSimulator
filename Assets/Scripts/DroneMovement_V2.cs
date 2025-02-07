using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovement_V2 : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;

    void Awake() {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    Vector2 currentRotation = Vector2.zero; // Current X, Z tilt
    Vector2 velocity = Vector2.zero; // Velocity for smooth motion
    Vector2 targetRotation = Vector2.zero;

    float maxTilt = 45f;
    float springStrength = 200f;
    float damping = 10f;
    float deadZone = 0.35f;

    void OnLook(InputValue inputValue) {
        Vector2 inputVector = inputValue.Get<Vector2>();
        targetRotation = new Vector2(inputVector.y * maxTilt, inputVector.x * -maxTilt);
    }

    void Update() {
        // Spring physics: acceleration = (target - current) * spring strength
        Vector2 acceleration = (targetRotation - currentRotation) * springStrength;

        // Apply acceleration to velocity
        velocity += acceleration * Time.deltaTime;

        // Apply damping to velocity (exponential decay)
        velocity *= Mathf.Exp(-damping * Time.deltaTime);

        // Update rotation
        currentRotation += velocity * Time.deltaTime;

        // Apply dead zone: If rotation is small enough, snap it to zero
        if (Mathf.Abs(currentRotation.x) < deadZone) currentRotation.x = 0;
        if (Mathf.Abs(currentRotation.y) < deadZone) currentRotation.y = 0;

        // Clamp rotation to prevent exceeding tiltStrength
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxTilt, maxTilt);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxTilt, maxTilt);

        // Apply rotation to the object
        transform.rotation = Quaternion.Euler(currentRotation.x, 0, currentRotation.y);
    }

}
