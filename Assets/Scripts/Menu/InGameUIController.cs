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

    GameObject current;

    public void ShowLevelProgress()
        => SwitchCurrent(levelProgress);

    public void ShowGameOver()
        => SwitchCurrent(gameOver);

    public void ShowLevelComplete()
        => SwitchCurrent(levelComplete);

    void Start()
    {
        var playerType = GameTypeResolver.Instance.playerType;

        if (playerType != PlayerType.MOBILE) {
            Destroy(gameObject);
            return;
        }
        ShowLevelProgress();

        EventHub.Instance.AddListener<LevelCompleteEvent>(OnLevelCompleteEvent);
        EventHub.Instance.AddListener<GameOverEvent>(OnGameOverEvent);
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
}
