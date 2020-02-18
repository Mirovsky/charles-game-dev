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
        LevelGameState gameState;

        public void OnGameStart()
        {
            gameState = FindObjectOfType<LevelGameState>();
            if (gameState == null || gameState.levelData == null) {
                Debug.Log($"Missing game state!");
                return;
            }

            countdownTimerText.text = "--:--";
        }

        void Update()
        {
            if (gameState == null)
                return;

            if (gameState.TotalTime > gameState.ElapsedTime)
            {
                var remaining = (int)(gameState.TotalTime - gameState.ElapsedTime);
                var minutes = remaining / 60;
                var seconds = (remaining % 60);

                if (seconds < 10)
                {
                    countdownTimerText.text = minutes + ":" + "0" + seconds;
                }
                else
                {
                    countdownTimerText.text = minutes + ":" + seconds;
                }
            }
            else
            {
                countdownTimerText.text = "00:00";

                EventHub.Instance.FireEvent(
                    new GameOverEvent() { gameOverReasong = GameOverEvent.GameOverReason.TIME_UP }
                );
            }
        }
    }
}