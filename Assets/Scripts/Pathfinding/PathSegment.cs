using UnityEngine;
using BansheeGz.BGSpline.Curve;


public class PathSegment : MonoBehaviour
{
    public BGCurve spline;

    public Transform pathStart;
    public Transform pathEnd;

    public PathSegment nextSegment;
    public PathSegment prevSegment;
    public bool inversed;

    public bool processed;

    public bool HasPrev => prevSegment != null;
}
