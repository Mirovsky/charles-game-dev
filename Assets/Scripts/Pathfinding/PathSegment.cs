using UnityEngine;
using PathCreation;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class PathSegment : MonoBehaviour
{
    const EndOfPathInstruction endOfPath = EndOfPathInstruction.Stop;

    public PathCreator currentPath;

    public Transform pathStart;
    public Transform pathEnd;

    [HideInInspector]
    public PathSegment nextSegment;
    [HideInInspector]
    public PathSegment prevSegment;
    [HideInInspector]
    public bool inversed;
    [HideInInspector]
    public MultiPathDescriptor multiPath;

    public int PathDirection => inversed ? -1 : 1;

    public float Length => currentPath.path.length;

    public bool HasNext => nextSegment != null;

    public bool HasPrev => prevSegment != null;

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
