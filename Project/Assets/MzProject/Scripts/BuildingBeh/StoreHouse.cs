using UnityEngine;
using System.Collections;


public class GameResource {
	public int Food;
	public int Wood;
	public int Gold;
	public int Stone;
	
	public GameResource (int food, int wood, int gold, int stone) {
		this.Food = food;
		this.Wood = wood;
		this.Gold = gold;
		this.Stone = stone;
	}
};

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
	public static string description = "The Storehouse functions as a resource drop site \n " +
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
//	public static void UsedResource(int food, int wood, int gold, int stone) {
//		sumOfFood -= food;
//		sumOfWood -= wood;
//		sumOfGold -= gold;
//		sumOfStone -= stone;
//	}	
	public static void UsedResource(GameResource usedResource) {
		sumOfFood -= usedResource.Food;
		sumOfWood -= usedResource.Wood;
		sumOfGold -= usedResource.Gold;
		sumOfStone -= usedResource.Stone;
	}

	// Use this for initialization
	void Start () {
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
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    protected override void CreateWindow(int windowID)
    {
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), building_Skin.customStyles[0]))
        {
            _clicked = false;
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