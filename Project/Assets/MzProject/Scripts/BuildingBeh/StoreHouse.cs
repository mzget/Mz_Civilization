using UnityEngine;
using System.Collections;


public class StoreHouse : Buildings {
	
    private int food = 500;
    private int wood = 500;
    private int gold = 500;
    private int stone = 500;
    public int ID;
    public int Level { get { return base.level; } set { base.level = value; } }
	
	//<!-- Requirements Resource.
	public static GameResource CreateResource = new GameResource(80, 120, 40, 60);
    public GameResource[] UpgradeResource = new GameResource[10] {
		new GameResource(80, 120, 40, 60),
        new GameResource(200, 200, 200, 200),
        new GameResource(300, 300, 300, 300),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
	};
    private int[] maxCapacities = new int[] { 500, 800, 1200, 1700, 2300, };
	private int currentMaxCapacity;
//    public int MaxCapacity { get { return maxCapacity; } set { maxCapacity = value; } }
	
    //<!-- Static Data.
	public static int sumOfFood = 500;
	public static int sumOfWood = 500;
	public static int sumOfGold = 500;
	public static int sumOfStone = 500;
    public static int SumOfCapacity = 500;
    public static string BuildingName = "Store House";
    private static string Description_TH = "โรงหลอมแร่ แร่ทองแดงถูกหลอมขึ้นที่นี่ อาวุธและชุดเกราะในกองทัพของคุณจำเป็นต้องใช้มัน \n" + " อัพเกรดเพื่อเพิ่มกำลังการผลิต";
    private static string Description_EN = "The Storehouse functions as a resource drop site \n " +
		"It is also the place where resource Gathering technologies are researched";
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
    }
	
	// Use this for initialization
    void Start()
    {
        this.ReCalculationCapacityData();
    }

    void ReCalculationCapacityData()
    {
        for (int i = 0; i < maxCapacities.Length; i++)
        {
            if (base.level == i)
            {
                this.currentMaxCapacity = this.maxCapacities[i];
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
    protected override void DestroyBuildingProcess(Buildings obj)
    {
        base.DestroyBuildingProcess(obj);

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != Buildings.BuildingStatus.none) {
            this.ReCalculationCapacityData();
            this.currentBuildingStatus = Buildings.BuildingStatus.none;
        }
    }

    #endregion
	
	// Update is called once per frame
	void Update () {
	    
	}

    protected override void CreateWindow(int windowID)
    {
		base.CreateWindow(windowID);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.background_Rect.width, base.background_Rect.height),
			scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height));
        {
            GUI.BeginGroup(base.background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.buildingIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.level, standard_Skin.box);
                GUI.BeginGroup(base.description_Rect, CurrentDescription, building_Skin.textArea);
                {   //<!-- group draw order.

                    //<!-- Current Production rate.
                    GUI.Label(currentProduction_Rect, "Current Max Capacity : " + currentMaxCapacity, building_Skin.label);
                    GUI.Label(nextProduction_Rect, "Next Max Capacity : " + this.maxCapacities[this.level], building_Skin.label);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Label(GameResource.Food_Rect, new GUIContent(this.UpgradeResource[level].Food.ToString(), base.food_icon), standard_Skin.box);
                        GUI.Label(GameResource.Wood_Rect, new GUIContent(this.UpgradeResource[level].Wood.ToString(), base.wood_icon), standard_Skin.box);
                        GUI.Label(GameResource.Copper_Rect, new GUIContent(this.UpgradeResource[level].Gold.ToString(), base.copper_icon), standard_Skin.box);
                        GUI.Label(GameResource.Stone_Rect, new GUIContent(this.UpgradeResource[level].Stone.ToString(), base.stone_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();

                    //<!-- Upgrade Button.
                    if (StoreHouse.sumOfFood >= this.UpgradeResource[level].Food && StoreHouse.sumOfWood >= this.UpgradeResource[level].Wood &&
                        StoreHouse.sumOfGold >= this.UpgradeResource[level].Gold && StoreHouse.sumOfStone >= this.UpgradeResource[level].Stone)
                    {
                        Buildings._CanUpgradeLevel = this.CheckingCanUpgradeLevel();

                        if (Buildings._CanUpgradeLevel)
                        {
                            if (GUI.Button(base.upgradeButton_Rect, new GUIContent("Upgrade")))
                            {
                                base.currentBuildingStatus = Buildings.BuildingStatus.onUpgradeProcess;
                                base.OnUpgradeProcess(this);
                                base._isShowInterface = false;
                            }
                        }
                    }
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
