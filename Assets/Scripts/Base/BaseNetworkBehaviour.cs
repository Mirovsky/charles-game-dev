using System;
using Mirror;

namespace OOO.Base
{
    public class BaseNetworkBehaviour: NetworkBehaviour
    {
        protected bool IsMobilePlayer => GameTypeResolver.Instance.type == GameType.MOBILE;

        protected bool IsVrPlayer => GameTypeResolver.Instance.type == GameType.VR;
    }
}