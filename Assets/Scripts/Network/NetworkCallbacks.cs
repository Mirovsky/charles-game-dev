﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;


public class NetworkCallbacks : Bolt.GlobalEventListener
{

    public GameObject mobileCamera;
    public GameObject vrCamera;

    public override void SceneLoadLocalDone(string scene)
    {
        if (!BoltNetwork.IsServer) {


            if (GameInstance.Instance.type == PlayerConfig.TYPE.DEAD_ONE)
            {
                BoltNetwork.Instantiate(BoltPrefabs.DeadOne, Vector3.zero, Quaternion.identity);
                Instantiate(mobileCamera);
            }
            else
            {
                Instantiate(vrCamera);
            }
        }
    }
}
