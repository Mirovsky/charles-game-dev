using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent (typeof (Player))]
public class DeadOnePlayerInput : MonoBehaviour
{
    Player player;
    PlayerInputActions playerInput;
    LevelGameState gameState;


    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Awake() {
        playerInput = new PlayerInputActions();
        player = GetComponent<Player> ();
        gameState = FindObjectOfType<LevelGameState>();

        playerInput.Player.Jump.started += OnJumpPerformed;
        playerInput.Player.Switch.started += OnSwitchPerformed;
    }

    void Update()
    {
        if (gameState.paused)
            return;

        player.SetDirectionalInput(playerInput.Player.Movement.ReadValue<float>());
    }

    void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (gameState.paused)
            return;

        player.OnJumpInputDown();
    }

    void OnSwitchPerformed(InputAction.CallbackContext ctx)
    {
        if (gameState.paused)
            return;

        player.OnSwitchDown();
    }
}
