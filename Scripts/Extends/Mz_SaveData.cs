using UnityEngine;
using System.Collections;

public class Mz_StorageManagement {    
    /// <summary>
    /// Standard storage game data.
    /// </summary>
	public static int SaveSlot = 0;	//<!-- Save Game Slot.
	public static string Username = string.Empty;	//<!-- User Name.
}

public class Mz_SaveData : ISaveData
{		
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
	
	public const string KEY_BuildingAreaState = "BuildingAreaState";
	public const string TownCenter_level = "TownCenter_level";
	
	#region <@-- Utility Section.

	//<!-- House data key.
	public const string numberOfHouse_Instance = "numberOfHouse_Instance";
	public const string house_position_ = "house_position_";
	public const string house_level_ = "house_level_";
	//<!-- Academy key data.
	public const string KEY_AcademyInstance = "AcademyInstance";
	public const string KEY_AcademyPosition = "AcademyPosition";
	public const string KEY_AcademyLevel = "AcademyLevel";

	#endregion

	#region <@-- Economy Section.
       
    //<!-- Storehouse key. 
	public const string numberOfStorehouseInstance = "numberOfStorehouseInstance";
	public const string storehouse_position_ = "storehouse_position_";
	public const string storehouse_level_ = "storehouse_level_";
	//<!-- Market key.
    public const string KEY_MarketInstance = "MarketInstance";
    public const string KEY_MarketPosition = "positionOfMarket";
    public const string KEY_MarketLevel = "levelOfMarket";

	#endregion

	#region <@-- Military Section.
    
	//<!-- Barrack data key.
	public const string numberOf_BarracksInstancs = "numberOf_BarracksInstancs";
	public const string barracks_position_ = "barracks_position_";
	public const string barracks_level_ = "barracks_level_";

	#endregion

	#region <@-- Mission storage key.

	public const string KEY_CURRENT_MISSION_ID = "KEY_CURRENT_MISSION_ID";
	public const string KEY_ARRAY_MISSION_COMPLETE = "KEY_ARRAY_MISSION_COMPLETE";

	#endregion

	public class GreekAI_DataStore {
		public const string KEY_GREEK_AI_SPEARMAN = "GREEK_AI_SPEARMAN";
		public const string KEY_GREEK_AI_HAPASPIST = "GREEK_AI_HAPASPIST";
		public const string KEY_GREEK_AI_HOPLITE = "GREEK_AI_HOPLITE";
	}


