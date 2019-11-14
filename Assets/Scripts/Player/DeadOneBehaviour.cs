using Mirror;
using OOO.Base;
using UnityEngine;

public class DeadOneBehaviour : BaseNetworkBehaviour
{
    DeadOneController controller;
    PlayerInputActions playerInput;

    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Awake() {
        playerInput = new PlayerInputActions();
        controller = GetComponent<DeadOneController>();
    }

    void Update() {
        if (hasAuthority && IsMobilePlayer) {
            controller.Direction = playerInput.Player.Movement.ReadValue<float>();
        }
    }


    [Server]
    private void OnCollisionEnter(Collision other) {
        if (isServer) {
            if (other.gameObject.CompareTag("Platform")) {
                RpcChangeParent(other.gameObject.transform);
            }
        }
    }

    [ClientRpc]
    private void RpcChangeParent(Transform other) {
        this.transform.parent = other;
    }

    [Server]
    private void OnCollisionExit(Collision other) {
        if (isServer) {
            if (other.gameObject.CompareTag("Platform")) {
                RpcChangeParent(null);
            }
        }
    }


    void OnJump() {
    }

    void OnSlam() {
    }
}