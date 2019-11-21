using System;
using OOO.Base;
using OOO.Utils;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class DeadOneController : BaseNetworkBehaviour
{
    [HideInInspector] public CollisionChecks collisions;
    [HideInInspector] public Vector3 Velocity;

    public float Gravity { get; set; }
    public float JumpVelocity { get; set; }
    public float Direction { private get; set; }

    public Action OnHitBelow;
    public Action OnHitAbove;
    public Action OnHitLeft;
    public Action OnHitRight;

    [Header("Move Settings")]
    [SerializeField] float moveSpeed = default;
    [Header("Movement Dampening")]
    [SerializeField] float groundedDampen = default;
    [SerializeField] float airbornDampen = default;
    [Space]
    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = default;
    [SerializeField] float timeToJumpApex = default;
    [SerializeField] bool enableMultiJump = default;
    [SerializeField] int multiJumpCount = 2;
    [Space]
    [Header("Slam Settings")]
    [SerializeField] bool slamEnabled = default;

    [Header("BorderChecks")]
    [SerializeField] SingleUnityLayer groundLayer = default;
    [SerializeField] Transform aboveCheck = default;
    [SerializeField] Transform rightCheck = default;
    [SerializeField] Transform belowCheck = default;
    [SerializeField] Transform leftCheck = default;

    Rigidbody rb;
    int jumpCount = 0;
    float moveSmoothing = 0;

    public void Jump()
    {
        if (collisions.below || (enableMultiJump && jumpCount < multiJumpCount)) {
            Velocity.y = JumpVelocity;

            jumpCount += 1;
        }

        rb.velocity = Velocity;
    }

    public void Slam()
    {
        if (slamEnabled && !collisions.below) {
            Velocity.x = 0;
            Velocity.y = -JumpVelocity;
        }

        rb.velocity = Velocity;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        Gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        JumpVelocity = Mathf.Abs(Gravity) * timeToJumpApex;

        OnHitBelow += HitBelow;
    }

    void FixedUpdate()
    {
        if (!hasAuthority || !IsMobilePlayer) {
            return;
        }

        HandleCollisions();
        MoveCharacter();

        Velocity.y += Gravity * Time.deltaTime;

        rb.velocity = Velocity;
    }

    void MoveCharacter()
    {
        float targetXVelocity = Direction * moveSpeed * Time.deltaTime;

        Velocity.x = Mathf.SmoothDamp(
            Velocity.x,
            targetXVelocity,
            ref moveSmoothing,
            collisions.below ? groundedDampen : airbornDampen
        );
    }

    void HandleCollisions()
    {
        var prev = collisions;

        var position = rb.position;
        collisions.above = Physics.Linecast(position, aboveCheck.position, groundLayer.Mask);
        collisions.below = Physics.Linecast(position, belowCheck.position, groundLayer.Mask);
        collisions.left = Physics.Linecast(position, leftCheck.position, groundLayer.Mask);
        collisions.right = Physics.Linecast(position, rightCheck.position, groundLayer.Mask);

        if (!prev.below && collisions.below) OnHitBelow?.Invoke();
        if (!prev.above && collisions.above) OnHitAbove?.Invoke();
        if (!prev.left && collisions.left) OnHitLeft?.Invoke();
        if (!prev.right && collisions.right) OnHitRight?.Invoke();
    }

    void HitBelow()
    {
        jumpCount = 0;
    }

    [System.Serializable]
    public struct CollisionChecks
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
