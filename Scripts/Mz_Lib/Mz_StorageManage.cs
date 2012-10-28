using UnityEngine;
using System.Collections;

public class Mz_StorageManage {

    #region Standard storage game data.

    //<!-- Save Game Slot.
    private static int saveSlot = 0;
    public static int SaveSlot
    {
        get { return saveSlot; }
        set { saveSlot = value; }
    }

    //<!-- User Name.
    private static string username = string.Empty;
    public static string Username
    {
        get { return username; }
        set { username = value; }
    }

    #endregion
}
