using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingsTimeData {
	
	public float[] arrBuildingTimesData = new float[Buildings.MAX_LEVEL];

    public BuildingsTimeData(Buildings.BuildingType r_buildingType)
    {
        if (r_buildingType == Buildings.BuildingType.general)
        {
            float[] time_generalType = { 30f, 50f, 90f, 120f, 180f, 220f, 250f, 300f, 400f, 500f, };

            arrBuildingTimesData = time_generalType;
        }
        else if (r_buildingType == Buildings.BuildingType.resource)
        {
            float[] time_resourceType = { 30f, 50f, 90f, 120f, 180f, 220f, 250f, 300f, 400f, 500f, };

            arrBuildingTimesData = time_resourceType;
        }
        else if (r_buildingType == Buildings.BuildingType.storehouse)
        {
            return;
        }
        else if (r_buildingType == Buildings.BuildingType.barrack) {
            float[] time_resourceType = { 30f, 50f, 90f, 120f, 180f, 220f, 250f, 300f, 400f, 500f, };

            arrBuildingTimesData = time_resourceType;
        }
    }
};

public class Buildings : MonoBehaviour {
	
    public const string PathOf_BuildingIcons = "Textures/Building_Icons/";
	public const int MAX_LEVEL = 10;

	//<!-- Font, Skin, Styles.
    public Font showG_font;
    protected Font ubuntu_font;
    public GUISkin standard_Skin;
    public GUISkin building_Skin;
    public GUISkin taskbar_Skin;
    protected GUIStyle job_style;
    protected GUIStyle status_style;
    protected GUIStyle closeButton_Style;
    //<!-- Building Icon.
    protected Texture2D buildingIcon_Texture;
    protected Texture2D food_icon;
    protected Texture2D wood_icon;
    protected Texture2D stone_icon;
    protected Texture2D gold_icon;

	protected GameObject processbar_Obj_parent;
    protected OTSprite processBar_Scolling;
    public OTSprite sprite;
    public StageManager stageManager;

    private int level = 0;
    public int Level { get { return level; } set { level = value; } }
	protected int indexOfPosition;
	public int IndexOfPosition  {get {return indexOfPosition; } set {indexOfPosition = value; }}
    private bool _IsShowInterface = false;
	public enum BuildingStatus { 
        none = 0, 
        onBuildingProcess = 1, 
        buildingComplete, 
        onUpgradeProcess, 
        OnDestructionProcess, 
        OnDestructionComplete, 
    };
	public BuildingStatus currentBuildingStatus;
	public enum BuildingType { general = 0, resource, storehouse, barrack, };
	protected BuildingType buildingType;
    protected BuildingsTimeData buildingTimeData;

    //<!-- Static Data.
//    protected static bool _CanUpgradeLevel = false;
//	  public static bool _CanCreateBuilding = false;
//    protected bool _CanDestruction = true;
    public static List<Buildings> onBuilding_Obj = new List<Buildings>();
    public static Buildings OnDestruction_Obj = null;
    //<!--- Utility.
    public static List<HouseBeh> House_Instances = new List<HouseBeh>();
	//<!-- Resource.
    public static List<Farm> Farm_Instance = new List<Farm>();
	public static List<Sawmill> Sawmill_Instance = new List<Sawmill>();
	public static List<MillStone> MillStoneInstance = new List<MillStone>();
	public static List<Smelter> SmelterInstance = new List<Smelter>();
	//<!-- Economy.
    public static List<StoreHouse> StoreHouseInstance = new List<StoreHouse>();
    public static List<MarketBeh> MarketInstances = new List<MarketBeh>();
    //<!-- Military.
    public static List<BarracksBeh> Barrack_Instances = new List<BarracksBeh>();
	
