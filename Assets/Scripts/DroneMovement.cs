using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private GameObject yawBall;
    private Vector2 moveAxis;
    private Vector2 moveVector;
    public float speed = 5f;
    public float rotationSpeed = 1f;
    Quaternion rotationQuaternion;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        if (!rb || !playerInput) {
            Debug.LogError("Required component not found!");
        }
    }

    void Update() {
        if (playerInput.currentControlScheme == "Keyboard&Mouse") {
            Vector2 currentPosition = yawBall.transform.localPosition;
            Vector2 targetPosition = moveAxis;
            yawBall.transform.localPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

            MoveUp(moveAxis.y*0.6f);
            RotateYaw(moveAxis.x);
        } else if (playerInput.currentControlScheme == "Gamepad") {
            yawBall.transform.localPosition = moveVector;

            MoveUp(moveVector.y * 0.8f);
            RotateYaw(moveVector.x);
        }
        Debug.Log(playerInput.currentControlScheme);

        rb.MoveRotation(rotationQuaternion);
    }

    private void OnMove(InputValue inputValue) {
        Vector2 input = inputValue.Get<Vector2>();
        moveVector = new Vector2(input.x * 50, Mathf.Clamp(input.y * 125, 0, 125));
    }

    private void OnYaw(InputValue inputValue) {
        moveAxis.x = Mathf.Clamp(inputValue.Get<float>() * 49, -50, 50);
    }

    private void OnThrottle(InputValue inputValue) {
        moveAxis.y = Mathf.Clamp(inputValue.Get<float>() * 124, 0, 125);
    }

    private void MoveUp(float force) {
        rb.AddRelativeForce(new Vector3(0, force, 0));
    }

    private void RotateYaw(float yawChangeValue) {
        Quaternion targetRotation = rb.rotation * Quaternion.Euler(0f, yawChangeValue * rotationSpeed, 0f);
        rb.MoveRotation(targetRotation);
    }

    void OnPitch(InputValue inputValue) {
        rotationQuaternion.x *= inputValue.Get<float>();
    }

    void OnRoll(InputValue inputValue) {
        rotationQuaternion.z *= inputValue.Get<float>();
    }
}