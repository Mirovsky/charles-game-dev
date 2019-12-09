using UnityEngine;
using PathCreation;


public class SegmentSwitcher : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    bool directSwitchOnEnter;
    [SerializeField] PathSegment segment;
    [SerializeField, Tooltip("")]
    bool directSwitchOnExit;
    // [SerializeField] PathSegment 

    public MultiPathDescriptor GetNextPath(MultiPathDescriptor currentPath, bool force)
    {
        /*// Player is on the same path as this switcher
        if (currentPath == myPath) {
            if (directSwitchOnExit || force) {
                return nextPath;
            }
        }

        // Player is on path this switcher leads to (or from)
        if (currentPath == nextPath) {
            if (directSwitchOnEnter || force) {
                return myPath;
            }
        }

        return null; */

        return null;
    }
}
