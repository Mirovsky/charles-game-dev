using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConnectedSegmentSwitcher : AbstractSegmentSwitcher
{
    public enum SwitcherDirection
    {
        PREV,
        NEXT
    }

    [SerializeField]
    SwitcherDirection direction;
    [SerializeField]
    PathSegment segment;


    public override PathSegment GetNextSegment()
    {
        if (direction == SwitcherDirection.PREV)
            return segment.prevPath;

        if (direction == SwitcherDirection.NEXT)
            return segment.nextPath;

        return null;
    }
}
