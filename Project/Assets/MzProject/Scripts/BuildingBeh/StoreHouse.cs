using UnityEngine;
using System.Collections;


public class StoreHouse : Buildings {

    //<!-- Requirements Resource.
    public static GameResource CreateResource = new GameResource(80, 120, 40, 60);
    public static GameResource[] UpgradeResource = new GameResource[10] {
		new GameResource(80, 120, 40, 60),
        new GameResource(200, 200, 200, 200),
        new GameResource(300, 300, 300, 300),
        new GameResource(400, 400, 400, 400),
        new GameResource(500, 500, 500, 500),
        new GameResource(600, 600, 600, 600),
        new GameResource(700, 700, 700, 700),
        new GameResource(800, 800, 800, 800),
        new GameResource(900, 900, 900, 900),
        new GameResource(1000, 1000, 1000, 1000),
	};
	
    //<!-- Static Data.
	public static int sumOfFood = 500;
	public static int sumOfWood = 500;
	public static int sumOfGold = 500;
	public static int sumOfStone = 500;
    public static int SumOfCapacity = 500;
    public static string BuildingName = "Store House";
    private static string Description_TH = "โรงหลอมแร่ แร่ทองแดงถูกหลอมขึ้นที่นี่ อาวุธและชุดเกราะในกองทัพของคุณจำเป็นต้องใช้มัน \n" + " อัพเกรดเพื่อเพิ่มกำลังการผลิต";
    private static string Description_EN = "The Storehouse functions as a resource drop site \n " + "It is also the place where resource Gathering technologies are researched";
    public static string CurrentDescription {
        get
        {
            string temp = Description_EN;

            if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.Thai)
                temp = Description_TH;

            return temp;
        }
    }

    public int Level { get { return base.level; } set { base.level = value; } }

    private int food = 500;
    private int wood = 500;
    private int gold = 500;
    private int stone = 500;
    private int[] maxCapacities = new int[10] { 500, 800, 1200, 1800, 2500, 3500, 4800, 6000, 7800, 10000, };
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


    protected override void Awake()
    {
		base.Awake();
		base.sprite = this.gameObject.GetComponent<OTSprite>();
		
        this.name = StoreHouse.BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
		
		buildingIcon_Texture = Resources.Load("Textures/Building_Icons/Storehouse", typeof(Texture2D)) as Texture2D;
    }
	
	// Use this for initialization
    void Start()
    {
        this.ReCalculationCapacityData();
    }

    void ReCalculationCapacityData()
    {
        for (int i = 1; i <= maxCapacities.Length; i++)
        {
            if (base.level == i)
            {
                this.currentMaxCapacity = this.maxCapacities[i - 1];
                StoreHouse.CalculationSumOfMaxCapacity();
                break;
            }
        }
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
                GUI.Label(base.levelLable_Rect, "Level " + this.level, base.status_style);
                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {   //<!-- group draw order.

                    //<!-- Current Production rate.
					if(base.level < 10)
                    {
                        GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                        GUI.Label(nextProduction_Rect, "Next Max Capacity : " + this.maxCapacities[base.level], base.job_style);
						
	                    //<!-- Requirements Resource.
	                    GUI.BeginGroup(update_requireResource_Rect);
	                    {
                            GUI.Box(GameResource.Food_Rect, new GUIContent(UpgradeResource[level].Food.ToString(), base.food_icon), standard_Skin.box);
                            GUI.Box(GameResource.Wood_Rect, new GUIContent(UpgradeResource[level].Wood.ToString(), base.wood_icon), standard_Skin.box);
                            GUI.Box(GameResource.Copper_Rect, new GUIContent(UpgradeResource[level].Gold.ToString(), base.copper_icon), standard_Skin.box);
                            GUI.Box(GameResource.Stone_Rect, new GUIContent(UpgradeResource[level].Stone.ToString(), base.stone_icon), standard_Skin.box);
	                    }
	                    GUI.EndGroup();

	                    //<!-- Upgrade Button.
	                    if (StoreHouse.sumOfFood >= UpgradeResource[level].Food && StoreHouse.sumOfWood >= UpgradeResource[level].Wood &&
	                        StoreHouse.sumOfGold >= UpgradeResource[level].Gold && StoreHouse.sumOfStone >= UpgradeResource[level].Stone)
	                    {	
                            bool enableUpgrade = base.CheckingCanUpgradeLevel();

                            GUI.enabled = enableUpgrade;
                            if (GUI.Button(base.upgradeButton_Rect, new GUIContent("Upgrade")))
                            {
                                StoreHouse.UsedResource(UpgradeResource[level]);

                                base.currentBuildingStatus = Buildings.BuildingStatus.onUpgradeProcess;
                                base.OnUpgradeProcess(this);
                                base._isShowInterface = false;
                            }
                            GUI.enabled = true;
	                    }
					}
					else {
                        GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                        GUI.Label(nextProduction_Rect, "Max upgrade building.", base.job_style);
					}
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
