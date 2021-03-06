using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class CapitalCity : Mz_BaseScene {
	
	public const string PrototypeObjects_ResourcePath = "Prototypes/";
	public const string PathOfEconomyBuilding = "Buildings/Economy/";
	public const string PathOfUtilityBuilding =  "Buildings/Utility/";
	public const string PathOfMilitaryBuilding = "Buildings/Military/";

    public GUISkin mainBuildingSkin;
    public TaskManager taskManager;

	public GameObject terrainManager_prefab;
    private TerrainManager terrainManager;
    public IsometricEngine isomatricEngine;
	public Mz_SaveData saveManager;
    public List<GameMaterial> gameMaterials = new List<GameMaterial>();
    // Map and building area.
    public static bool[] arr_buildingAreaState = new bool[24];

    //<!--- Private Data Fields.
//    private Vector2 scrollPosition = Vector2.zero;
//    private Rect mainGUIRect = new Rect(Main.FixedGameWidth / 2 - 300, Main.FixedGameHeight - 100, 600, 100);
//    private Rect windowRect = new Rect(Main.FixedGameWidth / 2 - 300, Main.FixedGameHeight / 2 - 150, 600, 320);
//    private Rect imgRect = new Rect(30, 80, 100, 100);
//    private Rect contentRect = new Rect(160, 40, 400, 200);
//    private Rect buttonRect = new Rect(460, 200, 100, 30);

    internal BuildingBeh buildingBeh;

    #region <!-- Building prefab objects.
    
    public GameObject house_prefab;
    public GameObject academy_prefab;

    public GameObject farm_prefab;
    public GameObject sawmill_prefab;
    public GameObject StoneCrushingPlant_prefab;
    public GameObject smelter_prefab;
    public GameObject storehouse_prefab;
    public GameObject market_prefab;

    public GameObject barracks_prefab;

    #endregion

    int numberOfHouse_Instance = 0;
	bool academyInstance = false;
    int amount_Farm_Instance = 0;
    int amount_Sawmill_Instance = 0;
    int amount_MillStone_Instance = 0;
    int amount_Smelter_Instance = 0;   
	int numberOfStoreHouseInstances = 0;
	bool marketInstance = false;
	bool barracksInstance = false;

    public float dayTime = 0;
    public float resourceCycleTime = 0;
    public event System.EventHandler dayCycle_Event;
    public event System.EventHandler resourceCycle_Event;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        StartCoroutine(this.Initializing());
    }

	private IEnumerator Initializing ()
	{		
        taskManager = this.gameObject.GetComponent<TaskManager>();
        saveManager = new Mz_SaveData();

        yield return StartCoroutine(this.InitializeIsoMetricEngine());

        //this.GenerateBackground();
        //this.CreateBuildingArea();
		this.initCenterCamera();
		this.PrepareBuildingPrefabsFromResource();
		this.Load_AmountOfBuildingInstance();
		this.LoadingDataStorage();		

        yield return null;

        StartCoroutine(this.InitializeAudio());
        StartCoroutine(this.CreateGameMaterials());
        StartCoroutine(this.InitializeAICities());
		
		if (BuildingBeh.StoreHouseInstance.Count == 0) {
			StoreHouse.CalculationSumOfMaxCapacity();
			dayCycle_Event += StoreHouse.ReachDayCycle;
		}
		
		if (BuildingBeh.House_Instances.Count == 0)
			HouseBeh.CalculationSumOfPopulation();

        if (BuildingBeh.TownCenter != null)
            buildingBeh = BuildingBeh.TownCenter;
        else
            Debug.LogError("BuildingBeh.TownCenter == null");
		
#if UNITY_WEBPLAYER || UNITY_EDITOR
		StartCoroutine(InitializeJoystick());
#endif

        //<@-- Admob integration.
#if UNITY_ANDROID

		AdBannerObserver.Initialize("a150c2e14a5d753", "DIAT-GE5P-2J5H-2", 30f);

#endif

        _StageInitialized = true;
    }

	IEnumerator InitializeIsoMetricEngine ()
	{
		isomatricEngine = this.GetComponent<IsometricEngine>();
		isomatricEngine.sceneController = this;
		yield return StartCoroutine(isomatricEngine.CreateTilemap());
		this.InitailizeTerrainManager();
	}
	
	void initCenterCamera() {	
		Camera.main.transform.position = new Vector3(isomatricEngine.map_width/2, 0, Camera.main.transform.position.z);
	}
	
	private void InitailizeTerrainManager ()
	{
		GameObject terrainManager_obj = Instantiate(terrainManager_prefab) as GameObject;
		terrainManager = terrainManager_obj.GetComponent<TerrainManager>();
	}

	private new IEnumerator InitializeAudio ()
	{
		base.InitializeAudio();

        base.audioBackground_Obj.audio.clip = base.background_clip;
        base.audioBackground_Obj.audio.loop = false;
        audioBackground_Obj.audio.Play();

		yield return null;
	}

    private IEnumerator CreateGameMaterials()
    {
        gameMaterials.Add(new GameMaterial() { name = "food" });	/// 0.
        gameMaterials.Add(new GameMaterial() { name = "wood" });	// 1.
        gameMaterials.Add(new GameMaterial() { name = "stone" });	// 2.
        gameMaterials.Add(new GameMaterial() { name = "copper" });	// 3.
        gameMaterials.Add(new GameMaterial() { name = "armor" });	// 4.
        gameMaterials.Add(new GameMaterial() { name = "weapon" });	// 5.

        yield return 0;
    }
    
    public static List<AICities> list_AICity;
    private IEnumerator InitializeAICities() {
        taskManager.GreekIcon_Texture = Resources.Load(TaskManager.PathOfTribes_Texture + "Greek", typeof(Texture2D)) as Texture2D;
        taskManager.PersianIcon_Texture = Resources.Load(TaskManager.PathOfTribes_Texture + "Persian", typeof(Texture2D)) as Texture2D;

        GreekBeh greek = new GreekBeh();
        greek.name = GreekBeh.NAME;
        greek.symbols = taskManager.GreekIcon_Texture;
        greek.tribe = AICities.Tribes.Greek;
        greek.distance = 30;
        greek.defenseBonus = 30;
        greek.attackBonus = 20;
		GreekBeh.Spearman_unit = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_SPEARMAN);
		GreekBeh.Hapaspist_unit = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HAPASPIST);
		GreekBeh.Hoplite_unit = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HOPLITE);
        
		list_AICity = new List<AICities>();
        list_AICity.Add(greek);
        list_AICity.Add(new AICities() { name = "Egyptian", symbols = taskManager.EgyptianIcon_Texture, tribe = AICities.Tribes.Egyptian, });
        list_AICity.Add(new AICities() { name = "Persian", symbols = taskManager.PersianIcon_Texture, tribe = AICities.Tribes.Persian, });
        list_AICity.Add(new AICities() { name = "Celtic", symbols = taskManager.CelticIcon_Texture, tribe = AICities.Tribes.Celtic, });

        yield return 0;
    }

    public GameObject joystick_base_obj;
    public GameObject joystick_obj;
	public JoystickManager joystickManager;
	private float moveCamSpeed;
    private IEnumerator InitializeJoystick() {
        joystick_base_obj = Instantiate(Resources.Load(ResourcePathName.PathOfGUI_PREFABS + "GUI_Joystickbase", typeof(GameObject))) as GameObject;
        joystick_obj = Instantiate(Resources.Load(ResourcePathName.PathOfGUI_PREFABS + "GUI_Joystick", typeof(GameObject))) as GameObject;
		
        yield return 0;
    }
	
