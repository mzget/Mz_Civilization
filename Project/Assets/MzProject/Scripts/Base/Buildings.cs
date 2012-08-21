using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingsTimeData {
	
	public float[] arrBuildingTimesData = new float[Buildings.MAX_LEVEL];
	
	public BuildingsTimeData(Buildings.BuildingType r_buildingType) 
	{
		if(r_buildingType == Buildings.BuildingType.general) {
            float[] time_generalType = {
				30f, 90f, 300f, 600f, 1200f
			};
			
			arrBuildingTimesData = time_generalType;
		}
        else if (r_buildingType == Buildings.BuildingType.storehouse) {
            return;
        }
        else if (r_buildingType == Buildings.BuildingType.resource) {
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
    public GUISkin taskbar_Skin;
    //<!-- Styles.
    protected GUIStyle closeButton_Style;
    //<!-- Building Icon.
    public Texture2D buildingIcon_Texture;
    protected Texture2D food_icon;
    protected Texture2D wood_icon;
    protected Texture2D stone_icon;
    protected Texture2D copper_icon;

	protected GameObject processbar_Obj_parent;
    protected OTSprite processBar_Scolling;
    [System.NonSerialized]    public OTSprite sprite;
		
    protected int level = 0;
	protected int indexOfPosition;
	public int IndexOfPosition  {get {return indexOfPosition; } set {indexOfPosition = value; }}
    protected bool _isShowInterface = false;
    protected bool _CanDestruction = true;
	
	public enum BuildingStatus { none = 0, onBuildingProcess = 1, buildingComplete, onUpgradeProcess, };
	public BuildingStatus currentBuildingStatus;
	public enum BuildingType { general = 0, resource, storehouse, };
	protected BuildingType buildingType;

    protected BuildingsTimeData buildingTimeData;

    /// <summary>
    /// Static Data.
    /// </summary>
    protected static bool _CanUpgradeLevel = false;
    public static bool _CanCreateBuilding = false;
    public static List<Buildings> onBuilding_Obj = new List<Buildings>();
	//<!-- Resource.
    public static List<Farm> Farm_Instance = new List<Farm>();
	public static List<Sawmill> Sawmill_Instance = new List<Sawmill>();
	public static List<MillStone> MillStoneInstance = new List<MillStone>();
	public static List<Smelter> SmelterInstance = new List<Smelter>();
	//<!-- Economy.
    public static List<StoreHouse> StoreHouseInstance = new List<StoreHouse>();
	


    protected virtual void Awake()
    {
        closeButton_Style = building_Skin.customStyles[0];
        food_icon = taskbar_Skin.customStyles[0].normal.background;
        wood_icon = taskbar_Skin.customStyles[1].normal.background;
        copper_icon = taskbar_Skin.customStyles[2].normal.background;
        stone_icon = taskbar_Skin.customStyles[3].normal.background;
    }

    public static bool CheckingCanCreateBuilding()
    {
        if (onBuilding_Obj.Count < 2)
            return true;
        else
            return false;
    }
    public bool CheckingCanUpgradeLevel()
    {
        if (this.currentBuildingStatus == BuildingStatus.none)
        {
            if (onBuilding_Obj.Count < 2)
                return true;
            else
                return false;
        }
        else return false;
    }

    protected virtual void DestructionBuilding() { 
    
    }
	
	#region Building, Upgrade Process Section.
	
	protected virtual void OnUpgradeProcess(Buildings L_building) {
        Debug.Log("Class :: Building" + L_building.name + ": OnBuildingProcess()");

        if (onBuilding_Obj.Count < 2)
        {
            L_building.CreateProcessBar(this.currentBuildingStatus);
            onBuilding_Obj.Add(L_building);
        }
	}

	public virtual void OnBuildingProcess(Buildings L_buildind) {
        Debug.Log("Class :: Building" + L_buildind.name + ": OnBuildingProcess()");

        if (onBuilding_Obj.Count < 2)
        {
            L_buildind.CreateProcessBar(this.currentBuildingStatus);
            onBuilding_Obj.Add(L_buildind);
        }
	}
    protected virtual void CreateProcessBar(BuildingStatus buildingState)
    {
        if (processbar_Obj_parent == null)
        {
            processbar_Obj_parent = Instantiate(Resources.Load("Processbar_Group", typeof(GameObject)),
                new Vector3(this.sprite.position.x, this.sprite.position.y - ((this.sprite.size.y/2)+12), 0),
                Quaternion.identity) as GameObject;

            OTSprite backgroundSprite = processbar_Obj_parent.GetComponentInChildren<OTSprite>();
            backgroundSprite.size = new Vector2(128, 24);

            if (processBar_Scolling == null)
            {
                var scrolling = Instantiate(Resources.Load("processbar_scroll", typeof(GameObject))) as GameObject;
                scrolling.transform.parent = processbar_Obj_parent.transform;

                processBar_Scolling = scrolling.GetComponent<OTSprite>();
                processBar_Scolling.pivot = OTObject.Pivot.Left;
                processBar_Scolling.position = new Vector2((-backgroundSprite.size.x / 2) + 2, 0);
                processBar_Scolling.size = new Vector2(12, 24);
            }
        }
        
        Hashtable scaleData = new Hashtable();

        if (buildingState == BuildingStatus.onBuildingProcess)
        {
            scaleData.Add("from", new Vector2(12, 24));
            scaleData.Add("to", new Vector2(124, 24));
            scaleData.Add("time", buildingTimeData.arrBuildingTimesData[level]);
            scaleData.Add("onupdate", "BuildingProcess");
            scaleData.Add("easetype", iTween.EaseType.linear);
            scaleData.Add("oncomplete", "DestroyBuildingProcess");
            scaleData.Add("oncompleteparams", this);
            scaleData.Add("oncompletetarget", this.gameObject);
        }
        else if (buildingState == BuildingStatus.onUpgradeProcess) {
            scaleData.Add("from", new Vector2(12, 24));
            scaleData.Add("to", new Vector2(124, 24));
            scaleData.Add("time", buildingTimeData.arrBuildingTimesData[level]);
            scaleData.Add("onupdate", "BuildingProcess");
            scaleData.Add("easetype", iTween.EaseType.linear);
            scaleData.Add("oncomplete", "DestroyBuildingProcess");
            scaleData.Add("oncompleteparams", this);
            scaleData.Add("oncompletetarget", this.gameObject);
        }

        iTween.ValueTo(this.gameObject, scaleData);
    }
	private void BuildingProcess(Vector2 Rvalue) {
		Debug.Log("Class :: Buildings" + ":: BuildingProcess");
		
		if(this.processBar_Scolling)
			this.processBar_Scolling.size = Rvalue;
	}
	
	protected virtual void DestroyBuildingProcess(Buildings obj) {
        Debug.Log("Class :: Buildings" + obj.name + ": DestroyBuildingProcess");

        this.level += 1;
        onBuilding_Obj.Remove(obj);
	}
	
	#endregion
	
	#region Incloud Mouse Event.
	
    protected void OnMouseOver()
    {
		
    }
    protected void OnMouseDown()
    {
        _isShowInterface = true;
    }
    protected void OnMouseExit()
    {

    }

    #endregion

    protected virtual IEnumerator BuildingTimer()
    {
        yield return 0;
    }
	
	
    protected Vector2 scrollPosition = Vector2.zero;
    protected Rect windowRect;
    protected Rect exitButton_Rect;
    protected Rect background_Rect;
    protected Rect destructionButton_Rect;
    protected Rect imgContent_Rect = new Rect(40, 60, 100, 100);
    protected Rect buildingIcon_Rect = new Rect(40, 48, 80, 80);
    protected Rect levelLable_Rect = new Rect(25, 132, 120, 32);
    protected Rect description_Rect;
    protected Rect contentRect = new Rect(170, 20, 590, 160);
    protected Rect warnningMessage_Rect = new Rect(Main.GAMEWIDTH / 2 - 150, Main.GAMEHEIGHT / 2 - 120, 300, 240);

    protected Rect upgradeButton_Rect;
    protected Rect update_requireResource_Rect = new Rect(10, 240, 400, 32);
    protected Rect currentProduction_Rect;
    protected Rect nextProduction_Rect;
	
    protected void OnGUI()
    {
        windowRect = new Rect(Main.GAMEWIDTH / 2 - 350, Main.GAMEHEIGHT / 2 - 200, 700, 400);
        exitButton_Rect = new Rect(windowRect.width - 34, 2, 32, 32);
        upgradeButton_Rect = new Rect(description_Rect.width - 100, update_requireResource_Rect.y, 100, 32);
        currentProduction_Rect = new Rect(10, update_requireResource_Rect.y - 80, background_Rect.width, 32);
        nextProduction_Rect = new Rect(10, update_requireResource_Rect.y - 40, background_Rect.width, 32);
        destructionButton_Rect = new Rect(windowRect.width - 250, 32, 100, 32);
        background_Rect = new Rect(0, 0, windowRect.width - 12, 320);
        description_Rect = new Rect(150, 24, 520, background_Rect.height - 45);

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Main.FixedWidthRatio, Main.FixedHeightRatio, 1));

        standard_Skin.window.font = showG; 

        building_Skin.box.wordWrap = true;
        building_Skin.box.fontSize = 16;
        building_Skin.box.fontStyle = FontStyle.Normal;
        building_Skin.box.contentOffset = new Vector2(128, 38);

        GUIStyle buildingWindowStyle = GUI.skin.window;
        buildingWindowStyle.font = building_Skin.window.font;
        buildingWindowStyle.fontSize = building_Skin.window.fontSize;

        if (_isShowInterface) {
            windowRect = GUI.Window(0, windowRect, CreateWindow, new GUIContent(name, "GUI window"), buildingWindowStyle);
        }
    }

    protected virtual void CreateWindow(int windowID)
    {
        //<!-- Exit Button.
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), closeButton_Style))
        {
            _isShowInterface = false;
        }

        if (_CanDestruction)
        {
            if (GUI.Button(destructionButton_Rect, new GUIContent("Destruction"), GUI.skin.button))
            {
                //GUI.Box()
            }
        }
    }
}
