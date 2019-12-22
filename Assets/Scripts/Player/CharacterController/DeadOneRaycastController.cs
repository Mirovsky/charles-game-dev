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
        VerticalCollisions(ref gravity, normal, wantsToJump);

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

    void VerticalCollisions(ref Vector3 gravity, Vector3 normal, bool wantsToJump)
    {
        var origin = transform.position + center;
        var up = origin + normal * (radius + skinWidth);
        var down = origin + normal * -(radius + skinWidth);

        var col = Collisions;
        col.above = Physics.Linecast(origin, up, out var aboveHit, collisionMask);
        col.below = Physics.Linecast(origin, down, out var belowHit, collisionMask);
        Collisions = col;

        Debug.DrawLine(origin, up, Color.yellow, 1f);
        Debug.DrawLine(origin, down, Color.green, 1f);

        if (col.above)
            gravity = Vector3.zero;

        if (col.below && !wantsToJump)
            gravity = normal * (radius - belowHit.distance - skinWidth);
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
