using UnityEngine;
using System.Collections;


public class StoreHouse : BuildingBeh {

    public const string PathOfResourcesTexture_Icon = "Textures/Resource_icons/";
	public const int STOREHOUSE_MAX_LEVEL = 21;

    //<!--- Requirements Resource.
    public static GameResource[] RequireResource = new GameResource[STOREHOUSE_MAX_LEVEL] {
		new GameResource() {Food = 80, Wood = 120, Gold = 60, Employee = 3},
		new GameResource() {Food = 130, Wood = 160, Gold = 90, Employee = 5},
		new GameResource() {Food = 165, Wood = 205, Gold = 115, Employee = 8},
		new GameResource() {Food = 215, Wood = 260, Gold = 145, Employee = 12},
		new GameResource() {Food = 275, Wood = 335, Gold = 190, Employee = 15},
		new GameResource() {Food = 350, Wood = 430, Gold = 240, Employee = 20},		//5.
		new GameResource() {Food = 445, Wood = 550, Gold = 310, Employee = 24},
		new GameResource() {Food = 570, Wood = 705, Gold = 395, Employee = 30},
		new GameResource() {Food = 730, Wood = 900, Gold = 505, Employee = 36},
		new GameResource() {Food = 935, Wood = 1115, Gold = 650, Employee = 45},
		new GameResource() {Food = 1200, Wood = 1475, Gold = 830, Employee = 50},		//10.
		new GameResource() {Food = 1535, Wood = 1890, Gold = 1065, Employee = 56},
		new GameResource() {Food = 1965, Wood = 2420, Gold = 1360, Employee = 64},
		new GameResource() {Food = 2515, Wood = 3195, Gold = 1740, Employee = 72},
		new GameResource() {Food = 3220, Wood = 3960, Gold = 2230, Employee = 84},
		new GameResource() {Food = 4120, Wood = 5070, Gold = 2850, Employee = 100},		// 15.
		new GameResource() {Food = 5275, Wood = 6490, Gold = 3650, Employee = 120},
		new GameResource() {Food = 6750, Wood = 8310, Gold = 4675, Employee = 144},		
        new GameResource() {Food = 8640, Wood = 10635, Gold = 5980, Employee = 175},
        new GameResource() {Food = 11060, Wood = 12610, Gold = 7655, Employee = 200},
        new GameResource() {Food = 14155, Wood = 17420, Gold = 9800, Employee = 250},		//20.
	};

    //<!--- Static Data.
	public static int sumOfGold = 0;
	public static int sumOfWeapon = 0;
	public static int sumOfArmor = 0;

	#region <@-- Game resources.

	private static int sumOfFood = 0;
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

	private static int sumOfWood = 0;
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

	private static int sumOfStone = 0;
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

    private static int sumOfCopper = 0;
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
        if (BuildingBeh.Farm_Instance.Count == 0 && sumOfFood < SumOfMaxCapacity)
            sumOfFood += 2;
        if (BuildingBeh.Sawmill_Instance.Count == 0 && sumOfWood < SumOfMaxCapacity)
            sumOfWood += 2;
        if (BuildingBeh.MillStoneInstance.Count == 0 && sumOfStone < SumOfMaxCapacity)
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

    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, p_indexPosition, p_level);

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
		this.CheckingQuestComplete();
    }

    #endregion
	
	void CheckingQuestComplete ()
	{		
		if(QuestSystemManager.arr_isMissionComplete[4] == false) {
			sceneController.taskManager.questManager.list_questBeh[4]._IsComplete = true;
			QuestSystemManager.arr_isMissionComplete[4] = true;
			
			if (QuestSystemManager.CurrentMissionTopic_ID == 4) {
				sceneController.taskManager.questManager.ActiveBeh_NoticeButton();
			}
		}
	}
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        base.sceneController.dayCycle_Event -= StoreHouse.ReachDayCycle;
		BuildingBeh.StoreHouseInstance.Remove(this);
	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

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
                            GUI.Label(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), 
                                base.sceneController.taskManager.food_icon), standard_Skin.box);
                            GUI.Label(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), 
                                base.sceneController.taskManager.wood_icon), standard_Skin.box);
                            //GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                            //    base.stageManager.taskbarManager.stone_icon), standard_Skin.box);
                            GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(),
                                base.sceneController.taskManager.gold_icon), standard_Skin.box);
                            GUI.Label(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), 
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
