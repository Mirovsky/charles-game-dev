using UnityEngine;
using BezierSolution;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class PathSegment : MonoBehaviour
{
    public BezierSpline spline;

    public Transform pathStart;
    public Transform pathEnd;

    public PathSegment nextSegment;
    public PathSegment prevSegment;
    public bool inversed;
    public MultiPathDescriptor multiPath;

    public bool processed;

    public int PathDirection => inversed ? -1 : 1;

    public bool HasNext => nextSegment != null;

    public bool HasPrev => prevSegment != null;
    

    public float Length => currentPath.path.length;

    public Vector3 GetPointAtDistance(float d) => currentPath.path.GetPointAtDistance(d, endOfPath);

    public Vector3 GetNormalAtDistance(float d) => currentPath.path.GetNormalAtDistance(d, endOfPath);

    public Vector3 GetDirectionAtDistance(float d) => currentPath.path.GetDirectionAtDistance(d, endOfPath);

    public Quaternion GetRotationAtDistance(float d)
    {
        var rot = currentPath.path.GetRotationAtDistance(d, endOfPath);
        rot *= Quaternion.Euler(0, inversed ? 180f : 0f, 0);
        return rot;
    }

    public float GetClosestDistanceAlongPath(Vector3 p) => currentPath.path.GetClosestDistanceAlongPath(p);

    void Start()
    {
        multiPath = GetComponentInParent<MultiPathDescriptor>();
        if (multiPath == null) {
            Debug.LogError($"PathSegment {name} exists without MultiPathDescriptor in hierarchy!");
        }
    }
}
