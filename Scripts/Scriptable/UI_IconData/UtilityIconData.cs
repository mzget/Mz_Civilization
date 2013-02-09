using UnityEngine;
using System.Collections;

[System.Serializable]
public class UtilityIconData {
	
	public const string HOUSE_ICON_NAME = "House";
	internal const string ACADEMY_ICON_NAME = "Academy";
	
	public string[] nameOfBuildingIcon = new string[] {
		HOUSE_ICON_NAME, ACADEMY_ICON_NAME,	
	};
	public TileArea[] areaOfBuildings = new TileArea[] {
		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
	};
}
