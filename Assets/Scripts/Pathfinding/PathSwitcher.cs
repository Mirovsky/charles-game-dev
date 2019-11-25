using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class PathSwitcher : MonoBehaviour
{
    [SerializeField]
    PathCreator rightPath;

    [SerializeField]
    PathCreator leftPath;

    public PathCreator GetNextPath(float direction)
    {
        return direction >= 0 ?
            rightPath :
            leftPath;
    }
}
