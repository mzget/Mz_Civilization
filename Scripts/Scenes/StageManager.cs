using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : Mz_BaseScene {
	
	public const string PrototypeObjects_ResourcePath = "Prototypes/";
	public const string PathOfEconomyBuilding = "Buildings/Economy/";
	public const string PathOfUtilityBuilding =  "Buildings/Utility/";
	public const string PathOfMilitaryBuilding = "Buildings/Military/";

    public GUISkin mainBuildingSkin;
    public TaskManager taskManager;
    public IsometricEngine isomatricEngine;
	public Mz_SaveData saveManager;
    public List<GameMaterial> gameMaterials = new List<GameMaterial>();
    // Map and building area.
    public Tile[,] tiles_list = new Tile[25, 25];
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
	public static List<BuildingArea> buildingArea_Objs = new List<BuildingArea>(24);
    public static bool[] arr_buildingAreaState = new bool[24];
    //<!--- Private Data Fields.
    private Vector2 scrollPosition = Vector2.zero;
    private Rect mainGUIRect = new Rect(Main.FixedGameWidth / 2 - 300, Main.FixedGameHeight - 100, 600, 100);
    private Rect windowRect = new Rect(Main.FixedGameWidth / 2 - 300, Main.FixedGameHeight / 2 - 150, 600, 320);
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
	protected override void Initializing ()
	{
		base.Initializing ();


        this.gameObject.AddComponent<TaskManager>();
        taskManager = this.gameObject.GetComponent<TaskManager>();
        
        saveManager = new Mz_SaveData ();

        isomatricEngine = this.GetComponent<IsometricEngine>();
        isomatricEngine.sceneController = this;
        StartCoroutine(isomatricEngine.CreateTilemap());

        this.StartCoroutine(this.InitializeAudio());
		this.StartCoroutine(this.CreateGameMaterials());
        StartCoroutine(this.InitializeAICities());
		
        //this.GenerateBackground();
        //this.CreateBuildingArea();
		this.PrepareBuildingPrefabsFromResource();
		this.Load_AmountOfBuildingInstance();
		this.LoadingDataStorage();
		
		if (BuildingBeh.StoreHouseInstance.Count == 0) {
			StoreHouse.CalculationSumOfMaxCapacity();
			dayCycle_Event += StoreHouse.ReachDayCycle;
		}
		
		if (BuildingBeh.House_Instances.Count == 0)
			HouseBeh.CalculationSumOfPopulation();
		
#if UNITY_WEBPLAYER || UNITY_EDITOR
		StartCoroutine(InitializeJoystick());
#endif

        //<@-- Admob integration.
#if UNITY_ANDROID

		AdBannerObserver.Initialize("a150c2e14a5d753", "DIAT-GE5P-2J5H-2", 30f);

#endif

        _StageInitialized = true;
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
        GreekBeh.Spearman_unit = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_SPEARMAN);
        GreekBeh.Hapaspist_unit = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HAPASPIST);
        GreekBeh.Hoplite_unit = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.GreekAI_DataStore.KEY_GREEK_AI_HOPLITE);
        
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
    void PrepareBuildingPrefabsFromResource()
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

    void Load_AmountOfBuildingInstance() {
        //<!-- Utility --->>
		numberOfHouse_Instance = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.numberOfHouse_Instance);
		academyInstance = PlayerPrefsX.GetBool(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_AcademyInstance);
		//<!-- Resource --->>
		amount_Farm_Instance = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.amount_farm_instance);
		amount_Sawmill_Instance = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.amount_sawmill_instance);
		amount_MillStone_Instance = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.amount_millstone_instance);
		amount_Smelter_Instance = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.amount_smelter_instance);
        //<!-- Economy --->>
		numberOfStoreHouseInstances = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.numberOfStorehouseInstance);
		marketInstance = PlayerPrefsX.GetBool(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_MarketInstance);
        //<!-- Millitary --->>
		barracksInstance = PlayerPrefsX.GetBool(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_BarracksInstance);
    }

	void LoadingDataStorage()
    {
		//<!--- Load level of towncenter.
		BuildingBeh.TownCenter.Level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.TownCenter_level, 0);
		
        #region <!--- House instance data.
		
        if (numberOfHouse_Instance != 0) {
            for (int i = 0; i < numberOfHouse_Instance; i++)
            {
				int temp_level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.house_level_ + i);
				int temp_pos = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.house_position_ + i);

                GameObject tempHouse = Instantiate(house_prefab) as GameObject;
                HouseBeh house = tempHouse.GetComponent<HouseBeh>();
                house.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, temp_pos, temp_level);
            }
        }
		
        #endregion
		#region <!--- Academy.
		
		if(academyInstance) {
			int position = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_AcademyPosition);
			int level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_AcademyLevel);
			
			GameObject academy = Instantiate(academy_prefab) as GameObject;
			AcademyBeh academyBeh = academy.GetComponent<AcademyBeh>();
        	academyBeh.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, position, level);
		}
		
		#endregion

        #region <!--- Farm_Data.

        if (amount_Farm_Instance != 0) 
        {
	        for (int i = 0; i < amount_Farm_Instance; i++) 
            {
				int Level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.farm_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.farm_position_ + i);

                GameObject temp_farm = Instantiate(farm_prefab) as GameObject;
				Farm farm = temp_farm.GetComponent<Farm>();
                farm.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);
	        }
        }

        #endregion
        #region <!--- Sawmill Data.

        if (amount_Sawmill_Instance != 0)
        {
            for (int i = 0; i < amount_Sawmill_Instance; i++)
            {
				int Level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.sawmill_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.sawmill_position_ + i);

                GameObject new_Sawmill = Instantiate(sawmill_prefab) as GameObject;
                Sawmill sawmill = new_Sawmill.GetComponent<Sawmill>();
                sawmill.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);
            }
        }

        #endregion
        #region <!--- MillStone Data.

        if (amount_MillStone_Instance != 0)
        {
            for (int i = 0; i < amount_MillStone_Instance; i++)
            {
				int Level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.millstone_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.millstone_position_ + i);

                GameObject new_millstone = Instantiate(millstone_prefab) as GameObject;
                StoneCrushingPlant millstone = new_millstone.GetComponent<StoneCrushingPlant>();
                millstone.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);
            }
        }

        #endregion
        #region <!--- Smelter Data.

        if (amount_Smelter_Instance != 0)
        {
            for (int i = 0; i < amount_Smelter_Instance; i++)
            {
				int Level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.smelter_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.smelter_position_ + i);

                GameObject new_smelter = Instantiate(smelter_prefab) as GameObject;
                Smelter smelter = new_smelter.GetComponent<Smelter>();
                smelter.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);
            }
        }

        #endregion

        #region <!--- StoreHouse Data.

        if (numberOfStoreHouseInstances != 0)
        {
            for (int i = 0; i < numberOfStoreHouseInstances; i++)
            {
				int Level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.storehouse_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.storehouse_position_ + i);
				
                GameObject temp_storehouse = Instantiate(storehouse_prefab) as GameObject;
                StoreHouse new_storehouse = temp_storehouse.GetComponent<StoreHouse>();
                new_storehouse.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, Position, Level);
            }
        }

        #endregion
		#region <!--- Market data.
		
		if(marketInstance) {
			int position = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_MarketPosition);
			int level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + ":" + Mz_SaveData.KEY_MarketLevel);
			
			GameObject market_Obj = Instantiate(market_prefab) as GameObject;
			MarketBeh marketBeh = market_Obj.GetComponent<MarketBeh>();
        	marketBeh.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, position, level);
		}
		
		#endregion

        #region <!--- Barracks Data.

        if (barracksInstance) {
			int level = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_BarracksLevel);
			int position = PlayerPrefs.GetInt(Mz_StorageManagement.SaveSlot + Mz_SaveData.KEY_BarracksPosition);
			
			GameObject barracks_obj = Instantiate(barracks_prefab) as GameObject;
			BarracksBeh barracks = barracks_obj.GetComponent<BarracksBeh>();
			barracks.InitializingBuildingBeh(BuildingBeh.BuildingStatus.none, position, level);
        }

        #endregion

        Debug.Log("StageManager.LoadingDataStorage");
    }
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
		
        if(TaskManager.IsShowInteruptGUI == false) 
            base.ImplementTouchPostion();
        if (Camera.main.transform.position.x > 700)
            Camera.main.transform.position = new Vector3(700, Camera.main.transform.position.y, Camera.main.transform.position.z); 	//Vector3.left * Time.deltaTime;
        if (Camera.main.transform.position.x < 220)
            Camera.main.transform.position = new Vector3(220, Camera.main.transform.position.y, Camera.main.transform.position.z);	 //Vector3.right * Time.deltaTime;
        if (Camera.main.transform.position.y > 140)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 140, Camera.main.transform.position.z);
        if (Camera.main.transform.position.y < -140)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, -140, Camera.main.transform.position.z);
		
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
    }

    protected override void MovingCameraTransform()
    {
        base.MovingCameraTransform();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            float speed = Time.deltaTime * 0.5f;
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

    public override void OnInput(string nameInput)
    {
        base.OnInput(nameInput);

        if (nameInput == "Left_button")
        {
            if (Camera.main.transform.position.x > -640)
                Camera.main.transform.Translate(Vector3.left * moveCamSpeed);
        }
        if (nameInput == "Right_button")
        {
            if (Camera.main.transform.position.x < 640)
                Camera.main.transform.Translate(Vector3.right * moveCamSpeed);
        }

        if (nameInput == "Up_button")
        {
            if (Camera.main.transform.position.y < 400)
                Camera.main.transform.Translate(Vector3.up * moveCamSpeed);
        }
        if (nameInput == "Down_button")
        {
            if (Camera.main.transform.position.y > -400)
                Camera.main.transform.Translate(Vector3.down * moveCamSpeed);
        }
    }

    void OnApplicationQuit() {
		saveManager.Save();
    }

    void OnApplicationPause()
    {
		if(saveManager != null)
			this.saveManager.Save();
    }

    public override void OnDispose()
    {
        base.OnDispose();
		
		buildingArea_Objs.Clear();
		BuildingBeh.ClearStaticData();
		HouseBeh.ClearStaticData();

        list_AICity.Clear();
        list_AICity = null;
    }
}
