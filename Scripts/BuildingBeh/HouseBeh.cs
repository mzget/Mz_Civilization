using UnityEngine;
using System.Collections;

public class HouseBeh : BuildingBeh {

	//<!--- The require resource.
    public static GameMaterialDatabase[] RequireResource = new GameMaterialDatabase[5] {
		new GameMaterialDatabase(80, 120, 40, 60, 0),
        new GameMaterialDatabase(160, 240, 80, 120, 0),
        new GameMaterialDatabase(320, 480, 160, 240, 0),
        new GameMaterialDatabase(640, 960, 320, 480, 0),
        new GameMaterialDatabase(1280, 1920, 640, 960, 0),
//        new GameResource(600, 600, 600, 600, 0),
//        new GameResource(700, 700, 700, 700, 0),
//        new GameResource(800, 800, 800, 800, 0),
//        new GameResource(900, 900, 900, 900, 0),
//        new GameResource(1000, 1000, 1000, 1000, 0),
	};

    public const string BuildingName = "House";
    private const string Description_TH = "��ҹ�ѡ���ҧ������������ӹǹ��Ъҡ��٧�ش�ͧ�س";
    private const string Description_EN = "The house can be built to increase your maximum population.";
    public static string CurrentDescription {
        get {
            string temp = "";
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                temp = Description_TH;
            return temp;
        }
    }
    public static int SumOfPopulation;
    public static int SumOfEmployee;
    public static int SumOfUnemployed;
    private static bool _IsAddEvent = false;
    public static void CalculationSumOfPopulation() {
        SumOfPopulation = 10;
        foreach (HouseBeh house in BuildingBeh.House_Instances)
            SumOfPopulation += house.currentMaxDweller;

        SumOfUnemployed = SumOfPopulation - SumOfEmployee;
    }
	public static new void ClearStaticData() {
		HouseBeh._IsAddEvent = false;
		HouseBeh.SumOfPopulation = 0;
		HouseBeh.SumOfEmployee = 0;
		HouseBeh.SumOfUnemployed = 0;
	}
	
    private int[] maxDweller = new int[5] { 10, 25, 50, 90, 150, };
    private int currentMaxDweller;

    //<!--- Awake --->
    protected override void Awake()
    {
        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);

        base.processbar_offsetPos = Vector3.up * 60;

