using System.Linq;
using UnityEngine;
using PathCreation;


public class MultiPathDescriptor : MonoBehaviour
{
    void Awake()
    {
        var segments = GetComponentsInChildren<PathSegment>();

        ConnectPathSegments(segments);
    }

    void ConnectPathSegments(PathSegment[] segments)
    {
        var connectorMask = LayerMask.NameToLayer("PathConnectors");

        foreach (var segment in segments)
        {
            var collider = segment.nextConnectorCollider;
            collider.enabled = false;
            var overlaps = Physics.OverlapBox(collider.transform.position + collider.center, collider.size / 2f, Quaternion.identity, 1 << connectorMask);
            collider.enabled = true;

            // We don't need overlap since it can be last segment.
            if (overlaps.Length == 0)
                continue;

            // We don't support connection with more than one segment.
            if (overlaps.Length > 1)
            {
                Debug.LogWarning($"MapCreatorRuntime: Segment has more connections than one. ({overlaps.Length})");
                continue;
            }

            var nextSegment = overlaps[0].GetComponentInParent<PathSegment>();
            nextSegment.prevPath = segment;
            segment.nextPath = nextSegment;
        }
    }
}
