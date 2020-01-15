using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using OOO.Base;
using OOO.Camera;
using TMPro;
using UnityEngine.SceneManagement;


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

        [Header("2D Camera"), SerializeField]
        GameObject mobileCamera;

        [Header("VR Camera"), SerializeField]
        GameObject vrCamera;
        [Header("VR Hands"), SerializeField]
        GameObject leftHand;
        [SerializeField]
        GameObject rightHand;


        public override void OnStartLocalPlayer() {
            var sceneName = SceneManager.GetActiveScene().name;
            var spawnAvailable = sceneName == "RoomScene" || sceneName == "MainMenu" || sceneName == "Loading";
            if (spawnAvailable) {
                SpawnCamera();
                return;
            }

            OnEnvironmentSetup();
        }

        /** The local player is connected and we can setup the cameras and platforms. */
        void OnEnvironmentSetup() {
            SpawnCamera();
            AssignNetworkedIdentity();
            OnGameStart();

            FindObjectOfType<LevelGameState>().Initialize();
        }
        
        /**
         * Game starts.
         * Spawn the player and destroy this object
         */
        void OnGameStart() {
            SpawnMobilePlayer();
            StartTimer();

            if (isServer) {
                //The server destroys both.
                RpcDestroySpawner();
            }
        }

        void SpawnCamera() {
            if (isLocalPlayer && IsMobilePlayer) {
                SpawnMobileCamera();
            }

            if (isLocalPlayer && IsVrPlayer) {
                SpawnVRCamera();
            }
        }
        
        [Client]
        void AssignNetworkedIdentity() {
            var networkedObjects = FindObjectsOfType<ONetworkedObject>();

            if (IsVrPlayer && isLocalPlayer) {
                var vrAuthorityObjects = networkedObjects
                    .Where(o => o.authority == ONetworkedObject.ObjectAuthority.VR)
                    .Select(o => o.gameObject)
                    .ToArray();


                CmdAssignCorrectNetworkedObjectIdentity(vrAuthorityObjects);
            }

            if (IsMobilePlayer && isLocalPlayer) {
                var mobileAuthorityObjects = networkedObjects
                    .Where(o => o.authority == ONetworkedObject.ObjectAuthority.MOBILE)
                    .Select(o => o.gameObject)
                    .ToArray();

                CmdAssignCorrectNetworkedObjectIdentity(mobileAuthorityObjects);
            }
        }

        [Command]
        void CmdAssignCorrectNetworkedObjectIdentity(GameObject[] objects)
        {
            foreach (var o in objects) {
                var identity = o.GetComponent<NetworkIdentity>();
                identity.AssignClientAuthority(connectionToClient);
            }
        }

        void StartTimer() {
            if (IsMobilePlayer) {
                // RpcStartTimer();
            }
        }

        [ClientRpc]
        void RpcStartTimer() {
            if (IsMobilePlayer && isLocalPlayer) {
                mobileCamera.GetComponentInChildren<TimerTextHandler>().OnGameStart();
            }

            /* if (IsVrPlayer && isLocalPlayer) {
                vrCamera.GetComponentInChildren<TimerTextHandler>().OnGameStart();
            } */
        }

        [Client]
        void SpawnMobilePlayer() {
            if (IsMobilePlayer && isLocalPlayer) {
                CmdSpawnMobilePlayer();
            }
        }

        [Command]
        void CmdSpawnMobilePlayer() {
            var player = Instantiate(playerPrefab);
            NetworkServer.SpawnWithClientAuthority(player, NetworkServer.localConnection);
        }


        [ClientRpc]
        void RpcDestroySpawner() {
            Destroy(gameObject);
        }

        void SpawnMobileCamera()
        {
            mobileCamera = Instantiate(mobileCamera);
        }

        void SpawnVRCamera()
        {
            Debug.Log("SPAWNING VR CAMERA!");


            vrCamera = Instantiate(vrCamera);

            leftHand = Instantiate(leftHand);
            rightHand = Instantiate(rightHand);

            leftHand.GetComponent<Grabber>().SetParentTransform(vrCamera.transform);
            rightHand.GetComponent<Grabber>().SetParentTransform(vrCamera.transform);
        }
    }
}