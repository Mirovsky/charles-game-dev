﻿using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent (typeof (Player))]
public class DeadOnePlayerInput : MonoBehaviour
{
    Player player;
    PlayerInputActions playerInput;

    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Awake() {
        playerInput = new PlayerInputActions();
        player = GetComponent<Player> ();

        playerInput.Player.Jump.started += OnJumpPerformed;
    }

    void Update()
        => player.SetDirectionalInput(playerInput.Player.Movement.ReadValue<float>());

    void OnJumpPerformed(InputAction.CallbackContext ctx)
        => player.OnJumpInputDown();
}
