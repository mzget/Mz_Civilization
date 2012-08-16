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

        PlayerPrefs.SetInt("amount_farm_instance", Buildings.FarmInstance.Count);
		PlayerPrefs.SetInt("amount_sawmill_instance", Buildings.SawmillInstance.Count);
		PlayerPrefs.SetInt("amount_millStone_instance", Buildings.MillStoneInstance.Count);
		PlayerPrefs.SetInt("amount_smelter_instance", Buildings.SmelterInstance.Count);
		for(int i = 0; i < Buildings.FarmInstance.Count; i++) {
			PlayerPrefs.SetInt("farm_Position_" + i, Buildings.FarmInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt("farm_Level_" + i, Buildings.FarmInstance[i].level);
		}
		for(int i = 0; i < Buildings.SawmillInstance.Count; i++) {
			PlayerPrefs.SetInt("sawmill_Position_" + i, Buildings.SawmillInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt("sawmill_Level_" + i, Buildings.SawmillInstance[i].level);
		}
		for(int i = 0; i < Buildings.MillStoneInstance.Count; i++) {
			PlayerPrefs.SetInt("millstone_Position_" + i, Buildings.MillStoneInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt("millstone_Level_" + i, Buildings.MillStoneInstance[i].level);
		}
		for(int i = 0; i < Buildings.SmelterInstance.Count; i++) {
			PlayerPrefs.SetInt("smelter_Position_" + i, Buildings.SmelterInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt("smelter_Level_" + i, Buildings.SmelterInstance[i].level);
		}
		
        //PlayerPrefs.SetInt("sawmill_Level", Buildings.SawmillInstance.level);
        //PlayerPrefs.SetInt("millstone_Level", Buildings.MillStoneInstance.level);
        //PlayerPrefs.SetInt("smelter_Level", Buildings.SmelterInstance.level);
		
		Debug.Log("Saving...");
    }
}
