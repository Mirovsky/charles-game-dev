using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(DoorController))]
public class DoorSoundController : MonoBehaviour
{
    [SerializeField]
    [EventRef]
    string doorOpenEventRef;

    DoorController doorController;


    void Start()
    {
        doorController = GetComponent<DoorController>();
        doorController.onOpen += OnOpen;
    }

    void OnDestroy()
    {
        doorController.onOpen -= OnOpen;
    }

    void OnOpen()
    {
        RuntimeManager.PlayOneShot(doorOpenEventRef);
    }
}
