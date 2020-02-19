using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    [Scene]
    string mainMenuScene;

    LevelGameState gameState;


    public void OpenMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(gameState.levelData.scenePath);
    }

    public void NextLevel()
    {
        var nextLevel = gameState.levelData.nextLevel;
        if (nextLevel == null)
            return;

        var nextLevelScene = nextLevel.scenePath;
        SceneManager.LoadScene(nextLevelScene);
    }

    void Start()
    {
        gameState = FindObjectOfType<LevelGameState>();
    }
}
