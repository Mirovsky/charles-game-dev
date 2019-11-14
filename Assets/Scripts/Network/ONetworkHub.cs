using Mirror;
using OOO.Base;
using OOO.Utils;
using UnityEngine;

namespace OOO.Network
{
    [RequireComponent(typeof(NetworkManager))]
    public class ONetworkHub: MonoBehaviour
    {
        private NetworkManager manager;
        
        public void OnGUI() {
            manager = GetComponent<NetworkManager>();
            
            if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active) {
                if (GameTypeResolver.Instance.type == GameType.MOBILE) {
                    manager.StartHost();
                }
                else {
                    manager.StartClient();
                }
            }
        }
    }
}