using UnityEngine;
using System.Collections;

public class HouseBeh : BuildingBeh {

	//<!--- The require resource.
    public static GameResource[] RequireResource = new GameResource[5] {
		new GameResource(80, 120, 40, 60, 0),
        new GameResource(160, 240, 80, 120, 0),
        new GameResource(320, 480, 160, 240, 0),
        new GameResource(640, 960, 320, 480, 0),
        new GameResource(1280, 1920, 640, 960, 0),
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
    public static void CalculationSumOfPopulation() {
        SumOfPopulation = 10;
        foreach (HouseBeh house in BuildingBeh.House_Instances)
            SumOfPopulation += house.currentMaxDweller;
		
		SumOfUnemployed = SumOfPopulation - SumOfEmployee;
    }
	
    private int[] maxDweller = new int[5] { 10, 20, 30, 40, 50, };
    private int currentMaxDweller;


    //<!--- Awake --->
    protected override void Awake()
    {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = BuildingName;
        base.buildingType = BuildingType.general;
		base.buildingTimeData = new BuildingsTimeData(buildingType);
    }

	// Use this for initialization
	IEnumerator Start () {
        this.InitializeTexturesResource();
        this.ReCalculatePopulationData();
		
		base.NotEnoughResource_Notification_event += HandleBaseNotEnoughResourceNotification_event;
		
        yield return 0;
    }
	
	#region <!--- Event Handle.
	
	void HandleBaseNotEnoughResourceNotification_event (object sender, NoEnoughResourceNotificationArg eventArg)
	{
		base.notificationText = eventArg.notification_args;
	}
	
	#endregion
	
    protected override void InitializeTexturesResource()
    {
        base.InitializeTexturesResource();

        //<!-- Load textures resources.
        buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "House", typeof(Texture2D)) as Texture2D;
    }

    private void ReCalculatePopulationData()
    {
        for (int i = 1; i <= maxDweller.Length; i++)
        {
            if (base.Level == i)
            {
                this.currentMaxDweller = this.maxDweller[i - 1];
                CalculationSumOfPopulation();
                break;
            }
        }
    }
	
	public override void InitializingBuildingBeh (BuildingStatus p_buildingState, int p_indexPosition, int p_level)
	{
		base.InitializingBuildingBeh (p_buildingState, p_indexPosition, p_level);

        BuildingBeh.House_Instances.Add(this);
		this.CalculateNumberOfEmployed(p_level);
	}
	protected override void CalculateNumberOfEmployed (int p_level)
	{
//		base.CalculateNumberOfEmployed (p_level);
		int sumOfEmployed = 0;
		for (int i = 0; i < p_level; i++) {
			sumOfEmployed += RequireResource[i].Employee;
		}
		
		HouseBeh.SumOfEmployee += sumOfEmployed;
	}

    #region <!--- Building Processing.

    public override void OnBuildingProcess(BuildingBeh obj)
    {
        base.OnBuildingProcess(obj);
    }
    protected override void CreateProcessBar(BuildingBeh.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void BuildingProcessComplete(BuildingBeh obj)
    {
        base.BuildingProcessComplete(obj);

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != BuildingBeh.BuildingStatus.none)
        {
            this.ReCalculatePopulationData();

            this.currentBuildingStatus = BuildingBeh.BuildingStatus.none;
        }
    }

    #endregion
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();
		
		base.NotEnoughResource_Notification_event -= HandleBaseNotEnoughResourceNotification_event;
		BuildingBeh.House_Instances.Remove(this);
	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        if(base.notificationText == "") {
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
                            GUI.Box(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), stageManager.taskManager.food_icon), standard_Skin.box);
                            GUI.Box(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), stageManager.taskManager.wood_icon), standard_Skin.box);
                            GUI.Box(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), stageManager.taskManager.stone_icon), standard_Skin.box);
                            GUI.Box(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), stageManager.taskManager.gold_icon), standard_Skin.box);
                            GUI.Box(GameResource.Fifth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), stageManager.taskManager.employee_icon), standard_Skin.box);
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

                bool enableUpgrade = false;
                if (base.Level < RequireResource.Length && base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level]))
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
