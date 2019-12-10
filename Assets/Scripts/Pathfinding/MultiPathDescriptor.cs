
using UnityEngine;


public class MultiPathDescriptor : MonoBehaviour
{
    [SerializeField, Help("Starting segment of MultiPath. If MultiPath is circular, pick any PathSegment.")]
    PathSegment startingSegment;

    public PathSegment StartingSegment => startingSegment;

    void Awake()
    {
        ConnectPathSegments();
    }

    void ConnectPathSegments()
    {
        var connectorMask = LayerMask.NameToLayer("PathConnectors");

        if (startingSegment == null) {
            Debug.LogWarning("MultiPathDescriptor: Starting segments is NULL.");
            return;
        }

        var segment = startingSegment;
        while (segment != null) {
            var overlaps = GetConnectorOverlap(segment.rightConnectorCollider, connectorMask);

            if (overlaps.Length == 0) {
                break;
            }

            // We don't support connection with more than one segment.
            if (overlaps.Length > 1) {
                Debug.LogWarning($"MapCreatorRuntime: Segment has more connections than one. ({overlaps.Length})");
                break;
            }

            var nextSegment = overlaps[0].GetComponentInParent<PathSegment>();
            if (nextSegment.HasRight) {
                break;
            }

            segment.leftSegment = nextSegment;
            nextSegment.rightSegment = segment;

            segment = nextSegment;
        }
    }

    Collider[] GetConnectorOverlap(BoxCollider collider, LayerMask connectorMask)
    {
        collider.enabled = false;
        var overlaps = Physics.OverlapBox(collider.transform.position + collider.center, collider.size / 2f, Quaternion.identity, 1 << connectorMask);
        collider.enabled = true;

        return overlaps;
    }
}
