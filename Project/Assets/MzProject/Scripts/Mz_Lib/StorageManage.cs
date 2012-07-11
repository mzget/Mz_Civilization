using UnityEngine;
using System.Collections;

public class StorageManage {

    //<!-- User Name.

    private static string username = string.Empty;
    public static string Username {
        get { return username; }
        set { username = value; }
    }

    //<!-- Score

    private static int score = 0;
    public static int Score
    {
        get { return score; }
        set { score = value; }
    }

    //public static Farm FarmInstance = null;
    //public static Sawmill SawMillInstance = null;
    //public static MillStone MillStoneInstance = null;
    //public static Smelter SmelterInstance = null;
}
