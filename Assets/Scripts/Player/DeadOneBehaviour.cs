using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadOneBehaviour : Bolt.EntityBehaviour<IDeadOneState>
{
    CustomCharacterController controller;
    PlayerInputActions playerInput;

    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Awake()
    {
        playerInput = new PlayerInputActions();
        controller = GetComponent<CustomCharacterController>();
    }

    public override void Attached()
    {
        state.SetTransforms(state.DeadOneTransform, transform);
    }

    public override void SimulateOwner()
    {
        controller.Direction = playerInput.Player.Movement.ReadValue<float>();
    }
}
