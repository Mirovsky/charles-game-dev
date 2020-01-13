using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent (typeof (Player))]
public class DeadOnePlayerInput : MonoBehaviour
{
    [SerializeField]
    Pathfinding pathfinding;

    Player player;
    PlayerInputActions playerInput;

    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Awake() {
        playerInput = new PlayerInputActions();
        player = GetComponent<Player> ();

        playerInput.Player.Jump.started += OnJumpPerformed;
        playerInput.Player.Switch.started += OnSwitchPerformed;
    }

    void Update()
        => player.SetDirectionalInput(playerInput.Player.Movement.ReadValue<float>());

    void OnJumpPerformed(InputAction.CallbackContext ctx)
        => player.OnJumpInputDown();

    void OnSwitchPerformed(InputAction.CallbackContext ctx)
        => player.OnSwitchDown();
}
