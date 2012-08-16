using UnityEngine;
using System.Collections;


public class StoreHouse : Buildings {
	
    private int food = 500;
	public int Food { get { return food; } }
    private int wood = 500;
    private int gold = 500;
    private int stone = 500;
    public int ID;
    public int Level { get { return level; } set { level = value; } }
    private int maxCapacity;
    public int MaxCapacity { get { return maxCapacity; } set { maxCapacity = value; } }
	//<!-- Requirements Resource.
	public static GameResource CreateResource = new GameResource(80, 120, 40, 60);
    //<!-- Static Data.
	public static int sumOfFood = 500;
	public static int sumOfWood = 500;
	public static int sumOfGold = 500;
	public static int sumOfStone = 500;
    public static int SumOfCapacity = 500;
    public static string BuildingName = "Store House";
	public static string Description = "The Storehouse functions as a resource drop site \n " +
		"It is also the place where resource Gathering technologies are researched";
	
	

	public static void CalculationSumofFood() {
		sumOfFood = 0;
        foreach (StoreHouse obj in Buildings.storeHouseId) {
            sumOfFood += obj.food;
        }
	}
	public static void CalculationSumofWood() {
		sumOfWood = 0;
        foreach (StoreHouse obj in Buildings.storeHouseId) {
            sumOfWood += obj.wood;
        }
	}
	public static void CalculationSumofGold() {
		sumOfGold = 0;
        foreach (StoreHouse obj in Buildings.storeHouseId) {
            sumOfGold += obj.gold;
        }
	}
	public static void CalculationSumofStone() {
		sumOfStone = 0;
        foreach (StoreHouse obj in Buildings.storeHouseId) {
            sumOfStone += obj.stone;
        }
	}
    public static void CalculationMaxCapacity() {
        SumOfCapacity = 0;
        foreach (StoreHouse obj in Buildings.storeHouseId) {
            SumOfCapacity += obj.maxCapacity;
        }
    }	
	public static void UsedResource(GameResource usedResource) {
		sumOfFood -= usedResource.Food;
		sumOfWood -= usedResource.Wood;
		sumOfGold -= usedResource.Gold;
		sumOfStone -= usedResource.Stone;
	}


    void Awake()
    {
		base.sprite = this.gameObject.GetComponent<OTSprite>();
    }
	
	// Use this for initialization
    void Start()
    {
        name = "StoreHouse";
        Debug.Log("StoreHouse:ID ::" + ID + "Level ::" + level);

        switch (level)
        {
            case 0: maxCapacity = 500;
                break;
            case 1: maxCapacity = 800;
                break;
            case 2: maxCapacity = 1200;
                break;
            case 3: maxCapacity = 1700;
                break;
            case 4: maxCapacity = 2300;
                break;
            default:
                break;
        }


        CalculationMaxCapacity();
        //		CalculationSumofFood();
        //		CalculationSumofWood();
        //		CalculationSumofGold();
        //		CalculationSumofStone();

        this.name = StoreHouse.BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
		
        this.currentBuildingStatus = Buildings.BuildingStatus.onBuildingProcess;
        this.OnBuildingProcess(this);
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

        if (this.currentBuildingStatus != Buildings.BuildingStatus.buildingComplete)
            this.currentBuildingStatus = Buildings.BuildingStatus.buildingComplete;
    }

    #endregion
	
	// Update is called once per frame
	void Update () {
	    
	}

    protected override void CreateWindow(int windowID)
    {
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), building_Skin.customStyles[0]))
        {
            _isShowInterface = false;
        }

        scrollPosition = GUI.BeginScrollView(new Rect(0, 100, windowRect.width, windowRect.height - 40), scrollPosition, new Rect(0, 0, windowRect.width - 20, windowRect.height - 40));
        {
            GUI.BeginGroup(background_Rect, new GUIContent(name), building_Skin.box);
            {

            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();

        base.CreateWindow(windowID);
    }
}