    protected Vector2 scrollPosition = Vector2.zero;
    protected Rect windowRect;
    protected Rect exitButton_Rect;
    protected Rect background_Rect;
    protected Rect tagName_Rect = new Rect(20, 16, 120, 32);    //<!-- Tag name rect.
    protected Rect imgIcon_Rect = new Rect(40, 40, 80, 80);     //<!-- Images Icon rect.
    protected Rect levelLable_Rect = new Rect(25, 132, 120, 32);
    protected Rect building_background_Rect;
    protected Rect descriptionGroup_Rect;
    protected Rect contentRect = new Rect(170, 20, 590, 160);
    protected Rect warnningMessage_Rect = new Rect(Main.GAMEWIDTH / 2 - 150, Main.GAMEHEIGHT / 2 - 120, 300, 240);
    protected Rect update_requireResource_Rect;
    protected Rect currentProduction_Rect;
    protected Rect nextProduction_Rect;
    protected Rect upgrade_Button_Rect = new Rect(25, 174, 120, 32);
    protected Rect destruction_Button_Rect = new Rect(25, 260, 120, 32);
	



    public static bool CheckingCanCreateBuilding() {
        if (onBuilding_Obj.Count < 2)
            return true;
        else
            return false;
    }
    public bool CheckingCanUpgradeLevel()
	{
        if (this.currentBuildingStatus == BuildingStatus.none) {
            if (onBuilding_Obj.Count < 2)
                return true;
            else
                return false;
        }
        else 
			return false;
    }
	public static bool CheckingEnoughUpgradeResource(GameResource upgradeResource) {
		if (StoreHouse.sumOfFood >= upgradeResource.Food && StoreHouse.sumOfWood >= upgradeResource.Wood &&
			StoreHouse.sumOfGold >= upgradeResource.Gold && StoreHouse.sumOfStone >= upgradeResource.Stone) {
			return true;
		}
		else
			return false;
	}

    protected virtual void Awake()
    {
        if (stageManager == null)
            stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();

        if (standard_Skin == null)
            standard_Skin = Resources.Load("GUISkins/Standard_Skin", typeof(GUISkin)) as GUISkin;
        if (taskbar_Skin == null)
            taskbar_Skin = Resources.Load("GUISkins/TaskbarUI_Skin", typeof(GUISkin)) as GUISkin;
        if (building_Skin == null)
            building_Skin = Resources.Load("GUISkins/Building_Skin", typeof(GUISkin)) as GUISkin;
        if (showG_font == null)
            showG_font = Resources.Load("Fonts/SHOWG", typeof(Font)) as Font;
        if (ubuntu_font == null)
            ubuntu_font = Resources.Load("Fonts/Ubuntu-R", typeof(Font)) as Font;

        closeButton_Style = taskbar_Skin.customStyles[6];
        food_icon = taskbar_Skin.customStyles[0].normal.background;
        wood_icon = taskbar_Skin.customStyles[1].normal.background;
        stone_icon = taskbar_Skin.customStyles[2].normal.background;
        gold_icon = taskbar_Skin.customStyles[3].normal.background;

        job_style = new GUIStyle(standard_Skin.box);
        job_style.font = ubuntu_font;
        job_style.alignment = TextAnchor.MiddleLeft;

        status_style = standard_Skin.box;
        status_style.font = ubuntu_font;
        status_style.alignment = TextAnchor.MiddleCenter;

        windowRect = new Rect(Main.GAMEWIDTH / 2 - 350, Main.GAMEHEIGHT / 2 - 180, 700, 400);
        background_Rect = new Rect(0, 0, windowRect.width - 16, 320);
        building_background_Rect = new Rect(background_Rect.x, background_Rect.y, windowRect.width, background_Rect.height);
        descriptionGroup_Rect = new Rect(150, 24, windowRect.width - 155, background_Rect.height - 45);
        exitButton_Rect = new Rect(windowRect.width - 34, 2, 32, 32);
        update_requireResource_Rect = new Rect(10, 240, 500, 32);
//        upgradeButton_Rect = new Rect(descriptionGroup_Rect.width - 120, update_requireResource_Rect.y, 100, 32);
        currentProduction_Rect = new Rect(10, update_requireResource_Rect.y - 80, descriptionGroup_Rect.width - 20, 32);
        nextProduction_Rect = new Rect(10, update_requireResource_Rect.y - 40, descriptionGroup_Rect.width - 20, 32);
//        destructionButton_Rect = new Rect(windowRect.width - 110, 40, 100, 32);
    }
	
	protected virtual void LoadTextureResource() {
        Debug.Log("Building.LoadTextureResource");
	}

