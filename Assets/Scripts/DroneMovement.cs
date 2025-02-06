using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private GameObject yawBall;
    private Vector2 move;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        if (!rb || !playerInput) {
            Debug.LogError("Required component not found!");
        }
    }

    void Update() {
        yawBall.transform.localPosition = move;
    }

    private void OnYaw(InputValue inputValue) {
        move.x = Mathf.Clamp(inputValue.Get<float>() * 50, -50, 50);
    }

    private void OnThrottle(InputValue inputValue) {
        move.y = Mathf.Clamp(inputValue.Get<float>() * 125, 0, 125);
    }
}
