using UnityEngine;
using System.Collections;

[System.Serializable]
public class MilitaryIconData {

    public const string BARRACKS_ICON_NAME = "Barracks";
    

    public string[] nameOfBuildingIcon = new string[] {
		BARRACKS_ICON_NAME,	
	};
	public TileArea[] areaOfBuildings = new TileArea[] {
		new TileArea() { x = 0, y = 0, numSlotWidth = 3, numSlotHeight = 2, },
//		new TileArea() { x = 0, y = 0, numSlotWidth = 2, numSlotHeight = 2, },
	};
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
