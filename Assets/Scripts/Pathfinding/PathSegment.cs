using UnityEngine;
using PathCreation;


public class PathSegment : MonoBehaviour
{
    public PathCreator currentPath;

    public PathSegment rightSegment;
    public PathSegment leftSegment;

    public BoxCollider rightConnectorCollider;

    public float Length => currentPath.path.length;

    public bool HasRight => rightSegment != null;
    public bool HasLeft => leftSegment != null;

    void OnDrawGizmos()
    {
        DrawLine();
    }

    void DrawLine()
    {
        var vPath = currentPath.path;

        Gizmos.color = Color.black;

        var inversed = gameObject.name.Contains("Inversed");

        var pos = transform.position + transform.up * .1f;
        var right = inversed ? -transform.right : transform.right;

        Gizmos.DrawLine(pos, pos + transform.forward * .1f + right * .1f);
        Gizmos.DrawLine(pos, pos - transform.forward * .1f + right * .1f);

        for (int i = 0; i < vPath.NumPoints - 1; i++)
        {
            Gizmos.DrawLine(vPath.GetPoint(i) + vPath.GetNormal(i) * .1f, vPath.GetPoint(i + 1) + vPath.GetNormal(i + 1) * .1f);
        }
    }
}
