using UnityEngine;


public class SingleSegmentSwitcher : AbstractSegmentSwitcher
{
    [SerializeField]
    MultiPathDescriptor otherSegment;

    public override MultiPathDescriptor GetNextPath(MultiPathDescriptor currentSegment, bool force = false)
        => otherSegment;

    public override void RotateSwitcher(MultiPathDescriptor currentSegment, float distance) { }
}
