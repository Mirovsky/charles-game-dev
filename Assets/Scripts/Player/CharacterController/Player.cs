using System;
using UnityEngine;


public class Player : MonoBehaviour
{
    public Action<float> onMove;
    public Action onJump;
    public Action onLand;


    [Header("Jumping")]
    [SerializeField]
    float maxJumpHeight = default;
    [SerializeField]
    float timeToJumpApex = default;
    [SerializeField]
    float accelerationTimeAirborne;
    [SerializeField]
    float accelerationTimeGrounded;

    [Header("Movement")]
    [SerializeField]
    float moveSpeed;

    [Header("Pathfinding")]
    [SerializeField]
    Pathfinding pathfinding;


    float gravity;

    DeadOneRaycastController controller;

    float maxJumpVelocity;
    float yVelocity;

    bool wantsToJump;


    void Start()
    {
        controller = GetComponent<DeadOneRaycastController>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        yVelocity += gravity * Time.deltaTime;

        if (controller.Collisions.below)
            yVelocity = 0;

        if (controller.Collisions.above)
            yVelocity = gravity * Time.deltaTime;

        if (controller.Collisions.below && wantsToJump) {
            onJump?.Invoke();
            yVelocity = maxJumpVelocity;
        }

        if (controller.Collisions.landed)
            onLand?.Invoke();

        var normal = pathfinding.GetNormal().normalized;
        var direction = pathfinding.GetDirection().normalized;

        float nextStep = Direction * moveSpeed * Time.deltaTime;
        var currentPos = pathfinding.GetPosition();
        var pos = pathfinding.GetPosition(nextStep);

        var g = normal * yVelocity * Time.deltaTime;
        var d = (pos - currentPos);

        controller.Move(d, g, direction, normal, wantsToJump);

        ConfirmMove(nextStep);

        wantsToJump = false;
    }

    void ConfirmMove(float step)
    {
        if (!controller.Collisions.sides && !Mathf.Approximately(step, 0f))
            pathfinding.UpdateDistance(step);

        var attr = controller.Collisions.sides ? 0 : Direction;
        onMove?.Invoke(attr);
    }

    public float Direction { get; private set; }

    public void SetDirectionalInput (float d) {
        Direction = d;
    }

    public void OnJumpInputDown() {
        wantsToJump = true;
    }

    [System.Serializable]
    public struct CollisionInfo
    {
        public bool below, above;
        public bool left, right, sides;
    }
}
