using UnityEngine;
using PathCreation;


public class Pathfinding : MonoBehaviour
{
    [SerializeField] PathSegment segment = default;
    [SerializeField] AbstractSegmentSwitcher nextSwitcher = null;

    float distance;

    void Awake()
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
        distance += deltaDistance * segment.PathDirection;
        distance = Mathf.Clamp(distance, 0, segment.Length);

        if (segment.inversed) {
            if (Mathf.Approximately(distance, 0) && !Mathf.Approximately(deltaDistance, 0) && segment.HasNext) {
                segment = segment.nextSegment;
                distance = segment.GetClosestDistanceAlongPath(transform.position);
            } else if (Mathf.Approximately(distance, segment.Length) && !Mathf.Approximately(deltaDistance, 0) && segment.HasPrev) {
                segment = segment.prevSegment;
                distance = segment.GetClosestDistanceAlongPath(transform.position);
            }
        } else {
            if (Mathf.Approximately(distance, segment.Length) && !Mathf.Approximately(deltaDistance, 0) && segment.HasNext) {
                segment = segment.nextSegment;
                distance = segment.GetClosestDistanceAlongPath(transform.position);
            } else if (Mathf.Approximately(distance, 0) && !Mathf.Approximately(deltaDistance, 0) && segment.HasPrev) {
                segment = segment.prevSegment;
                distance = segment.GetClosestDistanceAlongPath(transform.position);
            }
        }
    }

    public Vector3 GetPosition(float nextStep = 0)
    {
        var pos = segment.GetPointAtDistance(distance + nextStep * segment.PathDirection);
        pos.y = 0;
        return pos;
    }

    public Vector3 GetNormal(float nextStep = 0)
    {
        return segment.GetNormalAtDistance(distance + nextStep * segment.PathDirection);
    }

    public Vector3 GetDirection(float nextStep = 0)
    {
        return segment.GetDirectionAtDistance(distance + nextStep * segment.PathDirection);
    }

    public Quaternion GetRotation(float nextStep = 0)
    {
        return segment.GetRotationAtDistance(distance + nextStep * segment.PathDirection);
    }
}
