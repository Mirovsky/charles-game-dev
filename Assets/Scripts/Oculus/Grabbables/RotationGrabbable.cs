using UnityEngine;

public class RotationGrabbable : BaseGrabbable
{
    [SerializeField] private Transform origin;

    [SerializeField] private float radius;
    
    /** The normal orthogonal of the disk plane*/
    [SerializeField] private Vector3 planeNormal = new Vector3(0, 0, 1);

    private void LateUpdate() {
        var pos = transform.position;
        var originPos = origin.position;
        
        var rawDistanceToHand = pos - originPos;

        var projection = Vector3.ProjectOnPlane(rawDistanceToHand, planeNormal);

        transform.position = originPos + (projection.normalized * radius);
        
        transform.rotation = Quaternion.identity;
    }
}
