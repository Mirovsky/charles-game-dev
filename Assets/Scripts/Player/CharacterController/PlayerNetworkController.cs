using UnityEngine;
using OOO.Base;

public class PlayerNetworkController : BaseNetworkBehaviour
{
    [SerializeField]
    DeadOneRaycastController characterController = default;
    [SerializeField]
    Player player = default;
    [SerializeField]
    GameObject cameraObject = default;
    [SerializeField]
    Pathfinding pathfinding = default;
    [SerializeField]
    DeadOnePlayerInput playerInput = default;
    [SerializeField]
    DeadOneKeyCollector keyCollector = default;


    void Awake()
    {
        if (IsMobilePlayer)
            return;

        Destroy(characterController);
        Destroy(playerInput);
        Destroy(pathfinding);
        Destroy(cameraObject);
        Destroy(keyCollector);
        Destroy(player);
    }
}
