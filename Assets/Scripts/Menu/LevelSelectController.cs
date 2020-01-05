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
        networkManager.ServerChangeScene(level.scenePath);
    }
}
