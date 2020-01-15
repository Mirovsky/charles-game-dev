using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(ExitController))]
public class ExitSoundController : MonoBehaviour
{
    [SerializeField]
    [EventRef]
    string exitAppearEventRef;

    ExitController exitController;

    void Start()
    {
        exitController = GetComponent<ExitController>();
        exitController.onOpen += OnOpen;
    }

    void OnDestroy()
    {
        if (exitController == null)
            return;

        exitController.onOpen -= OnOpen;
    }

    void OnOpen()
    {
        RuntimeManager.PlayOneShot(exitAppearEventRef);
    }
}
