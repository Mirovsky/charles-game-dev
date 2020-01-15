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
        var prevBelow = Collisions.below;
        Collisions.Reset();

        var velocity = motion + gravity;
        var prev = transform.position;
        transform.Translate(velocity);
        
        ResolveCollisions();
        CheckCollisions(direction, normal, prevBelow);
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

    void CheckCollisions(Vector3 direction, Vector3 normal, bool prevBelow)
    {
        HorizontalCollisions(direction, normal);
        VerticalCollisions(direction, normal, prevBelow);
    }

    void HorizontalCollisions(Vector3 direction, Vector3 normal)
    {
        var origin = transform.position + center;

        var col = Collisions;

        var fwd = direction * (radius + skinWidth);
        for (int i = 0; i < 1; i++) {
            var pos = origin + normal * i * radius;

            col.forward |= Physics.Linecast(pos, pos + fwd, collisionMask);
            Debug.DrawLine(pos, pos + fwd, Color.yellow);
        }

        for (int i = 0; i < 1; i++) {
            var pos = origin + normal * i * radius;
            
            col.backward |= Physics.Linecast(pos, pos - fwd, collisionMask);
            Debug.DrawLine(pos, pos - fwd, Color.green);
        }
        col.sides = col.forward | col.backward;

        Collisions = col;
    }

    void VerticalCollisions(Vector3 direction, Vector3 normal, bool prevBelow)
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
        col.landed = !prevBelow && col.below;
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
        public bool landed;

        public void Reset()
        {
            faceDir = 0;

            below = above = false;
            forward = backward = false;
            sides = false;
            landed = false;
        }
    }
}
