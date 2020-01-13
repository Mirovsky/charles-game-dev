using UnityEngine;
using OOO;


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
    }

    void SwitchCurrent(GameObject next)
    {
        if (current != null)
            current.SetActive(false);
        
        current = next;
        
        if (current != null)
            current.SetActive(true);
    }
}
