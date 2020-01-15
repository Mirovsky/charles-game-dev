using System;
using Mirror;
using TMPro;
using UnityEngine;
using OOO.Utils;


namespace OOO.Camera
{
    /** Attached to the Camera obj*/
    public class TimerTextHandler: MonoBehaviour
    {
        public Action onCountdownElapsed;

        [SerializeField]
        TextMeshProUGUI countdownTimerText;
        [SerializeField]
        double elapsed;

        LevelGameState gameState;

        /* in seconds */
        private double totalTime = 120;
        
        private double startTime = 0;
        private bool hasStarted = false;
        bool initialized;

        bool wasPaused;
        [SerializeField]
        double pausedTime;
        [SerializeField]
        double pausedAmount;
        
        public void OnGameStart()
        {
            hasStarted = true;
            startTime = NetworkTime.time;
        }

        void Start()
        {
            gameState = FindObjectOfType<LevelGameState>();
            if (gameState == null || gameState.levelData == null)
                return;

            totalTime = gameState.levelData.timeLimit;

            countdownTimerText.text = "--:--";
            initialized = true;
        }

        void Update()
        {
            if (!initialized)
                return;

            if (hasStarted) {
                if (!wasPaused && gameState.paused) {
                    pausedTime = NetworkTime.time;
                    wasPaused = true;

                    return;
                }

                if (wasPaused && !gameState.paused) {
                    pausedAmount += (NetworkTime.time - pausedTime);
                    wasPaused = false;

                    return;
                }

                if (gameState.paused)
                    return;

                elapsed = (NetworkTime.time - startTime - pausedAmount);

                if (totalTime > elapsed) {
                    var remaining =(int) (totalTime - elapsed);
                    var minutes = remaining / 60;
                    var seconds = (remaining % 60);

                    if (seconds < 10) {
                        countdownTimerText.text = minutes + ":" + "0" + seconds;
                    }
                    else {
                        countdownTimerText.text = minutes + ":" + seconds;
                    }
                } else {
                    countdownTimerText.text = "00:00";

                    EventHub.Instance.FireEvent(
                        new GameOverEvent() { gameOverReasong = GameOverEvent.GameOverReason.TIME_UP }
                    );
                }
            }
        }
    }
}