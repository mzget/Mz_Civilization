using UnityEngine;
using System.Collections;

public class Mz_SaveData
{	
	public const string username = "username";
	public const string sumoffood = "sumoffood";
	public const string sumofwood = "sumofwood";
	public const string sumofgold = "sumofgold";
	public const string sumofstone = "sumofstone";
//	public const string sumOfEmployee = "sumOfEmployee" ;
	
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
	
	public const string BuildingAreaState = "BuildingAreaState";
	public const string TownCenter_level = "TownCenter_level";
	
	/// <summary>
	/// Utility Section.
	/// </summary>
	public const string numberOfHouse_Instance = "numberOfHouse_Instance";
	public const string house_position_ = "house_position_";
	public const string house_level_ = "house_level_";
	
    /// <summary>
    /// Economy Section.
    /// </summary>
   
    //<!-- Storehouse key. 
	public const string numberOfStorehouseInstance = "numberOfStorehouseInstance";
	public const string storehouse_position_ = "storehouse_position_";
	public const string storehouse_level_ = "storehouse_level_";
	//<!-- Market key.
    public const string MarketInstance = "MarketInstance";
    public const string positionOfMarket = "positionOfMarket";
    public const string levelOfMarket = "levelOfMarket";

    /// <summary>
    /// Military Section.
    /// </summary>
	public const string numberOf_BarracksInstancs = "numberOf_BarracksInstancs";
	public const string barracks_position_ = "barracks_position_";
	public const string barracks_level_ = "barracks_level_";


    public static void Save()
    {
        Debug.LogWarning("Starting Saving...");
		
		if(Mz_StorageManage.Username != "") {
            PlayerPrefs.SetString(Mz_StorageManage.SaveSlot + ":" + username, Mz_StorageManage.Username);
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + sumoffood, StoreHouse.sumOfFood);
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + sumofwood, StoreHouse.sumOfWood);
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + sumofstone, StoreHouse.sumOfStone);
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + sumofgold, StoreHouse.sumOfGold);
			//<!-- TownCenter.
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + TownCenter_level, BuildingBeh.TownCenter.Level);
			
			#region <<!--- Utility Section.
			
			//<!--- House instance data. --->>
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + numberOfHouse_Instance, BuildingBeh.House_Instances.Count);
			for (int i = 0; i < BuildingBeh.House_Instances.Count; i++) {
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + house_position_ + i, BuildingBeh.House_Instances[i].IndexOfPosition);
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + house_level_ + i, BuildingBeh.House_Instances[i].Level);
			}
			
			
			#endregion
	
	        #region <<!--- Resource Section.
	
	        //<!-- Farm Data.
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + amount_farm_instance, BuildingBeh.Farm_Instance.Count);
			for(int i = 0; i < BuildingBeh.Farm_Instance.Count; i++) {
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + farm_position_ + i, BuildingBeh.Farm_Instance[i].IndexOfPosition);
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + farm_level_ + i, BuildingBeh.Farm_Instance[i].Level);
			}
			
			//<!-- Sawmill Data.
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + amount_sawmill_instance, BuildingBeh.Sawmill_Instance.Count);
			for(int i = 0; i < BuildingBeh.Sawmill_Instance.Count; i++) {
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + sawmill_position_ + i, BuildingBeh.Sawmill_Instance[i].IndexOfPosition);
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + sawmill_level_ + i, BuildingBeh.Sawmill_Instance[i].Level);
			}
			
			//<!-- MillStone Data.
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + amount_millstone_instance, BuildingBeh.MillStoneInstance.Count);
			for(int i = 0; i < BuildingBeh.MillStoneInstance.Count; i++) {
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + millstone_position_ + i, BuildingBeh.MillStoneInstance[i].IndexOfPosition);
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + millstone_level_ + i, BuildingBeh.MillStoneInstance[i].Level);
			}
			
			//<!-- Smelter Data.
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + amount_smelter_instance, BuildingBeh.SmelterInstance.Count);
			for(int i = 0; i < BuildingBeh.SmelterInstance.Count; i++) {
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + smelter_position_ + i, BuildingBeh.SmelterInstance[i].IndexOfPosition);
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + smelter_level_ + i, BuildingBeh.SmelterInstance[i].Level);
	        }
	
	        #endregion
	
	        #region <<!--- Economy Section.
	
	        //<!-- Save Storehouse data.
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + numberOfStorehouseInstance, BuildingBeh.StoreHouseInstance.Count);
			for (int i = 0; i < BuildingBeh.StoreHouseInstance.Count; i++) {
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + storehouse_position_ + i, BuildingBeh.StoreHouseInstance[i].IndexOfPosition);
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + storehouse_level_ + i, BuildingBeh.StoreHouseInstance[i].Level);
	        }
	
	        //<!-- Market data.
			if(BuildingBeh.MarketInstances) {
            	PlayerPrefsX.SetBool(Mz_StorageManage.SaveSlot + ":" + MarketInstance, BuildingBeh.MarketInstances);
        		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + positionOfMarket , BuildingBeh.MarketInstances.IndexOfPosition);
        		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + levelOfMarket, BuildingBeh.MarketInstances.Level);
			}

	        #endregion
			
			#region <<!--- Military Section.
	
	        //<!-- Save Barracks data.
            PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + numberOf_BarracksInstancs, BuildingBeh.Barrack_Instances.Count);
	        for (int i = 0; i < BuildingBeh.Barrack_Instances.Count; i++)
	        {
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + barracks_position_ + i, BuildingBeh.Barrack_Instances[i].IndexOfPosition);
                PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + ":" + barracks_level_ + i, BuildingBeh.Barrack_Instances[i].Level);
	        }
			
			#endregion
			
	        Debug.LogWarning("PlayerPrefs.Save --->");
			
	        PlayerPrefs.Save();
			
	        Debug.LogWarning("Saving complete...");
		}
		else {
			Debug.LogWarning("Saving failed.");
		}
    }
}
