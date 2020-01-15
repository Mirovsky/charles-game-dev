using UnityEngine;


public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    MultiPathDescriptor path = default;
    [SerializeField]
    AbstractSegmentSwitcher nextSwitcher = null;

    [SerializeField]
    float distance;

    void Awake()
    {
        path = FindObjectOfType<DefaultMultiPathDescriptor>()
            .DefaultMultiPath;

        path.GetClosestPointAndDistanceByPoint(transform.position, out distance);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PathSwitcher")) {
            nextSwitcher = other.GetComponent<AbstractSegmentSwitcher>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        nextSwitcher = null;
    }

    public void SetNextSwitcher(AbstractSegmentSwitcher switcher)
    {
        nextSwitcher = switcher;
    }

    public void TriggerAvailablePathSwitch(bool force = false)
    {
        if (nextSwitcher == null)
            return;

        var nextPath = nextSwitcher.GetNextPath(path, force);
        if (nextPath == null)
            return;

        path = nextPath;
        path.GetClosestPointAndDistanceByPoint(transform.position, out distance);

        nextSwitcher.RotateSwitcher(path, distance);
    }

    public void UpdateDistance()
        => path.GetClosestPointAndDistanceByPoint(transform.position, out distance);

    public Vector3 GetPosition(float nextStep = 0)
        => path.GetPoint(ModDistance(distance + nextStep));

    public Vector3 GetNormal(float nextStep = 0)
        => path.GetNormal(ModDistance(distance + nextStep));

    public Vector3 GetDirection(float nextStep = 0)
        => path.GetTangent(ModDistance(distance + nextStep));
    
    public Quaternion GetRotation(float nextStep = 0)
        => Quaternion.LookRotation(
            path.GetTangent(ModDistance(distance + nextStep)),
            path.GetNormal(ModDistance(distance + nextStep))
    );

    float ModDistance(float dist)
    {
        if (path == null)
            return 0;

        if (!path.Closed)
            return Mathf.Clamp(dist, 0, path.Length);

        var length = path.Length;

        if (dist > length)
            dist -= length;

        if (dist < 0)
            dist += length;

        return dist;
    }
}
