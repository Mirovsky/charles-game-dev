using UnityEngine;

class Persistence
{
    const string KEY_FIRST_START = "ooo_first_start";


    public static bool IsLevelLocked(string levelId)
        => GetInt(levelId, 0) == 0;

    public static void UnlockLevel(string levelId)
        => PlayerPrefs.SetInt(levelId, 1);

    public static bool IsFirstStart()
        => GetBool(KEY_FIRST_START, false);

    public static void SetGameStarted()
        => PlayerPrefs.SetInt(KEY_FIRST_START, 1);


    static int GetInt(string key, int def = 0)
    {
        var exists = PlayerPrefs.HasKey(key);
        if (!exists)
            return def;

        return PlayerPrefs.GetInt(key);
    }

    static string GetString(string key, string def = "")
    {
        var exists = PlayerPrefs.HasKey(key);
        if (!exists)
            return def;

        return PlayerPrefs.GetString(key);
    }

    static float GetFloat(string key, float def = 0)
    {
        var exists = PlayerPrefs.HasKey(key);
        if (!exists)
            return def;

        return PlayerPrefs.GetFloat(key);
    }

    static bool GetBool(string key, bool def = false)
    {
        var exists = PlayerPrefs.HasKey(key);
        if (!exists)
            return def;

        return PlayerPrefs.GetInt(key) == 1;
    }
}
