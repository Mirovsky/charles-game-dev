using System.Collections;
using System.Collections.Generic;
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


    public void Move(Vector3 motion, Vector3 gravity, Vector3 direction, Vector3 normal, bool wantsToJump)
    {
        Collisions.Reset();

        HorizontalCollisions(ref motion, direction);
        VerticalCollisions(ref gravity, normal, direction, wantsToJump);

        transform.Translate(motion + gravity);
    }

    void HorizontalCollisions(ref Vector3 moveAmount, Vector3 direction)
    {
        var origin = transform.position + center;
        var fwd = origin + direction * (radius + skinWidth);
        var bwd = origin + direction * -(radius + skinWidth);

        var col = Collisions;
        col.forward = Physics.Linecast(origin, fwd, collisionMask);
        col.backward = Physics.Linecast(origin, bwd, collisionMask);
        Collisions = col;

        Debug.DrawLine(origin, fwd, Color.green, 1f);
        Debug.DrawLine(origin, bwd, Color.yellow, 1f);

        if (col.forward || col.backward)
            moveAmount = Vector3.zero;
    }

    void VerticalCollisions(ref Vector3 gravity, Vector3 normal, Vector3 direction, bool wantsToJump)
    {
        var origin = transform.position + center;

        var col = Collisions;

        var normalDistance = normal * (radius + skinWidth);
        for (int i = -1; i <= 1; i++) {
            var pos = origin + direction * i * radius;

            col.above |= Physics.Linecast(pos, pos + normalDistance, collisionMask);
            Debug.DrawLine(pos, pos + normalDistance, Color.yellow, 1f);
        }

        var belowHitDistance = radius + skinWidth;
        for (int i = -1; i <= 1; i++) {
            var pos = origin + direction * i * radius;

            col.below |= Physics.Linecast(pos, pos - normalDistance, out var belowHit, collisionMask);
            belowHitDistance = Mathf.Min(belowHitDistance, belowHit.distance);
            Debug.DrawLine(pos, pos - normalDistance, Color.green, 1f);
        }

        Collisions = col;

        if (col.above)
            gravity = Vector3.zero;

        if (col.below && !wantsToJump)
            gravity = normal * (radius - belowHitDistance - skinWidth);
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
