using UnityEngine;
using System.Collections;

public class Mz_SaveData
{	
	#region Standard storage game data.
	
	//<!-- Save Game Slot.
	public static int SaveSlot = 0;
	//<!-- User Name.
	public static string Username = string.Empty;
	
	#endregion


	public const string KEY_username = "username";
	public const string KEY_sumoffood = "sumoffood";
	public const string KEY_sumofwood = "sumofwood";
	public const string KEY_sumofstone = "sumofstone";
	public const string KEY_SUMOFCOPPER = "SUMOFCOPPER";
    public const string KEY_sumOfArmor = "sumofarmor";
    public const string KEY_sumOfWeapon = "sumofweapon";
	public const string KEY_sumofgold = "sumofgold";
	public const string KEY_AMOUNT_OF_SPEARMAN = "AMOUNT_OF_SPEARMAN";
	public const string KEY_AMOUNT_OF_HAPASPIST = "AMOUNT_OF_HAPASPIST";
	public const string KEY_AMOUNT_OF_HOPLITE = "AMOUNT_OF_HOPLITE";
	
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
	//<!-- Academy key data.
	public const string KEY_AcademyInstance = "AcademyInstance";
	public const string KEY_AcademyPosition = "AcademyPosition";
	public const string KEY_AcademyLevel = "AcademyLevel";
	
    /// <summary>
    /// Economy Section.
    /// </summary>
   
    //<!-- Storehouse key. 
	public const string numberOfStorehouseInstance = "numberOfStorehouseInstance";
	public const string storehouse_position_ = "storehouse_position_";
	public const string storehouse_level_ = "storehouse_level_";
	//<!-- Market key.
    public const string KEY_MarketInstance = "MarketInstance";
    public const string KEY_MarketPosition = "positionOfMarket";
    public const string KEY_MarketLevel = "levelOfMarket";

    /// <summary>
    /// Military Section.
    /// </summary>
	public const string numberOf_BarracksInstancs = "numberOf_BarracksInstancs";
	public const string barracks_position_ = "barracks_position_";
	public const string barracks_level_ = "barracks_level_";


    public static void Save()
    {
        Debug.LogWarning("Starting Saving...");
		
		if(Mz_SaveData.Username != "") {
			PlayerPrefs.SetString(Mz_SaveData.SaveSlot + ":" + KEY_username, Mz_SaveData.Username);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_sumoffood, StoreHouse.SumOfFood);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_sumofwood, StoreHouse.SumOfWood);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_sumofstone, StoreHouse.SumOfStone);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_SUMOFCOPPER, StoreHouse.SumOfCopper);
            PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_sumOfArmor, StoreHouse.sumOfArmor);
            PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_sumOfWeapon, StoreHouse.sumOfWeapon);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_sumofgold, StoreHouse.sumOfGold);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_AMOUNT_OF_SPEARMAN, BarracksBeh.AmountOfSpearman);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_AMOUNT_OF_HAPASPIST, BarracksBeh.AmountOfHapaspist);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_AMOUNT_OF_HOPLITE, BarracksBeh.AmountOfHoplite);
			//<!-- TownCenter.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + TownCenter_level, BuildingBeh.TownCenter.Level);
			
			#region <<!--- Utility Section.
			
			//<!--- House instance data. --->>
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + numberOfHouse_Instance, BuildingBeh.House_Instances.Count);
			for (int i = 0; i < BuildingBeh.House_Instances.Count; i++) {
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + house_position_ + i, BuildingBeh.House_Instances[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + house_level_ + i, BuildingBeh.House_Instances[i].Level);
			}
			//<!-- Academy.
			if(BuildingBeh.AcademyInstance) {
				PlayerPrefsX.SetBool(Mz_SaveData.SaveSlot + ":" + KEY_AcademyInstance, BuildingBeh.AcademyInstance);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_AcademyPosition , BuildingBeh.AcademyInstance.IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_AcademyLevel, BuildingBeh.AcademyInstance.Level);
			}
			
			#endregion
	
	        #region <<!--- Resource Section.
	
	        //<!-- Farm Data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + amount_farm_instance, BuildingBeh.Farm_Instance.Count);
			for(int i = 0; i < BuildingBeh.Farm_Instance.Count; i++) {
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + farm_position_ + i, BuildingBeh.Farm_Instance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + farm_level_ + i, BuildingBeh.Farm_Instance[i].Level);
			}
			
			//<!-- Sawmill Data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + amount_sawmill_instance, BuildingBeh.Sawmill_Instance.Count);
			for(int i = 0; i < BuildingBeh.Sawmill_Instance.Count; i++) {
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + sawmill_position_ + i, BuildingBeh.Sawmill_Instance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + sawmill_level_ + i, BuildingBeh.Sawmill_Instance[i].Level);
			}
			
			//<!-- MillStone Data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + amount_millstone_instance, BuildingBeh.MillStoneInstance.Count);
			for(int i = 0; i < BuildingBeh.MillStoneInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + millstone_position_ + i, BuildingBeh.MillStoneInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + millstone_level_ + i, BuildingBeh.MillStoneInstance[i].Level);
			}
			
			//<!-- Smelter Data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + amount_smelter_instance, BuildingBeh.SmelterInstance.Count);
			for(int i = 0; i < BuildingBeh.SmelterInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + smelter_position_ + i, BuildingBeh.SmelterInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + smelter_level_ + i, BuildingBeh.SmelterInstance[i].Level);
	        }
	
	        #endregion
	
	        #region <<!--- Economy Section.
	
	        //<!-- Save Storehouse data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + numberOfStorehouseInstance, BuildingBeh.StoreHouseInstance.Count);
			for (int i = 0; i < BuildingBeh.StoreHouseInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + storehouse_position_ + i, BuildingBeh.StoreHouseInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + storehouse_level_ + i, BuildingBeh.StoreHouseInstance[i].Level);
	        }
	
	        //<!-- Market data.
			if(BuildingBeh.MarketInstance) {
				PlayerPrefsX.SetBool(Mz_SaveData.SaveSlot + ":" + KEY_MarketInstance, BuildingBeh.MarketInstance);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_MarketPosition , BuildingBeh.MarketInstance.IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_MarketLevel, BuildingBeh.MarketInstance.Level);
			}

	        #endregion
			
			#region <<!--- Military Section.
	
	        //<!-- Save Barracks data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + numberOf_BarracksInstancs, BuildingBeh.Barrack_Instances.Count);
	        for (int i = 0; i < BuildingBeh.Barrack_Instances.Count; i++)
	        {
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + barracks_position_ + i, BuildingBeh.Barrack_Instances[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + barracks_level_ + i, BuildingBeh.Barrack_Instances[i].Level);
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
