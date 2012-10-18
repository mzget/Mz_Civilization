using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : Mz_BaseScene {
	
	public const string PathOfEconomyBuilding = "Buildings/Economy/";
	public const string PathOfUtilityBuilding =  "Buildings/Utility/";
	public const string PathOfMilitaryBuilding = "Buildings/Military/";

    public GUI_Manager gui_Manager;
    public GUISkin mainBuildingSkin;
    // Map and building area.
    public Texture2D mapTex;
    private OTFilledSprite background;
	public static List<Vector2> buildingArea_Pos = new List<Vector2>(24) {
		new	Vector2(0, 180), 
        new Vector2(138, 100),
        new Vector2(138, -100),
		new Vector2(0, -180),
        new Vector2(-138, -100), 
        new Vector2(-138, 100),
        new Vector2(280, 0), 
        new Vector2(-260, 0),
		new Vector2(-260, 180), 
        new Vector2(-260, -180),
        new Vector2(0, 360),
        new Vector2(-138, 260), 
        new Vector2(138, 260),
        new Vector2(0, -360),
        new Vector2(-138, -260),
        new Vector2(138, -260),
		new Vector2(-380, 90), 
        new Vector2(-380, -90),
        new Vector2(400, 90),
		new Vector2(280, 180),
        new Vector2(280, -180),
	};
	public static List<BuildingArea> buildingArea_Obj = new List<BuildingArea>(24);    
    //<!--- Private Data Fields.
    private Vector2 scrollPosition = Vector2.zero;
    private Rect mainGUIRect = new Rect(Main.GAMEWIDTH / 2 - 300, Main.GAMEHEIGHT - 100, 600, 100);
    private Rect windowRect = new Rect(Main.GAMEWIDTH / 2 - 300, Main.GAMEHEIGHT / 2 - 150, 600, 320);
    private Rect imgRect = new Rect(30, 80, 100, 100);
    private Rect contentRect = new Rect(160, 40, 400, 200);
    private Rect buttonRect = new Rect(460, 200, 100, 30);
	
    // Building prefab objects.
    public GameObject house_prefab;
    public GameObject academy_prefab;

    public GameObject farm_prefab;
    public GameObject sawmill_prefab;
    public GameObject millstone_prefab;
    public GameObject smelter_prefab;
    public GameObject storehouse_prefab;
    public GameObject market_prefab;

    public GameObject barracks_prefab;

    public float dayTime = 0;
    public float resourceCycleTime = 0;
    public event System.EventHandler dayCycle_Event;
    public event System.EventHandler resourceCycle_Event;
    private bool _hasQuitCommand = false;


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
        background.size = new Vector2(2560, 1536);
        // Set the fill image size to 50 x 50 pixels
        background.fillSize = new Vector2(128, 128);

        background.name = "Background";
    }
    void CreateBuildingArea() {
        var building_area_group = GameObject.Find("Building_Area_Group");

        for (int i = 0; i < 8; i++) {
            GameObject Temp_obj = OT.CreateObject("Building_Area");
            Temp_obj.transform.parent = building_area_group.transform;

            buildingArea_Obj.Add(Temp_obj.GetComponent<BuildingArea>());
            buildingArea_Obj[i].Sprite.position = buildingArea_Pos[i];
            buildingArea_Obj[i].Sprite.size = new Vector2(128, 128);
            buildingArea_Obj[i].IndexOfAreaPosition = i;
            buildingArea_Obj[i].Sprite.rotation = 45f;
            buildingArea_Obj[i].areaState = BuildingArea.AreaState.In_Active;
        }
		
		for (int i = 8; i < buildingArea_Pos.Count; i++) {
            GameObject Temp_obj = OT.CreateObject("Building_Area");
			Temp_obj.transform.parent = building_area_group.transform;

            buildingArea_Obj.Add(Temp_obj.GetComponent<BuildingArea>());
            buildingArea_Obj[i].Sprite.position = buildingArea_Pos[i];
            buildingArea_Obj[i].Sprite.size = new Vector2(128, 128);
            buildingArea_Obj[i].IndexOfAreaPosition = i;
            buildingArea_Obj[i].Sprite.rotation = 45f;
			
			int state = PlayerPrefs.GetInt(Mz_SaveData.BuildingAreaState + i);
			if(state == 0)
				buildingArea_Obj[i].areaState = BuildingArea.AreaState.De_Active;
			else if(state == 1)
				buildingArea_Obj[i].areaState = BuildingArea.AreaState.In_Active;
		}
    }

	// Use this for initialization
    IEnumerator Start()
    {
        this.CreateBuildingPrefabsFromResource();
        this.LoadingAmountOfBuildingInstance();
        StartCoroutine(this.LoadingDataStorage());

        gui_Manager = this.gameObject.GetComponent<GUI_Manager>();

        yield return 0;
		
        //if(Buildings.StoreHouseInstance.Count == 0)
			dayCycle_Event += StoreHouse.ReachDayCycle;
		
        if (BuildingBeh.House_Instances.Count == 0)
			HouseBeh.CalculationSumOfPopulation();
    }

    private void CreateBuildingPrefabsFromResource()
    {
        //<!--- Utility.
        house_prefab = Resources.Load(PathOfUtilityBuilding + "House", typeof(GameObject)) as GameObject;
        academy_prefab = Resources.Load(PathOfUtilityBuilding + "Academy", typeof(GameObject)) as GameObject;
        //<!--- Economy.
        farm_prefab = Resources.Load(PathOfEconomyBuilding + "Farm", typeof(GameObject)) as GameObject;
        sawmill_prefab = Resources.Load(PathOfEconomyBuilding + "Sawmill", typeof(GameObject)) as GameObject;
        millstone_prefab = Resources.Load(PathOfEconomyBuilding + "MillStone", typeof(GameObject)) as GameObject;
        smelter_prefab = Resources.Load(PathOfEconomyBuilding + "Smelter", typeof(GameObject)) as GameObject;
        storehouse_prefab = Resources.Load(PathOfEconomyBuilding + "Storehouse", typeof(GameObject)) as GameObject;
        market_prefab = Resources.Load(PathOfEconomyBuilding + "Market", typeof(GameObject)) as GameObject;
        //<!--- Military.
        barracks_prefab = Resources.Load(PathOfMilitaryBuilding + "Barracks", typeof(GameObject)) as GameObject;
    }

    int numberOfHouse_Instance = 0;

    int amount_Farm_Instance = 0;
    int amount_Sawmill_Instance = 0;
    int amount_MillStone_Instance = 0;
    int amount_Smelter_Instance = 0;    
	int numberOfStoreHouseInstances = 0;
	int numberOfMarketInstances = 0;
	
	int numberOf_BarracksInstance = 0;

    private void LoadingAmountOfBuildingInstance() {
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
    }
	IEnumerator LoadingDataStorage()
    {
		//<!--- Load level of towncenter.
		BuildingBeh.TownCenter.Level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.TownCenter_level);
		
        #region <!--- House instance data.
		
        if (numberOfHouse_Instance != 0) {
            for (int i = 0; i < numberOfHouse_Instance; i++)
            {
                int temp_level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.house_level_ + i);
                int temp_pos = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.house_position_ + i);

                GameObject tempHouse = Instantiate(house_prefab) as GameObject;
                HouseBeh house = tempHouse.GetComponent<HouseBeh>();
                house.InitializeBuildingBeh(BuildingBeh.BuildingStatus.none, temp_pos, temp_level);
            }
        }
		
        #endregion

        #region <!--- Farm_Data.

        if (amount_Farm_Instance != 0) 
        {
	        for (int i = 0; i < amount_Farm_Instance; i++) 
            {
	            int Level = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.farm_level_ + i);
				int Position = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.farm_position_ + i);

                GameObject temp_farm = Instantiate(farm_prefab) as GameObject;
				Farm farm = temp_farm.GetComponent<Farm>();
                farm.InitializeBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);

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
                sawmill.InitializeBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);

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
                millstone.InitializeBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);

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
                smelter.InitializeBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);

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
				
                GameObject temp_storehouse = Instantiate(storehouse_prefab) as GameObject;
                StoreHouse new_storehouse = temp_storehouse.GetComponent<StoreHouse>();
                new_storehouse.InitializeBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);

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
				MarketBeh marketBeh = market_Obj.GetComponent<MarketBeh>();
                marketBeh.InitializeBuildingBeh(BuildingBeh.BuildingStatus.none, position, level);
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
                barracks.InitializeBuildingBeh(BuildingBeh.BuildingStatus.none, position, level);
            }
        }
        #endregion

        yield return 0;
    }
	
	// Update is called once per frame
    void Update()
    {
#if UNITY_IPHONE || UNITY_ANDROID

            Mz_SmartDeviceInput.IOS_GUITouch();

            //<!--- Check escape key down.
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu)) {
                Mz_SaveData.Save();
                _hasQuitCommand = true;
                return;
            }

#endif
		
        dayTime += Time.deltaTime;
        if (dayTime >= 60) {
            dayTime = 0;
            if (dayCycle_Event != null)
                dayCycle_Event(this, System.EventArgs.Empty);
        }

        resourceCycleTime += Time.deltaTime;
        if(resourceCycleTime >= 10) {
            resourceCycleTime = 0;
            if (resourceCycle_Event != null)
                resourceCycle_Event(this, System.EventArgs.Empty);
        }
    }

    void OnGUI() {
		GUI.depth = 0;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));

        if(_hasQuitCommand) {
            GUI.BeginGroup(new Rect(Main.GAMEWIDTH / 2 - 150, Main.GAMEHEIGHT / 2 - 100, 300, 200), "Do you want to quit ?", GUI.skin.window);
            {
                if (GUI.Button(new Rect(40, 120, 100, 32), "Yes"))
                    Application.Quit();
                else if (GUI.Button(new Rect(160, 120, 100, 32), "No"))
                    _hasQuitCommand = false;
            }
            GUI.EndGroup();
        }
    }

    void OnApplicationQuit() {
        Mz_SaveData.Save();
    }

    void OnApplicationPause()
    {
        Mz_SaveData.Save();
    }
}
