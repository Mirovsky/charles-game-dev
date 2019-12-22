using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadOneMeshRotator : MonoBehaviour
{
    [SerializeField]
    Pathfinding pathfinding;


    void Update()
    {
        transform.rotation = pathfinding.GetRotation();
    }
}
