using UnityEngine;
using System.Collections;

public class Mz_SaveData
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

        PlayerPrefs.SetInt("amountFarmInstance", Buildings.FarmInstance.Count);
		for(int i = 0; i < Buildings.FarmInstance.Count; i++) {
//			PlayerPrefs.SetString("farm_Position_" + i, Buildings.FarmInstance[i].buildingPosition_Data);
			PlayerPrefs.SetInt("farm_Position_" + i, Buildings.FarmInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt("farm_Level_" + i, Buildings.FarmInstance[i].level);
		}
		
        //PlayerPrefs.SetInt("sawmill_Level", Buildings.SawmillInstance.level);
        //PlayerPrefs.SetInt("millstone_Level", Buildings.MillStoneInstance.level);
        //PlayerPrefs.SetInt("smelter_Level", Buildings.SmelterInstance.level);
		
		Debug.Log("Saving...");
    }
}
