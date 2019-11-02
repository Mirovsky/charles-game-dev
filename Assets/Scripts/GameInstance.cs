using System.Collections;
using System.Collections.Generic;

public class GameInstance
{
    private static GameInstance instance = null;
    public static GameInstance Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameInstance();
            }
            return instance;
        }
    }


    public PlayerConfig.TYPE type = PlayerConfig.TYPE.DEAD_ONE;
}
