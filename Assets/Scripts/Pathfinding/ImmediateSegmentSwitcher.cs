using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ImmediateSegmentSwitcher : MonoBehaviour
{
    static readonly string PLAYER_2D = "Player_2D";

    [SerializeField]
    AbstractSegmentSwitcher switcher;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_2D))
            return;

        var pathfinding = other.GetComponent<Pathfinding>();

        pathfinding.SetNextSwitcher(switcher);
        pathfinding.TriggerAvailablePathSwitch();
    }
}
