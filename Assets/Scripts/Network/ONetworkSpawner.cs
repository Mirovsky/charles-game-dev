using UnityEngine;
using OOO.Base;
using OOO.Camera;
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
        

        void Start() {
            var sceneName = SceneManager.GetActiveScene().name;
            var isMenu = sceneName == "RoomScene" || sceneName == "MainMenu" || sceneName == "Loading";
            if (isMenu) {
                return;
            }

            OnEnvironmentSetup();
        }

        /** The local player is connected and we can setup the cameras and platforms. */
        void OnEnvironmentSetup() {
            FindObjectOfType<LevelGameState>().Initialize();
            
            OnGameStart();
        }
        
        /**
         * Game starts.
         * Spawn the player and destroy this object
         */
        void OnGameStart() {
            SpawnMobilePlayer();
            StartUI();
            StartTimer();
            StartMusic();

            DestroySpawner();    
        }

        void StartUI()
        {
            mobileCamera.GetComponentInChildren<InGameUIController>().OnGameStart();
        }

        void StartTimer()
        {
            mobileCamera.GetComponentInChildren<TimerTextHandler>().OnGameStart();
            vrCamera.GetComponentInChildren<TimerTextHandler>().OnGameStart();
        }

        void StartMusic()
        {
            mobileCamera.GetComponentInChildren<AmbienceSoundController>().OnGameStart();
            vrCamera.GetComponentInChildren<AmbienceSoundController>().OnGameStart();
        }

        void SpawnMobilePlayer() {
            Instantiate(playerPrefab);
        }

        void DestroySpawner() {
            Destroy(gameObject);
        }
    }
}