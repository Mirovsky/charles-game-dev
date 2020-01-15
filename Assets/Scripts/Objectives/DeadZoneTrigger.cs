﻿using UnityEngine;
using OOO.Base;
using OOO.Utils;


public class DeadZoneTrigger : BaseNetworkBehaviour
{
    public override void OnStartClient()
    {
        if (IsVrPlayer) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player_2D")) {
            EventHub.Instance.FireEvent(
                new GameOverEvent() { gameOverReasong = GameOverEvent.GameOverReason.FALL }
            );
        }   
    }
}
