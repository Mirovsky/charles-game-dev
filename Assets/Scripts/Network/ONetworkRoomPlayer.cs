using UnityEngine;
using Mirror;


public class ONetworkRoomPlayer : NetworkRoomPlayer
{
    public override void OnClientEnterRoom()
    {
        CmdChangeReadyState(true);
    }
}
