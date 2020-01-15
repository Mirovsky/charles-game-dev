using UnityEngine;
using OOO.Utils;


public class DeadZoneTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            EventHub.Instance.FireEvent(
                new GameOverEvent() { gameOverReasong = GameOverEvent.GameOverReason.FALL }
            );
        }   
    }
}
