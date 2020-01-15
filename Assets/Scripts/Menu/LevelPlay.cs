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

    [Header("")]
    [SerializeField]
    LevelSelectController levelSelectController;


    void Awake()
    {
        playButton.onClick.AddListener(PlayLevel);
    }

    void PlayLevel()
    {
        levelSelectController.PlayLevel(level);
    }
}
