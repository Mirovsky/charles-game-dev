using OOO.Base;


class ONetworkedObject : BaseNetworkBehaviour
{
    public ObjectAuthority authority;

    public enum ObjectAuthority
    {
        MOBILE,
        VR
    }
}