using System;
using UnityEngine;
using OOO.Utils;


public class ExitController : MonoBehaviour
{
    static readonly string PLAYER_2D = "Player_2D";
    static readonly string PLAYER_VR = "Player_VR";

    public Action onOpen;
    public Action onEnter;

    public bool isOpen;

    void Awake()
    {
        EventHub.Instance.AddListener<ExitOpenEvent>(ExitOpenEventHandler);

        // Trigger only after all keys are collected
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EventHub.Instance.RemoveListener<ExitOpenEvent>(ExitOpenEventHandler);    
    }

    void ExitOpenEventHandler(ExitOpenEvent e)
    {
        onOpen?.Invoke();
        // TODO: Figure out proper animation of exit appearing
        gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        var e = new ExitOccupancyChangeEvent() { type = ExitOccupancyChangeEvent.Type.ENTER };

        SetPlayerType(other, ref e);

        EventHub.Instance.FireEvent(e);
    }

    void OnTriggerExit(Collider other)
    {
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
}
