using UnityEngine;
using UnityEngine.SceneManagement;
using OOO;


public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    string vrLobbyScenePath;
    [SerializeField]
    string mainMenuScenePath;

    [SerializeField]
    GameObject wrongPlayerTypeInfo;

    void Start()
    {
        wrongPlayerTypeInfo.SetActive(false);

        var playerType = GameTypeResolver.Instance.playerType;  
    
        if (playerType == PlayerType.MOBILE) {
            SceneManager.LoadScene(mainMenuScenePath);

            return;
        } else if (playerType == PlayerType.VR) {
            SceneManager.LoadScene(vrLobbyScenePath);

            return;
        } else {
            wrongPlayerTypeInfo.SetActive(true);
        }
    }
}
