using UnityEngine;
using System.Collections;

public class Mz_SaveData : MonoBehaviour
{	
    // Use this for initialization
    void Start()
    {

    }

    public static void Save() 
	{
        PlayerPrefs.SetString("username", StorageManage.Username);
        PlayerPrefs.SetInt("sumoffood", StoreHouse.sumOfFood);
		PlayerPrefs.SetInt("sumofwood", StoreHouse.sumOfWood);
		PlayerPrefs.SetInt("sumofgold", StoreHouse.sumOfGold);
		PlayerPrefs.SetInt("sumofstone", StoreHouse.sumOfStone);
//        PlayerPrefs.SetInt("farmlevel", Buildings.FarmInstance.level);
//        PlayerPrefs.SetInt("sawmilllevel", Buildings.SawMillInstance.level);
    }

    void OnApplicationQuit() {
        Save();
    }
}
