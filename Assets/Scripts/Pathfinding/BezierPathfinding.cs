using UnityEngine;
using PathCreation;


public class BezierPathfinding : MonoBehaviour
{
    [SerializeField]
    PathCreator pathCreator;

    [SerializeField]
    Player player;

    float distance;
    [SerializeField]
    PathSwitcher nextSwitcher = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PathSwitcher")) {
            nextSwitcher = other.GetComponent<PathSwitcher>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        nextSwitcher = null;
    }

    public void TriggerAvailablePathSwitch(bool force = false)
    {
        if (nextSwitcher == null)
            return;

        var nextPath = nextSwitcher.GetNextPath(pathCreator, force);
        if (nextPath == null)
            return;

        pathCreator = nextPath;
        distance = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    public void UpdateDistance(float direction)
    {
        distance += direction;
    }

    public Vector3 GetPosition(float nextStep = 0)
    {
        Debug.Log(pathCreator);
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
