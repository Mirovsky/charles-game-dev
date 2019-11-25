using UnityEngine;
using PathCreation;


public class BezierPathfinding : MonoBehaviour
{
    [SerializeField]
    PathCreator pathCreator;

    [SerializeField]
    Player player;

    float distance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PathSwitcher")) {
            pathCreator = other
                .GetComponent<PathSwitcher>()
                .GetNextPath(player.Direction);

            distance = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }

    public void UpdateDistance(float direction)
    {
        distance += direction;
    }

    public Vector3 GetPosition(float nextStep = 0)
    {
        var pos = pathCreator.path.GetPointAtDistance(distance + nextStep);
        pos.y = 0;
        return pos;
    }

    public Vector3 GetNormal(float nextStep = 0)
    {
        return pathCreator.path.GetNormalAtDistance(distance + nextStep);
    }

    public Vector3 GetDirection(float nextStep = 0)
    {
        return pathCreator.path.GetDirectionAtDistance(distance + nextStep);
    }

    public Quaternion GetRotation(float nextStep = 0)
    {
        return pathCreator.path.GetRotationAtDistance(distance + nextStep);
    }
}
