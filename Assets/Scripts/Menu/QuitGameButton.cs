using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class QuitGameButton : MonoBehaviour
{
    Button btn;


    void Start()
    {
        btn = GetComponent<Button>();

        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Debug.Log("quit");

        Application.Quit();
    }
}
