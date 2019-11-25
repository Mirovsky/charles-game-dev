using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineVirtualCameraTargetSetter : MonoBehaviour
{
    void Awake()
    {
        var player = FindObjectOfType<Player>();
        if (player == null) {
            Debug.LogWarning("OOO: Player component is missing in scene!");
            return;
        }

        var camera = GetComponent<CinemachineVirtualCamera>();

        camera.Follow = player.transform;
        camera.LookAt = player.transform;
    }
}
