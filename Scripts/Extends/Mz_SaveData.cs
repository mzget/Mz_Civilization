using UnityEngine;
using System.Collections;
using JsonFx.Json;

public class Mz_SaveData : ISaveData
{
    /// <summary>
    /// Standard storage game data.
    /// </summary>
	public static int SaveSlot = 0;	//<!-- Save Game Slot.
	public static string Username = string.Empty;	//<!-- User Name.
	
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

	public const string KEY_FARM_AREA_ = "farm_area_";
    public const string farm_level_ = "farm_level_";

    public const string KEY_SAWMILL_AREA_ = "sawmill_position_";
    public const string sawmill_level_ = "sawmill_level_";

    public const string KEY_AMOUNT_OF_StoneCrushingPlant = "KEY_AMOUNT_OF_StoneCrushingPlant";
    public const string KEY_StoneCrushingPlant_AREA_ = "KEY_StoneCrushingPlant_AREA_";
    public const string KEY_StoneCrushingPlant_LEVEL_ = "KEY_StoneCrushingPlant_LEVEL_";

    public const string KEY_AMOUNT_OF_SMELTER = "KEY_AMOUNT_OF_SMELTER";
    public const string KEY_SMELTER_AREA_ = "KEY_SMELTER_AREA_";
    public const string KEY_SMELTER_LEVEL_ = "KEY_SMELTER_LEVEL_";
	
	public const string KEY_BuildingAreaState = "BuildingAreaState";
	public const string TownCenter_level = "TownCenter_level";
	
	#region <@-- Utility Section.

	//<!-- House data key.
	public const string numberOfHouse_Instance = "numberOfHouse_Instance";
    public const string KEY_HOUSE_AREA_ = "HOUSE_AREA_";
	public const string house_level_ = "house_level_";
	//<!-- Academy key data.
	public const string KEY_AcademyInstance = "AcademyInstance";
	public const string KEY_AcademyPosition = "AcademyPosition";
	public const string KEY_AcademyLevel = "AcademyLevel";

	#endregion

	#region <@-- Economy Section.
       
    //<!-- Storehouse key. 
	public const string numberOfStorehouseInstance = "numberOfStorehouseInstance";
	public const string KEY_STOREHOUSE_AREA_ = "storehouse_position_";
	public const string KEY_STOREHOUSE_LEVEL_ = "storehouse_level_";
	//<!-- Market key.
    public const string KEY_MarketInstance = "MarketInstance";
    public const string KEY_Market_Area = "positionOfMarket";
    public const string KEY_Market_Level = "levelOfMarket";

	#endregion

	#region <@-- Military Section.
    
	//<!-- Barrack data key.
	public const string KEY_BarracksInstance = "BarracksInstance";
	public const string KEY_BarracksPosition = "BarracksPosition";
	public const string KEY_BarracksLevel = "BarracksLevel";

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

			//<@-- Mission data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_CURRENT_MISSION_ID, QuestSystemManager.CurrentMissionTopic_ID);
			PlayerPrefsX.SetBoolArray(Mz_SaveData.SaveSlot + KEY_ARRAY_MISSION_COMPLETE, QuestSystemManager.arr_isMissionComplete);

            //<@-- Building area state data.
			PlayerPrefsX.SetBoolArray(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_BuildingAreaState, CapitalCity.arr_buildingAreaState);

			//<!-- TownCenter.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + TownCenter_level, BuildingBeh.TownCenter.Level);
			
			#region <<!--- Utility Section.

