using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField]
    float moveSpeed;
    [Space]
    [Header("Jump Settings")]
    [SerializeField]
    float jumpHeight;
    [SerializeField]
    float timeToJumpApex;
    [Space]
    [Header("Dampening")]
    [SerializeField]
    float groundedDampen;
    [SerializeField]
    float airbornDampen;

    PlayerInputActions playerInput;

    Rigidbody rb;

    float gravity;
    float jumpVelocity;
    float moveSmoothing;

    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Awake()
    {
        playerInput = new PlayerInputActions();

        rb = GetComponent<Rigidbody>();
        // Need to disable gravity so we can calculate our own but world gravity still works as expected
        rb.useGravity = false;
        rb.isKinematic = false;

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void FixedUpdate()
    {
        var moveDirection = playerInput.Player.Movement.ReadValue<float>();

        float targetXVelocity = moveDirection * moveSpeed * Time.deltaTime;

        var velocity = rb.velocity;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetXVelocity, ref moveSmoothing, groundedDampen);
        velocity.y += gravity * Time.deltaTime;
        rb.velocity = velocity;
    }

    void OnJump()
    {
        var velocity = rb.velocity;
        velocity.y = jumpVelocity;
        rb.velocity = velocity;
    }
}
