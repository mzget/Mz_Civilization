using UnityEngine;
using System.Collections;


[System.Serializable]
public class EconomyIconData {

	public const string FARM_ICON_NAME = "Farm";
	public const string SAWMILL_ICON_NAME = "Sawmill";
	public const string STONECRUSHINGPLANT_ICON_NAME = "StoneCrushingPlant";
	public const string SMELTER_ICON_NAME = "Smelter";
	public const string STOREHOUSE_ICON_NAME = "Storehouse";
	public const string MARKET_ICON_NAME = "Market";
	
	public string[] nameOfBuildingIcon = new string[] {
		FARM_ICON_NAME, SAWMILL_ICON_NAME,
		STONECRUSHINGPLANT_ICON_NAME, SMELTER_ICON_NAME,
		STOREHOUSE_ICON_NAME, MARKET_ICON_NAME,
	};
	public TileArea[] areaOfBuildings = new TileArea[] {
		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
	};
}
