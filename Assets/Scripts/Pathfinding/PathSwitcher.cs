using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class PathSwitcher : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    bool directSwitchOnEnter;
    [SerializeField]
    PathCreator myPath;
    [SerializeField, Tooltip("")]
    bool directSwitchOnExit;
    [SerializeField]
    PathCreator nextPath;

    public PathCreator GetNextPath(PathCreator currentPath, bool force)
    {
        // Player is on the same path as this switcher
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

        return null;
    }
}
