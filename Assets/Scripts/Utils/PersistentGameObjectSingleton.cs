using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameObjectSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// Static instance of PersistentGameObjectSingleton which allows it to be accessed by any other script.
    /// </summary>
    public static PersistentGameObjectSingleton<T> Instance { get; private set; }

    /// <summary>
    /// Things to do as soon as the scene starts up
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            //if not, set instance to this
            Instance = this;
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GlobalManager.
            DestroyImmediate(gameObject);
            return;
        }
    }
}
