using System;
using UnityEngine;

public class DeadOneCollisionsController : MonoBehaviour
{
    public Action<float> onMove;
    public Action onJump;
    public Action onLand;


    public CollisionInfo Collisions;

    [SerializeField]
    Vector3 center;
    [SerializeField]
    float radius;
    [SerializeField]
    float skinWidth;
    [SerializeField]
    LayerMask collisionMask = default;
    [SerializeField]
    Transform raycastTransform;

    Vector3 prevPosition = Vector3.zero;

    void Start()
    {
        prevPosition = transform.position;
    }

    void Update()
    {
        var deltaDir = transform.position - prevPosition;
        var delta = Vector3.Project(deltaDir, raycastTransform.forward).magnitude;
        prevPosition = transform.position;

        var prev = Collisions;
        Collisions.Reset();

        CheckCollisions(raycastTransform.forward, raycastTransform.up);

        if (prev.below && !Collisions.below)
            onJump?.Invoke();

        if (!prev.below && Collisions.below)
            onLand?.Invoke();

        onMove?.Invoke(Mathf.Approximately(delta, 0) ? 0 : Mathf.Sign(delta));
    }

    void CheckCollisions(Vector3 direction, Vector3 normal)
    {
        HorizontalCollisions(direction, normal);
        VerticalCollisions(direction, normal);
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

    void VerticalCollisions(Vector3 direction, Vector3 normal)
    {
        var origin = transform.position + center;

        var col = Collisions;

        var normalDistance = normal * (radius + skinWidth);
        for (int i = -1; i <= 1; i++) {
            var pos = origin + direction * i * radius;

            col.above |= Physics.Linecast(pos, pos + normalDistance, collisionMask);
            Debug.DrawLine(pos, pos + normalDistance, Color.yellow);
        }

        var belowHitDistance = radius + skinWidth;
        for (int i = -1; i <= 1; i++) {
            var pos = origin + direction * i * radius;

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
