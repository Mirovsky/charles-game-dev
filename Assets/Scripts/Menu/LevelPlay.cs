using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class LevelPlay : MonoBehaviour
{
    [SerializeField]
    LevelScriptableObject level;

    [Header("UI")]
    [SerializeField]
    Button playButton;
    [SerializeField]
    GameObject locked;

    [Header("")]
    [SerializeField]
    LevelSelectController levelSelectController;


    void Awake()
    {
        var isLocked = Persistence.IsLevelLocked(level.levelIdentifier);

        locked.SetActive(false);
        // playButton.interactable = !isLocked;

        // if (!isLocked)
            playButton.onClick.AddListener(PlayLevel);
    }

    void PlayLevel()
    {
        levelSelectController.PlayLevel(level);
    }
}
