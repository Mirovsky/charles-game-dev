using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField]
    EventSystem eventSystem;
 

    public void PlayLevel(LevelScriptableObject level)
    {
        eventSystem.enabled = false;

        ChangeScene(level.scenePath);
    }

    void ChangeScene(string path)
    {
        SceneManager.LoadScene(path);
    }
}
