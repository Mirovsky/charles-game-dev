using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(Player))]
public class PlayerSoundController : MonoBehaviour
{
    const string SPEED_PARAM = "Speed";

    [SerializeField]
    [EventRef]
    string movementEventRef;
    [SerializeField]
    [EventRef]
    string jumpEventRef;
    [SerializeField]
    [EventRef]
    string landEventRef;

    DeadOneCollisionsController collisions;
    
    FMOD.Studio.EventInstance movementEvent;


    void Start()
    {
        collisions = GetComponent<DeadOneCollisionsController>();

        collisions.onMove += OnMove;
        collisions.onJump += OnJump;
        collisions.onLand += OnLand;

        movementEvent = RuntimeManager.CreateInstance(movementEventRef);
        movementEvent.start();

        OnMove(0);
    }

    void OnDestroy()
    {
        movementEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        if (collisions == null)
            return;

        collisions.onMove -= OnMove;
        collisions.onJump -= OnJump;
        collisions.onLand -= OnLand;
    }

    void OnMove(float dist)
    {
        movementEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform));

        movementEvent.setParameterByName(SPEED_PARAM, collisions.Collisions.below ? dist : 0);
    }

    void OnJump()
    {
        RuntimeManager.PlayOneShot(jumpEventRef, transform.position);
    }

    void OnLand()
    {
        RuntimeManager.PlayOneShot(landEventRef, transform.position);
    }
}
