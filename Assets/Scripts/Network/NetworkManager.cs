using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkManager2: MonoBehaviour
{
    /* public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer) {
            BoltMatchmaking.CreateSession(Guid.NewGuid().ToString(), null, "Main");
        }
    }

    public override void SessionListUpdated(UdpKit.Map<Guid, UdpKit.UdpSession> sessionList)
    {
        foreach (var session in sessionList) {
            var photonSession = session.Value as UdpKit.UdpSession;

            if (photonSession.Source == UdpKit.UdpSessionSource.Photon) {
                BoltNetwork.Connect(photonSession);
            }
        }
    }

    void Start()
    {
        var server = GetArg("-s");

        var clientType = GetArg("-type");

        if (server == "true") {
            BoltLauncher.StartServer();
        } else {

            if (clientType == PlayerConfig.TYPE.DEAD_ONE.ToString())
            {
                GameInstance.Instance.type = PlayerConfig.TYPE.DEAD_ONE;
            }
            else
            {
                GameInstance.Instance.type = PlayerConfig.TYPE.OOO;
            }

            BoltLauncher.StartClient();
        }
    }

    static string GetArg(params string[] names)
    {
        var args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            foreach (var name in names)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
        }
        return null;
    } */
}
