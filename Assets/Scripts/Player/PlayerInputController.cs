using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CustomCharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    CustomCharacterController controller;
    PlayerInputActions playerInput;

    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Awake()
    {
        playerInput = new PlayerInputActions();
        controller = GetComponent<CustomCharacterController>();

        controller.OnHitAbove += HitAbove;
        controller.OnHitBelow += HitBelow;
        controller.OnHitLeft += HitLeft;
        controller.OnHitRight += HitRight;
    }

    void Update() => controller.Direction = playerInput.Player.Movement.ReadValue<float>();

    void OnJump() => controller.Jump();

    void OnSlam() => controller.Slam();

    void HitBelow() {}

    void HitAbove() {}
    
    void HitLeft() {}
    
    void HitRight() {}
}
