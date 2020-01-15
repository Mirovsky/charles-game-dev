using System;
using UnityEngine;
using Mirror;
using OOO.Base;
using OOO.Utils;


public class ExitController : BaseNetworkBehaviour
{
    static readonly string PLAYER_2D = "Player_2D";
    static readonly string PLAYER_VR = "Player_VR";

    public Action onOpen;
    public Action onEnter;


    public bool isOpen;

    [SerializeField]
    GameObject exitObject;


    public override void OnStartAuthority()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (IsVrPlayer)
            return;

        EventHub.Instance.AddListener<ExitOpenEvent>(ExitOpenEventHandler);

        // Trigger only after all keys are collected
        CmdTriggerVisibilityChange(false);
    }

    void OnDestroy()
    {
        EventHub.Instance.RemoveListener<ExitOpenEvent>(ExitOpenEventHandler);    
    }

    void ExitOpenEventHandler(ExitOpenEvent e)
    {
        onOpen?.Invoke();

        CmdTriggerVisibilityChange(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsVrPlayer)
            return;

        var e = new ExitOccupancyChangeEvent() { type = ExitOccupancyChangeEvent.Type.ENTER };

        SetPlayerType(other, ref e);

        EventHub.Instance.FireEvent(e);
    }

    void OnTriggerExit(Collider other)
    {
        if (IsVrPlayer)
            return;

        var e = new ExitOccupancyChangeEvent() { type = ExitOccupancyChangeEvent.Type.LEAVE };

        SetPlayerType(other, ref e);

        EventHub.Instance.FireEvent(e);
    }

    void SetPlayerType(Collider other, ref ExitOccupancyChangeEvent e)
    {
        if (other.CompareTag(PLAYER_2D)) {
            e.playerType = OOO.PlayerType.MOBILE;
        } else if (other.CompareTag(PLAYER_VR)) {
            e.playerType = OOO.PlayerType.VR;
        }
    }

    [Command]
    void CmdTriggerVisibilityChange(bool visibility)
    {
        RpcToggleExitVisibility(visibility);
    }

    [ClientRpc]
    void RpcToggleExitVisibility(bool visibility)
    {
        gameObject.SetActive(visibility);
    }
}
