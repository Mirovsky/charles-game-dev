using UnityEngine;

public class VectorGrabbable : BaseGrabbable
{
    [SerializeField] private Vector3 directionCap = Vector3.up;

    private Vector3 initalPosition;

    void Start() {
        base.Start();
        initalPosition = transform.position;
    }

    private void LateUpdate() {
        var pos = transform.position;

        var toHandDis = pos - initalPosition;

        var lineAng = Vector3.Angle(directionCap, toHandDis);

        var corrected = (Mathf.Cos(lineAng * Mathf.Deg2Rad) * toHandDis.magnitude) * directionCap;

        var directionOne = initalPosition + corrected;
        var directionTwo = initalPosition - corrected;

        //We figure out which way is closest to the current position. 
        transform.position = Vector3.Distance(directionOne, pos) < Vector3.Distance(directionTwo, pos)
            ? directionOne
            : directionTwo;

        transform.rotation = Quaternion.identity;
    }
}