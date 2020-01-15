using UnityEngine;


public class AmbienceSoundController : MonoBehaviour
{
    LevelGameState levelGameState;
    FMOD.Studio.EventInstance ambienceInstance;

    void Start()
    {
        levelGameState = FindObjectOfType<LevelGameState>();

        if (levelGameState == null || levelGameState.levelData == null) {
            Debug.LogWarning("Missing assigned LevelScriptableObject for this level!");
            enabled = false;
            return;
        }

        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(levelGameState.levelData.levelAmbience);
        ambienceInstance.start();
    }

}
