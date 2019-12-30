
using System.Collections.Generic;
using UnityEngine;


public class MultiPathDescriptor : MonoBehaviour
{
    [SerializeField, Help("Starting segment of MultiPath. If MultiPath is circular, pick any PathSegment.")]
    PathSegment startingSegment;

    public PathSegment StartingSegment => startingSegment;


    void Awake() => ConnectPathSegments();

    void ConnectPathSegments()
    {
        if (startingSegment == null) {
            Debug.LogWarning("MultiPathDescriptor: Starting segments is NULL.");
            return;
        }

        var segments = GetComponentsInChildren<PathSegment>();

        var segment = startingSegment;
        while (segment != null) {
            var shouldBeInversed = false;
            var next = GetSegment(segments, segment, true, segment.inversed);
            
            if (next == null) {
                next = GetSegment(segments, segment, false, segment.inversed);
                shouldBeInversed = true;
            }

            if (next == null || next.HasPrev)
                break;

            next.inversed = shouldBeInversed;
            segment.nextSegment = next;
            next.prevSegment = segment;

            segment = next;
        }
    }

    PathSegment GetSegment(PathSegment[] segments, PathSegment current, bool fromStart = true, bool startPos = false)
    {
        var pos = startPos ? current.pathStart.position : current.pathEnd.position;

        foreach (var s in segments) {
            if (fromStart) {
                if (Vector3.Distance(s.pathStart.position, pos) <= .5f && s != current)
                    return s;
            } else {
                if (Vector3.Distance(s.pathEnd.position, pos) <= .5f && s != current)
                    return s;
            }
        }

        return null;
    }
}
