using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TaskManager : MonoBehaviour 
{
	public const string PATH_OF_GUISKIN = "GUISkins/";
	public const string PathOfGUISprite = "UI_Sprites/";
    public const string PathOfMainGUI = "Textures/MainGUI/";
    public const string PathOfGameItemTextures = "Textures/GameItems/";
    public const string Advisor_ResourcePath = "Textures/Advisors/";
    public const string PathOfTribes_Texture = "Textures/Tribes_Icons/";
    public const string PathOf_TroopIcons = "Textures/Troop_Icons/";

	internal const string DISPLAY_MESSAGE_ACTIVITY = "DisplayMessageActivity";
    internal const string DISPLAY_QUEST_ACTIVITY = "DisplayQuestActivity";
	internal const string DISPLAY_MISSION_COMPLETE_ACTIVITY = "DisplayMissionCompleteActivity";
    internal const string DISPLAY_FOREIGN_ACTIVITY = "DisplayForeignActivity";

    public static bool IsShowInteruptGUI = false;

	#region <@-- Right side bar data fields.

    public enum TopSidebarState { 
        none = 0, 
        show_domination, 
        show_agriculture, 
        show_industry, 
        show_commerce, 
        show_military, 
        show_ForeignTab, 
        show_setting, 
    };
    public TopSidebarState currentTopSidebarState = TopSidebarState.none;	
	private bool _showBaseRightSideBarGUIState = false;
	public GameObject baseSpriteRightSideBar_obj;
	private Hashtable moveInRightSideBarGUI_hash;
	private Hashtable moveOutRightSideBarGUI_hash;
	private Vector3 rightSide_BaseSpritePos_movein = new Vector3(-150f, -384f, 50f);
	private Vector3 rightSide_BaseSpritePos_moveout = new Vector3(148f, -384f, 50f);

	public GameObject menubarIcon_button;
	public GameObject utility_icon_button;
	public GameObject economy_icon_button;
    public GameObject military_icon_button;
	public GameObject setting_icon_button;
	
	public GameObject buildingIcon_prefab;
    public Transform[] buildingIcon_transform = new Transform[8];
	private BuildingIconBeh[] buildingIcon_beh = new BuildingIconBeh[8]; 
	public tk2dSprite[] buildingsIcon_sprite = new tk2dSprite[8];
	internal UtilityIconData utilityIconData = new UtilityIconData();
	internal EconomyIconData economyIconData = new EconomyIconData();
    internal MilitaryIconData militaryIconData = new MilitaryIconData();

	#endregion

	public enum BaseGUIStateBeh { None = 0, UserQuitApplication, }
	public BaseGUIStateBeh baseGUIState;

    public CapitalCity stageManager;
	public DisplayTroopsActivity displayTroopsActivity;
    public MessageManager messageManager;
    public ForeignManager foreignManager;
    public QuestSystemManager questManager;

    public GUISkin taskbarUI_Skin;
    public GUIStyle left_button_Style;
    public GUIStyle right_button_Style;
    internal GUIStyle foreignActivityStyle;
		
	//<@-- Textures.
    public Texture2D food_icon;
    public Texture2D wood_icon;
    public Texture2D stone_icon;
    public Texture2D copper_icon;
    public Texture2D armor_icon;
    public Texture2D weapon_icon;
    public Texture2D gold_icon;
    public Texture2D employee_icon;
	public Texture2D domination_icon;
	public Texture2D agriculture_icon;
	public Texture2D industry_icon;
	public Texture2D commerce_icon;
	public Texture2D military_icon;
    public Texture2D map_icon;
	public Texture2D setting_icon;

    public Texture2D marketTradingIcon;

    public Texture2D advisor_villageElder;
	public Texture2D messageFormSystem_icon;
	internal Texture2D newQuestAdvisor_img;
	internal Texture2D completeQuestAdvisor_img;
    //@!-- Troops units.
    public Texture2D spearmanUnitIcon;
    public Texture2D hypaspistUnitIcon;
    public Texture2D hopliteUnitIcon;
    public Texture2D ToxotesUnitIcon;
    //<!-- Cities symbol.
    public Texture2D GreekIcon_Texture;
    public Texture2D EgyptianIcon_Texture;
    public Texture2D PersianIcon_Texture;
    public Texture2D CelticIcon_Texture;

	//<@!-- Rectangles.
    protected Rect header_group_rect;
    protected Rect header_button_rect;
	Rect first_rect, second_rect, third_rect, fourth_rect, fifth_rect, sixth_rect;
    public Rect baseSidebarGroup_rect;
    Rect sidebarButtonGroup_rect = new Rect(0, 0, 50, Main.FixedGameHeight);
    Rect sidebarContentGroup_rect;
	Rect sidebarContentBox_rect;
    Rect first_button_rect = new Rect(1 * Mz_OnGUIManager.Extend_heightScale, 2, 48 * Mz_OnGUIManager.Extend_heightScale, 56);
	Rect second_button_rect = new Rect(1 * Mz_OnGUIManager.Extend_heightScale, 60, 48 * Mz_OnGUIManager.Extend_heightScale, 56);
	Rect third_button_rect = new Rect(1 * Mz_OnGUIManager.Extend_heightScale, 120, 48 * Mz_OnGUIManager.Extend_heightScale, 56);
	Rect fourth_button_rect = new Rect(1 * Mz_OnGUIManager.Extend_heightScale, 180, 48 * Mz_OnGUIManager.Extend_heightScale, 56);
	Rect fifth_button_rect = new Rect(1 * Mz_OnGUIManager.Extend_heightScale, 240, 48 * Mz_OnGUIManager.Extend_heightScale, 56);
	Rect sixth_button_rect = new Rect(1 * Mz_OnGUIManager.Extend_heightScale, 300, 48 * Mz_OnGUIManager.Extend_heightScale, 56);
    Rect seventh_button_rect;
	//<@--- world map section.
	private Rect showSymbol_rect;
	private Rect showNameOfAIcity_rect;
    private Rect previousButton_rect;
    private Rect nextButton_rect;
    public Rect standardWindow_rect;
    public Rect exitButton_Rect;

	//<@-- Notification display rectangles.
	internal Rect leftSidebarGroup_rect;
    private Rect normalSidebarGroup_rect;
	private Rect moveOutLeftSidebarGroup_rect;
    internal Rect notificationRect_1;
    internal Rect notificationRect_2;
    
	
	// Use this for initialization
    IEnumerator Start()
    {
        while (Mz_BaseScene._StageInitialized == false)
			yield return null;

        TaskManager.IsShowInteruptGUI = false;

		StartCoroutine(this.InitializeTweenHashData());
        this.InitializeOnGUIDataFields();
        this.InitializeTextureResource();

		GameObject gamecontroller = this.gameObject;
		stageManager = gamecontroller.GetComponent<CapitalCity>();

		gamecontroller.AddComponent<DisplayTroopsActivity>();
        displayTroopsActivity = gamecontroller.GetComponent<DisplayTroopsActivity>();
		
		gamecontroller.AddComponent<ForeignManager>();
        foreignManager = gamecontroller.GetComponent<ForeignManager>();

		gamecontroller.AddComponent<MessageManager>();
        messageManager = gamecontroller.GetComponent<MessageManager>();
		
		gamecontroller.AddComponent<QuestSystemManager>();
        questManager = gamecontroller.GetComponent<QuestSystemManager>();
    }
	
    void InitializeOnGUIDataFields()
    {
		taskbarUI_Skin = Resources.Load(PATH_OF_GUISKIN + "TaskbarUI_Skin", typeof(GUISkin)) as GUISkin;
        taskbarUI_Skin.button.alignment = TextAnchor.MiddleCenter;
		taskbarUI_Skin.box.alignment = TextAnchor.MiddleCenter;
		
		left_button_Style = new GUIStyle(taskbarUI_Skin.button);
        right_button_Style = new GUIStyle(taskbarUI_Skin.button);
        foreignActivityStyle = new GUIStyle(taskbarUI_Skin.box);
        foreignActivityStyle.alignment = TextAnchor.UpperCenter;
				
        header_group_rect = new Rect(0, 0, (Screen.width/6) * 4, 50);
		header_button_rect = new Rect(0, 0, header_group_rect.width / 5, 50);

        first_rect = new Rect(0, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        second_rect = new Rect((header_button_rect.width) * 1, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        third_rect = new Rect((header_button_rect.width) * 2, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        fourth_rect = new Rect((header_button_rect.width) * 3, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        fifth_rect = new Rect((header_button_rect.width) * 4, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        sixth_rect = new Rect((header_button_rect.width) * 5, header_button_rect.y, header_button_rect.width, header_button_rect.height);
		seventh_button_rect = new Rect(1 * Mz_OnGUIManager.Extend_heightScale, 360, 48 * Mz_OnGUIManager.Extend_heightScale, 56);

        showSymbol_rect = new Rect(0, 50, 100 * Mz_OnGUIManager.Extend_heightScale, 100);
        showSymbol_rect.x = sidebarContentBox_rect.width / 2 - (showSymbol_rect.width / 2);
        showNameOfAIcity_rect = new Rect(5 * Mz_OnGUIManager.Extend_heightScale, 155, sidebarContentBox_rect.width, 40);
        previousButton_rect = new Rect(18 * Mz_OnGUIManager.Extend_heightScale, 85, 32 * Mz_OnGUIManager.Extend_heightScale, 32);
        nextButton_rect = new Rect(sidebarContentBox_rect.width - (50 * Mz_OnGUIManager.Extend_heightScale), 85, 32 * Mz_OnGUIManager.Extend_heightScale, 32);

        standardWindow_rect = new Rect((Main.FixedGameWidth / 2) - 400, Main.FixedGameHeight / 2 - 250, 800, 500);
        exitButton_Rect = new Rect(standardWindow_rect.width - (34 * Mz_OnGUIManager.Extend_heightScale), 2, 32 * Mz_OnGUIManager.Extend_heightScale, 32);

        //<@-- Notification display rectangles.
        moveOutLeftSidebarGroup_rect = new Rect(-120, 50, 120 * Mz_OnGUIManager.Extend_heightScale, 500);
        normalSidebarGroup_rect = new Rect(0, 50, 120 * Mz_OnGUIManager.Extend_heightScale, 500);
        leftSidebarGroup_rect = normalSidebarGroup_rect;
        notificationRect_1 = new Rect(0, 5, 120 * Mz_OnGUIManager.Extend_heightScale, 80);
        notificationRect_2 = new Rect(0, 90, 120 * Mz_OnGUIManager.Extend_heightScale, 80);
    }
	
    void InitializeTextureResource() 
    {
        left_button_Style.normal.background = Resources.Load(PathOfMainGUI + "Back_up", typeof(Texture2D)) as Texture2D;
        left_button_Style.active.background = Resources.Load(PathOfMainGUI + "Back_down", typeof(Texture2D)) as Texture2D;
        right_button_Style.normal.background = Resources.Load(PathOfMainGUI + "Next_up", typeof(Texture2D)) as Texture2D;
        right_button_Style.active.background = Resources.Load(PathOfMainGUI + "Next_down", typeof(Texture2D)) as Texture2D;

        food_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Grain", typeof(Texture2D)) as Texture2D;
        wood_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "PinePlanks", typeof(Texture2D)) as Texture2D;
        stone_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "StoneBlock", typeof(Texture2D)) as Texture2D;
        copper_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "CopperIngot", typeof(Texture2D)) as Texture2D;
        armor_icon = Resources.Load(PathOfGameItemTextures + "Armor", typeof(Texture2D)) as Texture2D;
		weapon_icon = Resources.Load(PathOfGameItemTextures + "Weapon", typeof(Texture2D)) as Texture2D;
		
        gold_icon = Resources.Load(PathOfMainGUI + "GoldIngot", typeof(Texture2D)) as Texture2D;
        employee_icon = Resources.Load(PathOfMainGUI + "Employee_icon", typeof(Texture2D)) as Texture2D;
		
		domination_icon = Resources.Load(PathOfMainGUI + "Domination", typeof(Texture2D)) as Texture2D;
		agriculture_icon = Resources.Load(PathOfMainGUI + "Agriculture", typeof(Texture2D)) as Texture2D;
		industry_icon = Resources.Load(PathOfMainGUI + "Industry", typeof(Texture2D)) as Texture2D;
		commerce_icon = Resources.Load(PathOfMainGUI + "Commerce", typeof(Texture2D)) as Texture2D;
		military_icon = Resources.Load(PathOfMainGUI + "Military", typeof(Texture2D)) as Texture2D;
        map_icon = Resources.Load(PathOfMainGUI + "Map_Texture", typeof(Texture2D)) as Texture2D;
		setting_icon = Resources.Load(PathOfMainGUI + "Setting", typeof(Texture2D)) as Texture2D;

        marketTradingIcon = Resources.Load(PathOfMainGUI + "Market", typeof(Texture2D)) as Texture2D;

		advisor_villageElder = Resources.Load(Advisor_ResourcePath + "VillageElder_idle", typeof(Texture2D)) as Texture2D;
		newQuestAdvisor_img = Resources.Load(TaskManager.Advisor_ResourcePath + "VillageElder_idle", typeof(Texture2D)) as Texture2D;
		completeQuestAdvisor_img = Resources.Load(TaskManager.Advisor_ResourcePath + "VillageElder_idle", typeof(Texture2D)) as Texture2D;

		messageFormSystem_icon = Resources.Load(PathOfMainGUI + "MessageIcon", typeof(Texture2D)) as Texture2D;
        //<!-- Load troop icon.
        spearmanUnitIcon = Resources.Load(PathOf_TroopIcons + "Spearman", typeof(Texture2D)) as Texture2D;
        hypaspistUnitIcon = Resources.Load(PathOf_TroopIcons + "Hypaspist", typeof(Texture2D)) as Texture2D;
        hopliteUnitIcon = Resources.Load(PathOf_TroopIcons + "Hoplite", typeof(Texture2D)) as Texture2D;
        ToxotesUnitIcon = Resources.Load(PathOf_TroopIcons + "Toxotai", typeof(Texture2D)) as Texture2D;
    }

	public void Handle_OnInput(string nameInput)
	{
		if(nameInput == menubarIcon_button.name) {
            if (TaskManager.IsShowInteruptGUI == false)
                MoveRightSideBarGUI();
            else
                return;
		}
		else if(nameInput == utility_icon_button.name) {
			this.ActivateBuildingIconArrObject(false);
			for (int i = 0; i < utilityIconData.nameOfBuildingIcon.Length; i++) {				
				buildingsIcon_sprite[i].gameObject.SetActive(true);
				buildingsIcon_sprite[i].spriteId = buildingsIcon_sprite[i].GetSpriteIdByName(utilityIconData.nameOfBuildingIcon[i]);
				buildingsIcon_sprite[i].gameObject.name = utilityIconData.nameOfBuildingIcon[i];
				
				buildingIcon_beh[i] = buildingsIcon_sprite[i].GetComponent<BuildingIconBeh>();
				buildingIcon_beh[i].constructionArea = utilityIconData.areaOfBuildings[i];
			}
		}
		else if(nameInput == economy_icon_button.name) {
			this.ActivateBuildingIconArrObject(false);
			for (int i = 0; i < economyIconData.nameOfBuildingIcon.Length; i++) {				
				buildingsIcon_sprite[i].gameObject.SetActive(true);
				buildingsIcon_sprite[i].spriteId = buildingsIcon_sprite[i].GetSpriteIdByName(economyIconData.nameOfBuildingIcon[i]);
				buildingsIcon_sprite[i].gameObject.name = economyIconData.nameOfBuildingIcon[i];
				
				buildingIcon_beh[i] = buildingsIcon_sprite[i].GetComponent<BuildingIconBeh>();
				buildingIcon_beh[i].constructionArea = economyIconData.areaOfBuildings[i];
			}
		}
        else if(nameInput == military_icon_button.name) {
            this.ActivateBuildingIconArrObject(false);
            for (int i = 0; i < militaryIconData.nameOfBuildingIcon.Length; i++)
            {
                buildingsIcon_sprite[i].gameObject.SetActive(true);
                buildingsIcon_sprite[i].spriteId = buildingsIcon_sprite[i].GetSpriteIdByName(militaryIconData.nameOfBuildingIcon[i]);
                buildingsIcon_sprite[i].gameObject.name = militaryIconData.nameOfBuildingIcon[i];
				
				buildingIcon_beh[i] = buildingsIcon_sprite[i].GetComponent<BuildingIconBeh>();
				buildingIcon_beh[i].constructionArea = militaryIconData.areaOfBuildings[i];
            }
        }
		else if(nameInput == setting_icon_button.name) {
			this.ActivateBuildingIconArrObject(false);
			buildingsIcon_sprite[0].gameObject.SetActive(true);
			buildingsIcon_sprite[0].spriteId = buildingsIcon_sprite[0].GetSpriteIdByName("ExitButton");
			buildingsIcon_sprite[0].gameObject.name = "ExitButton";
		}
		else if(nameInput == "ExitButton") {
			this.GoToMainMenu();
		}
	}

	void ActivateBuildingIconArrObject (bool activate)
	{
		for (int i = 0; i < buildingsIcon_sprite.Length; i++) {
			buildingsIcon_sprite[i].gameObject.SetActive(activate);
			buildingIcon_beh[i] = null;
		}
	}

	// Update is called once per frame
	void Update () { 		
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
		{
			//Application.Quit();
			baseGUIState = BaseGUIStateBeh.UserQuitApplication;
		}
	}

    void OnGUI ()
	{		
		if(CapitalCity._StageInitialized == false)
			return;
		
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, Screen.height / Main.FixedGameHeight, 1));

		if (baseGUIState == BaseGUIStateBeh.UserQuitApplication) {			
			#region <@-- Exit Application Machanism.
			
			TaskManager.IsShowInteruptGUI = true;

			GUI.BeginGroup (new Rect (Screen.width / 2 - (200 * Mz_OnGUIManager.Extend_heightScale), Main.FixedGameHeight / 2 - 100, 400 * Mz_OnGUIManager.Extend_heightScale, 200), "Do you want to quit ?", GUI.skin.window);
			{
				if (GUI.Button (new Rect (60 * Mz_OnGUIManager.Extend_heightScale, 155, 100 * Mz_OnGUIManager.Extend_heightScale, 40), "Yes"))
				{
					this.GoToMainMenu();
				}
				else if (GUI.Button (new Rect (240 * Mz_OnGUIManager.Extend_heightScale, 155, 100 * Mz_OnGUIManager.Extend_heightScale, 40), "No")) {
					baseGUIState = BaseGUIStateBeh.None;			
					TaskManager.IsShowInteruptGUI = false;
				}
			}
			GUI.EndGroup ();

			#endregion
		} 

        this.DrawTopSidebarGUI();
//        this.DrawRightSidebar ();
        this.DrawLeftSideBar();
    }

    private void DrawTopSidebarGUI()
    {
        if(currentTopSidebarState == TopSidebarState.none) {
	        
        }
		GUI.BeginGroup(header_group_rect, "", taskbarUI_Skin.box);
        {
            GUI.Box(first_rect, new GUIContent(StoreHouse.SumOfFood + "/" + StoreHouse.SumOfMaxCapacity, food_icon), taskbarUI_Skin.button);
            GUI.Box(second_rect, new GUIContent(StoreHouse.SumOfWood + "/" + StoreHouse.SumOfMaxCapacity, wood_icon), taskbarUI_Skin.button);
            GUI.Box(third_rect, new GUIContent(StoreHouse.SumOfStone + "/" + StoreHouse.SumOfMaxCapacity, stone_icon), taskbarUI_Skin.button);
            GUI.Box(fourth_rect, new GUIContent(StoreHouse.SumOfCopper + "/" + StoreHouse.SumOfMaxCapacity, copper_icon), taskbarUI_Skin.button);
            GUI.Box(fifth_rect, new GUIContent(HouseBeh.SumOfPopulation.ToString(), employee_icon), taskbarUI_Skin.button);
        }
        GUI.EndGroup();
    }

    private void GoToMainMenu() {
		stageManager.saveManager.Save();
        stageManager.OnDispose();
        if (Application.isLoadingLevel == false)
        {
            Mz_LoadingScreen.TargetSceneName = Mz_BaseScene.ScenesInstance.MainMenu.ToString();
            Application.LoadLevel(Mz_BaseScene.ScenesInstance.LoadingScreen.ToString());
        }
    }

	IEnumerator InitializeTweenHashData ()
	{
		moveInRightSideBarGUI_hash = new Hashtable();
		moveInRightSideBarGUI_hash.Add("position", rightSide_BaseSpritePos_movein);
		moveInRightSideBarGUI_hash.Add("islocal", true);
		moveInRightSideBarGUI_hash.Add("time", 1f);
		moveInRightSideBarGUI_hash.Add("easetype", iTween.EaseType.easeOutBounce);

		moveOutRightSideBarGUI_hash = new Hashtable();
		moveOutRightSideBarGUI_hash.Add("position", rightSide_BaseSpritePos_moveout);
		moveOutRightSideBarGUI_hash.Add("islocal", true);
		moveOutRightSideBarGUI_hash.Add("time", 1f);
		moveOutRightSideBarGUI_hash.Add("easetype", iTween.EaseType.easeOutBounce);


		yield return null;
	}

	internal void MoveRightSideBarGUI()
	{
		print("FUNC_MoveRightSideBarGUI()");
		
		if(_showBaseRightSideBarGUIState == false) {
            MoveIn_RightSidebarGUI();
		}
		else {
            MoveOut_RightSidebarGUI();
		}
	}

    internal void MoveOut_RightSidebarGUI()
    {
        iTween.MoveTo(baseSpriteRightSideBar_obj, moveOutRightSideBarGUI_hash);
        _showBaseRightSideBarGUIState = false;
        BuildingBeh.ActivateColliderComponent(true);
    }

    internal void MoveIn_RightSidebarGUI()
    {
        iTween.MoveTo(baseSpriteRightSideBar_obj, moveInRightSideBarGUI_hash);
        _showBaseRightSideBarGUIState = true;
        BuildingBeh.ActivateColliderComponent(false);
    }

	#region <@-- Left sidebar group rect.

    private void DrawLeftSideBar()
    {
        //<@-- "LeftSideBox".
        GUI.BeginGroup(leftSidebarGroup_rect, string.Empty, GUIStyle.none);
        {
            messageManager.DrawGUI_MessageIcon();
            questManager.DrawQuestNoticeIcon();
        }
        GUI.EndGroup();
    }

	internal void MoveOutLeftSidebar(string completeParam) {
		iTween.ValueTo(this.gameObject, iTween.Hash("from", normalSidebarGroup_rect, "to", moveOutLeftSidebarGroup_rect, "time", 0.5f,
                    "onupdate", "OnMoveLeftSidebarCallBack", "onupdatetarget", this.gameObject,
                    "oncomplete", "MoveOutLeftSidebarCompleteCallBack", "oncompleteparams", completeParam, "oncompletetarget", this.gameObject));
	}
    private void OnMoveLeftSidebarCallBack(Rect callback) {
        leftSidebarGroup_rect = callback;
    }
    private void MoveOutLeftSidebarCompleteCallBack(string completeParam)
    {
		
        TaskManager.IsShowInteruptGUI = true;
        this.SendMessage(completeParam, SendMessageOptions.DontRequireReceiver);
    }

    internal void MoveInLeftSidebar()
    {
        iTween.ValueTo(this.gameObject, iTween.Hash("from", moveOutLeftSidebarGroup_rect, "to", normalSidebarGroup_rect, "time", 0.5f,
            "onupdate", "OnMoveLeftSidebarCallBack", "onupdatetarget", this.gameObject,
            "oncomplete", "MoveInLeftSidebarCompleteCallBack", "oncompletetarget", this.gameObject));
    }

    private void MoveInLeftSidebarCompleteCallBack() {
        TaskManager.IsShowInteruptGUI = false;
    }

    #region <@-- Call by const string method name HookUp.

    private void DisplayMessageActivity()
    {
        messageManager.InitializeMessageMechanism();
    }

    private void DisplayQuestActivity() {
        questManager.InitializeMessageMechanism(QuestSystemManager.QuestManagerStateBeh.DrawMissionActivity);
    }

	private void DisplayMissionCompleteActivity() {
		questManager.InitializeMessageMechanism(QuestSystemManager.QuestManagerStateBeh.DrawCompleteMissionActivity);
	}

    private void DisplayForeignActivity() {
        foreignManager.currentForeignTabStatus = ForeignManager.ForeignTabStatus.DrawActivity;
    }

    #endregion

    #endregion

    #region <@-- Right sidebar group rect.
/*
    private void DrawRightSidebar() 
	{		
		GUI.BeginGroup(baseSidebarGroup_rect, GUIContent.none, GUI.skin.box);
        {
            GUI.BeginGroup(sidebarButtonGroup_rect);
            {
                if (GUI.Button(first_button_rect, new GUIContent(domination_icon))) {
                    if (currentRightSideState != RightSideState.show_domination) {
                        currentRightSideState = RightSideState.show_domination;
                    }
                }
                else if (GUI.Button(second_button_rect, new GUIContent(agriculture_icon))) {
                    if (currentRightSideState != RightSideState.show_agriculture) {
						currentRightSideState = RightSideState.show_agriculture;
                    }
                }
                else if (GUI.Button(third_button_rect, new GUIContent(industry_icon))) {
                    if (currentRightSideState != RightSideState.show_industry) {
						currentRightSideState = RightSideState.show_industry;
                    }
                }
                else if (GUI.Button(fourth_button_rect, new GUIContent(commerce_icon))) {
                    if (currentRightSideState != RightSideState.show_commerce) {
						currentRightSideState = RightSideState.show_commerce;
                    }
                }
                else if (GUI.Button(fifth_button_rect, new GUIContent(military_icon))) {
                    if (currentRightSideState != RightSideState.show_military) {
						currentRightSideState = RightSideState.show_military;
                    }
                }
                else if (GUI.Button(sixth_button_rect, new GUIContent(map_icon))) {
                    if (currentRightSideState != RightSideState.show_ForeignTab) {
						currentRightSideState = RightSideState.show_ForeignTab;
                    }
                }
				else if(GUI.Button(seventh_button_rect, new GUIContent(setting_icon))) {
                    if (currentRightSideState != RightSideState.show_setting) {
                        currentRightSideState = RightSideState.show_setting;
                    }
				}
            }
            GUI.EndGroup();
		
			if(currentRightSideState == RightSideState.show_domination) {
				DrawDomination_tab();
			}
			else if (currentRightSideState == RightSideState.show_military) {
				DrawMilitaryTab();
			}
			else if (currentRightSideState == RightSideState.show_commerce) {
				DrawCommerce_tab();
			}
			else if(currentRightSideState == RightSideState.show_ForeignTab) {
				this.DrawForeignTab();
			}
        }
		GUI.EndGroup();
	}

	private void DrawForeignTab ()
	{
		GUI.BeginGroup(sidebarContentGroup_rect, GUIContent.none, GUI.skin.box);
		{
			float label_width = sidebarContentGroup_rect.width - 20;
			GUI.Box(new Rect(5, 10, label_width + 10, 40), "Foreign land", taskbarUI_Skin.box);
			
			if (GUI.Button(previousButton_rect, "<")) { }
            else if (GUI.Button(nextButton_rect, ">")) { }

			GUI.DrawTexture(showSymbol_rect, CapitalCity.list_AICity[0].symbols);
			GUI.Box(showNameOfAIcity_rect, CapitalCity.list_AICity[0].name, taskbarUI_Skin.box);
			
			GUI.BeginGroup(new Rect(5, sidebarContentGroup_rect.height - 205, sidebarContentGroup_rect.width - 10, 200));
			{
				if (GUI.Button(new Rect(5, 0, label_width, 40), "Pillage", taskbarUI_Skin.button))
				{
					if(IsShowInteruptGUI == false) {
                        this.MoveOutLeftSidebar(TaskManager.DISPLAY_FOREIGN_ACTIVITY);
					}
				}
				else if (GUI.Button(new Rect(5, 45, label_width, 40), "Conquer", taskbarUI_Skin.button)) {

				}
				GUI.Button(new Rect(5, 90, label_width, 40), "");
				GUI.Button(new Rect(5, 135, label_width, 40), "");
			} GUI.EndGroup();
		}
		GUI.EndGroup();
	}
	private void DrawDomination_tab() {
		GUI.BeginGroup(sidebarContentGroup_rect, GUIContent.none, GUI.skin.box);
		{
			float label_width = sidebarContentGroup_rect.width - 10;
            GUI.Box(new Rect(5, 2, label_width, 32), "Population", taskbarUI_Skin.textField);

			GUI.Box(new Rect(5, 100, label_width, 32), "Population : " + HouseBeh.SumOfPopulation, taskbarUI_Skin.box);
			GUI.Box(new Rect(5, 145, label_width, 32), "Employee : " + HouseBeh.SumOfEmployee, taskbarUI_Skin.box);
			GUI.Box(new Rect(5, 190, label_width, 32), "Unemployed : " + HouseBeh.SumOfUnemployed, taskbarUI_Skin.box);
		}
		GUI.EndGroup();		
	}
    private void DrawMilitaryTab() {
        GUI.BeginGroup(sidebarContentGroup_rect, GUIContent.none, GUI.skin.box);
        {
            float label_width = sidebarContentGroup_rect.width - 10;
            GUI.Box(new Rect(5, 2, label_width, 32), "troops", taskbarUI_Skin.box);

            GUI.Box(new Rect(5, 100, label_width, 32), UnitDatabase.GreekUnitDatabase.Spearman_Unit.NAME + " : " + BarracksBeh.AmountOfSpearman, taskbarUI_Skin.box);
            GUI.Box(new Rect(5, 145, label_width, 32), UnitDatabase.GreekUnitDatabase.Hapaspist_Unit.NAME + " : " + BarracksBeh.AmountOfHapaspist, taskbarUI_Skin.box);
            GUI.Box(new Rect(5, 190, label_width, 32), UnitDatabase.GreekUnitDatabase.Hoplite_Unit.NAME + " : " + BarracksBeh.AmountOfHoplite, taskbarUI_Skin.box);
        }
        GUI.EndGroup();	
    }
	private void DrawCommerce_tab() {
		GUI.BeginGroup(sidebarContentGroup_rect, GUIContent.none, GUI.skin.box);
		{
			float label_width = sidebarContentBox_rect.width;
			float label_height = sidebarContentBox_rect.height;
            GUI.Box(new Rect(5, 2, label_width, 32), new GUIContent("Commerce", commerce_icon), taskbarUI_Skin.box);
            //<!-- Food.
            if (MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[0])) {
                GUI.Box(sidebarContentBox_rect,
                    new GUIContent("Food : " + StoreHouse.SumOfFood + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
            }
            else {
                GUI.Box(sidebarContentBox_rect,
                    new GUIContent("Food : " + StoreHouse.SumOfFood + "/" + StoreHouse.SumOfMaxCapacity), taskbarUI_Skin.box);
            }
            //<!-- Wood.
            if (MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[1])) {
                GUI.Box(new Rect(sidebarContentBox_rect.x, (sidebarContentBox_rect.y + label_height) + 5, label_width, label_height),
                    new GUIContent("Wood : " + StoreHouse.SumOfWood + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
            }
            else {
                GUI.Box(new Rect(sidebarContentBox_rect.x, (sidebarContentBox_rect.y + label_height) + 5, label_width, label_height),
                    new GUIContent("Wood : " + StoreHouse.SumOfWood + "/" + StoreHouse.SumOfMaxCapacity), taskbarUI_Skin.box);
            }
			//<!-- Stone.
			if(MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[2])) {
				GUI.Box(new Rect(sidebarContentBox_rect.x, (sidebarContentBox_rect.y + (label_height + 5) * 2), label_width, label_height),
				new GUIContent("Stone : " + StoreHouse.SumOfStone + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
			}
			else {
				GUI.Box(new Rect(sidebarContentBox_rect.x, (sidebarContentBox_rect.y + (label_height + 5) * 2), label_width, label_height),
				"Stone : " + StoreHouse.SumOfStone + "/" + StoreHouse.SumOfMaxCapacity, taskbarUI_Skin.box);
			}
			//<!-- Copper.
			if(MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[3])) {
				GUI.Box(new Rect(sidebarContentBox_rect.x, sidebarContentBox_rect.y + ((label_height + 5) * 3), label_width, label_height),
                new GUIContent("Copper : " + StoreHouse.SumOfCopper + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
			}
			else {
				GUI.Box(new Rect(sidebarContentBox_rect.x, sidebarContentBox_rect.y + ((label_height + 5) * 3), label_width, label_height),
                "Copper : " + StoreHouse.SumOfCopper + "/" + StoreHouse.SumOfMaxCapacity, taskbarUI_Skin.box);
			}	
            ///<!-- Draw Weapon status.
			if(MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[5])) {
	            GUI.Box(new Rect(sidebarContentBox_rect.x, sidebarContentBox_rect.y + ((label_height + 5) * 4), label_width, label_height),
	                new GUIContent("Weapon : " + StoreHouse.sumOfWeapon + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
			}
			else {
				GUI.Box(new Rect(sidebarContentBox_rect.x, sidebarContentBox_rect.y + ((label_height + 5) * 4), label_width, label_height),
				        "Weapon : " + StoreHouse.sumOfWeapon + "/" + StoreHouse.SumOfMaxCapacity, taskbarUI_Skin.box);
			}
			///<c!-- Draw Armor Status.
			if(MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[4])) {
				GUI.Box(new Rect(sidebarContentBox_rect.x, sidebarContentBox_rect.y + ((label_height + 5) * 5), label_width, label_height),
				        new GUIContent("Armor : " + StoreHouse.sumOfArmor + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);			
			}
			else {
				GUI.Box(new Rect(sidebarContentBox_rect.x, sidebarContentBox_rect.y + ((label_height + 5) * 5), label_width, label_height),
				        new GUIContent("Armor : " + StoreHouse.sumOfArmor + "/" + StoreHouse.SumOfMaxCapacity), taskbarUI_Skin.box);		
			}
		}
		GUI.EndGroup();	
	}
*/
	#endregion

	public void CreateBuildingIconInRightSidebar (string icon_name)
    {
        #region <@-- UtilityIcons section.

        if (icon_name == UtilityIconData.HOUSE_ICON_NAME) {
			GameObject obj_icon = Instantiate(buildingIcon_prefab) as GameObject;
            obj_icon.transform.parent = buildingIcon_transform[0];
            obj_icon.transform.localPosition = Vector3.zero;
            obj_icon.name = icon_name;
            buildingsIcon_sprite[0] = obj_icon.GetComponent<tk2dSprite>();

            BuildingIconBeh iconBeh = obj_icon.GetComponent<BuildingIconBeh>();
            iconBeh.constructionArea = utilityIconData.areaOfBuildings[0];
		}
		else if(icon_name == UtilityIconData.ACADEMY_ICON_NAME) {
            GameObject academy_obj = Instantiate(buildingIcon_prefab) as GameObject;
            academy_obj.transform.parent = buildingIcon_transform[1];
            academy_obj.transform.localPosition = Vector3.zero;
            academy_obj.name = icon_name;
			buildingsIcon_sprite[1] = academy_obj.GetComponent<tk2dSprite>();
			
			BuildingIconBeh iconBeh = academy_obj.GetComponent<BuildingIconBeh>();
			iconBeh.constructionArea = utilityIconData.areaOfBuildings[1];
        }

        #endregion

        #region <@-- EconomyIcon section.

        else if(icon_name == EconomyIconData.FARM_ICON_NAME) {
			GameObject farm_obj = Instantiate(buildingIcon_prefab) as GameObject;
			farm_obj.transform.parent = buildingIcon_transform[0];
			farm_obj.transform.localPosition = Vector3.zero;
			farm_obj.name = icon_name;
			buildingsIcon_sprite[0] = farm_obj.GetComponent<tk2dSprite>();
			
			BuildingIconBeh iconBeh = farm_obj.GetComponent<BuildingIconBeh>();
			iconBeh.constructionArea = economyIconData.areaOfBuildings[0];
		}
		else if(icon_name == EconomyIconData.SAWMILL_ICON_NAME) {
			GameObject icon_obj = Instantiate(buildingIcon_prefab) as GameObject;
			icon_obj.transform.parent = buildingIcon_transform[1];
			icon_obj.transform.localPosition = Vector3.zero;
			icon_obj.name = icon_name;
			buildingsIcon_sprite[1] = icon_obj.GetComponent<tk2dSprite>();
			
			BuildingIconBeh iconBeh = icon_obj.GetComponent<BuildingIconBeh>();
			iconBeh.constructionArea = economyIconData.areaOfBuildings[1];
		}
		else if(icon_name == EconomyIconData.STONECRUSHINGPLANT_ICON_NAME) {
			GameObject icon_obj = Instantiate(buildingIcon_prefab) as GameObject;
			icon_obj.transform.parent = buildingIcon_transform[2];
			icon_obj.transform.localPosition = Vector3.zero;
			icon_obj.name = icon_name;
			buildingsIcon_sprite[2] = icon_obj.GetComponent<tk2dSprite>();
			
			BuildingIconBeh iconBeh = icon_obj.GetComponent<BuildingIconBeh>();
			iconBeh.constructionArea = economyIconData.areaOfBuildings[2];
		}
		else if(icon_name == EconomyIconData.SMELTER_ICON_NAME) {
			GameObject icon_obj = Instantiate(buildingIcon_prefab) as GameObject;
			icon_obj.transform.parent = buildingIcon_transform[3];
			icon_obj.transform.localPosition = Vector3.zero;
			icon_obj.name = icon_name;
			buildingsIcon_sprite[3] = icon_obj.GetComponent<tk2dSprite>();
			
			BuildingIconBeh iconBeh = icon_obj.GetComponent<BuildingIconBeh>();
			iconBeh.constructionArea = economyIconData.areaOfBuildings[3];
		}
		else if(icon_name == EconomyIconData.STOREHOUSE_ICON_NAME) {
			GameObject icon_obj = Instantiate(buildingIcon_prefab) as GameObject;
			icon_obj.transform.parent = buildingIcon_transform[4];
			icon_obj.transform.localPosition = Vector3.zero;
			icon_obj.name = icon_name;
			buildingsIcon_sprite[4] = icon_obj.GetComponent<tk2dSprite>();
			
			BuildingIconBeh iconBeh = icon_obj.GetComponent<BuildingIconBeh>();
			iconBeh.constructionArea = economyIconData.areaOfBuildings[4];
		}
		else if(icon_name == EconomyIconData.MARKET_ICON_NAME) {
			GameObject icon_obj = Instantiate(buildingIcon_prefab) as GameObject;
			icon_obj.transform.parent = buildingIcon_transform[5];
			icon_obj.transform.localPosition = Vector3.zero;
			icon_obj.name = icon_name;
			buildingsIcon_sprite[5] = icon_obj.GetComponent<tk2dSprite>();
			
			BuildingIconBeh iconBeh = icon_obj.GetComponent<BuildingIconBeh>();
			iconBeh.constructionArea = economyIconData.areaOfBuildings[5];
		}
		
		#endregion
		
		#region <@-- MilitaryIcon section.
		
        else if(icon_name == MilitaryIconData.BARRACKS_ICON_NAME) {
            GameObject obj_icon = Instantiate(buildingIcon_prefab) as GameObject;
            obj_icon.transform.parent = buildingIcon_transform[0];
            obj_icon.transform.localPosition = Vector3.zero;
            obj_icon.name = icon_name;
            buildingsIcon_sprite[0] = obj_icon.GetComponent<tk2dSprite>();

            BuildingIconBeh iconBeh = obj_icon.GetComponent<BuildingIconBeh>();
            iconBeh.constructionArea =  militaryIconData.areaOfBuildings[0];
        }
		
		#endregion
	}

    internal void ShowWarnningCannotCreateBuilding()
    {
        Debug.LogWarning("Cannot create building, Your construction queue is full.");
    }
}
