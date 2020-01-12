using UnityEngine;
using BansheeGz.BGSpline.Curve;


public class PathSegment : MonoBehaviour
{
    public BGCurve spline;

    public Transform pathStart;
    public Transform pathEnd;

    [HideInInspector]
    public PathSegment nextSegment;
    [HideInInspector]
    public PathSegment prevSegment;
    [HideInInspector]
    public bool inversed;
    [HideInInspector]
    public bool processed;

    public bool HasPrev => prevSegment != null;
}
