using UnityEngine;
using PathCreation;


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
    
    public float PathDirection => inversed ? -1 : 1;

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

    void OnDrawGizmos()
    {
        DrawLine();
    }

    void DrawLine()
    {
        var vPath = currentPath.path;

        Gizmos.color = Color.black;
        for (int i = 0; i < vPath.NumPoints - 1; i++)
        {
            Gizmos.DrawLine(vPath.GetPoint(i) + vPath.GetNormal(i) * ARROW_SIZE, vPath.GetPoint(i + 1) + vPath.GetNormal(i + 1) * ARROW_SIZE);
        }
    }
}
