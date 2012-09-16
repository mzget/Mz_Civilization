using UnityEngine;
using System.Collections;


public class StoreHouse : Buildings {

    public const string PathOfResourcesTexture_Icon = "Textures/Resource_icons/";

    //<!--- Requirements Resource.
    public static GameResource[] RequireResource = new GameResource[10] {
		new GameResource(80, 120, 40, 60, 5),
        new GameResource(200, 200, 200, 200, 10),
        new GameResource(300, 300, 300, 300, 15),
        new GameResource(400, 400, 400, 400, 20),
        new GameResource(500, 500, 500, 500, 25),
        new GameResource(600, 600, 600, 600, 30),
        new GameResource(700, 700, 700, 700, 35),
        new GameResource(800, 800, 800, 800, 40),
        new GameResource(900, 900, 900, 900, 45),
        new GameResource(1000, 1000, 1000, 1000, 50),
	};
	
    //<!--- Static Data.
	public static int sumOfFood = 500;
	public static int sumOfWood = 500;
	public static int sumOfGold = 500;
	public static int sumOfStone = 500;
    public static int SumOfCapacity = 500;
    public static string BuildingName = "Store House";
    private static string Description_TH = "โรงหลอมแร่ แร่ทองแดงถูกหลอมขึ้นที่นี่ อาวุธและชุดเกราะในกองทัพของคุณจำเป็นต้องใช้มัน \n" + " อัพเกรดเพื่อเพิ่มกำลังการผลิต";
    private static string Description_EN = "The Storehouse functions as a resource drop site \n " + "It is also the place where resource Gathering technologies are researched";
    public static string CurrentDescription {
        get {
            string temp = Description_EN;

            if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.Thai)
                temp = Description_TH;

            return temp;
        }
    }

    private int food = 500;
    private int wood = 500;
    private int gold = 500;
    private int stone = 500;
    private int[] maxCapacities = new int[10] { 800, 1200, 1800, 2500, 3500, 4800, 6000, 8000, 12000, 15000, };
    private int currentMaxCapacity;
	
	
	public static void CalculationSumofFood() {
		sumOfFood = 0;
        foreach (StoreHouse obj in Buildings.StoreHouseInstance) {
            sumOfFood += obj.food;
        }
	}
	public static void CalculationSumofWood() {
		sumOfWood = 0;
        foreach (StoreHouse obj in Buildings.StoreHouseInstance) {
            sumOfWood += obj.wood;
        }
	}
	public static void CalculationSumofGold() {
		sumOfGold = 0;
        foreach (StoreHouse obj in Buildings.StoreHouseInstance) {
            sumOfGold += obj.gold;
        }
	}
	public static void CalculationSumofStone() {
		sumOfStone = 0;
        foreach (StoreHouse obj in Buildings.StoreHouseInstance) {
            sumOfStone += obj.stone;
        }
	}
    public static void CalculationSumOfMaxCapacity() {
        SumOfCapacity = 0;
        foreach (StoreHouse obj in Buildings.StoreHouseInstance) {
            SumOfCapacity += obj.currentMaxCapacity;
        }
    }	
	public static void UsedResource(GameResource usedResource) {
		sumOfFood -= usedResource.Food;
		sumOfWood -= usedResource.Wood;
		sumOfGold -= usedResource.Gold;
		sumOfStone -= usedResource.Stone;
	}

    public static void ReachDayCycle(object sender, System.EventArgs e) {
        if (Buildings.Farm_Instance.Count == 0 && sumOfFood < SumOfCapacity)
            sumOfFood += 2;
        if (Buildings.Sawmill_Instance.Count == 0 && sumOfWood < SumOfCapacity)
            sumOfWood += 2;
        if (Buildings.SmelterInstance.Count == 0 && sumOfGold < SumOfCapacity)
            sumOfGold += 2;
        if (Buildings.MillStoneInstance.Count == 0 && sumOfStone < SumOfCapacity)
            sumOfStone += 2;
		
		//<!--- Check capacity.
		if(sumOfFood > SumOfCapacity) sumOfFood = SumOfCapacity;
		if(sumOfWood > SumOfCapacity) sumOfWood = SumOfCapacity;
		if(sumOfStone > SumOfCapacity) sumOfStone = SumOfCapacity;
		if(sumOfGold > SumOfCapacity) sumOfGold = SumOfCapacity;
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
		this.LoadTextureResource();
        this.ReCalculationCapacityData();
    }

    protected override void LoadTextureResource()
    {
        base.LoadTextureResource();

        //<!-- Load textures resources.
        buildingIcon_Texture = Resources.Load(Buildings.PathOf_BuildingIcons + "Storehouse", typeof(Texture2D)) as Texture2D;
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

    public override void InitializeData(Buildings.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializeData(p_buildingState, p_indexPosition, p_level);

        Buildings.StoreHouseInstance.Add(this);
    }
	
    #region Building Processing.

    public override void OnBuildingProcess(Buildings obj)
    {
        base.OnBuildingProcess(obj);
    }
    protected override void CreateProcessBar(Buildings.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void BuildingProcessComplete(Buildings obj)
    {
        base.BuildingProcessComplete(obj);

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != Buildings.BuildingStatus.none) 
		{
            this.ReCalculationCapacityData();
			
            this.currentBuildingStatus = Buildings.BuildingStatus.none;
        }
    }

    #endregion
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        base.stageManager.dayCycle -= ReachDayCycle;
		Buildings.StoreHouseInstance.Remove(this);
	}
	
	// Update is called once per frame
	void Update () {

	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        GUI.Box(new Rect(48, 24, 256, 48), base.currentBuildingStatus.ToString(), standard_Skin.box);

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
                        GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                        GUI.Label(nextProduction_Rect, "Next Max Capacity : " + this.maxCapacities[base.Level], base.job_style);
						
	                    //<!-- Requirements Resource.
	                    GUI.BeginGroup(update_requireResource_Rect);
                        {
                            GUI.Label(GameResource.Food_Rect, new GUIContent(RequireResource[Level].Food.ToString(), base.stageManager.taskbarManager.food_icon), GUI.skin.box);
                            GUI.Label(GameResource.Wood_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), base.stageManager.taskbarManager.wood_icon), GUI.skin.box);
                            GUI.Label(GameResource.Stone_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), base.stageManager.taskbarManager.stone_icon), GUI.skin.box);
                            GUI.Label(GameResource.Gold_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), base.stageManager.taskbarManager.gold_icon), GUI.skin.box);
                            GUI.Label(GameResource.Employee_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), base.stageManager.taskbarManager.employee_icon), GUI.skin.box);
	                    }
	                    GUI.EndGroup();
					}
					else {
                        GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                        GUI.Label(nextProduction_Rect, "Max upgrade building.", base.job_style);
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
                    StoreHouse.UsedResource(RequireResource[Level]);

                    base.currentBuildingStatus = Buildings.BuildingStatus.onUpgradeProcess;
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
