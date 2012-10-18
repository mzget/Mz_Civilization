using UnityEngine;
using System.Collections;


public class StoreHouse : BuildingBeh {

    public const string PathOfResourcesTexture_Icon = "Textures/Resource_icons/";

    //<!--- Requirements Resource.
    public static GameResource[] RequireResource = new GameResource[10] {
		new GameResource() {Food = 80, Wood = 120, Gold = 60, Employee = 5},
        new GameResource() {Food = 160, Wood = 240, Gold = 120, Employee = 10},
        new GameResource() {Food = 320, Wood = 480, Gold = 240, Employee = 15},
        new GameResource() {Food = 640, Wood = 960, Gold = 480, Employee = 20},
        new GameResource() {Food = 1280, Wood = 1920, Gold = 960, Employee = 25},
        new GameResource() {Food = 2560, Wood = 3840, Gold = 1920, Employee = 30},
        new GameResource() {Food = 5120, Wood = 7680, Gold = 3840, Employee = 35},
        new GameResource() {Food = 10240, Wood = 15360, Gold = 7680, Employee = 40},
        new GameResource() {Food = 20480, Wood = 30720, Gold = 15360, Employee = 45},
        new GameResource() {Food = 40960, Wood = 61440, Gold = 30720, Employee = 50},
	};


//    private int gold = 500;	
	public static int sumOfGold = 500;
    //<!--- Static Data.
	public static int sumOfFood = 500;
	public static int sumOfWood = 500;
	public static int sumOfStone = 500;
    public static int sumOfCopper = 500;
    public static int SumOfMaxCapacity = 500;
    public static string BuildingName = "Store House";
    private static string Description_TH = "";
    private static string Description_EN = "The Storehouse functions as a resource drop site \n " + "It is also the place where resource Gathering technologies are researched";
    public static string CurrentDescription {
        get {
            string temp = Description_EN;
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                temp = Description_TH;
            return temp;
        }
    }

    private int food = 500;
    private int wood = 500;
    private int stone = 500;
    private int copper = 500;
    private int[] maxCapacities = new int[10] { 800, 1200, 1800, 2500, 3500, 4800, 6000, 8000, 12000, 15000, };
    private int currentMaxCapacity;
	
	
	public static void CalculationSumofFood() {
		sumOfFood = 0;
        foreach (StoreHouse obj in BuildingBeh.StoreHouseInstance) {
            sumOfFood += obj.food;
        }
	}
	public static void CalculationSumofWood() {
		sumOfWood = 0;
        foreach (StoreHouse obj in BuildingBeh.StoreHouseInstance) {
            sumOfWood += obj.wood;
        }
	}
	public static void CalculationSumofStone() {
		sumOfStone = 0;
        foreach (StoreHouse obj in BuildingBeh.StoreHouseInstance) {
            sumOfStone += obj.stone;
        }
	}
	public static void CalculationSumofCopper() {
		sumOfCopper = 0;
        foreach (StoreHouse obj in BuildingBeh.StoreHouseInstance) {
            sumOfCopper += obj.copper;
        }
	}
    public static void CalculationSumOfMaxCapacity() {
        SumOfMaxCapacity = 500;
        foreach (StoreHouse obj in BuildingBeh.StoreHouseInstance) {
            SumOfMaxCapacity += obj.currentMaxCapacity;
        }
    }	

    public static void ReachDayCycle(object sender, System.EventArgs e) {
        if (BuildingBeh.Farm_Instance.Count == 0 && sumOfFood < SumOfMaxCapacity)
            sumOfFood += 2;
        if (BuildingBeh.Sawmill_Instance.Count == 0 && sumOfWood < SumOfMaxCapacity)
            sumOfWood += 2;
        if (BuildingBeh.MillStoneInstance.Count == 0 && sumOfStone < SumOfMaxCapacity)
            sumOfStone += 2;
//        if (BuildingBeh.SmelterInstance.Count == 0 && sumOfCopper < SumOfMaxCapacity)
//            sumOfCopper += 2;
		
		//<!--- Check capacity.
		if(sumOfFood > SumOfMaxCapacity) 
			sumOfFood = SumOfMaxCapacity;
		if(sumOfWood > SumOfMaxCapacity) 
			sumOfWood = SumOfMaxCapacity;
		if(sumOfStone > SumOfMaxCapacity) 
			sumOfStone = SumOfMaxCapacity;
		if(sumOfCopper > SumOfMaxCapacity)
			sumOfCopper = SumOfMaxCapacity;
    }
	

	/// <summary>
	/// Awake this instance.
	/// </summary>
    protected override void Awake()
    {
		base.Awake();
		base.sprite = this.gameObject.GetComponent<OTSprite>();
		
        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
    }
	
	// Use this for initialization
    void Start()
    {
		this.InitializeTexturesResource();
        this.ReCalculationCapacityData();

        StoreHouse.ReachDayCycle(this, System.EventArgs.Empty);
        base.NotEnoughResource_Notification_event += StoreHouse_NotEnoughResource_Notification_event;
    }
	
	#region <!--- Event Handle.
	
    private void StoreHouse_NotEnoughResource_Notification_event(object sender, NoEnoughResourceNotificationArg e)
    {
        base.notificationText = e.notification_args;
    }
	
	#endregion
	
    protected override void InitializeTexturesResource()
    {
        base.InitializeTexturesResource();

        //<!-- Load textures resources.
        buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Storehouse", typeof(Texture2D)) as Texture2D;
    }

    void ReCalculationCapacityData()
    {
        for (int i = 1; i <= maxCapacities.Length; i++)
        {
            if (base.Level == i)
            {
                this.currentMaxCapacity = this.maxCapacities[i - 1];
                StoreHouse.CalculationSumOfMaxCapacity();
                break;
            }
        }
    }

    public override void InitializeBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializeBuildingBeh(p_buildingState, p_indexPosition, p_level);

        BuildingBeh.StoreHouseInstance.Add(this);
    }
	
    #region <!-- Building Processing.

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
            this.ReCalculationCapacityData();
			
            this.currentBuildingStatus = BuildingBeh.BuildingStatus.none;
        }
    }

    #endregion
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        base.stageManager.dayCycle_Event -= StoreHouse.ReachDayCycle;
		BuildingBeh.StoreHouseInstance.Remove(this);
	}
	
	// Update is called once per frame
	void Update () {

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

                #region <!--- Contents Group.

                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {   
                    //<!-- Current Production rate.
					if(base.Level < 10)
                    {
                        GUI.Label(currentJob_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                        GUI.Label(nextJob_Rect, "Next Max Capacity : " + this.maxCapacities[base.Level], base.job_style);
						
	                    //<!-- Requirements Resource.
	                    GUI.BeginGroup(update_requireResource_Rect);
                        {
                            GUI.Label(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), 
                                base.stageManager.gui_Manager.food_icon), standard_Skin.box);
                            GUI.Label(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), 
                                base.stageManager.gui_Manager.wood_icon), standard_Skin.box);
                            //GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                            //    base.stageManager.taskbarManager.stone_icon), standard_Skin.box);
                            GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(),
                                base.stageManager.gui_Manager.gold_icon), standard_Skin.box);
                            GUI.Label(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), 
                                base.stageManager.gui_Manager.employee_icon), standard_Skin.box);
	                    }
	                    GUI.EndGroup();
					}
					else {
                        GUI.Label(currentJob_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
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
