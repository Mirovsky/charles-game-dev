using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OOO.Base;
using OOO.Utils;


public class LevelGameState : BaseNetworkBehaviour
{
    [Header("Keys")]
    [SyncVar]
    public int collectedKeys;
    public int levelKeysCount;

    [Header("Exit")]
    [SyncVar]
    public bool playersInsideExit;
    [SyncVar]
    public bool mobileInsideExit;
    [SyncVar]
    public bool vrInsideExit;


    void Awake()
    {
        SetupEventListeners();

        levelKeysCount = FindObjectsOfType<KeyController>().Length;
    }

    void KeyCollectedEventHandler(KeyCollectedEvent e)
    {
        collectedKeys += 1;

        if (collectedKeys == levelKeysCount) {
            EventHub.Instance.FireEvent(new ExitOpenEvent());
        }
    }

    void ExitOccupancyChangeEventHandler(ExitOccupancyChangeEvent e)
    {
        if (e.type == ExitOccupancyChangeEvent.Type.ENTER) {
            if (e.playerType == OOO.GameType.MOBILE) {
                mobileInsideExit = true;
            }

            if (e.playerType == OOO.GameType.VR) {
                vrInsideExit = true;
            }
        }

        if (e.type == ExitOccupancyChangeEvent.Type.LEAVE) {
            if (e.playerType == OOO.GameType.MOBILE) {
                mobileInsideExit = false;
            }

            if (e.playerType == OOO.GameType.VR) {
                vrInsideExit = false;
            }
        }

        playersInsideExit = mobileInsideExit && vrInsideExit;
    }

    void SetupEventListeners()
    {
        var hub = EventHub.Instance;

        hub.AddListener<KeyCollectedEvent>(KeyCollectedEventHandler);
        hub.AddListener<ExitOccupancyChangeEvent>(ExitOccupancyChangeEventHandler);
    }
}
