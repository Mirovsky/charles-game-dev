using UnityEngine;
using TMPro;
using OOO.Utils;


public class LevelProgressController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI collectedKeys;
    [SerializeField]
    TextMeshProUGUI requiredKeys;

    LevelGameState gameState;

    void Start()
    {
        gameState = FindObjectOfType<LevelGameState>();

        Debug.Log(gameState);

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
