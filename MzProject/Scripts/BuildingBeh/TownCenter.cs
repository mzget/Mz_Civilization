using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownCenter : BuildingBeh {

    public static string BuildingName = "Town center";
    private static string Description_TH = "";
    private static string Description_EN = "Town center is the only building in your village at the start. It is always located just above the village centre, and can't destruction it.";
    public static string CurrentDescription
    {
        get
        {
            string temp = Description_EN;
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                temp = Description_TH;
            return temp;
        }
    }
    //<!--- Requirements Resource.
    public static GameResource[] RequireResource = new GameResource[10] {
		new GameResource() {Food = 80, Wood = 120, Gold = 60, Employee = 0},
        new GameResource() {Food = 160, Wood = 240, Gold = 120, Employee = 0},
        new GameResource() {Food = 320, Wood = 480, Gold = 240, Employee = 0},
        new GameResource() {Food = 640, Wood = 960, Gold = 480, Employee = 0},
        new GameResource() {Food = 1280, Wood = 1920, Gold = 960, Employee = 0},
        new GameResource() {Food = 2560, Wood = 3840, Gold = 1920, Employee = 0},
        new GameResource() {Food = 5120, Wood = 7680, Gold = 3840, Employee = 0},
        new GameResource() {Food = 10240, Wood = 15360, Gold = 7680, Employee = 0},
        new GameResource() {Food = 20480, Wood = 30720, Gold = 15360, Employee = 0},
        new GameResource() {Food = 40960, Wood = 61440, Gold = 30720, Employee = 0},
	};

    // Technology Point.
    private int[] increaseBuildingSpeed_arr = new int[10] { 800, 1200, 1800, 2500, 3500, 4800, 6000, 8000, 12000, 15000, };
    private int currentIncreaseBuildingSpeed;


    protected override void Awake()
    {
        base.Awake();
		
        BuildingBeh.TownCenter = this;

        base.sprite = this.gameObject.GetComponent<OTSprite>();
		
        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
    }

	// Use this for initialization
	void Start () {
        InitializeTexturesResource();
        base.buildingLevel_textmesh.text = Level.ToString();
		
		base.NotEnoughResource_Notification_event += HandleBaseNotEnoughResource_Notification_event;;
	}
	
	#region <!-- Event Handle.
	
	void HandleBaseNotEnoughResource_Notification_event (object sender, BuildingBeh.NoEnoughResourceNotificationArg e)
	{
		notificationText = e.notification_args;
	}
	
	#endregion

    protected override void InitializeTexturesResource()
    {
        base.InitializeTexturesResource();

        base.buildingIcon_Texture = Resources.Load(BuildingIcons_TextureResourcePath + "TownCenter", typeof(Texture2D)) as Texture2D;
    }
	
	#region <!--- Building && Upgrade processing.
	
    public override void OnBuildingProcess(BuildingBeh p_buildind)
    {
        base.OnBuildingProcess(p_buildind);
    }
    protected override void OnUpgradeProcess(BuildingBeh p_building)
    {
        base.OnUpgradeProcess(p_building);
    }
	protected override void BuildingProcessComplete (BuildingBeh obj)
	{
		base.BuildingProcessComplete (obj);

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != BuildingBeh.BuildingStatus.none) {
            this.currentBuildingStatus = BuildingBeh.BuildingStatus.none;
        }
	}
	
	#endregion
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        if (base.notificationText == "")
        {
            base.notificationText = base.currentBuildingStatus.ToString();
        }
        GUI.Box(base.notificationBox_rect, base.notificationText, standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.windowRect.width, base.background_Rect.height));
        {
            GUI.BeginGroup(base.building_background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

                #region <!--- Contents Group.

                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {
                    //<!-- Current Production rate.
                    if (base.Level < 10)
                    {
                        GUI.Label(currentJob_Rect, "Current Max Capacity : " + this.currentIncreaseBuildingSpeed, base.job_style);
                        GUI.Label(nextJob_Rect, "Next level: " + this.increaseBuildingSpeed_arr[base.Level], base.job_style);

                        //<!-- Requirements Resource.
                        GUI.BeginGroup(update_requireResource_Rect);
                        {
                            GUI.Label(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(),
                                base.stageManager.taskManager.food_icon), standard_Skin.box);
                            GUI.Label(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(),
                                base.stageManager.taskManager.wood_icon), standard_Skin.box);
                            //GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                            //    base.stageManager.taskbarManager.stone_icon), standard_Skin.box);
                            GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(),
                                base.stageManager.taskManager.gold_icon), standard_Skin.box);
                            GUI.Label(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(),
                                base.stageManager.taskManager.employee_icon), standard_Skin.box);
                        }
                        GUI.EndGroup();
                    }
                    else
                    {
                        GUI.Label(currentJob_Rect, "Current Max Capacity : " + this.currentIncreaseBuildingSpeed, base.job_style);
                        GUI.Label(nextJob_Rect, "Max upgrade building.", base.job_style);
                    }
                }
                GUI.EndGroup();

                #endregion

                #region <!--- Upgrade Button mechanichm.

                bool enableUpgrade = false;
                if (base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level]))
                    enableUpgrade = true;

                GUI.enabled = enableUpgrade;
                if (GUI.Button(base.upgrade_Button_Rect, new GUIContent("Upgrade")))
                {
                    GameResource.UsedResource(RequireResource[Level]);

                    base.currentBuildingStatus = BuildingBeh.BuildingStatus.onUpgradeProcess;
                    base.OnUpgradeProcess(this);
                    base.CloseGUIWindow();
                }
                GUI.enabled = true;

                #endregion

                #region <!--- Destruction button.

                GUI.enabled = false;
                if (GUI.Button(destruction_Button_Rect, new GUIContent("Destruct")))
                {
//                    this.currentBuildingStatus = BuildingStatus.OnDestructionProcess;
//                    this.DestructionBuilding();
//                    base.CloseGUIWindow();
                }
                GUI.enabled = true;

                #endregion
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
