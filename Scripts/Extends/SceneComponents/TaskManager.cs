using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TaskManager : MonoBehaviour {
	
	public const string PathOfGUISprite = "UI_Sprites/";
    public const string PathOfMainGUI = "Textures/MainGUI/";
    public const string PathOfGameItemTextures = "Textures/GameItems/";
    public const string Advisor_ResourcePath = "Textures/Advisors/";
    public const string PathOfTribes_Texture = "Textures/Tribes_Icons/";
    public const string PathOf_TroopIcons = "Textures/Troop_Icons/";

	internal const string DISPLAY_MESSAGE_ACTIVITY = "DisplayMessageActivity";
    internal const string DISPLAY_QUEST_ACTIVITY = "DisplayQuestActivity";
    internal const string DISPLAY_FOREIGN_ACTIVITY = "DisplayForeignActivity";

    public static bool IsShowInteruptGUI = false;
    public enum RightSideState { 
        none = 0, 
        show_domination, 
        show_agriculture, 
        show_industry,
        show_commerce,
        show_military,
        show_ForeignTab, 
        show_setting,
    };
    public RightSideState currentRightSideState = RightSideState.show_domination;
    private StageManager stageManager;
	public DisplayTroopsActivity displayTroopsActivity;
    private MessageManager messageManager;
    private ForeignManager foreignManager;
    public QuestManager questManager;

    public GUISkin taskbarUI_Skin;
    public GUIStyle left_button_Style;
    public GUIStyle right_button_Style;

    #region <@!-- Textures.

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

    public Texture2D elder_advisor;
	public Texture2D messageFormSystem_icon;
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

    #endregion

    #region <@!-- Rectangles.

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

    #endregion

    #region <@-- Events.

    #endregion

	
	// Use this for initialization
	IEnumerator Start ()
    {
        TaskManager.IsShowInteruptGUI = false;

        GameObject gamecontroller = GameObject.FindGameObjectWithTag("GameController");
        stageManager = gamecontroller.GetComponent<StageManager>();
        messageManager = gamecontroller.GetComponent<MessageManager>();
        foreignManager = gamecontroller.GetComponent<ForeignManager>();
        displayTroopsActivity = gamecontroller.GetComponent<DisplayTroopsActivity>();
        questManager = gamecontroller.GetComponent<QuestManager>();

        StartCoroutine(InitializeTextureResource());
        this.InitializeOnGUIDataFields();
		
        yield return 0;
    }
	
    void InitializeOnGUIDataFields()
    {
        taskbarUI_Skin.button.alignment = TextAnchor.MiddleCenter;
		taskbarUI_Skin.box.alignment = TextAnchor.MiddleCenter;
				
        baseSidebarGroup_rect = new Rect(Screen.width - (Screen.width / 4), 0, Screen.width / 4, Main.FixedGameHeight - 240);
        sidebarContentGroup_rect = new Rect(48 * Mz_OnGUIManager.Extend_heightScale, 0, baseSidebarGroup_rect.width - (48 * Mz_OnGUIManager.Extend_heightScale), baseSidebarGroup_rect.height);
        sidebarContentBox_rect = new Rect(5, 50, sidebarContentGroup_rect.width - 10, 32);
		
        header_group_rect = new Rect(0, 0, Screen.width - baseSidebarGroup_rect.width, 50);
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

        standardWindow_rect = new Rect((Screen.width * 3 / 4) / 2 - (350 * Mz_OnGUIManager.Extend_heightScale), Main.FixedGameHeight / 2 - 250, 700 * Mz_OnGUIManager.Extend_heightScale, 500);
        exitButton_Rect = new Rect(standardWindow_rect.width - (34 * Mz_OnGUIManager.Extend_heightScale), 2, 32 * Mz_OnGUIManager.Extend_heightScale, 32);

        //<@-- Notification display rectangles.
        moveOutLeftSidebarGroup_rect = new Rect(-100, 50, 90 * Mz_OnGUIManager.Extend_heightScale, 500);
        normalSidebarGroup_rect = new Rect(0, 50, 90 * Mz_OnGUIManager.Extend_heightScale, 500);
        leftSidebarGroup_rect = normalSidebarGroup_rect;
        notificationRect_1 = new Rect(0, 5, 80 * Mz_OnGUIManager.Extend_heightScale, 80);
        notificationRect_2 = new Rect(0, 90, 80 * Mz_OnGUIManager.Extend_heightScale, 80);
    }
	
    IEnumerator InitializeTextureResource() 
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

        elder_advisor = Resources.Load(Advisor_ResourcePath + "VillageElder", typeof(Texture2D)) as Texture2D;
		messageFormSystem_icon = Resources.Load(PathOfMainGUI + "MessageIcon", typeof(Texture2D)) as Texture2D;
        //<!-- Load troop icon.
        spearmanUnitIcon = Resources.Load(PathOf_TroopIcons + "Spearman", typeof(Texture2D)) as Texture2D;
        hypaspistUnitIcon = Resources.Load(PathOf_TroopIcons + "Hypaspist", typeof(Texture2D)) as Texture2D;
        hopliteUnitIcon = Resources.Load(PathOf_TroopIcons + "Hoplite", typeof(Texture2D)) as Texture2D;
        ToxotesUnitIcon = Resources.Load(PathOf_TroopIcons + "Toxotai", typeof(Texture2D)) as Texture2D;

        yield return 0;
    }

	// Update is called once per frame
	void Update () { }

    void OnGUI()
    {		
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));

        GUI.BeginGroup(header_group_rect);
        {
            GUI.Box(first_rect, new GUIContent(StoreHouse.SumOfFood + "/" + StoreHouse.SumOfMaxCapacity, food_icon), taskbarUI_Skin.button);
            GUI.Box(second_rect, new GUIContent(StoreHouse.SumOfWood + "/" + StoreHouse.SumOfMaxCapacity, wood_icon), taskbarUI_Skin.button);
            GUI.Box(third_rect, new GUIContent(StoreHouse.SumOfStone + "/" + StoreHouse.SumOfMaxCapacity, stone_icon), taskbarUI_Skin.button);
            GUI.Box(fourth_rect, new GUIContent(StoreHouse.sumOfGold.ToString(), gold_icon), taskbarUI_Skin.button);
            GUI.Box(fifth_rect, new GUIContent(HouseBeh.SumOfPopulation.ToString(), employee_icon), taskbarUI_Skin.button);
        }
        GUI.EndGroup();
		
		this.DrawRightSidebar();
        this.DrawLeftSideBar();
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

    private void DisplayMessageActivity()
    {
        messageManager.currentMessageManagerState = MessageManager.MessageManagetStateBeh.drawActivity;
    }

    private void DisplayQuestActivity() {
        questManager.currentQuestManagerStateBeh = QuestManager.QuestManagerStateBeh.DrawActivity;
    }

    private void DisplayForeignActivity() {
        foreignManager.currentForeignTabStatus = ForeignManager.ForeignTabStatus.DrawActivity;
    }

    #endregion

	#region <@-- Right sidebar group rect.

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
			else if (currentRightSideState == RightSideState.show_setting) {
				this.DrawSettingTab();
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
			
			if (GUI.Button(previousButton_rect, "", left_button_Style)) { }
            else if (GUI.Button(nextButton_rect, "", right_button_Style)) { }

			GUI.DrawTexture(showSymbol_rect, StageManager.list_AICity[0].symbols);
			GUI.Box(showNameOfAIcity_rect, StageManager.list_AICity[0].name, taskbarUI_Skin.box);
			
			GUI.BeginGroup(new Rect(5, sidebarContentGroup_rect.height - 205, sidebarContentGroup_rect.width - 10, 200));
			{
				if (GUI.Button(new Rect(5, 0, label_width, 40), "Pillage"))
				{
					if(IsShowInteruptGUI == false) {
                        this.MoveOutLeftSidebar(TaskManager.DISPLAY_FOREIGN_ACTIVITY);
					}
				}
				else if (GUI.Button(new Rect(5, 45, label_width, 40), "Conquer")) {

				}
				GUI.Button(new Rect(5, 90, label_width, 40), "");
				GUI.Button(new Rect(5, 135, label_width, 40), "");
			} GUI.EndGroup();
		}
		GUI.EndGroup();
	}
    private void DrawSettingTab()
    {
		GUI.BeginGroup(sidebarContentGroup_rect, GUIContent.none, GUI.skin.box);
		{
			float label_width = sidebarContentGroup_rect.width - 10;
            GUI.Box(new Rect(5, 2, label_width, 32), "Options", taskbarUI_Skin.textField);

            if (GUI.Button(new Rect(5, 100, label_width, 32), "Main Menu")) {
                Mz_SaveData.Save();
                stageManager.OnDispose();
                if (Application.isLoadingLevel == false) {
                    Mz_LoadingScreen.TargetSceneName = Mz_BaseScene.ScenesInstance.MainMenu.ToString();
                    Application.LoadLevel(Mz_BaseScene.ScenesInstance.LoadingScreen.ToString());
                }
            }

			if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
	            if (GUI.Button(new Rect(5, 145, label_width, 32), "Quit")) {
	                Mz_SaveData.Save();
	                stageManager._hasQuitCommand = true;
	            }
			}
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

            GUI.Box(new Rect(5, 100, label_width, 32), UnitDataStore.GreekUnitData.Spearman + " : " + BarracksBeh.AmountOfSpearman, taskbarUI_Skin.box);
            GUI.Box(new Rect(5, 145, label_width, 32), UnitDataStore.GreekUnitData.Hapaspist + " : " + BarracksBeh.AmountOfHapaspist, taskbarUI_Skin.box);
            GUI.Box(new Rect(5, 190, label_width, 32), UnitDataStore.GreekUnitData.Hoplite + " : " + BarracksBeh.AmountOfHoplite, taskbarUI_Skin.box);
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

	#endregion
}
