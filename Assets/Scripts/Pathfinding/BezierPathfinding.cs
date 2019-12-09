using UnityEngine;
using PathCreation;


public class BezierPathfinding : MonoBehaviour
{
    readonly EndOfPathInstruction endOfPath = EndOfPathInstruction.Stop;
    
    [SerializeField] PathSegment segment = default;
    [SerializeField] AbstractSegmentSwitcher nextSwitcher = null;

    float distance;

    void Start()
    {
        // TODO: set default path instead of find.
        segment = GameObject.Find("SegmentStraight01")
            .GetComponent<PathSegment>();
    }

    void OnTriggerEnter(Collider other)
    {
        /* if (other.CompareTag("PathSwitcher")) {
            nextSwitcher = other.GetComponent<AbstractSegmentSwitcher>();
        } */
    }

    void OnTriggerExit(Collider other)
    {
        nextSwitcher = null;
    }

    public void TriggerAvailablePathSwitch(bool force = false)
    {
        /* if (nextSwitcher == null)
            return;

        var nextPath = nextSwitcher.GetNextPath(segment.path, force);
        if (nextPath == null)
            return;

        pathCreator = nextPath;
        distance = pathCreator.path.GetClosestDistanceAlongPath(transform.position); */
    }

    public void UpdateDistance(float direction)
    {
        distance += direction;

        Debug.Log($"{distance} / {segment.Length} - Next: {segment.HasNext}, Prev: {segment.HasPrev}");
        if (distance >= segment.Length && segment.HasNext) {
            segment = segment.nextPath;
            distance = segment.path.path.GetClosestDistanceAlongPath(transform.position);
        } else if (distance <= 0 && segment.HasPrev) {
            segment = segment.prevPath;
            distance = segment.path.path.GetClosestDistanceAlongPath(transform.position);
        }

    }

    public Vector3 GetPosition(float nextStep = 0)
    {
        var pos = segment.path.path.GetPointAtDistance(distance + nextStep, endOfPath);
        pos.y = 0;
        return pos;
    }

    public Vector3 GetNormal(float nextStep = 0)
    {
        return segment.path.path.GetNormalAtDistance(distance + nextStep, endOfPath);
    }

    public Vector3 GetDirection(float nextStep = 0)
    {
        return segment.path.path.GetDirectionAtDistance(distance + nextStep, endOfPath);
    }

    public Quaternion GetRotation(float nextStep = 0)
    {
        return segment.path.path.GetRotationAtDistance(distance + nextStep, endOfPath);
    }
}
