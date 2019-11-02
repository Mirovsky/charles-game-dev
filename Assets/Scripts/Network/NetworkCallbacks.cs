using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;


[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string scene)
    {
        if (!BoltNetwork.IsServer) {
            var spawnPosition = new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));

            BoltNetwork.Instantiate(BoltPrefabs.DeadOne, spawnPosition, Quaternion.identity);
        }
    }
}
