using System;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public Action onCollected;

    static readonly string PLAYER_TAG = "Player_2D";

    public KeyType keyType;


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG))
            return;

        HandlePlayerCollision();
    }

    void HandlePlayerCollision()
    {
        onCollected?.Invoke();
        // Play collecting animation and then destroy
        Destroy(gameObject);
    }
}
