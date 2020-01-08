using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(VectorGrabbable))]
public class MovingPlatformSoundController : MonoBehaviour
{
    const string VELOCITY_PARAM = "Velocity";

    [SerializeField]
    [EventRef]
    string movingEvent;

    VectorGrabbable platformGrabbable;
    FMOD.Studio.EventInstance movingInstance;

    void Start()
    {
        platformGrabbable = GetComponent<VectorGrabbable>();

        movingInstance = RuntimeManager.CreateInstance(movingEvent);
        movingInstance.start();
    }

    void Update()
    {
        movingInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));

        Debug.Log(platformGrabbable.velocity);
        movingInstance.setParameterByName(VELOCITY_PARAM, platformGrabbable.velocity);
    }

    void OnDestroy()
    {
        movingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
