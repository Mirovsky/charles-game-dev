using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadOneBehaviour : MonoBehaviour
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

    void Update() => controller.Direction = playerInput.Player.Movement.ReadValue<float>();

    void OnJump() => controller.Jump();

    void OnSlam() => controller.Slam();
}
