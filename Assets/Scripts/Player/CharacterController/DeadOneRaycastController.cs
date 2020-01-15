using UnityEngine;

 
public class DeadOneRaycastController : MonoBehaviour
{
    [SerializeField]
    float radius;
    [SerializeField]
    LayerMask collisionMask = default;

    SphereCollider sphereCollider;

    public void Move(Vector3 motion, Vector3 gravity, Vector3 direction, Vector3 normal, bool wantsToJump)
    {
        var velocity = motion + gravity;
        transform.Translate(velocity);
        
        ResolveCollisions();
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
}
