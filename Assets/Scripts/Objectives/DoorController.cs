using System;
using System.Linq;
using UnityEngine;
using OOO.Utils;


public class DoorController : MonoBehaviour
{
    public Action onOpen;

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
        if (!gameObject.activeInHierarchy) {
    return;
}
        if (e.keyType == requiredKey) {
            collectedKeys++;
        }

        if (collectedKeys == requiredKeysCount) {
            onOpen?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
