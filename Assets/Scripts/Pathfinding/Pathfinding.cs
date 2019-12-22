using UnityEngine;


public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    PathSegment segment = default;
    [SerializeField]
    AbstractSegmentSwitcher nextSwitcher = null;

    [SerializeField]
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
        var isMoving = !Mathf.Approximately(deltaDistance, 0);
        var isAtStart = Mathf.Approximately(distance, 0);
        var isAtEnd = Mathf.Approximately(distance, segment.Length);

        if (
            (segment.inversed && isAtStart && isMoving && segment.HasNext) ||
            (!segment.inversed && isAtEnd && isMoving && segment.HasNext)
        ) {
            segment = segment.nextSegment;
        } else if (
            (segment.inversed && isAtEnd && isMoving && segment.HasPrev) ||
            (!segment.inversed && isAtStart && isMoving && segment.HasPrev)
        ) {
            segment = segment.prevSegment;
        }

        distance = segment.GetClosestDistanceAlongPath(transform.position);
    }

    public Vector3 GetPosition(float nextStep = 0)
        => segment.GetPointAtDistance(distance + nextStep);

    public Vector3 GetNormal(float nextStep = 0)
        => segment.GetNormalAtDistance(distance + nextStep);

    public Vector3 GetDirection(float nextStep = 0)
        => segment.GetDirectionAtDistance(distance + nextStep);

    public Quaternion GetRotation(float nextStep = 0)
        => segment.GetRotationAtDistance(distance + nextStep);

    public int GetSegmentRotation()
        => segment.PathDirection;
}
