using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    [Scene]
    string mainMenuScene;

    LevelGameState gameState;
    NetworkManager networkManager;

    public void OpenMenu()
    {
        networkManager.ServerChangeScene(mainMenuScene);
    }

    public void RestartLevel()
    {
        networkManager.ServerChangeScene(gameState.levelData.scenePath);
    }

    public void NextLevel()
    {
        var nextLevel = gameState.levelData.nextLevel;
        if (nextLevel == null)
            return;

        var nextLevelScene = nextLevel.scenePath;
        networkManager.ServerChangeScene(nextLevelScene);
    }

    void Start()
    {
        networkManager = NetworkManager.singleton;
        gameState = FindObjectOfType<LevelGameState>();
    }
}
