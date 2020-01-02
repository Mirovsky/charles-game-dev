using UnityEngine;

 
public class DeadOneRaycastController : MonoBehaviour
{
    public CollisionInfo Collisions;

    [SerializeField]
    Vector3 center;
    [SerializeField]
    float radius;
    [SerializeField]
    float skinWidth;
    [SerializeField]
    LayerMask collisionMask = default;

    SphereCollider sphereCollider;

    public void Move(Vector3 motion, Vector3 gravity, Vector3 direction, Vector3 normal, bool wantsToJump)
    {
        Collisions.Reset();

        var velocity = motion + gravity;
        transform.Translate(velocity);
        
        ResolveCollisions();
        CheckCollisions(direction, normal);
    }

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void ResolveCollisions()
    {
        var overlaps = Physics.OverlapSphere(transform.position, radius, collisionMask, QueryTriggerInteraction.UseGlobal);
        for (var i = 0; i < overlaps.Length; i++) {
            if (Physics.ComputePenetration(sphereCollider, transform.position, transform.rotation, overlaps[i], overlaps[i].transform.position, overlaps[i].transform.rotation, out var dir, out var dist)) {
                var penVector = dir * dist;

                transform.position = transform.position + penVector;
            }
        }
    }

    void CheckCollisions(Vector3 direction, Vector3 normal)
    {
        HorizontalCollisions(direction);
        VerticalCollisions(direction, normal);
    }

    void HorizontalCollisions(Vector3 direction)
    {
        var origin = transform.position + center;
        var fwd = origin + direction * (radius + skinWidth);
        var bwd = origin + direction * -(radius + skinWidth);

        var col = Collisions;
        col.forward = Physics.Linecast(origin, fwd, collisionMask);
        col.backward = Physics.Linecast(origin, bwd, collisionMask);
        col.sides = col.forward | col.backward;
        Collisions = col;

        Debug.DrawLine(origin, fwd, Color.green, 1f);
        Debug.DrawLine(origin, bwd, Color.yellow, 1f);
    }

    void VerticalCollisions(Vector3 direction, Vector3 normal)
    {
        var origin = transform.position + center;

        var col = Collisions;

        var normalDistance = normal * (radius + skinWidth);
        for (int i = -1; i <= 1; i++) {
            var pos = origin + direction * i * radius;

            col.above |= Physics.Linecast(pos, pos + normalDistance, collisionMask);
            Debug.DrawLine(pos, pos + normalDistance, Color.yellow);
        }

        var belowHitDistance = radius + skinWidth;
        for (int i = -1; i <= 1; i++) {
            var pos = origin + direction * i * radius;

            col.below |= Physics.Linecast(pos, pos - normalDistance, out var belowHit, collisionMask);
            belowHitDistance = Mathf.Min(belowHitDistance, belowHit.distance);
            Debug.DrawLine(pos, pos - normalDistance, Color.green);
        }

        Collisions = col;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + center, radius);
    }

    [System.Serializable]
    public struct CollisionInfo
    {
        public int faceDir;

        public bool below, above;
        public bool forward, backward;
        public bool sides;

        public void Reset()
        {
            faceDir = 0;

            below = above = false;
            forward = backward = false;
            sides = false;
        }
    }
}
