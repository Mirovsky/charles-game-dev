using UnityEngine;
using OOO.Utils;


public class DeadOneKeyCollector : MonoBehaviour
{
    static readonly string KEY_TAG = "Key";


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(KEY_TAG))
            return;

        HandleKeyCollision();
    }

    void HandleKeyCollision()
    {
        EventHub.Instance.FireEvent(new KeyCollectedEvent());   
    }
}