    public virtual void InitializeData(BuildingStatus p_buildingState, int p_indexPosition, int p_level) {
        Debug.Log("Building.InitializeData");

        currentBuildingStatus = p_buildingState;
        indexOfPosition = p_indexPosition;
        Level = p_level;

        this.sprite.position = StageManager.buildingArea_Pos[indexOfPosition];
        StageManager.buildingArea_Obj[indexOfPosition].gameObject.active = false;
    }

    protected virtual void OnUpgradeProcess(Buildings p_building) 
    {
        Debug.Log(p_building.name + ": OnBuildingProcess()");

        if (onBuilding_Obj.Count < 2)
        {
            p_building.CreateProcessBar(this.currentBuildingStatus);
            onBuilding_Obj.Add(p_building);
        }
	}

	public virtual void OnBuildingProcess(Buildings p_buildind) 
    {
        Debug.Log(p_buildind.name + ": OnBuildingProcess()");

        if (onBuilding_Obj.Count < 2)
        {
            p_buildind.CreateProcessBar(this.currentBuildingStatus);
            onBuilding_Obj.Add(p_buildind);
        }
	}

    protected virtual void CreateProcessBar(BuildingStatus buildingStatus)
    {
        if (processbar_Obj_parent == null)
        {
            processbar_Obj_parent = Instantiate(Resources.Load("Processbar_Group", typeof(GameObject)),
                new Vector3(this.sprite.position.x, this.sprite.position.y - ((this.sprite.size.y / 2) + 24), 0),
                Quaternion.identity) as GameObject;

            OTSprite backgroundSprite = processbar_Obj_parent.GetComponentInChildren<OTSprite>();
            backgroundSprite.size = new Vector2(128, 24);

            if (processBar_Scolling == null)
            {
                if (buildingStatus == BuildingStatus.onBuildingProcess || buildingStatus == BuildingStatus.onUpgradeProcess)
                {
                    GameObject scrolling = Instantiate(Resources.Load("UI_sprites/processbar_scroll", typeof(GameObject))) as GameObject;
                    scrolling.transform.parent = processbar_Obj_parent.transform;

                    processBar_Scolling = scrolling.GetComponent<OTSprite>();
                    processBar_Scolling.pivot = OTObject.Pivot.Left;
                    processBar_Scolling.position = new Vector2((-backgroundSprite.size.x / 2) + 2, 0);
                    processBar_Scolling.size = new Vector2(12, 24);
                }
                else if (buildingStatus == BuildingStatus.OnDestructionProcess)
                {
                    GameObject scrolling = Instantiate(Resources.Load("UI_sprites/Destruction_processbar", typeof(GameObject))) as GameObject;
                    scrolling.transform.parent = processbar_Obj_parent.transform;

                    processBar_Scolling = scrolling.GetComponent<OTSprite>();
                    processBar_Scolling.pivot = OTObject.Pivot.Left;
                    processBar_Scolling.position = new Vector2((-backgroundSprite.size.x / 2) + 2, 0);
                    processBar_Scolling.size = new Vector2(12, 24);
                }
            }

            Hashtable scaleData = new Hashtable();

            if (buildingStatus == BuildingStatus.onBuildingProcess)
            {
                scaleData.Add("from", new Vector2(12, 24));
                scaleData.Add("to", new Vector2(124, 24));
                scaleData.Add("time", buildingTimeData.arrBuildingTimesData[Level]);
                scaleData.Add("onupdate", "BuildingProcess");
                scaleData.Add("easetype", iTween.EaseType.linear);
                scaleData.Add("oncomplete", "BuildingProcessComplete");
                scaleData.Add("oncompleteparams", this);
                scaleData.Add("oncompletetarget", this.gameObject);
            }
            else if (buildingStatus == BuildingStatus.onUpgradeProcess)
            {
                scaleData.Add("from", new Vector2(12, 24));
                scaleData.Add("to", new Vector2(124, 24));
                scaleData.Add("time", buildingTimeData.arrBuildingTimesData[Level]);
                scaleData.Add("onupdate", "BuildingProcess");
                scaleData.Add("easetype", iTween.EaseType.linear);
                scaleData.Add("oncomplete", "BuildingProcessComplete");
                scaleData.Add("oncompleteparams", this);
                scaleData.Add("oncompletetarget", this.gameObject);
            }
            else if (buildingStatus == BuildingStatus.OnDestructionProcess)
            {
                scaleData.Add("from", new Vector2(12, 24));
                scaleData.Add("to", new Vector2(124, 24));
                scaleData.Add("time", buildingTimeData.arrBuildingTimesData[Level]);
                scaleData.Add("onupdate", "BuildingProcess");
                scaleData.Add("easetype", iTween.EaseType.linear);
                scaleData.Add("oncomplete", "DestructionBuildingComplete");
                scaleData.Add("oncompleteparams", this);
                scaleData.Add("oncompletetarget", this.gameObject);
            }

            iTween.ValueTo(this.gameObject, scaleData);
        }
        else
            return;
    }