			//<!--- House instance data. --->>
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + numberOfHouse_Instance, BuildingBeh.House_Instances.Count);
            for (int i = 0; i < BuildingBeh.House_Instances.Count; i++)
            {
                // this turns a C# object into a JSON string.
                string encodedString = JsonWriter.Serialize(new TileArea() { 
                    x = BuildingBeh.House_Instances[i].constructionArea.x,
                    y = BuildingBeh.House_Instances[i].constructionArea.y,
                    numSlotWidth = BuildingBeh.House_Instances[i].constructionArea.numSlotWidth,
                    numSlotHeight = BuildingBeh.House_Instances[i].constructionArea.numSlotHeight,
                });
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + ":" + KEY_HOUSE_AREA_ + i, encodedString);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + house_level_ + i, BuildingBeh.House_Instances[i].Level);
			}

			//<!-- Academy.
            if (BuildingBeh.AcademyInstance) {
                // this turns a C# object into a JSON string.
                string encodedString = JsonWriter.Serialize(new TileArea()
                {
                    x = BuildingBeh.AcademyInstance.constructionArea.x,
                    y = BuildingBeh.AcademyInstance.constructionArea.y,
                    numSlotWidth = BuildingBeh.AcademyInstance.constructionArea.numSlotWidth,
                    numSlotHeight = BuildingBeh.AcademyInstance.constructionArea.numSlotHeight,
                });
				PlayerPrefsX.SetBool(Mz_SaveData.SaveSlot + ":" + KEY_AcademyInstance, BuildingBeh.AcademyInstance);
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + ":" + KEY_AcademyPosition, encodedString);
                PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + KEY_AcademyLevel, BuildingBeh.AcademyInstance.Level);
			}
			
			#endregion
	
	        #region <<!--- Resource Section.
	
	        //<!-- Farm Data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + amount_farm_instance, BuildingBeh.Farm_Instances.Count);
			for(int i = 0; i < BuildingBeh.Farm_Instances.Count; i++) {
				// this turns a C# object into a JSON string.
				string encodedString = JsonWriter.Serialize(new TileArea() { 
					x = BuildingBeh.Farm_Instances[i].constructionArea.x,
					y = BuildingBeh.Farm_Instances[i].constructionArea.y,
					numSlotWidth = BuildingBeh.Farm_Instances[i].constructionArea.numSlotWidth,
					numSlotHeight = BuildingBeh.Farm_Instances[i].constructionArea.numSlotHeight,
				});
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + KEY_FARM_AREA_ + i, encodedString);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + farm_level_ + i, BuildingBeh.Farm_Instances[i].Level);
			}
			
			//<!-- Sawmill Data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + amount_sawmill_instance, BuildingBeh.Sawmill_Instances.Count);
			for(int i = 0; i < BuildingBeh.Sawmill_Instances.Count; i++) {
				// this turns a C# object into a JSON string.
				string encodedString = JsonWriter.Serialize(new TileArea() { 
					x = BuildingBeh.Sawmill_Instances[i].constructionArea.x,
					y = BuildingBeh.Sawmill_Instances[i].constructionArea.y,
					numSlotWidth = BuildingBeh.Sawmill_Instances[i].constructionArea.numSlotWidth,
					numSlotHeight = BuildingBeh.Sawmill_Instances[i].constructionArea.numSlotHeight,
				});
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + KEY_SAWMILL_AREA_ + i, encodedString);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + sawmill_level_ + i, BuildingBeh.Sawmill_Instances[i].Level);
			}

            //<!-- StoneCrushingPlant Data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_AMOUNT_OF_StoneCrushingPlant, BuildingBeh.StoneCrushingPlant_Instances.Count);
            for (int i = 0; i < BuildingBeh.StoneCrushingPlant_Instances.Count; i++)
            {
                // this turns a C# object into a JSON string.
                string encodedString = JsonWriter.Serialize(new TileArea()
                {
                    x = BuildingBeh.StoneCrushingPlant_Instances[i].constructionArea.x,
                    y = BuildingBeh.StoneCrushingPlant_Instances[i].constructionArea.y,
                    numSlotWidth = BuildingBeh.StoneCrushingPlant_Instances[i].constructionArea.numSlotWidth,
                    numSlotHeight = BuildingBeh.StoneCrushingPlant_Instances[i].constructionArea.numSlotHeight,
                });
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + KEY_StoneCrushingPlant_AREA_ + i, encodedString);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_StoneCrushingPlant_LEVEL_ + i, BuildingBeh.StoneCrushingPlant_Instances[i].Level);
			}
			
			//<!-- Smelter Data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_AMOUNT_OF_SMELTER, BuildingBeh.Smelter_Instances.Count);
            for (int i = 0; i < BuildingBeh.Smelter_Instances.Count; i++)
            {
                // this turns a C# object into a JSON string.
                string encodedString = JsonWriter.Serialize(new TileArea()
                {
                    x = BuildingBeh.Smelter_Instances[i].constructionArea.x,
                    y = BuildingBeh.Smelter_Instances[i].constructionArea.y,
                    numSlotWidth = BuildingBeh.Smelter_Instances[i].constructionArea.numSlotWidth,
                    numSlotHeight = BuildingBeh.Smelter_Instances[i].constructionArea.numSlotHeight,
                });
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + KEY_SMELTER_AREA_ + i, encodedString);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_SMELTER_LEVEL_ + i, BuildingBeh.Smelter_Instances[i].Level);
	        }
	
	        #endregion
	
	        #region <<!--- Economy Section.
	
	        //<!-- Save Storehouse data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + numberOfStorehouseInstance, BuildingBeh.StoreHouseInstance.Count);
            for (int i = 0; i < BuildingBeh.StoreHouseInstance.Count; i++) {
                // this turns a C# object into a JSON string.
                string encodedString = JsonWriter.Serialize(new TileArea()
                {
                    x = BuildingBeh.StoreHouseInstance[i].constructionArea.x,
                    y = BuildingBeh.StoreHouseInstance[i].constructionArea.y,
                    numSlotWidth = BuildingBeh.StoreHouseInstance[i].constructionArea.numSlotWidth,
                    numSlotHeight = BuildingBeh.StoreHouseInstance[i].constructionArea.numSlotHeight,
                });
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + KEY_STOREHOUSE_AREA_ + i, encodedString);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_STOREHOUSE_LEVEL_ + i, BuildingBeh.StoreHouseInstance[i].Level);
	        }
	
	        //<!-- Market data.
			if(BuildingBeh.MarketInstance) {
                // this turns a C# object into a JSON string.
                string encodedString = JsonWriter.Serialize(new TileArea()
                {
                    x = BuildingBeh.MarketInstance.constructionArea.x,
                    y = BuildingBeh.MarketInstance.constructionArea.y,
                    numSlotWidth = BuildingBeh.MarketInstance.constructionArea.numSlotWidth,
                    numSlotHeight = BuildingBeh.MarketInstance.constructionArea.numSlotHeight,
                });
				PlayerPrefsX.SetBool(Mz_SaveData.SaveSlot + KEY_MarketInstance, BuildingBeh.MarketInstance);
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + KEY_Market_Area , encodedString);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_Market_Level, BuildingBeh.MarketInstance.Level);
			}

	        #endregion
			
			#region <<!--- Military Section.
	
	        //<!-- Save Barracks data.
			if(BuildingBeh.Barrack_Instance) {
				// this turns a C# object into a JSON string.
				string encodedString = JsonWriter.Serialize(new TileArea()
				                                            {
					x = BuildingBeh.Barrack_Instance.constructionArea.x,
					y = BuildingBeh.Barrack_Instance.constructionArea.y,
					numSlotWidth = BuildingBeh.Barrack_Instance.constructionArea.numSlotWidth,
					numSlotHeight = BuildingBeh.Barrack_Instance.constructionArea.numSlotHeight,
				});
				PlayerPrefsX.SetBool(Mz_SaveData.SaveSlot + KEY_BarracksInstance, BuildingBeh.Barrack_Instance);
				PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + KEY_BarracksLevel, BuildingBeh.Barrack_Instance.Level);
                PlayerPrefs.SetString(Mz_SaveData.SaveSlot + KEY_BarracksPosition, encodedString);
			}
			
			#endregion
		
		    #region <!-- GAME AI section.

		    //<@!-- Greek tribe AI data.
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_SPEARMAN, GreekBeh.Spearman_unit);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HAPASPIST, GreekBeh.Hapaspist_unit);
			PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HOPLITE, GreekBeh.Hoplite_unit);
		
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
		Mz_SaveData.Username = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_username);
		StoreHouse.SumOfFood = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumoffood);
		StoreHouse.SumOfWood = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofwood);
		StoreHouse.SumOfStone = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofstone);
		StoreHouse.SumOfCopper = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_SUMOFCOPPER);
		StoreHouse.sumOfArmor = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_sumOfArmor);
		StoreHouse.sumOfWeapon = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_sumOfWeapon);
		StoreHouse.sumOfGold = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofgold);
		BarracksBeh.AmountOfSpearman = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_AMOUNT_OF_SPEARMAN, 0);
		BarracksBeh.AmountOfHapaspist = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_AMOUNT_OF_HAPASPIST, 0);
		BarracksBeh.AmountOfHoplite = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_AMOUNT_OF_HOPLITE, 0);
		
        //<@-- Load mission id.
		QuestSystemManager.CurrentMissionTopic_ID = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_CURRENT_MISSION_ID, 0);
		QuestSystemManager.arr_isMissionComplete = PlayerPrefsX.GetBoolArray(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_ARRAY_MISSION_COMPLETE, false, 16);
		
        Debug.Log("Load storage data to static variable complete.");
	}

    public void DeleteSave() {   
			
    }
}
