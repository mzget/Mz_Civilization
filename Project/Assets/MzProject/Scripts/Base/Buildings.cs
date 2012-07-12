using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingsTimeData {
	
	public float[] arrBuildingTimesData = new float[Buildings.MAX_LEVEL];
	
	public BuildingsTimeData(Buildings.BuildingType r_buildingType) 
	{
		if(r_buildingType == Buildings.BuildingType.general) {
//			float[] time_generalType = new float[];
//			time_generalType[0] = 10;
//			
//			buildingTimesData = time_generalType;
		}
		else if(r_buildingType == Buildings.BuildingType.resource) {
			float[] time_resourceType = { 
				30f, 90f, 300f, 600f, 1200f
			};
			
			arrBuildingTimesData = time_resourceType;
		}
	}
};

public class Buildings : MonoBehaviour {
	public const int MAX_LEVEL = 10;
	
	
    public Font showG;
    public GUISkin standard_Skin;
    public GUISkin building_Skin;
    public Texture2D buildingIcon_Texture;
	protected GameObject processbar_Obj_parent;
	protected OTSprite processBar_Scolling; 
		
    public int level = 0;
    protected bool _clicked = false;
    protected bool _CanDestruction = true;

    public static List<StoreHouse> storeHouseId = new List<StoreHouse>();
    public static Farm FarmInstance = null;
    public static Sawmill SawMillInstance = null;
    public static MillStone MillStoneInstance = null;
    public static Smelter SmelterInstance = null;
	
	public enum BuildingStatus { none = 0, buildingProcess = 1, };
	public BuildingStatus buildingStatus;
	public enum BuildingType { general = 0, resource, };
	protected BuildingType buildingType;

    protected BuildingsTimeData buildingTimeData;


    protected virtual void DestructionBuilding() { 
    
    }
	
	#region Building Process Section.
	
	public virtual void OnBuildingProcess() {
		Debug.Log("Class :: Building" + ":: OnBuildingProcess()");
	}
	
	protected virtual void BuildingProcess(Vector2 Rvalue) {
		Debug.Log("Class :: Buildings" + ":: BuildingProcess");
	}
	
	protected virtual void DestroyBuildingProcess() {		
		Debug.Log("Class :: Buildings" + ":: DestroyBuildingProcess");
	}
	
	#endregion
	
	#region Incloud Mouse Event.
	
    protected void OnMouseOver()
    {
		
    }

    protected void OnMouseDown()
    {
        _clicked = true;
    }

    protected void OnMouseExit()
    {

    }
	
	#endregion
	
    protected Rect windowRect;
    protected Rect exitButton_Rect;
    protected Vector2 scrollPosition = Vector2.zero;

    protected Rect background_Rect;
    protected Rect buildingIcon_Rect = new Rect(24, 24, 128, 128);
    protected Rect upgradeButton_Rect;
    protected Rect destructionButton_Rect;
    protected Rect imgContent_Rect = new Rect(40, 60, 100, 100);
    protected Rect contentRect = new Rect(170, 20, 590, 160);
	protected Rect warnningMessage_Rect = new Rect(Main.GameWidth/2 - 150, Main.GameHeight/2 -120, 300, 240);

    protected void OnGUI()
    {
        windowRect = new Rect(Main.GameWidth / 2 - 350, Main.GameHeight / 2 - 200, 700, 400);
        exitButton_Rect = new Rect(windowRect.width - 34, 2, 32, 32);
        upgradeButton_Rect = new Rect (windowRect.width - 140, 32, 100, 32);
        destructionButton_Rect = new Rect(windowRect.width - 250, 32, 100, 32);
        background_Rect = new Rect(0, 0, 700, 320);

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GameWidth, Screen.height / Main.GameHeight, 1));

        standard_Skin.window.font = showG; 

        building_Skin.box.wordWrap = true;
        building_Skin.box.fontSize = 16;
        building_Skin.box.fontStyle = FontStyle.Normal;
        building_Skin.box.contentOffset = new Vector2(128, 38);

        if (_clicked) {
            windowRect = GUI.Window(0, windowRect, CreateWindow, new GUIContent(name, "GUI window"), standard_Skin.window);
        }
    }

    protected virtual void CreateWindow(int windowID)
    {        
        //<!-- Exit Button.
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), building_Skin.customStyles[0])) {
            _clicked = false;
        }

        if (GUI.Button(upgradeButton_Rect, new GUIContent("Upgrade"), standard_Skin.button)) { 
        
        }

        if (_CanDestruction) {
            if (GUI.Button(destructionButton_Rect, new GUIContent("Destruction"), standard_Skin.button)) {
                //GUI.Box()
            }
        }
    }
	
	protected virtual IEnumerator BuildingTimer() {
		yield return 0;
	}
}
