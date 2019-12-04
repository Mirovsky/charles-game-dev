using UnityEngine;
using OOO.Utils;


public class ExitController : MonoBehaviour
{
    static readonly string PLAYER_2D = "Player_2D";
    static readonly string PLAYER_VR = "Player_VR";

    public bool isOpen;

    [SerializeField]
    GameObject graphics;

    void Awake()
    {
        EventHub.Instance.AddListener<ExitOpenEvent>(ExitOpenEventHandler);

        graphics.SetActive(false);
    }

    void ExitOpenEventHandler(ExitOpenEvent e)
    {
        isOpen = true;
        // TODO: Figure out proper animation of exit appearing
        graphics.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isOpen) return;

        var e = new ExitOccupancyChangeEvent() { type = ExitOccupancyChangeEvent.Type.ENTER };

        SetPlayerType(other, ref e);

        EventHub.Instance.FireEvent(e);
    }

    void OnTriggerExit(Collider other)
    {
        if (!isOpen) return;

        var e = new ExitOccupancyChangeEvent() { type = ExitOccupancyChangeEvent.Type.LEAVE };

        SetPlayerType(other, ref e);

        EventHub.Instance.FireEvent(e);
    }

    void SetPlayerType(Collider other, ref ExitOccupancyChangeEvent e)
    {
        if (other.CompareTag(PLAYER_2D)) {
            e.playerType = OOO.GameType.MOBILE;
        } else if (other.CompareTag(PLAYER_VR)) {
            e.playerType = OOO.GameType.VR;
        }
    }
}
