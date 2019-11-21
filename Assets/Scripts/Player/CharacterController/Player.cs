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

    float velocityXSmoothing;
    float velocityZSmoothing;

    bool wantsToJump;

    CharacterController controller;

    float direction;

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

        pathfinding.UpdateDistance(direction * moveSpeed * Time.deltaTime);
        var newPosDelta = pathfinding.GetPosition() - transform.position;

        newPosDelta.y = yVelocity * Time.deltaTime;

        transform.rotation = pathfinding.GetRotation();

        var flags = controller.Move(newPosDelta);
        ResolveCollisions(flags);

        wantsToJump = false;
    }

    void ResolveCollisions(CollisionFlags flags)
    {
        collisions.above = (flags & CollisionFlags.Above) != 0;
        collisions.below = (flags & CollisionFlags.Below) != 0;

        /* collisions.left = (flags & CollisionFlags.Sides) != 0;
        collisions.right = (flags & CollisionFlags.Above) != 0; */
    }

    public void SetDirectionalInput (float d) {
        direction = d;
    }

    public void OnJumpInputDown() {
        wantsToJump = true;
    }

    [System.Serializable]
    public struct CollisionInfo
    {
        public bool below, above;
        public bool left, right;
    }
}
