using UnityEngine;
using OOO.Base;


public class SegmentSwitcherNetworkController : BaseNetworkBehaviour
{
    [SerializeField]
    AbstractSegmentSwitcher switcher;
    [SerializeField]
    ImmediateSegmentSwitcher immediateSwitcher;

    void Awake()
    {
        if (IsMobilePlayer)
            return;

        Destroy(switcher);

        if (immediateSwitcher)
            Destroy(immediateSwitcher);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        switcher = GetComponent<AbstractSegmentSwitcher>();
        immediateSwitcher = GetComponent<ImmediateSegmentSwitcher>();
    }
#endif
}
