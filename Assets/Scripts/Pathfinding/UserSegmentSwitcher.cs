using UnityEngine;


public class UserSegmentSwitcher : AbstractSegmentSwitcher
{
    [SerializeField, Help("Segment on which this switcher sits.")]
    PathSegment segment;
    [SerializeField, Help("The other segment, which you should be able to switch to and from.")]
    PathSegment otherSegment;


    public override PathSegment GetNextSegment(PathSegment currentSegment)
    {
        if (currentSegment == segment) {
            return otherSegment;
        }

        if (currentSegment == otherSegment) {
            return segment;
        }

        return null;
    }
}
