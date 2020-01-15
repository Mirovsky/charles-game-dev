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

    [Header("Level")]
    public LevelScriptableObject levelData;

    [Header("Pause")]
    [SyncVar]
    public bool paused;


    void Awake()
    {
        SetupEventListeners();

        levelKeysCount = FindObjectsOfType<KeyController>().Length;
    }

    void OnDestroy()
    {
        RemoveEventListeners();
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
            if (e.playerType == OOO.PlayerType.MOBILE) {
                mobileInsideExit = true;
            }

            if (e.playerType == OOO.PlayerType.VR) {
                vrInsideExit = true;
            }
        }

        if (e.type == ExitOccupancyChangeEvent.Type.LEAVE) {
            if (e.playerType == OOO.PlayerType.MOBILE) {
                mobileInsideExit = false;
            }

            if (e.playerType == OOO.PlayerType.VR) {
                vrInsideExit = false;
            }
        }

        playersInsideExit = mobileInsideExit;
        if (playersInsideExit)
            TriggerLevelComplete();
    }

    void TriggerLevelComplete()
    {
        EventHub.Instance.FireEvent(new LevelCompleteEvent());

        paused = true;
    }

    void GamePauseEventHandler(GamePauseEvent e)
    {
        paused = e.state;
    }

    void GameOverEventHandler(GameOverEvent e)
    {
        paused = true;
    }

    void SetupEventListeners()
    {
        var hub = EventHub.Instance;

        hub.AddListener<KeyCollectedEvent>(KeyCollectedEventHandler);
        hub.AddListener<ExitOccupancyChangeEvent>(ExitOccupancyChangeEventHandler);
        hub.AddListener<GamePauseEvent>(GamePauseEventHandler);
        hub.AddListener<GameOverEvent>(GameOverEventHandler);
    }

    void RemoveEventListeners()
    {
        var hub = EventHub.Instance;

        hub.RemoveListener<KeyCollectedEvent>(KeyCollectedEventHandler);
        hub.RemoveListener<ExitOccupancyChangeEvent>(ExitOccupancyChangeEventHandler);
        hub.RemoveListener<GamePauseEvent>(GamePauseEventHandler);
        hub.RemoveListener<GameOverEvent>(GameOverEventHandler);
    }
}
