using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    static readonly string PLAYER_TAG = "Player_2D";


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG))
            return;

        HandlePlayerCollision();
    }

    void HandlePlayerCollision()
    {
        // Play collecting animation and then destroy
        Destroy(gameObject);
    }
}
