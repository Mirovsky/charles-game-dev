using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


public class LoadingManager : MonoBehaviour
{
    [Scene]
    public string scenePath;


    void Start()
    {
        SceneManager.LoadScene(scenePath);
    }
}
