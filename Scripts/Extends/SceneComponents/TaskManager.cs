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

    public static bool IsShowInteruptGUI = false;
    public enum RightSideState { 
        none = 0, 
        show_domination, 
        show_agriculture, 
        show_industry,
        show_commerce,
        show_military,
        show_map, 
        show_setting,
    };
    public RightSideState currentRightSideState = RightSideState.show_domination;
    private StageManager stageManager;
	private DisplayTroopsActivity displayTroopsActivity;
    private MessageManager messageManager;

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
    Rect sidebarButtonGroup_rect = new Rect(0, 0, 50, Main.GAMEHEIGHT);
    Rect sidebarContentGroup_rect;
	Rect sidebarContentBox_rect;
    Rect first_button_rect = new Rect(1, 2, 48, 56);
    Rect second_button_rect = new Rect(1, 60, 48, 56);
    Rect third_button_rect = new Rect(1, 120, 48, 56);
    Rect fourth_button_rect = new Rect(1, 180, 48, 56);
    Rect fifth_button_rect = new Rect(1, 240, 48, 56);
    Rect sixth_button_rect = new Rect(1, 300, 48, 56);
    Rect seventh_button_rect;
	//<@--- world map section.
	private Rect showSymbol_rect;
	private Rect showNameOfAIcity_rect;
    private Rect previousButton_rect;
    private Rect nextButton_rect;

    public Rect standardWindow_rect;
    public Rect exitButton_Rect;

    #endregion

    #region <@-- Events.

    #endregion


    void Awake() {
		TaskManager.IsShowInteruptGUI = false;

		GameObject gamecontroller = GameObject.FindGameObjectWithTag("GameController");
		stageManager = gamecontroller.GetComponent<StageManager>();
		displayTroopsActivity = gamecontroller.GetComponent<DisplayTroopsActivity>();

        gamecontroller.AddComponent<MessageManager>();
        messageManager = gamecontroller.GetComponent<MessageManager>();
	}
	
	// Use this for initialization
	IEnumerator Start () 
    {
        StartCoroutine(InitializeTextureResource());
        this.InitializeOnGUIDataFields();
		
        yield return 0;
    }
	
    void InitializeOnGUIDataFields()
    {
        taskbarUI_Skin.button.alignment = TextAnchor.MiddleCenter;
		taskbarUI_Skin.box.alignment = TextAnchor.MiddleCenter;
				
        baseSidebarGroup_rect = new Rect(Screen.width - (Screen.width / 4), 0, Screen.width / 4, Main.GAMEHEIGHT - 240);
        sidebarContentGroup_rect = new Rect(48 * Mz_OnGUIManager.Extend_heightScale, 0, baseSidebarGroup_rect.width - (48 * Mz_OnGUIManager.Extend_heightScale), baseSidebarGroup_rect.height);
        sidebarContentBox_rect = new Rect(5, 50, sidebarContentGroup_rect.width - 10, 32);
		
        header_group_rect = new Rect(0, 0, Screen.width - baseSidebarGroup_rect.width, 40);
		header_button_rect = new Rect(0, 0, header_group_rect.width / 5, 40);

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
        nextButton_rect = new Rect((showSymbol_rect.x + showSymbol_rect.width) + (32 * Mz_OnGUIManager.Extend_heightScale), 85, 32 * Mz_OnGUIManager.Extend_heightScale, 32);

        standardWindow_rect = new Rect((Screen.width * 3 / 4) / 2 - (350 * Mz_OnGUIManager.Extend_heightScale), Main.GAMEHEIGHT / 2 - 250, 700 * Mz_OnGUIManager.Extend_heightScale, 500);
        exitButton_Rect = new Rect(standardWindow_rect.width - (34 * Mz_OnGUIManager.Extend_heightScale), 2, 32 * Mz_OnGUIManager.Extend_heightScale, 32);

        if (Screen.height != Main.GAMEHEIGHT) {			
		    first_button_rect =  MzReCalculateScaleRectGUI.ReCalulateWidth(first_button_rect);
            second_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(second_button_rect);
            third_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(third_button_rect);
            fourth_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(fourth_button_rect);
            fifth_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(fifth_button_rect);
            sixth_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(sixth_button_rect);
        }
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
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));

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
		
		#region <@!-- ForeignTabStatus.
		
		if(currentForeignTabStatus == ForeignTabStatus.DrawActivity) {
			standardWindow_rect = GUI.Window(0, standardWindow_rect, DrawActivityWindow, new GUIContent("Select troops"));
		}		
		if(currentRightSideState != RightSideState.show_map && currentForeignTabStatus != ForeignTabStatus.None) {
			currentForeignTabStatus = ForeignTabStatus.None;
			IsShowInteruptGUI = false;
		}
		
		#endregion
    }
	
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
                    if (currentRightSideState != RightSideState.show_map) {
						currentRightSideState = RightSideState.show_map;
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
			else if(currentRightSideState == RightSideState.show_map) {
				this.DrawForeignTab();
			}
        }
		GUI.EndGroup();
	}

    private enum ForeignTabStatus
    {
        None = 0, DrawActivity = 1,
    };
    private ForeignTabStatus currentForeignTabStatus;
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
                        currentForeignTabStatus = ForeignTabStatus.DrawActivity;

						IsShowInteruptGUI = true;
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
	
	private Rect citiesSymbol_rect = new Rect(24 * Mz_OnGUIManager.Extend_heightScale, 24, 100 * Mz_OnGUIManager.Extend_heightScale, 100);
	private Rect citiesTagName_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 130, 120 * Mz_OnGUIManager.Extend_heightScale, 32);
    private Rect sendButton_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 170, 120 * Mz_OnGUIManager.Extend_heightScale, 32);
    private Rect selectTroopBox_rect = new Rect(150 * Mz_OnGUIManager.Extend_heightScale, 40, 545 * Mz_OnGUIManager.Extend_heightScale, 450);
    private Rect drawUnit_00_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 10, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
    private Rect selectUnitBoxRect_00 = new Rect(70 * Mz_OnGUIManager.Extend_heightScale, 24, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
    private Rect maxUnitButtonRect_00 = new Rect(130 * Mz_OnGUIManager.Extend_heightScale, 20, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
    private Rect drawUnitRect_01 = new Rect(240 * Mz_OnGUIManager.Extend_heightScale, 10, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
    private Rect selectUnitBoxRect_01 = new Rect(300 * Mz_OnGUIManager.Extend_heightScale, 24, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
    private Rect maxUnitButtonRect_01 = new Rect(360 * Mz_OnGUIManager.Extend_heightScale, 20, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
    private Rect drawUnitRect_10 = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 80, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
    private Rect selectUnitBoxRect_10 = new Rect(70 * Mz_OnGUIManager.Extend_heightScale, 94, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
    private Rect maxUnitButtonRect_10 = new Rect(130 * Mz_OnGUIManager.Extend_heightScale, 90, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
    private Rect drawUnitRect_11 = new Rect(240 * Mz_OnGUIManager.Extend_heightScale, 80, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
    private Rect selectUnitBoxRect_11 = new Rect(300 * Mz_OnGUIManager.Extend_heightScale, 94, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
    private Rect maxUnitButtonRect_11 = new Rect(360 * Mz_OnGUIManager.Extend_heightScale, 90, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
    private string numberOFUnit_00 = string.Empty; 
    private string numberOFUnit_01 = string.Empty;
    private string numberOFUnit_02 = string.Empty;

    private void DrawActivityWindow(int id)
    {
        //<!-- Exit Button.
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskbarUI_Skin.customStyles[6])) {
            CloseGUIWindow();
        }
		
		GUI.BeginGroup(selectTroopBox_rect, "Pillage", taskbarUI_Skin.box); {
			GUI.DrawTexture(drawUnit_00_rect, spearmanUnitIcon);
            numberOFUnit_00 = GUI.TextField(selectUnitBoxRect_00, numberOFUnit_00, 3, taskbarUI_Skin.textField);
            if (GUI.Button(maxUnitButtonRect_00, BarracksBeh.AmountOfSpearman.ToString())) {
                numberOFUnit_00 = BarracksBeh.AmountOfSpearman.ToString();
            }

            GUI.DrawTexture(drawUnitRect_01, hypaspistUnitIcon);
            GUI.TextField(selectUnitBoxRect_01, "0", 3, taskbarUI_Skin.textField);
            if (GUI.Button(maxUnitButtonRect_01, BarracksBeh.AmountOfHapaspist.ToString())) {
                numberOFUnit_01 = BarracksBeh.AmountOfHapaspist.ToString();
            }

            GUI.DrawTexture(drawUnitRect_10, hopliteUnitIcon);
            GUI.TextField(selectUnitBoxRect_10, "0", 3, taskbarUI_Skin.textField);
            if (GUI.Button(maxUnitButtonRect_10, BarracksBeh.AmountOfHoplite.ToString())) {
                numberOFUnit_02 = BarracksBeh.AmountOfHoplite.ToString();
            }

            GUI.DrawTexture(drawUnitRect_11, ToxotesUnitIcon);
            GUI.TextField(selectUnitBoxRect_11, "0", 3, taskbarUI_Skin.textField);
            GUI.Button(maxUnitButtonRect_11, "Max");
		}
		GUI.EndGroup();
        
        /// Draw cities symbol.
        GUI.DrawTexture(citiesSymbol_rect, StageManager.list_AICity[0].symbols);
        GUI.Box(citiesTagName_rect, StageManager.list_AICity[0].name);

        if (GUI.Button(sendButton_rect, "Send")) {
            try{
                int unit_0 = numberOFUnit_00 != string.Empty ? int.Parse(numberOFUnit_00) : 0;
                int unit_1 = numberOFUnit_01 != string.Empty ? int.Parse(numberOFUnit_01) : 0;
                int unit_2 = numberOFUnit_02 != string.Empty ? int.Parse(numberOFUnit_02) : 0;

                GroupOFUnitBeh groupTemp = new GroupOFUnitBeh();
                groupTemp.unitName.Add(UnitDataStore.GreekUnitData.Spearman);
				groupTemp.unitName.Add(UnitDataStore.GreekUnitData.Hapaspist);
				groupTemp.unitName.Add(UnitDataStore.GreekUnitData.Hoplite);
                groupTemp.member.Add(unit_0);
				groupTemp.member.Add(unit_1);
				groupTemp.member.Add(unit_2);

                if(unit_0 + unit_1 + unit_2 > 0) {
					displayTroopsActivity.MilitaryActivityList.Add(new TroopsActivity() {
						currentTroopsStatus = TroopsActivity.TroopsStatus.Pillage,
                        targetCity = StageManager.list_AICity[0],
                        timeToTravel = System.TimeSpan.FromSeconds(StageManager.list_AICity[0].distance),
                        startTime = System.DateTime.UtcNow,
						groupUnits = groupTemp,
					});
					
					Debug.Log ("displayTroopsActivity.MilitaryActivityList.Count : " + displayTroopsActivity.MilitaryActivityList.Count);
					
                    CloseGUIWindow();
                }
            }catch {
				
            }finally {
                numberOFUnit_00 = string.Empty;
                numberOFUnit_01 = string.Empty;
                numberOFUnit_02 = string.Empty;
            }
        }
    }
    private void CloseGUIWindow()
    {
        currentForeignTabStatus = ForeignTabStatus.None;
        IsShowInteruptGUI = false;
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
            GUI.Box(new Rect(5, 2, label_width, 32), "Commerce", taskbarUI_Skin.textField);
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
}
