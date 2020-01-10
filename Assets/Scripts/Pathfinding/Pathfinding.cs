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
        /* segment = FindObjectOfType<DefaultMultiPathDescriptor>()
            .DefaultMultiPath
            .StartingSegment; */
    }

    void OnTriggerEnter(Collider other)
    {
        /* if (other.CompareTag("PathSwitcher")) {
            nextSwitcher = other.GetComponent<AbstractSegmentSwitcher>();
        } */
    }

    void OnTriggerExit(Collider other)
    {
        // nextSwitcher = null;
    }

    public void SetNextSwitcher(AbstractSegmentSwitcher switcher)
    {
        // nextSwitcher = switcher;
    }

    public void TriggerAvailablePathSwitch(bool force = false)
    {
        /* if (nextSwitcher == null)
            return;

        var nextSegment = nextSwitcher.GetNextSegment(segment, force);
        if (nextSegment == null)
            return;

        if (nextSegment.multiPath == segment.multiPath)
            return;

        segment = nextSegment;
        distance = nextSegment.GetClosestDistanceAlongPath(transform.position);

        nextSwitcher.RotateSwitcher(segment, distance); */
    }

    public void UpdateDistance(float deltaDistance)
    {
        // segment = GetNextSegment(segment, deltaDistance);

        // distance = segment.GetClosestDistanceAlongPath(transform.position);
    }

    public Vector3 GetPosition(float nextStep = 0)
    {
        /* if (nextStep == 0)
            return segment.GetPointAtDistance(distance);

        var nextSegment = GetNextSegment(segment, nextStep * GetSegmentDirection());
        var nextDistance = nextSegment.GetClosestDistanceAlongPath(transform.position);

        return nextSegment.GetPointAtDistance(nextDistance + (nextStep * nextSegment.PathDirection)); */

        return Vector3.zero;
    }

    public Vector3 GetNormal(float nextStep = 0)
        => Vector3.zero; // segment.GetNormalAtDistance(distance + (nextStep * GetSegmentDirection()));

    public Vector3 GetDirection(float nextStep = 0)
        => Vector3.zero;
    // => segment.GetDirectionAtDistance(distance + (nextStep * GetSegmentDirection()));

    public Quaternion GetRotation(float nextStep = 0)
        => Quaternion.identity;
    // => segment.GetRotationAtDistance(distance + (nextStep * GetSegmentDirection()));

    public int GetSegmentDirection()
        => 1;
        // => segment.PathDirection;

    public PathSegment GetCurrentSegment()
        => null;

    PathSegment GetNextSegment(PathSegment s, float deltaDistance)
    {
        /* var isMoving = !Mathf.Approximately(deltaDistance, 0);
        var isAtStart = (distance) < 0;
        var isAtEnd = (distance) > s.Length;

        if (
            (s.inversed && isAtStart && isMoving && s.HasNext) ||
            (!s.inversed && isAtEnd && isMoving && s.HasNext)
        ) {
            return s.nextSegment;
        } else if (
            (s.inversed && isAtEnd && isMoving && s.HasPrev) ||
            (!s.inversed && isAtStart && isMoving && s.HasPrev)
        ) {
            return s.prevSegment;
        }

        return s; */

        return null;
    }
}
