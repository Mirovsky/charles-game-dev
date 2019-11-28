using UnityEngine;


public class Player : MonoBehaviour
{
    public CollisionInfo collisions;
    
    [Header("Jumping")]
    [SerializeField] float maxJumpHeight = default;
    [SerializeField] float minJumpHeight = default;
    [SerializeField] float timeToJumpApex = default;
    [SerializeField] float accelerationTimeAirborne;
    [SerializeField] float accelerationTimeGrounded;
    
    [Header("Movement")]
    [SerializeField] float moveSpeed;

    [Header("Pathfinding")]
    [SerializeField] BezierPathfinding pathfinding;


    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float yVelocity;

    bool wantsToJump;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs (gravity) * minJumpHeight);
    }

    void Update()
    {
        yVelocity += gravity * Time.deltaTime;

        if (collisions.below) {
            yVelocity = -controller.stepOffset * Time.deltaTime;
        }
        // Solve problem when character controller is stuck to the platform from below

        if (collisions.below && wantsToJump) {
            yVelocity = maxJumpVelocity;
        }

        float nextStep = Direction * moveSpeed * Time.deltaTime;

        var normal = pathfinding.GetNormal(nextStep);
        var position = pathfinding.GetPosition(nextStep);
        var rotation = pathfinding.GetRotation(nextStep);

        var vel = normal * yVelocity * Time.deltaTime;
        var pos = transform.position;
        pos.y = 0;
        vel += position - pos;

        transform.rotation = rotation;

        var flags = controller.Move(vel);
        ResolveCollisions(flags);
        ConfirmMove(nextStep);

        wantsToJump = false;
    }

    void ConfirmMove(float step)
    {
        if (!collisions.sides) {
            pathfinding.UpdateDistance(step);
        }
    }

    void ResolveCollisions(CollisionFlags flags)
    {
        collisions.above = (flags & CollisionFlags.Above) != 0;
        collisions.below = (flags & CollisionFlags.Below) != 0;
        collisions.sides = (flags & CollisionFlags.Sides) != 0;
        collisions.left = collisions.sides && Direction < 0;
        collisions.right = collisions.sides && Direction > 0;
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