/*	
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
*/	
	
/*
    void CreateBuildingArea()
    {
        GameObject building_area_group = GameObject.Find("Building_Area_Group");
        arr_buildingAreaState = PlayerPrefsX.GetBoolArray(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_BuildingAreaState, false, 24);

        for (int i = 0; i < 8; i++) {
//			GameObject Temp_obj = OT.CreateObject("Building_Area");
			GameObject Temp_obj = Instantiate(Resources.Load("Prototypes/BuildingArea", typeof(GameObject))) as GameObject;
            Temp_obj.transform.parent = building_area_group.transform;
			Temp_obj.transform.position = buildingArea_Pos[i];

            buildingArea_Objs.Add(Temp_obj.GetComponent<BuildingArea>());
//            buildingArea_Objs[i].Sprite.position = buildingArea_Pos[i];
            buildingArea_Objs[i].IndexOfAreaPosition = i;
            buildingArea_Objs[i].areaActiveState = true;
        }

        for (int i = 8; i < buildingArea_Pos.Count; i++) {
//			GameObject Temp_obj = OT.CreateObject("Building_Area");
			GameObject Temp_obj = Instantiate(Resources.Load("Prototypes/BuildingArea", typeof(GameObject))) as GameObject;
            Temp_obj.transform.parent = building_area_group.transform;
			Temp_obj.transform.position = buildingArea_Pos[i];

            buildingArea_Objs.Add(Temp_obj.GetComponent<BuildingArea>());
//            buildingArea_Objs[i].Sprite.position = buildingArea_Pos[i];
            buildingArea_Objs[i].IndexOfAreaPosition = i;
            buildingArea_Objs[i].areaActiveState = arr_buildingAreaState[i];
        }
    }
*/
	
    void PrepareBuildingPrefabsFromResource()
    {
        //<!--- Utility.
        house_prefab = Resources.Load(PathOfUtilityBuilding + "House", typeof(GameObject)) as GameObject;
        academy_prefab = Resources.Load(PathOfUtilityBuilding + "Academy", typeof(GameObject)) as GameObject;
        
        //<!--- Economy.
        farm_prefab = Resources.Load(PathOfEconomyBuilding + "Farm", typeof(GameObject)) as GameObject;
        sawmill_prefab = Resources.Load(PathOfEconomyBuilding + "Sawmill", typeof(GameObject)) as GameObject;
        StoneCrushingPlant_prefab = Resources.Load(PathOfEconomyBuilding + "StoneCrushingPlant", typeof(GameObject)) as GameObject;
        smelter_prefab = Resources.Load(PathOfEconomyBuilding + "Smelter", typeof(GameObject)) as GameObject;
        storehouse_prefab = Resources.Load(PathOfEconomyBuilding + "Storehouse", typeof(GameObject)) as GameObject;
        market_prefab = Resources.Load(PathOfEconomyBuilding + "Market", typeof(GameObject)) as GameObject;

        //<!--- Military.
        barracks_prefab = Resources.Load(PathOfMilitaryBuilding + "Barracks", typeof(GameObject)) as GameObject;
    }

    void Load_AmountOfBuildingInstance() {
        //<!-- Utility --->>
		numberOfHouse_Instance = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.numberOfHouse_Instance);
		academyInstance = PlayerPrefsX.GetBool(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_AcademyInstance);
		//<!-- Resource --->>
		amount_Farm_Instance = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.amount_farm_instance);
		amount_Sawmill_Instance = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.amount_sawmill_instance);
		amount_MillStone_Instance = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_AMOUNT_OF_StoneCrushingPlant);
		amount_Smelter_Instance = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_AMOUNT_OF_SMELTER);
        //<!-- Economy --->>
		numberOfStoreHouseInstances = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.numberOfStorehouseInstance);
		marketInstance = PlayerPrefsX.GetBool(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_MarketInstance);
        //<!-- Millitary --->>
		barracksInstance = PlayerPrefsX.GetBool(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_BarracksInstance);
    }

	void LoadingDataStorage()
    {
		//<!--- Load level of towncenter.
		BuildingBeh.TownCenter.Level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.TownCenter_level, 0);
	
        #region <!--- House instance data.

        Debug.Log("numberOfHouse_Instance : " + numberOfHouse_Instance);
        if (numberOfHouse_Instance != 0) {
            for (int i = 0; i < numberOfHouse_Instance; i++)
            {
				int temp_level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.house_level_ + i);
                string temp_areaData = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_HOUSE_AREA_ + i);

                // Turn the JSON into C# objects
                TileArea area = JsonReader.Deserialize<TileArea>(temp_areaData);

                GameObject tempHouse = Instantiate(house_prefab) as GameObject;
                HouseBeh house = tempHouse.GetComponent<HouseBeh>();
                house.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, temp_level);
            }
        }
		
        #endregion

		#region <!--- Academy.

		if(academyInstance) {
            int level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_AcademyLevel);
            string tempArea = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_AcademyPosition);

            // Turn the JSON into C# objects
            TileArea area = JsonReader.Deserialize<TileArea>(tempArea);
			
			GameObject academy = Instantiate(academy_prefab) as GameObject;
			AcademyBeh academyBeh = academy.GetComponent<AcademyBeh>();
        	academyBeh.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, level);
		}
		
		#endregion

        #region <!--- Farm_Data.
        
        if (amount_Farm_Instance != 0) {
	        for (int i = 0; i < amount_Farm_Instance; i++) {
				int Level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.farm_level_ + i);
                string tempArea = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_FARM_AREA_ + i);

                // Turn the JSON into C# objects
                TileArea area = JsonReader.Deserialize<TileArea>(tempArea);

                GameObject temp_farm = Instantiate(farm_prefab) as GameObject;
				Farm farm = temp_farm.GetComponent<Farm>();
                farm.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, Level);
	        }
        }

        #endregion
		
		#region <!--- Sawmill Data.
		
		if (amount_Sawmill_Instance != 0) {
			for (int i = 0; i < amount_Sawmill_Instance; i++) {
				int Level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.sawmill_level_ + i);
				string tempArea = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_SAWMILL_AREA_ + i);
				
				// Turn the JSON into C# objects.
				TileArea area = JsonReader.Deserialize<TileArea>(tempArea);
				
				GameObject temp_Sawmill = Instantiate(sawmill_prefab) as GameObject;
				Sawmill sawmill = temp_Sawmill.GetComponent<Sawmill>();
				sawmill.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, Level);
			}
		}
		
        #endregion

        #region <!-- StoneCrushingPlant Data.

        if (amount_MillStone_Instance != 0) {
            for (int i = 0; i < amount_MillStone_Instance; i++) {
                int Level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_StoneCrushingPlant_LEVEL_ + i);
                string tempArea = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_StoneCrushingPlant_AREA_ + i);

                // Turn the JSON into C# objects.
                TileArea area = JsonReader.Deserialize<TileArea>(tempArea);

                GameObject new_stoneCrushing = Instantiate(StoneCrushingPlant_prefab) as GameObject;
                StoneCrushingPlant stoneCrushingPlant = new_stoneCrushing.GetComponent<StoneCrushingPlant>();
                stoneCrushingPlant.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, Level);
            }
        }

        #endregion

        #region <!--- Smelter Data.

        if (amount_Smelter_Instance != 0)
        {
            for (int i = 0; i < amount_Smelter_Instance; i++)
            {
                int Level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_SMELTER_LEVEL_ + i);
                string tempArea = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_SMELTER_AREA_ + i);

                // Turn the JSON into C# objects.
                TileArea area = JsonReader.Deserialize<TileArea>(tempArea);

                GameObject new_smelter = Instantiate(smelter_prefab) as GameObject;
                Smelter smelter = new_smelter.GetComponent<Smelter>();
                smelter.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, Level);
            }
        }

        #endregion

        #region <!-- StoreHouse Data.

        if (numberOfStoreHouseInstances != 0) {
            for (int i = 0; i < numberOfStoreHouseInstances; i++) {
                int Level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_STOREHOUSE_LEVEL_ + i);
                string tempArea = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_STOREHOUSE_AREA_ + i);

                // Turn the JSON into C# objects
                TileArea area = JsonReader.Deserialize<TileArea>(tempArea);

                GameObject temp_storehouse = Instantiate(storehouse_prefab) as GameObject;
                StoreHouse storehouse = temp_storehouse.GetComponent<StoreHouse>();
                storehouse.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, Level);
            }
        }

        #endregion

        #region <!-- Market data.

        if (marketInstance) {
            int level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_Market_Level);
            string tempArea = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_Market_Area);

            // Turn the JSON into C# objects
            TileArea area = JsonReader.Deserialize<TileArea>(tempArea);

            GameObject new_market = Instantiate(market_prefab) as GameObject;
            MarketBeh marketBeh = new_market.GetComponent<MarketBeh>();
            marketBeh.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, level);
        }

        #endregion

		#region <!-- Barracks Data.

		Debug.Log("barracksInstance : " + barracksInstance);
		if (barracksInstance) {
			int level = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_BarracksLevel);
			string loadArea = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + Mz_SaveData.KEY_BarracksPosition);

			// Turn the JSON into C# objects
			TileArea area = JsonReader.Deserialize<TileArea>(loadArea);

			GameObject barracks_obj = Instantiate(barracks_prefab) as GameObject;
			BarracksBeh barracks = barracks_obj.GetComponent<BarracksBeh>();
			barracks.InitializingBuildingBeh(BuildingBeh.BuildingStatus.idle, area, level);
		}
        
        #endregion

        Debug.Log("StageManager.LoadingDataStorage");
    }

	internal BuildingBeh currentConstructionCommand;

	internal void CreateBuildingOnBuildingArea (string buildingName, Vector3 position, TileArea P_constructionArea)
	{
		print("CreateBuildingOnBuildingArea()");
		
		if(currentConstructionCommand != null)
			return;

		#region <@-- Utility buildings section.

		if(buildingName == UtilityIconData.HOUSE_ICON_NAME) {			
            bool _canCreateBuilding = (BuildingBeh.CheckingOnBuildingList() && 
                buildingBeh.CheckingEnoughUpgradeResource(HouseBeh.RequireResource[0])) ? true:false;
		
            if(_canCreateBuilding) {
                GameObject temp_House = Instantiate(house_prefab, position, Quaternion.identity) as GameObject;
                HouseBeh housebeh = temp_House.GetComponent<HouseBeh>();
				currentConstructionCommand = housebeh as BuildingBeh;
				taskManager.CreateConfirmationWindows(position);
				
				housebeh._canMovable = true;
				housebeh.constructionArea = P_constructionArea;
				housebeh.CreateConstructionEvent += (sender, e) => {
					//<!-- Remove used resource.
					GameMaterialDatabase.UsedResource(HouseBeh.RequireResource[0]);	
					//<!-- Initialize building.
					housebeh.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, P_constructionArea , 0);
					housebeh.OnBuildingProcess(housebeh);	
					housebeh._canMovable = false;
					currentConstructionCommand = null;
				};
				housebeh.destroyObj_Event += (object sender, System.EventArgs e) => {
					Tile.SetEmptyArea(housebeh.constructionArea);
					Destroy(temp_House.gameObject);
				};
            }
            else
            {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
		}
		else if(buildingName == UtilityIconData.ACADEMY_ICON_NAME) {   
			bool _canCreateBuilding = (BuildingBeh.CheckingOnBuildingList()
            && buildingBeh.CheckingEnoughUpgradeResource(AcademyBeh.RequireResource[0])
            && BuildingBeh.TownCenter.Level >= 5
            && BuildingBeh.AcademyInstance == null) ? true : false;
		
			if(_canCreateBuilding) {
                GameMaterialDatabase.UsedResource(AcademyBeh.RequireResource[0]);

                GameObject temp_Academy = Instantiate(academy_prefab, position, Quaternion.identity) as GameObject;
                AcademyBeh academy = temp_Academy.GetComponent<AcademyBeh>();
                academy.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, P_constructionArea, 0);
                academy.OnBuildingProcess(academy);
            }
            else
            {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
		}

		#endregion
		
		#region <@-- Economy building section..
		
		else if(buildingName == EconomyIconData.FARM_ICON_NAME) {	        
			bool _canCreateBuilding = (BuildingBeh.CheckingOnBuildingList() &&
    		buildingBeh.CheckingEnoughUpgradeResource(Farm.RequireResource[0])) ? true : false;

	        if (_canCreateBuilding) {			
				GameObject temp = Instantiate(farm_prefab, position, Quaternion.identity) as GameObject;					
				Farm farm = temp.GetComponent<Farm>();
				currentConstructionCommand = farm as BuildingBeh;
				taskManager.CreateConfirmationWindows(position);	

				farm._canMovable = true;
				farm.constructionArea = P_constructionArea;
				farm.CreateConstructionEvent += (sender, e) => {
					//<!-- Remove used resource.
					GameMaterialDatabase.UsedResource(Farm.RequireResource[0]);		
					//<!-- Initialize building.
				    farm.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, farm.constructionArea, 0);
				    farm.OnBuildingProcess(farm);
					farm._canMovable = false;
					currentConstructionCommand = null;
				};
				farm.destroyObj_Event += (object sender, System.EventArgs e) => {
					Tile.SetEmptyArea(farm.constructionArea);
					Destroy(temp.gameObject);
				};
            }
            else
            {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
		}
		else if(buildingName == EconomyIconData.SAWMILL_ICON_NAME) {
			bool _canCreateBuilding = (BuildingBeh.CheckingOnBuildingList() &&
				buildingBeh.CheckingEnoughUpgradeResource(Sawmill.RequireResource[0])) ? true : false;

			if(_canCreateBuilding) {
	            GameObject new_sawmill = Instantiate(sawmill_prefab, position, Quaternion.identity) as GameObject;
	            Sawmill sawmill = new_sawmill.GetComponent<Sawmill>();
				currentConstructionCommand = sawmill;
				taskManager.CreateConfirmationWindows(position);

				sawmill._canMovable = true;
				sawmill.constructionArea = P_constructionArea;
				sawmill.CreateConstructionEvent += (sender, e) => {
					//<!-- Remove used resource.
					GameMaterialDatabase.UsedResource(Sawmill.RequireResource[0]);
					//<!-- Initialize building.
					sawmill.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, P_constructionArea, 0);
					sawmill.OnBuildingProcess(sawmill);
					sawmill._canMovable = false;
					currentConstructionCommand = null;
				};
				sawmill.destroyObj_Event += (object sender, System.EventArgs e) => {
					Tile.SetEmptyArea(sawmill.constructionArea);
					Destroy(new_sawmill.gameObject);
				};
            }
            else
            {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
		}
		else if(buildingName == EconomyIconData.STONECRUSHINGPLANT_ICON_NAME) {
            bool _canCreateBuilding = (BuildingBeh.CheckingOnBuildingList() &&
                buildingBeh.CheckingEnoughUpgradeResource(StoneCrushingPlant.RequireResource[0])) ? true : false;

            /// Have to research stonecutter before building.
            if(_canCreateBuilding) {
                GameObject new_stoneCrushing = Instantiate(this.StoneCrushingPlant_prefab, position, Quaternion.identity) as GameObject;
                StoneCrushingPlant stoneCrushing = new_stoneCrushing.GetComponent<StoneCrushingPlant>();
				currentConstructionCommand = stoneCrushing;
				taskManager.CreateConfirmationWindows(position);
				
				stoneCrushing._canMovable = true;
				stoneCrushing.constructionArea = P_constructionArea;
				stoneCrushing.CreateConstructionEvent += (sender, e) => {
					//<!-- Remove used resource.
					GameMaterialDatabase.UsedResource(StoneCrushingPlant.RequireResource[0]);
					//<!-- Initialize building.
					stoneCrushing.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, P_constructionArea, 0);
					stoneCrushing.OnBuildingProcess(stoneCrushing);
					stoneCrushing._canMovable = false;
					currentConstructionCommand = null;
				};
				stoneCrushing.destroyObj_Event += (object sender, System.EventArgs e) => {
					Tile.SetEmptyArea(stoneCrushing.constructionArea);
					Destroy(new_stoneCrushing.gameObject);
				};
            }
            else
            {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
		}
		else if(buildingName == EconomyIconData.SMELTER_ICON_NAME) {
            bool _canCreateBuilding = (BuildingBeh.CheckingOnBuildingList() &&
            buildingBeh.CheckingEnoughUpgradeResource(Smelter.RequireResource[0]) &&
            BuildingBeh.TownCenter.Level >= 5) ? true : false;

            if (_canCreateBuilding) {
                GameObject new_smelter = Instantiate(this.smelter_prefab, position, Quaternion.identity) as GameObject;
                Smelter smelter = new_smelter.GetComponent<Smelter>();
				currentConstructionCommand = smelter as BuildingBeh;
				taskManager.CreateConfirmationWindows(position);
				
				smelter._canMovable = true;
				smelter.constructionArea = P_constructionArea;
				smelter.CreateConstructionEvent += (sender, e) => {
					//<!-- Remove used resource.
					GameMaterialDatabase.UsedResource(Smelter.RequireResource[0]);
					//<!-- Initialize building.
					smelter.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, P_constructionArea, 0);
					smelter.OnBuildingProcess(smelter);
					smelter._canMovable = false;
					currentConstructionCommand = null;
				};
				smelter.destroyObj_Event += (object sender, System.EventArgs e) => {
					Tile.SetEmptyArea(smelter.constructionArea);
					Destroy(new_smelter.gameObject);
				};
            }
            else {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
		}
		else if(buildingName == EconomyIconData.STOREHOUSE_ICON_NAME) {
            bool _canCreateBuilding = (BuildingBeh.CheckingOnBuildingList() &&
                buildingBeh.CheckingEnoughUpgradeResource(StoreHouse.RequireResource[0])) ? true : false;
            
            if(_canCreateBuilding) {
                GameObject new_storehouse = Instantiate(this.storehouse_prefab, position, Quaternion.identity) as GameObject;
                StoreHouse storeHouse = new_storehouse.GetComponent<StoreHouse>();
				currentConstructionCommand = storeHouse as BuildingBeh;
				taskManager.CreateConfirmationWindows(position);
				
				storeHouse._canMovable = true;
				storeHouse.constructionArea = P_constructionArea;
				storeHouse.CreateConstructionEvent += (sender, e) => {
					//<!-- Remove used resource.
					GameMaterialDatabase.UsedResource(StoreHouse.RequireResource[0]);
					//<!-- Initialize building.
					storeHouse.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, P_constructionArea, 0);
					storeHouse.OnBuildingProcess(storeHouse);
					storeHouse._canMovable = false;
					currentConstructionCommand = null;
				};
				storeHouse.destroyObj_Event += (object sender, System.EventArgs e) => {
					Tile.SetEmptyArea(storeHouse.constructionArea);
					Destroy(new_storehouse.gameObject);
				};
            }
            else
            {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
		}
		else if(buildingName == EconomyIconData.MARKET_ICON_NAME) {
            bool _CanCreateBuilding = (BuildingBeh.CheckingOnBuildingList() && 
                buildingBeh.CheckingEnoughUpgradeResource(MarketBeh.RequireResource[0]) &&
                BuildingBeh.MarketInstance == null) ? true : false;

            if (_CanCreateBuilding) {
                GameObject new_market = Instantiate(this.market_prefab, position, Quaternion.identity) as GameObject;
                MarketBeh market = new_market.GetComponent<MarketBeh>();
				currentConstructionCommand = market as BuildingBeh;
				taskManager.CreateConfirmationWindows(position);
				
				market._canMovable = true;
				market.constructionArea = P_constructionArea;
				market.CreateConstructionEvent += (sender, e) => {
					//<!-- Remove used resource.
					GameMaterialDatabase.UsedResource(MarketBeh.RequireResource[0]);
					//<!-- Initialize building.
					market.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, P_constructionArea, 0);
					market.OnBuildingProcess(market);
					market._canMovable = false;
					currentConstructionCommand = null;
				};
				market.destroyObj_Event += (object sender, System.EventArgs e) => {
					Tile.SetEmptyArea(market.constructionArea);
					Destroy(new_market.gameObject);
				};
            }
            else
            {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
		}
		
		#endregion
		
		#region <@-- Military buildings section.
		
        else if(buildingName == MilitaryIconData.BARRACKS_ICON_NAME) {
            bool _canCreateBuilding = false;
            if (BuildingBeh.CheckingOnBuildingList() &&
                buildingBeh.CheckingEnoughUpgradeResource(BarracksBeh.RequireResource[0]) &&
                BuildingBeh.Barrack_Instance == null && BuildingBeh.AcademyInstance != null)
            {
                _canCreateBuilding = (BuildingBeh.AcademyInstance.Level >= 3) ? true : false;
            }

            if (_canCreateBuilding) {
                GameObject new_barracks = Instantiate(barracks_prefab) as GameObject;
                BarracksBeh barracks = new_barracks.GetComponent<BarracksBeh>();
				currentConstructionCommand = barracks as BuildingBeh;
				taskManager.CreateConfirmationWindows(position);
				
				barracks._canMovable = true;
				barracks.constructionArea = P_constructionArea;
				barracks.CreateConstructionEvent += (sender, e) => {
					//<!-- Remove used resource.
					GameMaterialDatabase.UsedResource(BarracksBeh.RequireResource[0]);
					//<!-- Initialize building.
					barracks.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, P_constructionArea, 0);
					barracks.OnBuildingProcess(barracks);
					barracks._canMovable = false;
					currentConstructionCommand = null;
				};
				barracks.destroyObj_Event += (object sender, System.EventArgs e) => {
					Tile.SetEmptyArea(barracks.constructionArea);
					Destroy(new_barracks.gameObject);
				};
            }
            else {
                taskManager.ShowWarnningCannotCreateBuilding();
            }
        }
		
		#endregion
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
		
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
	
	void LateUpdate() {
		
        if(TaskManager.IsShowInteruptGUI == false)
            base.ImplementTouchPostion();
		
		if(Application.isWebPlayer || Application.isEditor) {
			if(joystick_obj != null) {
				if(joystickManager != null)
					this.UpdateJoystick();
				else
					joystickManager = joystick_obj.GetComponent<JoystickManager>();
			}
			
			#region <!-- Detech when used keybroad input.
			
			if (Input.GetKey(KeyCode.LeftArrow)) {
				Camera.main.transform.Translate(Vector3.left * moveCamSpeed);
			}
			else if (Input.GetKey(KeyCode.RightArrow)) {
				Camera.main.transform.Translate(Vector3.right * moveCamSpeed);
			}
			
			if (Input.GetKey(KeyCode.UpArrow)) {
				Camera.main.transform.Translate(Vector3.up * moveCamSpeed);
			}
			else if (Input.GetKey(KeyCode.DownArrow)) {
				Camera.main.transform.Translate(Vector3.down * moveCamSpeed);
			}

			#endregion
		}
		
        if (Camera.main.transform.position.x > isomatricEngine.crop_right)
            Camera.main.transform.position = new Vector3(isomatricEngine.crop_right, Camera.main.transform.position.y, Camera.main.transform.position.z); 	//Vector3.left * Time.deltaTime;
        if (Camera.main.transform.position.x < isomatricEngine.crop_left)
            Camera.main.transform.position = new Vector3(isomatricEngine.crop_left, Camera.main.transform.position.y, Camera.main.transform.position.z);	 //Vector3.right * Time.deltaTime;
        if (Camera.main.transform.position.y > isomatricEngine.crop_up)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, isomatricEngine.crop_up, Camera.main.transform.position.z);
        if (Camera.main.transform.position.y < isomatricEngine.crop_down)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, isomatricEngine.crop_down, Camera.main.transform.position.z);
	}

    protected override void MovingCameraTransform()
    {
        base.MovingCameraTransform();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            float speed = Time.deltaTime * 30f;
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = touch.deltaPosition;
            // Move object across XY plane
            //transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
            Camera.main.transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
        }
    }
	
	void UpdateJoystick() {
        //moveCamSpeed = Time.deltaTime * (Application.targetFrameRate * 2);
        moveCamSpeed = Time.deltaTime * (120);
		if(joystickManager.joystick.touchCount != 0)
		{
			if(joystickManager.joystick._isMoveGUI)
			{
				if(joystickManager.joystick.position.x > 0.2f) {
					Camera.main.transform.Translate(Vector3.right * moveCamSpeed);
				}
				else if(joystickManager.joystick.position.x < .2f) {			
					Camera.main.transform.Translate(Vector3.left * moveCamSpeed);
				}
				
				if(joystickManager.joystick.position.y > .2f) {
					Camera.main.transform.Translate(Vector3.up * moveCamSpeed);
				}
				else if(joystickManager.joystick.position.y < -.2f) {
					Camera.main.transform.Translate(Vector3.down * moveCamSpeed);
				}
			}
		}
	}

    public override void OnInput(string nameInput, string objectTag)
    {
        base.OnInput(nameInput, objectTag);

        if (nameInput == "Left_button") {
            Camera.main.transform.Translate(Vector3.left * moveCamSpeed);
        }
        if (nameInput == "Right_button") {
            Camera.main.transform.Translate(Vector3.right * moveCamSpeed);
        }

        if (nameInput == "Up_button") {
            Camera.main.transform.Translate(Vector3.up * moveCamSpeed);
        }
        if (nameInput == "Down_button") {
            Camera.main.transform.Translate(Vector3.down * moveCamSpeed);
        }
		
		if(objectTag == "GUI") {
			taskManager.Handle_OnInput(nameInput);
		}
    }

    void OnApplicationQuit()
    {
        if (saveManager != null)
            this.saveManager.Save();
    }

    void OnApplicationPause()
    {	
		if(saveManager != null)
			this.saveManager.Save();
    }

    public override void OnDispose()
    {
        base.OnDispose();
		
		BuildingBeh.ClearStaticData();
		HouseBeh.ClearStaticData();

        list_AICity.Clear();
        list_AICity = null;
    }
}
