using UnityEngine;


public class SegmentSwitcher : AbstractSegmentSwitcher
{
    [SerializeField, Help("Able to switch when entering this curve.")]
    bool switchOnEnter;
    [SerializeField, Help("Able to switch when leaving this curve.")]
    bool switchOnLeave;

    [SerializeField, Help("Segment on which this switcher sits.")]
    PathSegment segment;
    [SerializeField, Help("The other segment, which you should be able to switch to and from.")]
    PathSegment otherSegment;

    [SerializeField]
    Transform arrow;


    public override PathSegment GetNextSegment(PathSegment currentSegment, bool force = false)
    {
        if (currentSegment == segment)
            if (switchOnLeave || force)
                return otherSegment;

        if (currentSegment == otherSegment)
            if (switchOnEnter || force)
                return segment;

        return null;
    }

    public override void RotateSwitcher(PathSegment nextSegment, float distance)
    {
        if (nextSegment == null)
            return;

        arrow.rotation = nextSegment.GetRotationAtDistance(distance);
    }
}
