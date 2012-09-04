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
	
    /// <summary>
    /// Economy Section.
    /// </summary>
    
    //<!-- Storehouse key. 
	public const string numberOfStorehouseInstance = "numberOfStorehouseInstance";
	public const string storehouse_position_ = "storehouse_position_";
	public const string storehouse_level_ = "storehouse_level_";
	//<!-- Market key.
    public const string numberOfMarketInstance = "numberOfMarketInstance";
    public const string positionOfMarket_ = "positionOfMarket_";
    public const string levelOfMarket_ = "levelOfMarket_";

    /// <summary>
    /// Military Section.
    /// </summary>
	public const string numberOf_BarracksInstancs = "numberOf_BarracksInstancs";
	public const string barracks_position_ = "barracks_position_";
	public const string barracks_level_ = "barracks_level_";



    public static void Save()
    {
        Debug.Log("Saving...");

        PlayerPrefs.SetString(username, StorageManage.Username);
        PlayerPrefs.SetInt(sumoffood, StoreHouse.sumOfFood);
		PlayerPrefs.SetInt(sumofwood, StoreHouse.sumOfWood);
		PlayerPrefs.SetInt(sumofstone, StoreHouse.sumOfStone);
		PlayerPrefs.SetInt(sumofgold, StoreHouse.sumOfGold);

        #region Resource Section.

        //<!-- Farm Data.
        PlayerPrefs.SetInt(amount_farm_instance, Buildings.Farm_Instance.Count);
		for(int i = 0; i < Buildings.Farm_Instance.Count; i++) {
			PlayerPrefs.SetInt(farm_position_ + i, Buildings.Farm_Instance[i].IndexOfPosition);
			PlayerPrefs.SetInt(farm_level_ + i, Buildings.Farm_Instance[i].Level);
		}
		
		//<!-- Sawmill Data.
		PlayerPrefs.SetInt(amount_sawmill_instance, Buildings.Sawmill_Instance.Count);
		for(int i = 0; i < Buildings.Sawmill_Instance.Count; i++) {
			PlayerPrefs.SetInt(sawmill_position_ + i, Buildings.Sawmill_Instance[i].IndexOfPosition);
			PlayerPrefs.SetInt(sawmill_level_ + i, Buildings.Sawmill_Instance[i].Level);
		}
		
		//<!-- MillStone Data.
		PlayerPrefs.SetInt(amount_millstone_instance, Buildings.MillStoneInstance.Count);
		for(int i = 0; i < Buildings.MillStoneInstance.Count; i++) {
			PlayerPrefs.SetInt(millstone_position_ + i, Buildings.MillStoneInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt(millstone_level_ + i, Buildings.MillStoneInstance[i].Level);
		}
		
		//<!-- Smelter Data.
		PlayerPrefs.SetInt(amount_smelter_instance, Buildings.SmelterInstance.Count);
		for(int i = 0; i < Buildings.SmelterInstance.Count; i++) {
			PlayerPrefs.SetInt(smelter_position_ + i, Buildings.SmelterInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt(smelter_level_ + i, Buildings.SmelterInstance[i].Level);
        }

        #endregion

        #region <<!-- Economy Section.

        //<!-- Save Storehouse data.
		PlayerPrefs.SetInt(numberOfStorehouseInstance, Buildings.StoreHouseInstance.Count);
		for (int i = 0; i < Buildings.StoreHouseInstance.Count; i++) {
			PlayerPrefs.SetInt(storehouse_position_ + i, Buildings.StoreHouseInstance[i].IndexOfPosition);
			PlayerPrefs.SetInt(storehouse_level_ + i, Buildings.StoreHouseInstance[i].Level);
        }

        //<!-- Market data.
        PlayerPrefs.SetInt(numberOfMarketInstance, Buildings.MarketInstances.Count);
        for (int i = 0; i < Buildings.MarketInstances.Count; i++) {
            PlayerPrefs.SetInt(positionOfMarket_ + i, Buildings.MarketInstances[i].IndexOfPosition);
            PlayerPrefs.SetInt(levelOfMarket_ + i, Buildings.MarketInstances[i].Level);
        }

        #endregion

            //<!-- Save Barracks data.
            PlayerPrefs.SetInt(numberOf_BarracksInstancs, Buildings.Barrack_Instances.Count);
        for (int i = 0; i < Buildings.Barrack_Instances.Count; i++) {
            PlayerPrefs.SetInt(barracks_position_ + i, Buildings.Barrack_Instances[i].IndexOfPosition);
            PlayerPrefs.SetInt(barracks_level_ + i, Buildings.Barrack_Instances[i].Level);
        }

        Debug.Log("Saving complete...");
    }
}
