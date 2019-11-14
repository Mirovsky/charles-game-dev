using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(DeadOneController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : NetworkBehaviour
{
    DeadOneController controller;
    PlayerInputActions playerInput;

    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Awake()
    {
        playerInput = new PlayerInputActions();
        controller = GetComponent<DeadOneController>();

        controller.OnHitAbove += HitAbove;
        controller.OnHitBelow += HitBelow;
        controller.OnHitLeft += HitLeft;
        controller.OnHitRight += HitRight;
    }

    void Update() {
    }
    
    void OnJump() {
    }

    void OnSlam() {
    }
    
    void HitBelow() {}

    void HitAbove() {}
    
    void HitLeft() {}
    
    void HitRight() {}
}
