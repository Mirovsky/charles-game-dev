using System.Linq;
using UnityEngine;
using OOO.Utils;


public class DoorController : MonoBehaviour
{
    [SerializeField]
    KeyType requiredKey;
    [SerializeField]
    int requiredKeysCount;
    [SerializeField]
    int collectedKeys;


    void Start()
    {
        EventHub.Instance.AddListener<KeyCollectedEvent>(OnKeyCollected);

        requiredKeysCount = FindObjectsOfType<KeyController>()
            .Where((KeyController kc) => kc.keyType == requiredKey)
            .Count();
    } 

    /*
    TODO: Fix removal of multiple events at the same time.
    void OnDestroy()
    {
        EventHub.Instance.RemoveListener<KeyCollectedEvent>(OnKeyCollected);    
    }
    */

    void OnKeyCollected(KeyCollectedEvent e)
    {
        if (e.keyType == requiredKey) {
            collectedKeys++;
        }

        if (collectedKeys == requiredKeysCount) {
            Destroy(gameObject);
        }
    }
}
