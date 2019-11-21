using UnityEngine;
using PathCreation;


public class BezierPathfinding : MonoBehaviour
{
    [SerializeField]
    PathCreator pathCreator;

    float distance;

    public void UpdateDistance(float direction)
    {
        distance += direction;
    }

    /// <summary>
    /// Returns current position on curve.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return pathCreator.path.GetPointAtDistance(distance);
    }

    /// <summary>
    /// Returns current normal on curve based on traveled distance.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetNormal()
    {
        return pathCreator.path.GetNormalAtDistance(distance);
    }

    public Vector2 GetDirection()
    {
        return pathCreator.path.GetDirectionAtDistance(distance);
    }

    public Quaternion GetRotation()
    {
        return pathCreator.path.GetRotationAtDistance(distance);
    }
}
