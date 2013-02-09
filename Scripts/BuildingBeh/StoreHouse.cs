using UnityEngine;
using System.Collections;


public class StoreHouse : BuildingBeh {

    public const string PathOfResourcesTexture_Icon = "Textures/Resource_icons/";
	public const int STOREHOUSE_MAX_LEVEL = 21;

    //<!--- Requirements Resource.
    public static GameMaterialDatabase[] RequireResource = new GameMaterialDatabase[STOREHOUSE_MAX_LEVEL] {
		new GameMaterialDatabase() {Food = 80, Wood = 120, Gold = 60, Employee = 3},
		new GameMaterialDatabase() {Food = 130, Wood = 160, Gold = 90, Employee = 5},
		new GameMaterialDatabase() {Food = 165, Wood = 205, Gold = 115, Employee = 8},
		new GameMaterialDatabase() {Food = 215, Wood = 260, Gold = 145, Employee = 12},
		new GameMaterialDatabase() {Food = 275, Wood = 335, Gold = 190, Employee = 15},
		new GameMaterialDatabase() {Food = 350, Wood = 430, Gold = 240, Employee = 20},		//5.
		new GameMaterialDatabase() {Food = 445, Wood = 550, Gold = 310, Employee = 24},
		new GameMaterialDatabase() {Food = 570, Wood = 705, Gold = 395, Employee = 30},
		new GameMaterialDatabase() {Food = 730, Wood = 900, Gold = 505, Employee = 36},
		new GameMaterialDatabase() {Food = 935, Wood = 1115, Gold = 650, Employee = 45},
		new GameMaterialDatabase() {Food = 1200, Wood = 1475, Gold = 830, Employee = 50},		//10.
		new GameMaterialDatabase() {Food = 1535, Wood = 1890, Gold = 1065, Employee = 56},
		new GameMaterialDatabase() {Food = 1965, Wood = 2420, Gold = 1360, Employee = 64},
		new GameMaterialDatabase() {Food = 2515, Wood = 3195, Gold = 1740, Employee = 72},
		new GameMaterialDatabase() {Food = 3220, Wood = 3960, Gold = 2230, Employee = 84},
		new GameMaterialDatabase() {Food = 4120, Wood = 5070, Gold = 2850, Employee = 100},		// 15.
		new GameMaterialDatabase() {Food = 5275, Wood = 6490, Gold = 3650, Employee = 120},
		new GameMaterialDatabase() {Food = 6750, Wood = 8310, Gold = 4675, Employee = 144},		
        new GameMaterialDatabase() {Food = 8640, Wood = 10635, Gold = 5980, Employee = 175},
        new GameMaterialDatabase() {Food = 11060, Wood = 12610, Gold = 7655, Employee = 200},
        new GameMaterialDatabase() {Food = 14155, Wood = 17420, Gold = 9800, Employee = 250},		//20.
	};

    //<!--- Static Data.
	public static int sumOfGold = 2000;
	public static int sumOfWeapon = 2000;
	public static int sumOfArmor = 2000;

	#region <@-- Game resources.

	private static int sumOfFood = 2000;
	public static int SumOfFood {
		get { return sumOfFood; } 
		set { sumOfFood = value; } 
	}
	public static void Add_sumOfFood (int p_value) {
		if(sumOfFood < SumOfMaxCapacity) sumOfFood += p_value;

		if(SumOfFood > SumOfMaxCapacity) sumOfFood = SumOfMaxCapacity;
	}
	public static void Remove_sumOfFood (int p_value) {
		if(sumOfFood > 0) sumOfFood -= p_value;
		
		if(sumOfFood < 0) sumOfFood = 0;
	}

	private static int sumOfWood = 2000;
	public static int SumOfWood {
		get { return sumOfWood; } 
		set { sumOfWood = value; }
	}
	public static void Add_sumOfWood (int p_value) {
		if(sumOfWood < SumOfMaxCapacity) sumOfWood += p_value;

		if(sumOfWood > SumOfMaxCapacity) sumOfWood = SumOfMaxCapacity;
	}
	public static void Remove_sumOfWood (int p_value) {
		if(sumOfWood > 0) sumOfWood -= p_value;
		
		if(sumOfWood < 0) sumOfWood = 0;
	}

	private static int sumOfStone = 2000;
	public static int SumOfStone {		
		get { return sumOfStone; }				
		set { sumOfStone = value;}
	}
	public static void Add_sumOfStone (int p_value) {
		if(sumOfStone < SumOfMaxCapacity) sumOfStone += p_value;

		if(sumOfStone > SumOfMaxCapacity) sumOfStone = SumOfMaxCapacity;
	}
	public static void Remove_sumOfStone (int p_value) {
		if(sumOfStone > 0)	sumOfStone -= p_value;
		
		if(sumOfStone < 0)	sumOfStone = 0;
	}