	private void BuildingProcess(Vector2 Rvalue) {		
		if(this.processBar_Scolling)
			this.processBar_Scolling.size = Rvalue;
	}

	protected virtual void BuildingProcessComplete(Buildings obj) {
        Debug.Log(obj.name + ": BuildingProcessComplete");

        this.Level += 1;
        onBuilding_Obj.Remove(obj);
	}
	
	protected bool CheckingCanDestructionBuilding()
	{
		if(this.currentBuildingStatus == BuildingStatus.none) {
			if(OnDestruction_Obj == null)
				return true;
			else
				return false;
		}
		else 
			return false;
	}
    protected void DestructionBuilding()
    {
        Debug.Log("DestructionBuilding");

        if (OnDestruction_Obj == null)
        {
            OnDestruction_Obj = this;
            this.CreateProcessBar(currentBuildingStatus);
        }
        else
            return;
    }
    private void DestructionBuildingComplete()
    {
        Debug.Log("DestructionBuildingComplete");

        if (Level > 1)
            this.Level -= 1;
        else if (Level <= 1)
        {
			ClearStorageData();
        }
		
        OnDestruction_Obj = null;
    }
	protected virtual void ClearStorageData() {		
		Debug.Log("ClearStorageData");
		
		StageManager.buildingArea_Obj[this.indexOfPosition].gameObject.SetActiveRecursively(true);
        Destroy(this.gameObject);
        Destroy(this.processbar_Obj_parent.gameObject);
	}

	#region <!--- On Mouse Event.

    protected void OnMouseOver()
    {
		
    }
    protected void OnMouseDown()
    {
        if (TaskbarManager.IsShowInteruptGUI == false)
        {
            _IsShowInterface = true;
            TaskbarManager.IsShowInteruptGUI = true;
        }
    }
    protected void OnMouseExit()
    {

    }

    #endregion
	
    protected void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GAMEWIDTH, Screen.height / Main.GAMEHEIGHT, 1));

//        standard_Skin.window.font = showG; 

        building_Skin.box.wordWrap = true;
        building_Skin.box.fontSize = 16;
        building_Skin.box.fontStyle = FontStyle.Normal;
        building_Skin.box.contentOffset = new Vector2(128, 38);

        GUIStyle buildingWindowStyle = GUI.skin.window;
        buildingWindowStyle.font = building_Skin.window.font;
        buildingWindowStyle.fontSize = building_Skin.window.fontSize;

        if (_IsShowInterface) {
            windowRect = GUI.Window(0, windowRect, CreateWindow, new GUIContent(name, "GUI window"), buildingWindowStyle);
        }
    }

    protected virtual void CreateWindow(int windowID)
    {
        //<!-- Exit Button.
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), closeButton_Style))
        {
            CloseGUIWindow();
        }
        
		#region <!--- Destruction button.
		
//        GUI.enabled = this.CheckingCanDestructionBuilding();
//        if (GUI.Button(destruction_Button_Rect, new GUIContent("Destruct")))
//        {
//            this.currentBuildingStatus = BuildingStatus.OnDestructionProcess;
//            this.DestructionBuilding();
//            _IsShowInterface = false;
//        }
//		GUI.enabled = true;
		
		#endregion
    }
    protected void CloseGUIWindow() {
        _IsShowInterface = false;
        TaskbarManager.IsShowInteruptGUI = false;
    }
}
