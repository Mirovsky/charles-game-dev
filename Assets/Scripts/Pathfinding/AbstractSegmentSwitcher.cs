using UnityEngine;


public abstract class AbstractSegmentSwitcher : MonoBehaviour
{
    public abstract MultiPathDescriptor GetNextPath(MultiPathDescriptor currentSegment, bool force = false);

    public abstract void RotateSwitcher(MultiPathDescriptor currentSegment, float distance);
}
