using System;
using Mirror;

namespace OOO.Base
{
    public class BaseNetworkBehaviour: NetworkBehaviour
    {
        protected bool IsMobilePlayer => GameTypeResolver.Instance.playerType == PlayerType.MOBILE;

        protected bool IsVrPlayer => GameTypeResolver.Instance.playerType == PlayerType.VR;
    }
}