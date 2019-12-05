using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ImmediatePathSwitcher : MonoBehaviour
{
    static readonly string PLAYER_2D = "Player_2D";

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_2D))
            return;

        var pathfinding = other.GetComponent<BezierPathfinding>();
        pathfinding.TriggerAvailablePathSwitch();
    }
}
