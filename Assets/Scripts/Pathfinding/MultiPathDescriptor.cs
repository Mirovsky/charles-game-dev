using System;
using UnityEngine;

using BansheeGz.BGSpline.Curve;
using BansheeGz.BGSpline.Components;


public class MultiPathDescriptor : MonoBehaviour
{
    static class Keys
    {
        public const string NORMAL_FIELD = "Normal";
    }


    [SerializeField, Help("Starting segment of MultiPath. If MultiPath is circular, pick any PathSegment.")]
    PathSegment startingSegment;

    BGCurve spline;
    BGCcMath math;
    BGCcCursor cursor;


    public float Length
        => math.GetDistance();

    public bool Closed
        => spline.Closed;

    public Vector3 GetPoint(float distance)
        => math.CalcPositionByDistance(distance);

    public Vector3 GetTangent(float distance)
        => math.CalcTangentByDistance(distance);

    public Vector3 GetNormal(float distance)
    {
        cursor.Distance = distance;
        return cursor.LerpVector(Keys.NORMAL_FIELD);
    }

    public Vector3 GetClosestPointAndDistanceByPoint(Vector3 worldPosition, out float distance)
        => math.CalcPositionByClosestPoint(worldPosition, out distance);

    public Quaternion GetRotationAtDistance(float distance)
        => Quaternion.LookRotation(GetTangent(distance), GetNormal(distance));


    void Awake()
    {
        if (startingSegment == null) {
            Debug.LogWarning("MultiPathDescriptor: Starting segments is NULL.");
            return;
        }

        var segments = GetComponentsInChildren<PathSegment>();
        OrderSegments(segments);
        CreateSpline();
    }

    void OrderSegments(PathSegment[] segments)
    {
        var segment = startingSegment;
        while (segment != null) {
            var shouldBeInversed = false;
            var next = GetSegment(segments, segment, true, segment.inversed);

            if (next == null) {
                next = GetSegment(segments, segment, false, segment.inversed);
                shouldBeInversed = true;
            }

            if (next == null || next.HasPrev)
                break;

            next.inversed = shouldBeInversed;
            segment.nextSegment = next;
            next.prevSegment = segment;

            segment = next;
        }
    }

    void CreateSpline()
    {
        var splineObject = new GameObject("MultipathSpline");
        var splineTransform = splineObject.transform;
        splineTransform.SetParent(transform);

        CreateSplineComponents(splineObject);

        BGCurvePointI firstPoint = null, lastPoint = null;
        var segment = startingSegment;
        while (segment != null) {
            var isFirstSegment = (segment == startingSegment);
            var isLastSegment = (segment.nextSegment == startingSegment || segment.nextSegment == null);
            var isClosed = isLastSegment && (segment.nextSegment == startingSegment);

            var s = segment.spline;
            if (!segment.inversed) {
                for (var i = 0; i < s.PointsCount; i++) {
                    var p = s.Points[i];

                    var isFirstPoint = (i == 0);
                    var isLastPoint = (i == s.PointsCount - 1);

                    CopyPointToSpline(p, isFirstPoint, isLastPoint, isFirstSegment, isLastSegment, isClosed, segment.inversed, s, ref firstPoint, ref lastPoint);
                }
            } else {
                for (var i = s.PointsCount - 1; i >= 0; i--) {
                    var p = s.Points[i];

                    var isFirstPoint = (i == s.PointsCount - 1);
                    var isLastPoint = (i == 0);

                    CopyPointToSpline(p, isFirstPoint, isLastPoint, isFirstSegment, isLastSegment, isClosed, segment.inversed, s, ref firstPoint, ref lastPoint);
                }
            }

            // Destroy(segment.spline.gameObject);
            segment.processed = true;
            segment = segment.nextSegment;

            if (isClosed) {
                spline.Closed = true;
                return;
            }
        }
    }

    void CopyPointToSpline(
        BGCurvePointI p,
        bool isFirstPoint, bool isLastPoint,
        bool isFirstSegment, bool isLastSegment,
        bool isClosed,
        bool inversed,
        BGCurve originalCurve,
        ref BGCurvePointI firstPoint,
        ref BGCurvePointI lastPoint
    ) {
        if (isFirstSegment) {
            AddSplinePoint(p, originalCurve, ref lastPoint);

            if (isFirstPoint) {
                firstPoint = lastPoint;
            }

            return;
        }

        if (isFirstPoint && !isFirstSegment) {
            lastPoint.ControlSecondWorld = inversed ? p.ControlFirstWorld : p.ControlSecondWorld;

            return;
        }

        if (isLastPoint && inversed) {
            ReversePoint(p);
        }

        if (isLastSegment && isLastPoint) {
            firstPoint.ControlFirstWorld = p.ControlFirstWorld;

            if (isClosed) {
                return;
            }
        }

        AddSplinePoint(p, originalCurve, ref lastPoint);
    }

    void ReversePoint(BGCurvePointI p)
    {
        var temp = p.ControlFirstLocal;
        p.ControlFirstLocal = p.ControlSecondLocal;
        p.ControlSecondLocal = temp;
    }

    void AddSplinePoint(BGCurvePointI p, BGCurve originalCurve, ref BGCurvePointI point)
    {
        point = spline.AddPoint(
            new BGCurvePoint(
                spline,
                p.PositionWorld,
                BGCurvePoint.ControlTypeEnum.BezierIndependant,
                p.ControlFirstWorld,
                p.ControlSecondWorld,
                true
            )
        );

        var normal = originalCurve.transform.rotation * p.GetVector3(Keys.NORMAL_FIELD);
        point.SetVector3(Keys.NORMAL_FIELD, normal);
    }

    void CreateSplineComponents(GameObject splineObject)
    {
        spline = splineObject.AddComponent<BGCurve>();
        spline.PointsMode = BGCurve.PointsModeEnum.GameObjectsTransform;
        math = splineObject.AddComponent<BGCcMath>();
        math.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
        cursor = splineObject.AddComponent<BGCcCursor>();

        var field = spline.AddField(Keys.NORMAL_FIELD, BGCurvePointField.TypeEnum.Vector3);
    }

    PathSegment GetSegment(PathSegment[] segments, PathSegment current, bool fromStart = true, bool startPos = false)
    {
        var pos = startPos ? current.pathStart.position : current.pathEnd.position;

        foreach (var s in segments) {
            if (fromStart) {
                if (Vector3.Distance(s.pathStart.position, pos) <= .5f && s != current)
                    return s;
            } else {
                if (Vector3.Distance(s.pathEnd.position, pos) <= .5f && s != current)
                    return s;
            }
        }

        return null;
    }
}
