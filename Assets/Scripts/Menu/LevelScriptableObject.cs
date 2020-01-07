using UnityEngine;


[CreateAssetMenu(menuName = "OOO/LevelScriptableObject")]
public class LevelScriptableObject : ScriptableObject
{
    public string levelIdentifier;
    public string scenePath;

    public LevelScriptableObject nextLevel;

}