        base.Awake();
    }

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		this.InitializeTexturesResource();
		this.CalculationCurrentDweller();
		
		base.NotEnoughResource_Notification_event += HandleBaseNotEnoughResourceNotification_event;
		
		if(_IsAddEvent == false) {
			sceneController.dayCycle_Event += Handle_dayCycle_Event;
			_IsAddEvent = true;
		}
	}
	
	#region <!--- Events Handle.
	
	void HandleBaseNotEnoughResourceNotification_event (object sender, NoEnoughResourceNotificationArg eventArg)
	{
		base.notificationText = eventArg.notification_args;
	}
	
	static void Handle_dayCycle_Event (object sender, System.EventArgs e)
	{
		StoreHouse.Remove_sumOfFood(HouseBeh.SumOfPopulation);

        string output = string.Format("HouseBeh.Handle_dayCycle_Event..." + ":: remove '{0}' food from storehouse.", SumOfPopulation.ToString());
        Debug.Log(output);
	}
	
	#endregion
	
    protected override void InitializeTexturesResource()
    {
        base.InitializeTexturesResource();

        //<!-- Load textures resources.
        buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "House", typeof(Texture2D)) as Texture2D;
    }

    private void CalculationCurrentDweller()
    {
        for (int i = 1; i <= maxDweller.Length; i++) {
            if (base.Level == i) {
                this.currentMaxDweller = this.maxDweller[i - 1];
				HouseBeh.CalculationSumOfPopulation();
                return;
            }
        }

        Debug.Log("CalculationCurrentDweller");
    }
	
	public override void InitializingBuildingBeh (BuildingStatus p_buildingState, TileArea area, int p_level)
	{
		base.InitializingBuildingBeh (p_buildingState, area, p_level);

        BuildingBeh.House_Instances.Add(this);
		this.CalculateNumberOfEmployed(p_level);
	}
	
	protected override void CalculateNumberOfEmployed (int p_level)
	{
		base.CalculateNumberOfEmployed (p_level);
		
		int sumOfEmployed = 0;
		for (int i = 0; i < p_level; i++) {
			sumOfEmployed += RequireResource[i].Employee;
		}
		
		HouseBeh.SumOfEmployee += sumOfEmployed;
	}

	/// <summary>
	/// Buildings the process complete.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
    protected override void BuildingProcessComplete(BuildingBeh obj)
    {
        base.BuildingProcessComplete(obj);

        this.CalculationCurrentDweller();

        if (QuestSystemManager.CurrentMissionTopic_ID == 3)
            sceneController.taskManager.questManager.MissionComplete(3);
    }
	/// <summary>
	/// Decreases the building level.
	/// </summary>
	protected override void DecreaseBuildingLevel ()
	{	
		base.DecreaseBuildingLevel ();
		
		buildingStatus_textmesh.text = this.Level.ToString();
		this.CalculationCurrentDweller();
	}

	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

		base.NotEnoughResource_Notification_event -= HandleBaseNotEnoughResourceNotification_event;
		BuildingBeh.House_Instances.Remove(this);
		HouseBeh.CalculationSumOfPopulation();
	}

	protected override void OnTouchDown ()
	{
		if(TaskManager.IsShowInteruptGUI == true)
			return;
		
		base.OnTouchDown ();
		
//		sceneController.taskManager.currentRightSidebarState = TaskManager.RightSidebarState.show_domination;
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if(currentBuildingStatus == BuildingBeh.BuildingStatus.onBuildingProcess
			|| currentBuildingStatus == BuildingBeh.BuildingStatus.onUpgradeProcess) {
			buildingStatus_textmesh.text = base.notificationText;
		}
	}
	
    protected override void CreateWindow()
    {
        base.CreateWindow();

        //if(base.notificationText == "") {
        //    base.notificationText = base.currentBuildingStatus.ToString();
        //}
		
        GUI.Box(base.notificationBox_rect, base.notificationText, base.notification_Style);
		
        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.windowRect.width, base.background_Rect.height));
        {
            GUI.BeginGroup(base.building_background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

                #region <!--- Description group.

                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {
                    //<!-- Level < require resouce, can upgrade.
                    if (base.Level < RequireResource.Length)
                    {
                        GUI.Label(currentJob_Rect, "Current max dweller. : " + this.currentMaxDweller, base.job_style);
                        GUI.Label(nextJob_Rect, "Next max dweller. : " + this.maxDweller[base.Level], base.job_style);

                        //<!-- Requirements Resource.
                        GUI.BeginGroup(update_requireResource_Rect);
                        {
                            GUI.Box(GameMaterialDatabase.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), sceneController.taskManager.food_icon), standard_Skin.box);
                            GUI.Box(GameMaterialDatabase.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), sceneController.taskManager.wood_icon), standard_Skin.box);
                            GUI.Box(GameMaterialDatabase.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), sceneController.taskManager.stone_icon), standard_Skin.box);
                            GUI.Box(GameMaterialDatabase.Fourth_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), sceneController.taskManager.gold_icon), standard_Skin.box);
                            GUI.Box(GameMaterialDatabase.Fifth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), sceneController.taskManager.employee_icon), standard_Skin.box);
                        }
                        GUI.EndGroup();
                    }
                    else
                    {
                        GUI.Label(currentJob_Rect, "Current max population. : " + this.currentMaxDweller, base.job_style);
                        GUI.Label(nextJob_Rect, "Max upgrade building.", base.job_style);
                    }
                }
                GUI.EndGroup();

                #endregion

                #region <!--- Upgrade Button mechanichm.

                GUI.enabled = base.Level < RequireResource.Length && base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level]) ? true : false;
                if (GUI.Button(base.upgrade_Button_Rect, new GUIContent("Upgrade")))
                {
                    GameMaterialDatabase.UsedResource(RequireResource[Level]);

                    base.currentBuildingStatus = BuildingBeh.BuildingStatus.onUpgradeProcess;
                    base.OnUpgradeProcess(this);
                    base.CloseGUIWindow();
                }
                GUI.enabled = true;

                #endregion

                #region <!--- Destruction button.

                GUI.enabled = this.CheckingCanDestructionBuilding();
                if (GUI.Button(destruction_Button_Rect, new GUIContent("Destruct")))
                {
                    this.currentBuildingStatus = BuildingStatus.OnDestructionProcess;
                    this.DestructionBuilding();
                    base.CloseGUIWindow();
                }
                GUI.enabled = true;

                #endregion
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
