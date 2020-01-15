using System;
using UnityEngine;


public class Player : MonoBehaviour
{
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
    DeadOneCollisionsController collisions;

    float maxJumpVelocity;
    float yVelocity;

    bool wantsToJump;


    void Start()
    {
        controller = GetComponent<DeadOneRaycastController>();
        collisions = GetComponent<DeadOneCollisionsController>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        yVelocity += gravity * Time.deltaTime;

        if (collisions.Collisions.below)
            yVelocity = 0;

        if (collisions.Collisions.above)
            yVelocity = gravity * Time.deltaTime;

        if (collisions.Collisions.below && wantsToJump)
            yVelocity = maxJumpVelocity;

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
        if (!collisions.Collisions.sides && !Mathf.Approximately(step, 0f))
            pathfinding.UpdateDistance();
    }

    public float Direction { get; private set; }

    public void SetDirectionalInput (float d)
    {
        Direction = d;
    }

    public void OnJumpInputDown()
    {
        wantsToJump = true;
    }

    public void OnSwitchDown()
    {
        var currentPos = pathfinding.GetPosition();

        pathfinding.TriggerAvailablePathSwitch(true);

        var nextPos = pathfinding.GetPosition();

        controller.Move(nextPos - currentPos, Vector3.zero, Vector3.zero, Vector3.zero, false);
    }
}
