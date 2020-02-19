using UnityEngine;
using OOO;
using OOO.Utils;


public class InGameUIController : MonoBehaviour
{
    [SerializeField]
    GameObject levelProgress;
    [SerializeField]
    GameObject gameOver;
    [SerializeField]
    GameObject levelComplete;
    [SerializeField]
    GameObject gamePause;

    GameObject current;

    public void ShowLevelProgress()
        => SwitchCurrent(levelProgress);

    public void ShowGameOver()
        => SwitchCurrent(gameOver);

    public void ShowLevelComplete()
        => SwitchCurrent(levelComplete);

    public void ShowGamePause()
        => SwitchCurrent(gamePause);

    public void OnGameStart()
    {
        var gameState = FindObjectOfType<LevelGameState>();
        if (gameState == null) {
            Destroy(gameObject);
            return;
        }
        ShowLevelProgress();

        EventHub.Instance.AddListener<LevelCompleteEvent>(OnLevelCompleteEvent);
        EventHub.Instance.AddListener<GameOverEvent>(OnGameOverEvent);
        EventHub.Instance.AddListener<GamePauseEvent>(OnGamePause);
    }

    public void OnGameEnd()
    {
        EventHub.Instance.RemoveListener<LevelCompleteEvent>(OnLevelCompleteEvent);
        EventHub.Instance.RemoveListener<GameOverEvent>(OnGameOverEvent);
        EventHub.Instance.RemoveListener<GamePauseEvent>(OnGamePause);
    }

    void SwitchCurrent(GameObject next)
    {
        if (current != null)
            current.SetActive(false);
        
        current = next;
        
        if (current != null)
            current.SetActive(true);
    }

    void OnLevelCompleteEvent(LevelCompleteEvent e)
    {
        ShowLevelComplete();
    }

    void OnGameOverEvent(GameOverEvent e)
    {
        ShowGameOver();
    }

    void OnGamePause(GamePauseEvent e)
    {
        if (e.state) {
            ShowGamePause();
        } else {
            ShowLevelProgress();
        }
    }
}
