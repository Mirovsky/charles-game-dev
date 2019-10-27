using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] float moveSpeed = default;
    [Space]
    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = default;
    [SerializeField] float timeToJumpApex = default;
    [SerializeField] bool enableMultiJump = default;
    [SerializeField] int multiJumpCount = 2;
    [Space]
    [Header("Dampening")]
    [SerializeField] float groundedDampen = default;
    [SerializeField] float airbornDampen = default;
    [Space]
    [Header("BorderChecks")]
    [SerializeField] Transform aboveCheck = default;
    [SerializeField] Transform rightCheck = default;
    [SerializeField] Transform belowCheck = default;
    [SerializeField] Transform leftCheck = default;
    
    [Header("Debug Info")]
    [SerializeField] CollisionChecks collisions;

    PlayerInputActions playerInput;

    Rigidbody rb;

    float gravity;
    float jumpVelocity;
    float moveSmoothing;

    int jumpCount = 0;

    [System.Serializable]
    public struct CollisionChecks
    {
        public bool above, below;
        public bool left, right;
    }

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
        HandleCollisions();

        var moveDirection = playerInput.Player.Movement.ReadValue<float>();

        float targetXVelocity = moveDirection * moveSpeed * Time.deltaTime;

        var velocity = rb.velocity;
        velocity.x = Mathf.SmoothDamp(
            velocity.x,
            targetXVelocity,
            ref moveSmoothing,
            collisions.below ? groundedDampen : airbornDampen
        );
        velocity.y += gravity * Time.deltaTime;
        rb.velocity = velocity;
    }

    void OnJump()
    {
        var velocity = rb.velocity;
        
        if (collisions.below || (enableMultiJump && jumpCount < multiJumpCount)) {
            velocity.y = jumpVelocity;

            jumpCount += 1;
        }

        rb.velocity = velocity;
    }

    void HandleCollisions()
    {
        var prev = collisions;

        collisions.above = Physics.Linecast(transform.position, aboveCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        collisions.below = Physics.Linecast(transform.position, belowCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        collisions.left = Physics.Linecast(transform.position, leftCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        collisions.right = Physics.Linecast(transform.position, rightCheck.position, 1 << LayerMask.NameToLayer("Ground"));
    
        if (!prev.below && collisions.below) HitBelow();
        if (!prev.above && collisions.above) HitAbove();
        if (!prev.left && collisions.left) HitLeft();
        if (!prev.right && collisions.right) HitRight();
    }

    void HitBelow() {
        jumpCount = 0;
    }

    void HitAbove() {}
    
    void HitLeft() {}
    
    void HitRight() {}
}
