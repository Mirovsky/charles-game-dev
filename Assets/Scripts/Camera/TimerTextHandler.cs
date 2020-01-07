using System;
using Mirror;
using TMPro;
using UnityEngine;

namespace OOO.Camera
{
    /** Attached to the Camera obj*/
    public class TimerTextHandler: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownTimerText;

        /* in seconds */
        private const double totalTime = 120;
        
        private double startTime = 0;
        private bool hasStarted = false;
        
        public void OnGameStart() {
            // vrTimerTextField = vTextField;
            hasStarted = true;
            startTime = NetworkTime.time;
        }

        private void Start() {
            countdownTimerText.text = "--:--";
        }

        void Update()
        {
            if (hasStarted) {
                var elapsed = (NetworkTime.time - startTime);

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
                }
                else {
                    countdownTimerText.text = "00:00";
                }
            }
        }
    }
}