using UnityEngine;
using PathCreation;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class PathSegment : MonoBehaviour
{
    const EndOfPathInstruction endOfPath = EndOfPathInstruction.Stop;
    const float ARROW_SIZE = 1f;

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

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (currentPath == null || pathStart == null || pathEnd == null)
            return;

        var vPath = currentPath.path;

        DrawLine(vPath);
        DrawDistance(vPath);
    }

    void DrawLine(VertexPath vPath)
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < vPath.NumPoints - 1; i++)
        {
            Gizmos.DrawLine(vPath.GetPoint(i) + vPath.GetNormal(i) * ARROW_SIZE, vPath.GetPoint(i + 1) + vPath.GetNormal(i + 1) * ARROW_SIZE);
        }

        var length = vPath.length;

        var startDirection = vPath.GetDirection(0);
        var endDirection = vPath.GetDirection(length);

        Gizmos.DrawWireSphere(pathStart.position + startDirection * .5f, .05f);
        Gizmos.DrawWireSphere(pathEnd.position - endDirection * .5f, .05f);
    }

    void DrawDistance(VertexPath vPath)
    {
        var start = 0;
        var length = vPath.length;

        var startDirection = vPath.GetDirection(0);
        var endDirection = vPath.GetDirection(length);

        Handles.Label(pathStart.position + startDirection * .5f, start.ToString());
        Handles.Label(pathEnd.position - endDirection *.5f, length.ToString());
    }
#endif
}
