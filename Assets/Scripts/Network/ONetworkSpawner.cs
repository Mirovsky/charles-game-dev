using System;
using System.Collections.Generic;
using emotitron.NST;
using Mirror;
using OOO.Base;
using UnityEngine;
using UnityEngine.InputSystem;


namespace OOO.Network
{
    /**
     * This acts as a "player" from a network perspective.
     * (@see NetworkManager obj)
     * It has its own local authority so its suitable for calling RPC/Commands.
     *
     * It spawns the Mobile Player prefab and gives the proper authority (to mobile player).
     * It spawns the cameras locally for the individual clients.
     * It spawns the Platforms and gives authority to VR.
     *
     * All in all, does lots of cool stuff. 
     */
    public class ONetworkSpawner : BaseNetworkBehaviour
    {
        /** The actual DeadOne prefab */
        public GameObject playerPrefab;

        /** List of all the platforms that should be spawn on game start */
        public List<GameObject> platforms;

        [Header("Camera")] public GameObject mobileCamera;
        [SerializeField] public Transform mobileCameraTransform;
        public GameObject vrCamera;
        [SerializeField] public Transform vrCameraTransform;


        /** VR player hands */
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;

        public override void OnStartLocalPlayer() {
            OnEnvironmentSetup();
        }

        private void Update() {
            if (Keyboard.current.qKey.wasPressedThisFrame) {
                OnGameStart();
            }
        }
        

        /** The local player is connected and we can setup the cameras and platforms. */
        private void OnEnvironmentSetup() {
            SpawnCamera();
            SpawnPlatforms();
        }
        
        /**
         * Game starts.
         * Spawn the player and destroy this object
         */
        private void OnGameStart() {
            SpawnMobilePlayer();

            if (isServer) {
                //The server destroys both.
                RpcDestroySpawner();
            }
        }

//        [Client]
        private void SpawnCamera() {
            if (isLocalPlayer && IsMobilePlayer) {
                Instantiate(mobileCamera);
            }

            if (isLocalPlayer && IsVrPlayer) {
                vrCamera = Instantiate(vrCamera);
                
                leftHand = Instantiate(leftHand);
                rightHand = Instantiate(rightHand);
                
                leftHand.GetComponent<Grabber>().SetParentTransform(vrCamera.transform);
                rightHand.GetComponent<Grabber>().SetParentTransform(vrCamera.transform);
            }
        }
        

        [Client]
        private void SpawnPlatforms() {
            if (IsVrPlayer && isLocalPlayer) {
                CmdSpawnPlatforms();
            }
        }
        [Command]
        private void CmdSpawnPlatforms() {
            foreach (var platform in platforms) {
                var player = Instantiate(platform);
                NetworkServer.SpawnWithClientAuthority(player, connectionToClient);
            }
        }

        
        [Client]
        private void SpawnMobilePlayer() {
            if (IsMobilePlayer && isLocalPlayer) {
                CmdSpawnMobilePlayer();
            }
        }
        [Command]
        private void CmdSpawnMobilePlayer() {
            var player = Instantiate(playerPrefab);
            NetworkServer.SpawnWithClientAuthority(player, NetworkServer.localConnection);
        }


        [ClientRpc]
        private void RpcDestroySpawner() {
            Destroy(gameObject);
        }
    }
}