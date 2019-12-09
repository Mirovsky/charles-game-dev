using UnityEngine;
using PathCreation;


public class PathSegment : MonoBehaviour
{
    public PathCreator path;

    public PathSegment nextPath;
    public PathSegment prevPath;

    public BoxCollider nextConnectorCollider;
    public BoxCollider prevConnectorCollider;

    public float Length => path.path.length;

    public bool HasNext => nextPath != null;
    public bool HasPrev => prevPath != null;
}