    public void Save()
    {
        Debug.LogWarning("Starting Saving...");
		
		if(Mz_StorageManagement.Username != "") {
			PlayerPrefs.SetString(Mz_StorageManagement.SaveSlot + ":" + KEY_username, Mz_StorageManagement.Username);
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_sumoffood, StoreHouse.SumOfFood);
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_sumofwood, StoreHouse.SumOfWood);
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_sumofstone, StoreHouse.SumOfStone);
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + KEY_SUMOFCOPPER, StoreHouse.SumOfCopper);
            PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + KEY_sumOfArmor, StoreHouse.sumOfArmor);
            PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + KEY_sumOfWeapon, StoreHouse.sumOfWeapon);
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_sumofgold, StoreHouse.sumOfGold);
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + KEY_AMOUNT_OF_SPEARMAN, BarracksBeh.AmountOfSpearman);
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + KEY_AMOUNT_OF_HAPASPIST, BarracksBeh.AmountOfHapaspist);
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + KEY_AMOUNT_OF_HOPLITE, BarracksBeh.AmountOfHoplite);

			//<@-- Mission data.
            PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + KEY_CURRENT_MISSION_ID, QuestSystemManager.CurrentMissionTopic_ID);
            PlayerPrefsX.SetBoolArray(Mz_StorageManagement.SaveSlot + KEY_ARRAY_MISSION_COMPLETE, QuestSystemManager.arr_isMissionComplete);

            //<@-- Building area state data.
            PlayerPrefsX.SetBoolArray(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_BuildingAreaState, StageManager.arr_buildingAreaState);

			//<!-- TownCenter.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + TownCenter_level, BuildingBeh.TownCenter.Level);
			
			#region <<!--- Utility Section.
			
			//<!--- House instance data. --->>
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + numberOfHouse_Instance, BuildingBeh.House_Instances.Count);
			for (int i = 0; i < BuildingBeh.House_Instances.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + house_position_ + i, BuildingBeh.House_Instances[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + house_level_ + i, BuildingBeh.House_Instances[i].Level);
			}
			//<!-- Academy.
			if(BuildingBeh.AcademyInstance) {
				PlayerPrefsX.SetBool(Mz_StorageManagement.SaveSlot + ":" + KEY_AcademyInstance, BuildingBeh.AcademyInstance);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_AcademyPosition , BuildingBeh.AcademyInstance.IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_AcademyLevel, BuildingBeh.AcademyInstance.Level);
			}
			
			#endregion
	
	        #region <<!--- Resource Section.
	
	        //<!-- Farm Data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + amount_farm_instance, BuildingBeh.Farm_Instance.Count);
			for(int i = 0; i < BuildingBeh.Farm_Instance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + farm_position_ + i, BuildingBeh.Farm_Instance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + farm_level_ + i, BuildingBeh.Farm_Instance[i].Level);
			}
			
			//<!-- Sawmill Data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + amount_sawmill_instance, BuildingBeh.Sawmill_Instance.Count);
			for(int i = 0; i < BuildingBeh.Sawmill_Instance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + sawmill_position_ + i, BuildingBeh.Sawmill_Instance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + sawmill_level_ + i, BuildingBeh.Sawmill_Instance[i].Level);
			}
			
			//<!-- MillStone Data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + amount_millstone_instance, BuildingBeh.MillStoneInstance.Count);
			for(int i = 0; i < BuildingBeh.MillStoneInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + millstone_position_ + i, BuildingBeh.MillStoneInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + millstone_level_ + i, BuildingBeh.MillStoneInstance[i].Level);
			}
			
			//<!-- Smelter Data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + amount_smelter_instance, BuildingBeh.SmelterInstance.Count);
			for(int i = 0; i < BuildingBeh.SmelterInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + smelter_position_ + i, BuildingBeh.SmelterInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + smelter_level_ + i, BuildingBeh.SmelterInstance[i].Level);
	        }
	
	        #endregion
	
	        #region <<!--- Economy Section.
	
	        //<!-- Save Storehouse data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + numberOfStorehouseInstance, BuildingBeh.StoreHouseInstance.Count);
			for (int i = 0; i < BuildingBeh.StoreHouseInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + storehouse_position_ + i, BuildingBeh.StoreHouseInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + storehouse_level_ + i, BuildingBeh.StoreHouseInstance[i].Level);
	        }
	
	        //<!-- Market data.
			if(BuildingBeh.MarketInstance) {
				PlayerPrefsX.SetBool(Mz_StorageManagement.SaveSlot + ":" + KEY_MarketInstance, BuildingBeh.MarketInstance);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_MarketPosition , BuildingBeh.MarketInstance.IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_MarketLevel, BuildingBeh.MarketInstance.Level);
			}

	        #endregion
			
			#region <<!--- Military Section.
	
	        //<!-- Save Barracks data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + numberOf_BarracksInstancs, BuildingBeh.Barrack_Instances.Count);
	        for (int i = 0; i < BuildingBeh.Barrack_Instances.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + barracks_position_ + i, BuildingBeh.Barrack_Instances[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + barracks_level_ + i, BuildingBeh.Barrack_Instances[i].Level);
			}
			
			#endregion
		
		    #region <!-- GAME AI section.

		    //<@!-- Greek tribe AI data.
		    PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_SPEARMAN, StageManager.list_AICity[0].AmountOfUnits[0]);
		    PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HAPASPIST, StageManager.list_AICity[0].AmountOfUnits[1]);
		    PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HOPLITE, StageManager.list_AICity[0].AmountOfUnits[1]);
		
		    #endregion 
			
	        Debug.LogWarning("PlayerPrefs.Save --->");
			
	        PlayerPrefs.Save();
			
	        Debug.LogWarning("Saving complete...");
		}
		else {
			Debug.LogWarning("Saving failed.");
		}
    }

	public void Load() {
		
		Mz_StorageManagement.Username = PlayerPrefs.GetString(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_username);
		StoreHouse.SumOfFood = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_sumoffood);
		StoreHouse.SumOfWood = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_sumofwood);
		StoreHouse.SumOfStone = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_sumofstone);
		StoreHouse.SumOfCopper = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_SUMOFCOPPER);
		StoreHouse.sumOfArmor = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_sumOfArmor);
		StoreHouse.sumOfWeapon = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_sumOfWeapon);
		StoreHouse.sumOfGold = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_sumofgold);
		BarracksBeh.AmountOfSpearman = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_AMOUNT_OF_SPEARMAN, 0);
		BarracksBeh.AmountOfHapaspist = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_AMOUNT_OF_HAPASPIST, 0);
		BarracksBeh.AmountOfHoplite = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_AMOUNT_OF_HOPLITE, 0);
		
        //<@-- Load mission id.
        QuestSystemManager.CurrentMissionTopic_ID = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_CURRENT_MISSION_ID, 0);
		QuestSystemManager.arr_isMissionComplete = PlayerPrefsX.GetBoolArray(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_ARRAY_MISSION_COMPLETE, false, 16);
		
        Debug.Log("Load storage data to static variable complete.");
	}

    public void DeleteSave() {
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + ":" + KEY_username);
	    PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + ":" + KEY_sumoffood);
	    PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + ":" + KEY_sumofwood);
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + ":" + KEY_sumofstone);
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + KEY_SUMOFCOPPER);
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + KEY_sumOfArmor);
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + KEY_sumOfWeapon);
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + ":" + KEY_sumofgold);
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + KEY_AMOUNT_OF_SPEARMAN);
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + KEY_AMOUNT_OF_HAPASPIST);
        PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + KEY_AMOUNT_OF_HOPLITE);
        
			//<@-- Mission data.
            PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + KEY_CURRENT_MISSION_ID);
			PlayerPrefsX.SetBoolArray(Mz_StorageManagement.SaveSlot + KEY_ARRAY_MISSION_COMPLETE, QuestSystemManager.arr_isMissionComplete);
			//<!-- TownCenter.
			PlayerPrefs.DeleteKey(Mz_StorageManagement.SaveSlot + ":" + TownCenter_level);
			
			#region <<!--- Utility Section.
			
			//<!--- House instance data. --->>
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + numberOfHouse_Instance, BuildingBeh.House_Instances.Count);
			for (int i = 0; i < BuildingBeh.House_Instances.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + house_position_ + i, BuildingBeh.House_Instances[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + house_level_ + i, BuildingBeh.House_Instances[i].Level);
			}
			//<!-- Academy.
			if(BuildingBeh.AcademyInstance) {
				PlayerPrefsX.SetBool(Mz_StorageManagement.SaveSlot + ":" + KEY_AcademyInstance, BuildingBeh.AcademyInstance);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_AcademyPosition , BuildingBeh.AcademyInstance.IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_AcademyLevel, BuildingBeh.AcademyInstance.Level);
			}
			
			#endregion
	
	        #region <<!--- Resource Section.
	
	        //<!-- Farm Data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + amount_farm_instance, BuildingBeh.Farm_Instance.Count);
			for(int i = 0; i < BuildingBeh.Farm_Instance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + farm_position_ + i, BuildingBeh.Farm_Instance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + farm_level_ + i, BuildingBeh.Farm_Instance[i].Level);
			}
			
			//<!-- Sawmill Data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + amount_sawmill_instance, BuildingBeh.Sawmill_Instance.Count);
			for(int i = 0; i < BuildingBeh.Sawmill_Instance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + sawmill_position_ + i, BuildingBeh.Sawmill_Instance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + sawmill_level_ + i, BuildingBeh.Sawmill_Instance[i].Level);
			}
			
			//<!-- MillStone Data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + amount_millstone_instance, BuildingBeh.MillStoneInstance.Count);
			for(int i = 0; i < BuildingBeh.MillStoneInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + millstone_position_ + i, BuildingBeh.MillStoneInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + millstone_level_ + i, BuildingBeh.MillStoneInstance[i].Level);
			}
			
			//<!-- Smelter Data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + amount_smelter_instance, BuildingBeh.SmelterInstance.Count);
			for(int i = 0; i < BuildingBeh.SmelterInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + smelter_position_ + i, BuildingBeh.SmelterInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + smelter_level_ + i, BuildingBeh.SmelterInstance[i].Level);
	        }
	
	        #endregion
	
	        #region <<!--- Economy Section.
	
	        //<!-- Save Storehouse data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + numberOfStorehouseInstance, BuildingBeh.StoreHouseInstance.Count);
			for (int i = 0; i < BuildingBeh.StoreHouseInstance.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + storehouse_position_ + i, BuildingBeh.StoreHouseInstance[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + storehouse_level_ + i, BuildingBeh.StoreHouseInstance[i].Level);
	        }
	
	        //<!-- Market data.
			if(BuildingBeh.MarketInstance) {
				PlayerPrefsX.SetBool(Mz_StorageManagement.SaveSlot + ":" + KEY_MarketInstance, BuildingBeh.MarketInstance);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_MarketPosition , BuildingBeh.MarketInstance.IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + KEY_MarketLevel, BuildingBeh.MarketInstance.Level);
			}

	        #endregion
			
			#region <<!--- Military Section.
	
	        //<!-- Save Barracks data.
			PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + numberOf_BarracksInstancs, BuildingBeh.Barrack_Instances.Count);
	        for (int i = 0; i < BuildingBeh.Barrack_Instances.Count; i++) {
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + barracks_position_ + i, BuildingBeh.Barrack_Instances[i].IndexOfPosition);
				PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + ":" + barracks_level_ + i, BuildingBeh.Barrack_Instances[i].Level);
			}
			
			#endregion
		
		    #region <!-- GAME AI section.

		    //<@!-- Greek tribe AI data.
		    PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_SPEARMAN, StageManager.list_AICity[0].AmountOfUnits[0]);
		    PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HAPASPIST, StageManager.list_AICity[0].AmountOfUnits[1]);
		    PlayerPrefs.SetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HOPLITE, StageManager.list_AICity[0].AmountOfUnits[1]);
		
		    #endregion 
			
    }
}
