using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour {
	
	public const string PathOfEconomyBuilding = "Buildings/Economy/";
	public const string PathOfUtilityBuilding =  "Buildings/Utility/";
	public const string PathOfMilitaryBuilding = "Buildings/Military/";

    public TaskbarManager taskbarManager;
    public GUISkin mainBuildingSkin;
    /// <summary>
    /// Map and building area.
    /// </summary>
    public Texture2D mapTex;
    private OTFilledSprite background;
	public static List<Vector2> buildingArea_Pos = new List<Vector2>(12)
    {
		new	Vector2(0, 180), new Vector2(138, 100), new Vector2(138, -100),
		new Vector2(0, -180), new Vector2(-138, -100), new Vector2(-138, 100),
		new Vector2(300, 180), new Vector2(300, 0), new Vector2(300, -180),
		new Vector2(-300, 180), new Vector2(-300, 0), new Vector2(-300, -180),
	};
	public static List<BuildingArea> buildingArea_Obj = new List<BuildingArea>(12);

    //<!--- Private Data Fields.
    private Vector2 scrollPosition = Vector2.zero;
    private Rect mainGUIRect = new Rect(Main.GAMEWIDTH / 2 - 300, Main.GAMEHEIGHT - 100, 600, 100);
    private Rect windowRect = new Rect(Main.GAMEWIDTH / 2 - 300, Main.GAMEHEIGHT / 2 - 150, 600, 320);
    private Rect imgRect = new Rect(30, 80, 100, 100);
    private Rect contentRect = new Rect(160, 40, 400, 200);
    private Rect buttonRect = new Rect(460, 200, 100, 30);
	
    /// <summary>
    /// Building prefab objects.
    /// </summary>
    public GameObject house_prefab;

    public GameObject farm_prefab;
    public GameObject sawmill_prefab;
    public GameObject millstone_prefab;
    public GameObject smelter_prefab;
    public GameObject storehouse_prefab;
    public GameObject market_prefab;

    public GameObject barracks_prefab;


    public float gameTime = 0;
    public event System.EventHandler dayCycle;


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
        yield return StartCoroutine(this.CreateResourceBuildingPrefabs());
        StartCoroutine(this.LoadingNumberOfBuildingInstance());
        StartCoroutine(this.LoadingDataStorage());

        taskbarManager = this.gameObject.GetComponent<TaskbarManager>();

        yield return 0;
		
		if(Buildings.StoreHouseInstance.Count == 0)
			dayCycle += StoreHouse.ReachDayCycle;
    }

    private IEnumerator CreateResourceBuildingPrefabs()
    {
        house_prefab = Resources.Load(PathOfUtilityBuilding + "House", typeof(GameObject)) as GameObject;

        farm_prefab = Resources.Load(PathOfEconomyBuilding + "Farm", typeof(GameObject)) as GameObject;
        sawmill_prefab = Resources.Load(PathOfEconomyBuilding + "Sawmill", typeof(GameObject)) as GameObject;
        millstone_prefab = Resources.Load(PathOfEconomyBuilding + "MillStone", typeof(GameObject)) as GameObject;
        smelter_prefab = Resources.Load(PathOfEconomyBuilding + "Smelter", typeof(GameObject)) as GameObject;
        storehouse_prefab = Resources.Load(PathOfEconomyBuilding + "Storehouse", typeof(GameObject)) as GameObject;
        market_prefab = Resources.Load(PathOfEconomyBuilding + "Market", typeof(GameObject)) as GameObject;

        barracks_prefab = Resources.Load(PathOfMilitaryBuilding + "Barracks", typeof(GameObject)) as GameObject;

        yield return 0;
    }

    int numberOfHouse_Instance = 0;

    int amount_Farm_Instance = 0;
    int amount_Sawmill_Instance = 0;
    int amount_MillStone_Instance = 0;
    int amount_Smelter_Instance = 0;    
	int numberOfStoreHouseInstances = 0;
	int numberOfMarketInstances = 0;
	
	int numberOf_BarracksInstance = 0;

    IEnumerator LoadingNumberOfBuildingInstance() {
        //<!--- Utility --->>
        numberOfHouse_Instance = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.numberOfHouse_Instance);
        //<!--- Resource --->>
        amount_Farm_Instance = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.amount_farm_instance);
        amount_Sawmill_Instance = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.amount_sawmill_instance);
        amount_MillStone_Instance = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.amount_millstone_instance);
        amount_Smelter_Instance = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.amount_smelter_instance);
        //<!--- Economy --->>
        numberOfStoreHouseInstances = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.numberOfStorehouseInstance);
        numberOfMarketInstances = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.numberOfMarketInstance);
        //<!--- Millitary --->>
        numberOf_BarracksInstance = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.numberOf_BarracksInstancs);

        yield return 0;
    }
	IEnumerator LoadingDataStorage()
    {
        #region <!--- House instance data.
		
        if (numberOfHouse_Instance != 0) {
            for (int i = 0; i < numberOfHouse_Instance; i++)
            {
                int temp_level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.house_level_ + i);
                int temp_pos = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.house_position_ + i);

                GameObject tempHouse = Instantiate(house_prefab) as GameObject;
                HouseBeh house = tempHouse.GetComponent<HouseBeh>();
                house.InitializeData(Buildings.BuildingStatus.none, temp_pos, temp_level);
            }
        }
		
        #endregion

        #region <!--- Farm_Data.

        if (amount_Farm_Instance != 0) 
        {
	        for (int i = 0; i < amount_Farm_Instance; i++) 
            {
				GameObject new_Obj = Instantiate(Resources.Load("Buildings/Economy/Farm", typeof(GameObject))) as GameObject;
				Farm farm = new_Obj.GetComponent<Farm>();	
				
	            int Level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.farm_level_ + i);
				int Position = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.farm_position_ + i);
				
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
        #region <!--- Sawmill Data.

        if (amount_Sawmill_Instance != 0)
        {
            for (int i = 0; i < amount_Sawmill_Instance; i++)
            {
                int Level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.sawmill_level_ + i);
                int Position = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.sawmill_position_ + i);

                GameObject new_Sawmill = Instantiate(sawmill_prefab) as GameObject;
                Sawmill sawmill = new_Sawmill.GetComponent<Sawmill>();
                sawmill.InitializeData(Buildings.BuildingStatus.none, Position, Level);

        		Debug.Log("Loading sawmill instance.");
            }
        }

        #endregion
        #region <!--- MillStone Data.

        if (amount_MillStone_Instance != 0)
        {
            for (int i = 0; i < amount_MillStone_Instance; i++)
            {
                int Level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.millstone_level_ + i);
                int Position = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.millstone_position_ + i);

                GameObject new_millstone = Instantiate(millstone_prefab) as GameObject;
                MillStone millstone = new_millstone.GetComponent<MillStone>();
                millstone.InitializeData(Buildings.BuildingStatus.none, Position, Level);

        		Debug.Log("Loading millstone instance.");
            }
        }

        #endregion
        #region <!--- Smelter Data.

        if (amount_Smelter_Instance != 0)
        {
            for (int i = 0; i < amount_Smelter_Instance; i++)
            {
                int Level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.smelter_level_ + i);
                int Position = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.smelter_position_ + i);

                GameObject new_smelter = Instantiate(smelter_prefab) as GameObject;
                Smelter smelter = new_smelter.GetComponent<Smelter>();
                smelter.InitializeData(Buildings.BuildingStatus.none, Position, Level);

        		Debug.Log("Loading smelter instance.");
            }
        }

        #endregion

        #region <!--- StoreHouse Data.

        if (numberOfStoreHouseInstances != 0)
        {
            for (int i = 0; i < numberOfStoreHouseInstances; i++)
            {
                int Level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.storehouse_level_ + i);
                int Position = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.storehouse_position_ + i);
				
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
		#region <!--- Market data.
		
		if(numberOfMarketInstances != 0) {
			for (int i = 0; i < numberOfMarketInstances; i++) {
                int position = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.positionOfMarket_ + i);
                int level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.levelOfMarket_ + i);
				
				GameObject market_Obj = Instantiate(market_prefab) as GameObject;
				MarketBeh marketBehav = market_Obj.GetComponent<MarketBeh>();
                marketBehav.InitializeData(Buildings.BuildingStatus.none, position, level);
			}	
		}
		
		#endregion

        #region <!--- Barracks Data.
        if (numberOf_BarracksInstance != 0) {
            for (int i = 0; i < numberOf_BarracksInstance; i++) 
            {
                int level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.barracks_level_ + i);
                int position = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.barracks_position_ + i);
				
                GameObject barracks_obj = Instantiate(barracks_prefab) as GameObject;
				BarracksBeh barracks = barracks_obj.GetComponent<BarracksBeh>();
                barracks.InitializeData(Buildings.BuildingStatus.none, position, level);
            }
        }
        #endregion

        yield return 0;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Mz_SmartDeviceInput.IOS_GUITouch();

            //<!--- Check escape key down.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
        }

        gameTime += Time.deltaTime;
        if (gameTime >= 10) {
            gameTime = 0;
            if (dayCycle != null)
                dayCycle(this, System.EventArgs.Empty);
        }
    }

    void OnApplicationQuit() {
        Mz_SaveData.Save();
        Debug.LogWarning("Saving complete...");
    }

    void OnApplicationPause()
    {
        Mz_SaveData.Save();
        Debug.LogWarning("Saving complete...");
    }
}
