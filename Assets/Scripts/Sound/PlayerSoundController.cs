using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(Player))]
public class PlayerSoundController : MonoBehaviour
{
    const string speedParam = "Speed";

    [SerializeField]
    [EventRef]
    string movementEventRef;
    [SerializeField]
    [EventRef]
    string jumpEventRef;

    Player player;
    
    FMOD.Studio.EventInstance movementEvent;


    void Start()
    {
        player = GetComponent<Player>();

        player.onMove += OnMove;
        player.onJump += OnJump;

        movementEvent = RuntimeManager.CreateInstance(movementEventRef);
        movementEvent.start();
    }

    void OnDestroy()
    {
        movementEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        player.onMove -= OnMove;
        player.onJump -= OnJump;
    }

    void OnMove(float dist)
    {
        movementEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform));

        movementEvent.setParameterByName(speedParam, dist);
    }

    void OnJump()
    {
        RuntimeManager.PlayOneShot(jumpEventRef, transform.position);
    }

}
