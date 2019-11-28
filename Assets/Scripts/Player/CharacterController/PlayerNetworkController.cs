using UnityEngine;
using OOO.Base;

public class PlayerNetworkController : BaseNetworkBehaviour
{
    [SerializeField]
    CharacterController characterController = default;
    [SerializeField]
    Player player = default;
    [SerializeField]
    GameObject cameraObject = default;
    [SerializeField]
    BezierPathfinding pathfinding = default;
    [SerializeField]
    DeadOnePlayerInput playerInput = default;

    void Awake()
    {
        if (IsMobilePlayer)
            return;

        Destroy(characterController);
        Destroy(player);
        Destroy(playerInput);
        Destroy(pathfinding);
        Destroy(cameraObject);
    }
}
