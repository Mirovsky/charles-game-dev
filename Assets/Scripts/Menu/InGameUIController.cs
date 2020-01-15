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

    void Start()
    {
        var playerType = GameTypeResolver.Instance.playerType;

        var gameState = FindObjectOfType<LevelGameState>();
        if (playerType != PlayerType.MOBILE || gameState == null) {
            Destroy(gameObject);
            return;
        }
        ShowLevelProgress();

        EventHub.Instance.AddListener<LevelCompleteEvent>(OnLevelCompleteEvent);
        EventHub.Instance.AddListener<GameOverEvent>(OnGameOverEvent);
        EventHub.Instance.AddListener<GamePauseEvent>(OnGamePause);
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
