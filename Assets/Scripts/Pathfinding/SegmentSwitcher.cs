using UnityEngine;


public class SegmentSwitcher : AbstractSegmentSwitcher
{
    [SerializeField, Help("Able to switch when entering this curve.")]
    bool switchOnEnter;
    [SerializeField, Help("Able to switch when leaving this curve.")]
    bool switchOnLeave;

    [SerializeField, Help("Segment on which this switcher sits.")]
    MultiPathDescriptor currentPath;
    [SerializeField, Help("The other segment, which you should be able to switch to and from.")]
    MultiPathDescriptor otherPath;

    [SerializeField]
    Transform arrow;


    public override MultiPathDescriptor GetNextPath(MultiPathDescriptor currentSegment, bool force = false)
    {
        if (currentSegment == currentPath)
            if (switchOnLeave || force)
                return otherPath;

        if (currentSegment == otherPath)
            if (switchOnEnter || force)
                return currentPath;

        return null;
    }

    public override void RotateSwitcher(MultiPathDescriptor nextSegment, float distance)
    {
        if (nextSegment == null)
            return;

        arrow.rotation = nextSegment.GetRotationAtDistance(distance);
    }
}
