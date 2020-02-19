using UnityEngine;


public class CameraSetter : MonoBehaviour
{
    void Start()
    {
        var canvas = GetComponent<Canvas>();

        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 4;
    }
}
