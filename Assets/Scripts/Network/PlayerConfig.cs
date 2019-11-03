using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "OOO")]
public class PlayerConfig: ScriptableObject
{

    //Properties that will be encapsulated in GameInstance

    public enum TYPE
    {
        OOO,
        DEAD_ONE
    }
}
