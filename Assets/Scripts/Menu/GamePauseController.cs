using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OOO.Utils;

public class GamePauseController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI collectedKeys;
    [SerializeField]
    TextMeshProUGUI requiredKeys;

    LevelGameState gameState;


    public void TriggerUnpause()
    {
        EventHub.Instance.FireEvent(new GamePauseEvent() { state = false });
    }

    void Start()
    {
        gameState = FindObjectOfType<LevelGameState>();

        requiredKeys.text = gameState.levelKeysCount.ToString();
        UpdateCollectedKeys();

        EventHub.Instance.AddListener<KeyCollectedEvent>(OnKeyCollected);
    }

    void OnKeyCollected(KeyCollectedEvent e)
        => UpdateCollectedKeys();

    void UpdateCollectedKeys()
    {
        collectedKeys.text = gameState.collectedKeys.ToString();
    }
}
