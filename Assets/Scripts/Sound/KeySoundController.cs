using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(KeyController))]
public class KeySoundController : MonoBehaviour
{
    [SerializeField]
    [EventRef]
    string keyCollectEventRef;


    KeyController keyController;

    void Start()
    {
        keyController = GetComponent<KeyController>();
        keyController.onCollected += OnCollected;
    }

    void OnDestroy()
    {
        if (keyController == null)
            return;

        keyController.onCollected -= OnCollected;    
    }

    void OnCollected()
    {
        RuntimeManager.PlayOneShot(keyCollectEventRef, transform.position);
    }
}
