using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;


public class LevelSelectController : MonoBehaviour
{
    [SerializeField]
    NetworkManager networkManager;
    [SerializeField]
    EventSystem eventSystem;
 

    public void PlayLevel(LevelScriptableObject level)
    {
        eventSystem.enabled = false;

        ChangeScene(level.scenePath);
    }

    void ChangeScene(string path)
    {
        networkManager.ServerChangeScene(path);
    }

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }
}
