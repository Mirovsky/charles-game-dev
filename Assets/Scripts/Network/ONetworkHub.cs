using Mirror;
using UnityEngine;

namespace OOO.Network
{
    public class ONetworkHub: MonoBehaviour
    {

        void Start() {
            var manager = NetworkManager.singleton;

            manager.networkAddress = GameTypeResolver.Instance.ip;

            if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active) {

                var gameType = GameTypeResolver.Instance.gameType;
                var playerType = GameTypeResolver.Instance.playerType;

                var startHost = ((gameType == GameType.DEFAULT) && (playerType == PlayerType.MOBILE)) || (gameType == GameType.HOST);
                var startClient = ((gameType == GameType.DEFAULT) && (playerType == PlayerType.VR)) || (gameType == GameType.CLIENT);

                if (startHost) {
                    manager.StartHost();
                }

                if (startClient) {
                    manager.StartClient();
                }
            }
        }
    }
}
