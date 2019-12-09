using UnityEngine;


public abstract class AbstractSegmentSwitcher : MonoBehaviour
{
    public abstract PathSegment GetNextSegment();
}
