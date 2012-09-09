using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour {
	
	public const string PathOfEconomyBuilding = "Buildings/Economy/";
	public const string PathOfUtilityBuilding =  "Buildings/Utility/";
	public const string PathOfMilitaryBuilding = "Buildings/Military/";

    public TaskbarManager taskbarManager;
    public GUISkin mainBuildingSkin;
    public GUISkin mainInterface;
    /// Texture. 
    public Texture2D mapTex;
    public enum _clickedName { building = 0, none };
//    private _clickedName clickState = _clickedName.none;

    private OTFilledSprite background;
	public static List<Vector2> buildingArea_Pos = new List<Vector2>(12)
    {
		new	Vector2(0, 180), new Vector2(138, 100), new Vector2(138, -100),
		new Vector2(0, -180), new Vector2(-138, -100), new Vector2(-138, 100),
		new Vector2(300, 180), new Vector2(300, 0), new Vector2(300, -180),
		new Vector2(-300, 180), new Vector2(-300, 0), new Vector2(-300, -180),
	};
	public static List<BuildingArea> buildingArea_Obj = new List<BuildingArea>(12);
    //<!-- Private Data Fields.

//    private bool _Clicked = false;
//    private bool _preBuild = false;
    private Vector2 scrollPosition = Vector2.zero;
    private Rect mainGUIRect = new Rect(Main.GAMEWIDTH / 2 - 300, Main.GAMEHEIGHT - 100, 600, 100);
    private Rect windowRect = new Rect(Main.GAMEWIDTH / 2 - 300, Main.GAMEHEIGHT / 2 - 150, 600, 320);
    private Rect imgRect = new Rect(30, 80, 100, 100);
    private Rect contentRect = new Rect(160, 40, 400, 200);
    private Rect buttonRect = new Rect(460, 200, 100, 30);

    public GameObject sawmill_prefab;
    public GameObject millstone_prefab;
    public GameObject smelter_prefab;

    public GameObject market_prefab;

    public GameObject barracks_prefab;
	



	void Awake() {
        this.CreateObjectsPool();
        this.GenerateBackground();
        this.CreateBuildingArea();
	}

    void CreateObjectsPool() {
        OT.PreFabricate("Building_Area", 16);
    }
    void GenerateBackground() {
        // To create the background lets create a filled sprite object
        background = OT.CreateObject(OTObjectType.FilledSprite).GetComponent<OTFilledSprite>();
        // Set the image to our wyrmtale tile
        background.image = mapTex;
        // But this all the way back so all other objects will be located in front.
        background.depth = 10;
        // Set material reference to 'custom' green material - check OT material references
        background.materialReference = "white";
        // Set the size to match the screen resolution.
        background.size = new Vector2(5120, 1536);
        // Set the fill image size to 50 x 50 pixels
        background.fillSize = new Vector2(128, 128);

        background.name = "Background";
    }
    void CreateBuildingArea() {
        var building_area_group = GameObject.Find("Building_Area_Group");

        if (buildingArea_Obj.Count < buildingArea_Obj.Capacity)
        {
            for (int i = 0; i < buildingArea_Obj.Capacity; i++)
            {
                GameObject Temp_obj = OT.CreateObject("Building_Area");
                Temp_obj.transform.parent = building_area_group.transform;

                buildingArea_Obj.Add(Temp_obj.GetComponent<BuildingArea>());
                buildingArea_Obj[i].Sprite.position = buildingArea_Pos[i];
                buildingArea_Obj[i].IndexOfAreaPosition = i;
            }
        }
    }

	// Use this for initialization
    IEnumerator Start()
    {
        StartCoroutine(this.CreateStoreResourceObject());
        StartCoroutine(this.LoadingNumberOfBuildingInstance());
        StartCoroutine(this.LoadingDataStore());

        taskbarManager = this.gameObject.GetComponent<TaskbarManager>();

        yield return null;
    }

    private IEnumerator CreateStoreResourceObject()
    {
        sawmill_prefab = Resources.Load(PathOfEconomyBuilding + "Sawmill", typeof(GameObject)) as GameObject;
        millstone_prefab = Resources.Load(PathOfEconomyBuilding + "MillStone", typeof(GameObject)) as GameObject;
        smelter_prefab = Resources.Load(PathOfEconomyBuilding + "Smelter", typeof(GameObject)) as GameObject;

        market_prefab = Resources.Load(PathOfEconomyBuilding + "Market", typeof(GameObject)) as GameObject;

        barracks_prefab = Resources.Load(PathOfMilitaryBuilding + "Barracks", typeof(GameObject)) as GameObject;

        yield return null;
    }
	
    int amount_Farm_Instance = 0;
    int amount_Sawmill_Instance = 0;
    int amount_MillStone_Instance = 0;
    int amount_Smelter_Instance = 0;
    
	int numberOfStoreHouseInstances = 0;
	int numberOfMarketInstances = 0;
	
	int numberOf_BarracksInstance = 0;

    IEnumerator LoadingNumberOfBuildingInstance() {
        amount_Farm_Instance = PlayerPrefs.GetInt(Mz_SaveData.amount_farm_instance);
        amount_Sawmill_Instance = PlayerPrefs.GetInt(Mz_SaveData.amount_sawmill_instance);
        amount_MillStone_Instance = PlayerPrefs.GetInt(Mz_SaveData.amount_millstone_instance);
        amount_Smelter_Instance = PlayerPrefs.GetInt(Mz_SaveData.amount_smelter_instance);

        numberOfStoreHouseInstances = PlayerPrefs.GetInt(Mz_SaveData.numberOfStorehouseInstance);
        numberOfMarketInstances = PlayerPrefs.GetInt(Mz_SaveData.numberOfMarketInstance);

        numberOf_BarracksInstance = PlayerPrefs.GetInt(Mz_SaveData.numberOf_BarracksInstancs);

        yield return null;
    }
	IEnumerator LoadingDataStore() 
	{
        #region <!-- Farm_Data.

        if (amount_Farm_Instance != 0) 
        {
	        for (int i = 0; i < amount_Farm_Instance; i++) 
            {
				GameObject new_Obj = Instantiate(Resources.Load("Buildings/Economy/Farm", typeof(GameObject))) as GameObject;
				Farm farm = new_Obj.GetComponent<Farm>();	
				
	            int Level = PlayerPrefs.GetInt(Mz_SaveData.farm_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_SaveData.farm_position_ + i);
				
				farm.currentBuildingStatus = Buildings.BuildingStatus.none;				
                farm.Level = Level;	
				farm.IndexOfPosition = Position;
				farm.GetComponent<OTSprite>().position = StageManager.buildingArea_Pos[Position];	
				
				Buildings.Farm_Instance.Add(farm);	
				StageManager.buildingArea_Obj[Position].gameObject.SetActiveRecursively(false);

        		Debug.Log("Loading farm instance.");
	        }
        }

        #endregion
        #region <!-- Sawmill Data.

        if (amount_Sawmill_Instance != 0)
        {
            for (int i = 0; i < amount_Sawmill_Instance; i++)
            {
                int Level = PlayerPrefs.GetInt(Mz_SaveData.sawmill_level_ + i);
                int Position = PlayerPrefs.GetInt(Mz_SaveData.sawmill_position_ + i);

                GameObject new_Sawmill = Instantiate(sawmill_prefab) as GameObject;
                Sawmill sawmill = new_Sawmill.GetComponent<Sawmill>();
                sawmill.InitializeData(Buildings.BuildingStatus.none, Position, Level);

        		Debug.Log("Loading sawmill instance.");
            }
        }

        #endregion
        #region <!-- MillStone Data.

        if (amount_MillStone_Instance != 0)
        {
            for (int i = 0; i < amount_MillStone_Instance; i++)
            {
                int Level = PlayerPrefs.GetInt(Mz_SaveData.millstone_level_ + i);
                int Position = PlayerPrefs.GetInt(Mz_SaveData.millstone_position_ + i);

                GameObject new_millstone = Instantiate(millstone_prefab) as GameObject;
                MillStone millstone = new_millstone.GetComponent<MillStone>();
                millstone.InitializeData(Buildings.BuildingStatus.none, Position, Level);

        		Debug.Log("Loading millstone instance.");
            }
        }

        #endregion
        #region <!-- Smelter Data.

        if (amount_Smelter_Instance != 0)
        {
            for (int i = 0; i < amount_Smelter_Instance; i++)
            {
                int Level = PlayerPrefs.GetInt(Mz_SaveData.smelter_level_ + i);
                int Position = PlayerPrefs.GetInt(Mz_SaveData.smelter_position_ + i);

                GameObject new_smelter = Instantiate(smelter_prefab) as GameObject;
                Smelter smelter = new_smelter.GetComponent<Smelter>();
                smelter.InitializeData(Buildings.BuildingStatus.none, Position, Level);

        		Debug.Log("Loading smelter instance.");
            }
        }

        #endregion

        #region <!-- StoreHouse Data.

        if (numberOfStoreHouseInstances != 0)
        {
            for (int i = 0; i < numberOfStoreHouseInstances; i++)
            {
                int Level = PlayerPrefs.GetInt(Mz_SaveData.storehouse_level_ + i);
                int Position = PlayerPrefs.GetInt(Mz_SaveData.storehouse_position_ + i);
				
                GameObject storehouse_obj = Instantiate(Resources.Load(PathOfEconomyBuilding + "Storehouse", typeof(GameObject))) as GameObject;
                StoreHouse new_storehouse = storehouse_obj.GetComponent<StoreHouse>();

                new_storehouse.currentBuildingStatus = Buildings.BuildingStatus.none;
                new_storehouse.Level = Level;
                new_storehouse.sprite.position = StageManager.buildingArea_Pos[Position];
                new_storehouse.IndexOfPosition = Position;

                Buildings.StoreHouseInstance.Add(new_storehouse);
                StageManager.buildingArea_Obj[Position].gameObject.SetActiveRecursively(false);

        		Debug.Log("Loading storehouse instance.");
            }
        }

        #endregion
		#region <<!-- Market data.
		
		if(numberOfMarketInstances != 0) {
			for (int i = 0; i < numberOfMarketInstances; i++) {
                int position = PlayerPrefs.GetInt(Mz_SaveData.positionOfMarket_ + i);
                int level = PlayerPrefs.GetInt(Mz_SaveData.levelOfMarket_ + i);
				
				GameObject market_Obj = Instantiate(market_prefab) as GameObject;
				MarketBeh marketBehav = market_Obj.GetComponent<MarketBeh>();
                marketBehav.InitializeData(Buildings.BuildingStatus.none, position, level);
			}	
		}
		
		#endregion

        #region <!-- Barracks Data.

        if (numberOf_BarracksInstance != 0) {
            for (int i = 0; i < numberOf_BarracksInstance; i++) { 
				int level = PlayerPrefs.GetInt(Mz_SaveData.barracks_level_ + i);
				int position = PlayerPrefs.GetInt(Mz_SaveData.barracks_position_ + i);
				
                GameObject barracks_obj = Instantiate(barracks_prefab) as GameObject;
				BarracksBeh barracks = barracks_obj.GetComponent<BarracksBeh>();
                barracks.InitializeData(Buildings.BuildingStatus.none, position, level);

                Debug.Log("Loading barracks instance.");
            }
        }

        #endregion

        yield return null;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Mz_SmartDeviceInput.IOS_GUITouch();
        }
    }

    void OnApplicationQuit() {
        Mz_SaveData.Save();
    }
}