    private static int sumOfCopper = 2000;
	public static int SumOfCopper {
		get { return sumOfCopper; } 
		set {sumOfCopper = value;}
	}
	public static void Add_sumOfCopper (int p_value) {
		if(sumOfCopper < SumOfMaxCapacity) sumOfCopper += p_value;
		if(sumOfCopper > SumOfMaxCapacity) sumOfCopper = SumOfMaxCapacity;
	}
	public static void Remove_sumOfCopper (int p_value) {
		if(sumOfCopper > 0)	sumOfCopper -= p_value;
		
		if(sumOfCopper < 0)	sumOfCopper = 0;
	}

	#endregion

    public static int SumOfMaxCapacity = 0;
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

    private int[] maxCapacities = new int[STOREHOUSE_MAX_LEVEL] {
		800, 1200, 1700, 2300, 3100, 4000, 5000, 6300, 7800, 9600, 
		11800, 14400, 17600, 21400, 25900, 31300, 37900, 45700, 55100, 66400, 80000
	};
    private int currentMaxCapacity;
	
    public static void CalculationSumOfMaxCapacity() {
        SumOfMaxCapacity = 800;
        foreach (StoreHouse obj in BuildingBeh.StoreHouseInstance) {
            SumOfMaxCapacity += obj.currentMaxCapacity;
        }
    }	
	
    public static void ReachDayCycle(object sender, System.EventArgs e) {
        if (BuildingBeh.Farm_Instances.Count == 0 && sumOfFood < SumOfMaxCapacity)
            sumOfFood += 2;
        if (BuildingBeh.Sawmill_Instances.Count == 0 && sumOfWood < SumOfMaxCapacity)
            sumOfWood += 2;
        if (BuildingBeh.StoneCrushingPlant_Instances.Count == 0 && sumOfStone < SumOfMaxCapacity)
            sumOfStone += 2;
//        if (BuildingBeh.SmelterInstance.Count == 0 && sumOfCopper < SumOfMaxCapacity)
//            sumOfCopper += 2;
		
		//<!--- Check capacity.
		if(sumOfFood > SumOfMaxCapacity) 			sumOfFood = SumOfMaxCapacity;
		if(sumOfFood < 0) 							sumOfFood = 0;
		
		if(sumOfWood > SumOfMaxCapacity) 			sumOfWood = SumOfMaxCapacity;
		if(sumOfWood < 0)							sumOfWood = 0;
		
		if(sumOfStone > SumOfMaxCapacity) 			sumOfStone = SumOfMaxCapacity;
		if(sumOfStone < 0) 							sumOfStone = 0;
		
		if(sumOfCopper > SumOfMaxCapacity)			sumOfCopper = SumOfMaxCapacity;
		if(sumOfCopper < 0) 						sumOfCopper = 0;

        Debug.Log("StoreHouse.ReachDayCycle event");
    }

	/// Awake this instance.
    protected override void Awake()
    {
		base.Awake();
        base.sprite = this.gameObject.GetComponent<tk2dSprite>();
		
        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData();
		base.buildingTimeData.arrBuildingTimesData = new float[] { 
			30f, 50f, 90f, 120f, 180f, 220f, 250f, 300f, 400f, 500f,
			600f, 700f, 800f, 900f, 1000f, 1100f, 1200f, 1300f, 1400f, 1500f, 
		};

        base.processbar_offsetPos = Vector3.up * 60;
    }
	
	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        this.InitializeTexturesResource();
        this.ReCalculationCapacityData();

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

    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, TileArea area, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, area, p_level);
		
        BuildingBeh.StoreHouseInstance.Add(this);
		this.CalculateNumberOfEmployed(p_level);
    }
	protected override void CalculateNumberOfEmployed (int p_level)
	{
		int sumOfEmployed = 0;
		for (int i = 0; i < p_level; i++) {
			sumOfEmployed += RequireResource[i].Employee;
		}
		
		HouseBeh.SumOfEmployee += sumOfEmployed;
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if(currentBuildingStatus == BuildingBeh.BuildingStatus.onBuildingProcess
			|| currentBuildingStatus == BuildingBeh.BuildingStatus.onUpgradeProcess) {
			buildingLevel_textmesh.text = base.notificationText;
		}
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
        this.ReCalculationCapacityData();

        if (QuestSystemManager.arr_isMissionComplete[4] == false)
            sceneController.taskManager.questManager.MissionComplete(4);
    }

    #endregion
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        base.sceneController.dayCycle_Event -= StoreHouse.ReachDayCycle;
		BuildingBeh.StoreHouseInstance.Remove(this);
	}

    protected override void CreateWindow()
    {
        base.CreateWindow();

//        if(base.notificationText == "") {
//            base.notificationText = base.currentBuildingStatus.ToString();
//        }
        GUI.Box(base.notificationBox_rect, base.notificationText, base.notification_Style);

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
                            GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), 
                                base.sceneController.taskManager.food_icon), standard_Skin.box);
                            GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), 
                                base.sceneController.taskManager.wood_icon), standard_Skin.box);
                            //GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                            //    base.stageManager.taskbarManager.stone_icon), standard_Skin.box);
                            GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(),
                                base.sceneController.taskManager.gold_icon), standard_Skin.box);
                            GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), 
                                base.sceneController.taskManager.employee_icon), standard_Skin.box);
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

                GUI.enabled = (base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level])) ? true : false;
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
