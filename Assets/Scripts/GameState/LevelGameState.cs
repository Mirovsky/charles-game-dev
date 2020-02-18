using UnityEngine;
using OOO.Base;
using OOO.Utils;


public class LevelGameState : BaseNetworkBehaviour
{
    [Header("Keys")]
    public int collectedKeys;
    public int levelKeysCount;

    [Header("Exit")]
    public bool playersInsideExit;
    public bool mobileInsideExit;
    public bool vrInsideExit;

    [Header("Level")]
    public LevelScriptableObject levelData;

    [Header("Pause")]
    public bool paused;

    [Header("Timer")]
    double startTime = 0;
    bool wasPaused;
    double pausedTime;
    double pausedAmount;

    public void Initialize()
    {
        SetupEventListeners();

        levelKeysCount = FindObjectsOfType<KeyController>().Length;
    }

    public bool HasStarted { get; private set; }

    public double ElapsedTime { get; private set; }
    public double TotalTime { get; private set; }

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

        FindObjectOfType<InGameUIController>().OnGameEnd();
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

    void Update()
    {
        if (HasStarted)
        {
            if (!wasPaused && paused)
            {
                pausedTime = Time.time;
                wasPaused = true;

                return;
            }

            if (wasPaused && !paused)
            {
                pausedAmount += (Time.time - pausedTime);
                wasPaused = false;

                return;
            }

            if (paused)
                return;

            ElapsedTime = (Time.time - startTime - pausedAmount);

            if (TotalTime <= ElapsedTime)
            {
                EventHub.Instance.FireEvent(
                    new GameOverEvent() { gameOverReasong = GameOverEvent.GameOverReason.TIME_UP }
                );
            }
        }
    }
}
