using UnityEngine;
using Mirror;


public class ONetworkRoomPlayer : NetworkRoomPlayer
{
    public override void OnClientEnterRoom()
    {
    }

    public override void OnStartAuthority()
    {
        CmdChangeReadyState(true);
    }
}
