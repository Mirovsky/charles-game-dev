using UnityEngine;

public class VectorGrabbable : BaseGrabbable
{
    public float velocity;

    [HideInInspector]
    public Vector3 clampLeft, clampRight;
    
    [SerializeField]
    [Tooltip("Axis on which movement is restricted (in world space).")]
    private Vector3 directionCap = Vector3.up;

    [SerializeField] private float maxDistance;

    private Vector3 initialPosition;

    void Start() {
        base.Start();
    
        initialPosition = transform.position;

        clampLeft = initialPosition - directionCap * maxDistance;
        clampRight = initialPosition + directionCap * maxDistance;
    }

    /** @return the position that is corrected to be in the vector line .*/
    public Vector3 GetCorrectedPosition(Vector3 pos) {
        var toHandDis = pos - initialPosition;

        var lineAng = Vector3.Angle(directionCap, toHandDis);

        var corrected = (Mathf.Cos(lineAng * Mathf.Deg2Rad) * toHandDis.magnitude) * directionCap;

        var directionOne = initialPosition + corrected;
        var directionTwo = initialPosition - corrected;

        //We figure out which way is closest to the current position. 
        var newPosition = Vector3.Distance(directionOne, pos) < Vector3.Distance(directionTwo, pos)
            ? directionOne
            : directionTwo;

        velocity = (pos - newPosition).magnitude;

        var perc = InverseLerp(clampLeft, clampRight, newPosition);
        if (perc <= 0)
            return clampLeft;
        if (perc >= 1)
            return clampRight;

        return newPosition;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);

        velocity = 0;
    }

    public float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}