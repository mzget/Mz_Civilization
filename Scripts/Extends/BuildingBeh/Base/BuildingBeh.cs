using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class BuildingBeh : Base_ObjectBeh {
	
    public const string BuildingIcons_TextureResourcePath = "Textures/Building_Icons/";
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
	protected GUIStyle buildingWindowStyle;
    //<!-- Building Icon.
    protected Texture2D buildingIcon_Texture;
	
    public StageManager stageManager;
	protected GameObject processbar_Obj_parent;
    private OTSprite processBarBackground;
    private OTSprite processBar_Scolling;

    protected OTSprite sprite;
    protected TextMesh buildingLevel_textmesh;

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
    public static List<BuildingBeh> onBuilding_Obj = new List<BuildingBeh>();
    public static BuildingBeh OnDestruction_Obj = null;
    public static TownCenter TownCenter;
    //<!--- Utility.
    public static List<HouseBeh> House_Instances = new List<HouseBeh>();
	public static AcademyBeh AcademyInstance;
	//<!-- Resource.
    public static List<Farm> Farm_Instance = new List<Farm>();
	public static List<Sawmill> Sawmill_Instance = new List<Sawmill>();
	public static List<MillStone> MillStoneInstance = new List<MillStone>();
	public static List<Smelter> SmelterInstance = new List<Smelter>();
	//<!-- Economy.
    public static List<StoreHouse> StoreHouseInstance = new List<StoreHouse>();
	public static MarketBeh MarketInstance;
    //<!-- Military.
    public static List<BarracksBeh> Barrack_Instances = new List<BarracksBeh>();
	
    protected Vector2 scrollPosition = Vector2.zero;
    protected Rect windowRect;
    private Rect exitButton_Rect;
    protected Rect background_Rect;
    protected Rect tagName_Rect = new Rect(20, 16, 120, 32);    //<!-- Tag name rect.
    protected Rect imgIcon_Rect = new Rect(40, 40, 80, 80);     //<!-- Images Icon rect.
    protected Rect levelLable_Rect = new Rect(25, 132, 120, 32);
	protected Rect notificationBox_rect;
    protected Rect building_background_Rect;
    protected Rect descriptionGroup_Rect;
    protected Rect contentRect = new Rect(170, 20, 590, 160);
    protected Rect warnningMessage_Rect = new Rect(Main.FixedGameWidth / 2 - 150, Main.FixedGameHeight / 2 - 120, 300, 240);
    protected Rect update_requireResource_Rect;
    protected Rect currentJob_Rect;
    protected Rect nextJob_Rect;
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
	
	#region <!-- Checking Resource pre construction Event.
	
	public class NoEnoughResourceNotificationArg : EventArgs {
		public string notification_args { get; set; }
	}
	
	protected string notificationText = ""; 
	protected event EventHandler<NoEnoughResourceNotificationArg> NotEnoughResource_Notification_event;
	private void OnCheckingResource(NoEnoughResourceNotificationArg e) {
		if(NotEnoughResource_Notification_event != null)
			NotEnoughResource_Notification_event(this, e);
	}
	public bool CheckingEnoughUpgradeResource(GameResource upgradeResource)
	{
		if (StoreHouse.SumOfFood >= upgradeResource.Food && 
			StoreHouse.SumOfWood >= upgradeResource.Wood &&
			StoreHouse.SumOfStone >= upgradeResource.Stone &&
			StoreHouse.SumOfCopper >= upgradeResource.Copper &&
		    StoreHouse.sumOfGold >= upgradeResource.Gold &&
			HouseBeh.SumOfUnemployed >= upgradeResource.Employee) {
			return true;
		}
		else if(StoreHouse.SumOfFood < upgradeResource.Food) {
			OnCheckingResource(new NoEnoughResourceNotificationArg() {notification_args = "Not enough food."});
			return false;
		}
		else if(StoreHouse.SumOfWood < upgradeResource.Wood) {
			OnCheckingResource(new NoEnoughResourceNotificationArg() {notification_args = "Not enough wood."});
			return false;
		}
		else if(StoreHouse.SumOfStone < upgradeResource.Stone) {
			OnCheckingResource(new NoEnoughResourceNotificationArg() {notification_args = "Not enough stone."});
			return false;
		}
		else if(StoreHouse.SumOfCopper < upgradeResource.Copper) {
			OnCheckingResource(new NoEnoughResourceNotificationArg() {notification_args = "Not enough copper."});
			return false;
		}
		else if(StoreHouse.sumOfGold < upgradeResource.Gold) {
			OnCheckingResource(new NoEnoughResourceNotificationArg() {notification_args = "Not enough gold"});
			return false;
		}
		else if(HouseBeh.SumOfUnemployed < upgradeResource.Employee) {
			OnCheckingResource(new NoEnoughResourceNotificationArg() {notification_args = "Not enough employee."});
			return false;
		}
		else {
			return false;
		}
	}
	
	#endregion

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

        job_style = new GUIStyle(standard_Skin.box);
        job_style.font = ubuntu_font;
        job_style.alignment = TextAnchor.MiddleLeft;

        status_style = standard_Skin.box;
        status_style.font = ubuntu_font;
        status_style.alignment = TextAnchor.MiddleCenter;
		
		building_Skin.box.wordWrap = true;
		building_Skin.box.fontSize = 16;
		building_Skin.box.fontStyle = FontStyle.Normal;
		building_Skin.box.contentOffset = new Vector2(128, 38);
		
		buildingWindowStyle = new GUIStyle(standard_Skin.window);
		buildingWindowStyle.font = building_Skin.window.font;
		buildingWindowStyle.fontSize = building_Skin.window.fontSize;

        windowRect = new Rect((Main.FixedGameWidth * 3 / 4) / 2 - 350, Main.FixedGameHeight / 2 - 250, 700, 500);
        background_Rect = new Rect(0, 0, windowRect.width - 16, 420);
        building_background_Rect = new Rect(background_Rect.x, background_Rect.y, windowRect.width, background_Rect.height);
        descriptionGroup_Rect = new Rect(150, 24, windowRect.width - 160, background_Rect.height - 45);
        exitButton_Rect = new Rect(windowRect.width - 34, 2, 32, 32);
		notificationBox_rect = new Rect(50, 32, windowRect.width - 100, 32);
        update_requireResource_Rect = new Rect(10, 320, 500, 40);
//        upgradeButton_Rect = new Rect(descriptionGroup_Rect.width - 120, update_requireResource_Rect.y, 100, 32);
        currentJob_Rect = new Rect(10, update_requireResource_Rect.y - 80, descriptionGroup_Rect.width - 20, 32);
        nextJob_Rect = new Rect(10, update_requireResource_Rect.y - 40, descriptionGroup_Rect.width - 20, 32);
//        destructionButton_Rect = new Rect(windowRect.width - 110, 40, 100, 32);

        GameObject temp = Instantiate(Resources.Load("Buildings/Level_textmesh", typeof(GameObject))) as GameObject;
        buildingLevel_textmesh = temp.GetComponent<TextMesh>();
        buildingLevel_textmesh.gameObject.name = "Level_textmesh";
        buildingLevel_textmesh.transform.parent = this.transform;
        buildingLevel_textmesh.transform.localPosition = new Vector3(0, .6f, 0);
        buildingLevel_textmesh.transform.localScale = new Vector3(.1f, .1f, 1);
    }
	
	protected virtual void InitializeTexturesResource() {
        Debug.Log(this.name + " : LoadTextures_Resource");
	}
    protected virtual void InitializingData() {
        Debug.Log(this.name + " : InitializingData");
    }
    public virtual void InitializingBuildingBeh(BuildingStatus p_buildingState, int p_indexPosition, int p_level) {
        Debug.Log(this.name + " : InitializingBuildingBeh");

        currentBuildingStatus = p_buildingState;
        indexOfPosition = p_indexPosition;
        
        Level = p_level;
        buildingLevel_textmesh.text = this.level.ToString();

        this.sprite.position = StageManager.buildingArea_Pos[indexOfPosition];
        StageManager.buildingArea_Objs[indexOfPosition].gameObject.active = false;
    }
	protected virtual void CalculateNumberOfEmployed(int p_level) {	}

    protected virtual void OnUpgradeProcess(BuildingBeh p_building) 
    {
        Debug.Log(p_building.name + ": OnUpgradeProcess()");

        if (onBuilding_Obj.Count < 2)
        {
            p_building.CreateProcessBar(this.currentBuildingStatus);
            onBuilding_Obj.Add(p_building);
        }
	}
	public virtual void OnBuildingProcess(BuildingBeh p_buildind) 
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
            processbar_Obj_parent = new GameObject("ProcessbarObj_group");
            processbar_Obj_parent.transform.position = new Vector3(this.sprite.position.x, this.sprite.position.y - ((this.sprite.size.y / 2) + 24), 0);

            GameObject temp_processBar = Instantiate(Resources.Load(TaskManager.PathOfGUISprite + "Processbar", typeof(GameObject))) as GameObject;
            temp_processBar.transform.parent = processbar_Obj_parent.transform;
            temp_processBar.transform.localPosition = Vector3.zero;
            processBarBackground = temp_processBar.GetComponent<OTSprite>();
            processBarBackground.spriteContainer = OT.ContainerByName("GUI_Atlas");
            processBarBackground.frameIndex = 0;
            processBarBackground.size = new Vector2(128, 28);

            if (processBar_Scolling == null)
            {
                if (buildingStatus == BuildingStatus.onBuildingProcess || buildingStatus == BuildingStatus.onUpgradeProcess)
                {
                    GameObject scrolling = Instantiate(Resources.Load(TaskManager.PathOfGUISprite + "processbar_scroll", typeof(GameObject))) as GameObject;
                    scrolling.transform.parent = processbar_Obj_parent.transform;
                    processBar_Scolling = scrolling.GetComponent<OTSprite>();
					processBar_Scolling.spriteContainer = OT.ContainerByName("GUI_Atlas");
					processBar_Scolling.frameIndex = 1;
                    processBar_Scolling.pivot = OTObject.Pivot.Left;
                    processBar_Scolling.position = new Vector2((-processBarBackground.size.x / 2) + 2, -1);
                    processBar_Scolling.size = new Vector2(12, 24);
                }
                else if (buildingStatus == BuildingStatus.OnDestructionProcess)
                {
                    GameObject scrolling = Instantiate(Resources.Load(TaskManager.PathOfGUISprite + "Destruction_processbar", typeof(GameObject))) as GameObject;
                    scrolling.transform.parent = processbar_Obj_parent.transform;
                    processBar_Scolling = scrolling.GetComponent<OTSprite>();
					processBar_Scolling.spriteContainer = OT.ContainerByName("GUI_Atlas");
					processBar_Scolling.frameIndex = 2;
                    processBar_Scolling.pivot = OTObject.Pivot.Left;
                    processBar_Scolling.position = new Vector2((-processBarBackground.size.x / 2) + 2, -1);
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
	protected virtual void BuildingProcessComplete(BuildingBeh obj) {
        Debug.Log(obj.name + ": BuildingProcessComplete");

        this.Level += 1;
        buildingLevel_textmesh.text = this.Level.ToString();
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
    private void DestructionBuildingComplete() {
        Debug.Log("DestructionBuildingComplete");

        if (Level > 1)
            this.Level -= 1;
        else if (Level <= 1)
			ClearStorageData();
		
        OnDestruction_Obj = null;
    }
	protected virtual void ClearStorageData() {		
		Debug.Log("ClearStorageData");
		
		StageManager.buildingArea_Objs[this.indexOfPosition].gameObject.SetActiveRecursively(true);
        Destroy(this.gameObject);
        Destroy(this.processbar_Obj_parent.gameObject);
	}
	
	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
    protected override void OnMouseDown()
    {
		if(TaskManager.IsShowInteruptGUI == false) {
            _IsShowInterface = true;
            TaskManager.IsShowInteruptGUI = true;
        }

        base.OnMouseDown();
    }

	/// <summary>
    /// <!-- OnGUI Section.
	/// </summary>
    protected void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.FixedGameWidth, Screen.height / Main.FixedGameHeight, 1));
		
        if (_IsShowInterface) {
            windowRect = GUI.Window(0, windowRect, CreateWindow, new GUIContent(this.name, "GUI window"), buildingWindowStyle);
        }
    }

    protected virtual void CreateWindow(int windowID)
    {
        //<!-- Exit Button.
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), closeButton_Style))
        {
            CloseGUIWindow();
        }
    }
	
    protected void CloseGUIWindow() {
        _IsShowInterface = false;
        TaskManager.IsShowInteruptGUI = false;
    }
	
	/// <summary>
	/// Clears the static data.
	/// </summary>
	internal static void ClearStaticData() {
		House_Instances.Clear();
		AcademyInstance = null;

		Farm_Instance.Clear();
		Sawmill_Instance.Clear();
		MillStoneInstance.Clear();
		SmelterInstance.Clear();

		StoreHouseInstance.Clear();
		MarketInstance = null;

		Barrack_Instances.Clear();
	}
}
