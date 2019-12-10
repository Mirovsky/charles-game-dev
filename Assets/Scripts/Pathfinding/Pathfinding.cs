using UnityEngine;
using PathCreation;


public class Pathfinding : MonoBehaviour
{
    readonly EndOfPathInstruction endOfPath = EndOfPathInstruction.Stop;
    
    [SerializeField] PathSegment segment = default;
    [SerializeField] AbstractSegmentSwitcher nextSwitcher = null;

    float distance;

    void Start()
    {
        segment = FindObjectOfType<DefaultMultiPathDescriptor>()
            .DefaultMultiPath
            .StartingSegment;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PathSwitcher")) {
            nextSwitcher = other.GetComponent<AbstractSegmentSwitcher>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        nextSwitcher = null;
    }

    public void SetNextSwitcher(AbstractSegmentSwitcher switcher)
    {
        nextSwitcher = switcher;
    }

    public void TriggerAvailablePathSwitch(bool force = false)
    {
        if (nextSwitcher == null)
            return;

        var nextSegment = nextSwitcher.GetNextSegment(segment);
        if (nextSegment == null)
            return;

        segment = nextSegment;
        distance = nextSegment.currentPath.path.GetClosestDistanceAlongPath(transform.position);
    }

    public void UpdateDistance(float deltaDistance)
    {
        var newDistance = distance + deltaDistance;

        if (distance >= segment.Length && segment.HasRight && deltaDistance > 0) {
            segment = segment.rightSegment;
            distance = segment.currentPath.path.GetClosestDistanceAlongPath(transform.position);
        } else if (distance <= 0 && segment.HasLeft && deltaDistance < 0) {
            segment = segment.leftSegment;
            distance = segment.currentPath.path.GetClosestDistanceAlongPath(transform.position);
        } else {
            distance = newDistance;
        }
    }

    public Vector3 GetPosition(float nextStep = 0)
    {
        var pos = segment.currentPath.path.GetPointAtDistance(distance + nextStep, endOfPath);
        pos.y = 0;
        return pos;
    }

    public Vector3 GetNormal(float nextStep = 0)
    {
        return segment.currentPath.path.GetNormalAtDistance(distance + nextStep, endOfPath);
    }

    public Vector3 GetDirection(float nextStep = 0)
    {
        return segment.currentPath.path.GetDirectionAtDistance(distance + nextStep, endOfPath);
    }

    public Quaternion GetRotation(float nextStep = 0)
    {
        return segment.currentPath.path.GetRotationAtDistance(distance + nextStep, endOfPath);
    }
}
