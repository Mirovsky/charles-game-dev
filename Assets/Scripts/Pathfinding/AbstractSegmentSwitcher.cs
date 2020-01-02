using UnityEngine;


public abstract class AbstractSegmentSwitcher : MonoBehaviour
{
    public abstract PathSegment GetNextSegment(PathSegment currentSegment, bool force = false);

    public abstract void RotateSwitcher(PathSegment currentSegment, float distance);
}
