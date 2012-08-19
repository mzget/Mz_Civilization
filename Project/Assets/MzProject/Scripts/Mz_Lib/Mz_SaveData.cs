using UnityEngine;
using System.Collections;

public class Mz_SaveData
{	
	public const string username = "username";
	public const string sumoffood = "sumoffood";
	public const string sumofwood = "sumofwood";
	public const string sumofgold = "sumofgold";
	public const string sumofstone = "sumofstone";
	public const string amount_farm_instance = "amount_farm_instance";
	public const string amount_sawmill_instance = "amount_sawmill_instance"; 
	public const string amount_millstone_instance = "amount_millstone_instance";
	public const string amount_smelter_instance = "amount_smelter_instance";
	public const string farm_position_ = "farm_position_";
    public const string farm_level_ = "farm_level_";
    public const string sawmill_position_ = "sawmill_position_";
    public const string sawmill_level_ = "sawmill_level_";
    public const string millstone_position_ = "millstone_position_";
    public const string millstone_level_ = "millstone_level_";
    public const string smelter_position_ = "smelter_position_";
    public const string smelter_level_ = "smelter_level_";



    public static void Save() 
	{
        PlayerPrefs.SetString(username, StorageManage.Username);
        PlayerPrefs.SetInt(sumoffood, StoreHouse.sumOfFood);
		PlayerPrefs.SetInt(sumofwood, StoreHouse.sumOfWood);
		PlayerPrefs.SetInt(sumofstone, StoreHouse.sumOfStone);
		PlayerPrefs.SetInt(sumofgold, StoreHouse.sumOfGold);

        PlayerPrefs.SetInt(amount_farm_instance, Buildings.Farm_Instance.Count);
		PlayerPrefs.SetInt(amount_sawmill_instance, Buildings.Sawmill_Instance.Count);
		PlayerPrefs.SetInt(amount_millstone_instance, Buildings.MillStoneInstance.Count);
		PlayerPrefs.SetInt(amount_smelter_instance, Buildings.SmelterInstance.Count);
		
		for(int i = 0; i < Buildings.Farm_Instance.Count; i++) {
			PlayerPrefs.SetInt(farm_position_ + i, Buildings.Farm_Instance[i].IndexOfPosition);
			PlayerPrefs.SetInt(farm_level_ + i, Buildings.Farm_Instance[i].level);
		}
		for(int i = 0; i < Buildings.Sawmill_Instance.Count; i++) {
			PlayerPrefs.SetInt(sawmill_position_ + i, Buildings.Sawmill_Instance[i].IndexOfPosition);
			PlayerPrefs.SetInt(sawmill_level_ + i, Buildings.Sawmill_Instance[i].level);
		}
		for(int i = 0; i < Buildings.MillStoneInstance.Count; i++) {
			PlayerPrefs.SetInt(millstone_position_ + i, Buildings.MillStoneInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt(millstone_level_ + i, Buildings.MillStoneInstance[i].level);
		}
		for(int i = 0; i < Buildings.SmelterInstance.Count; i++) {
			PlayerPrefs.SetInt(smelter_position_ + i, Buildings.SmelterInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt(smelter_level_ + i, Buildings.SmelterInstance[i].level);
		}
		
		Debug.Log("Saving...");
    }
}
