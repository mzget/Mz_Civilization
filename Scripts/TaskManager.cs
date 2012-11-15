using UnityEngine;
using System.Collections;

public class TaskManager : MonoBehaviour {
	
	public const string PathOfGUISprite = "UI_Sprites/";
    public const string PathOfMainGUIResource = "Textures/MainGUI/";
    public const string PathOfGameItemTextures = "Textures/GameItems/";
    public const string Advisor_ResourcePath = "Textures/Advisors/";

    public static bool IsShowInteruptGUI = false;
//	public static bool IsShowSidebarGUI = false;
    public enum RightSideState { none = 0, show_domination, show_agriculture, show_industry, show_commerce, show_military, show_map, show_setting, };
    public RightSideState currentRightSideState = RightSideState.show_domination;
    private StageManager stageManager;

    public GUISkin taskbarUI_Skin;
    public GUIStyle left_button_Style;
    public GUIStyle right_button_Style;
	
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

    protected Rect header_group_rect;
    protected Rect header_button_rect;
    //private Rect baseSidebarGroup_rect = new Rect(Main.GAMEWIDTH- 50, 0, 50, Main.GAMEHEIGHT);
    Rect baseSidebarGroup_rect;
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

	
	void Awake() {
		TaskManager.IsShowInteruptGUI = false;
	}
	
	// Use this for initialization
	IEnumerator Start () 
    {
        var gamecontroller = GameObject.FindGameObjectWithTag("GameController");
        stageManager = gamecontroller.GetComponent<StageManager>();

        this.InitializeOnGUIData();
        StartCoroutine(InitializeTextureResource());

#if UNITY_WEBPLAYER || UNITY_EDITOR
        StartCoroutine(InitializeJoystick());
#endif
		
        yield return 0;
    }
	
    private void InitializeOnGUIData()
    {
        taskbarUI_Skin.button.alignment = TextAnchor.MiddleCenter;
		taskbarUI_Skin.box.alignment = TextAnchor.MiddleCenter;
		
		seventh_button_rect = new Rect(1 * Mz_GUIManager.Extend_heightScale, 360, 48 * Mz_GUIManager.Extend_heightScale, 56);
		
        if (Screen.height != Main.GAMEHEIGHT) {			
		    first_button_rect =  MzReCalculateScaleRectGUI.ReCalulateWidth(first_button_rect);
            second_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(second_button_rect);
            third_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(third_button_rect);
            fourth_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(fourth_button_rect);
            fifth_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(fifth_button_rect);
            sixth_button_rect = MzReCalculateScaleRectGUI.ReCalulateWidth(sixth_button_rect);
        }
		
        baseSidebarGroup_rect = new Rect(Screen.width - (Screen.width / 4), 0, Screen.width / 4, Main.GAMEHEIGHT - 240);
        sidebarContentGroup_rect = new Rect(first_button_rect.width, 0, baseSidebarGroup_rect.width - first_button_rect.width, baseSidebarGroup_rect.height);
        sidebarContentBox_rect = new Rect(5, 50, sidebarContentGroup_rect.width - 10, 32);
		
        header_group_rect = new Rect(0, 0, Screen.width - baseSidebarGroup_rect.width, 40);
		header_button_rect = new Rect(0, 0, header_group_rect.width / 5, 40);
    }
	
    IEnumerator InitializeTextureResource() 
    {
        left_button_Style.normal.background = Resources.Load(PathOfMainGUIResource + "Back_up", typeof(Texture2D)) as Texture2D;
        left_button_Style.active.background = Resources.Load(PathOfMainGUIResource + "Back_down", typeof(Texture2D)) as Texture2D;
        right_button_Style.normal.background = Resources.Load(PathOfMainGUIResource + "Next_up", typeof(Texture2D)) as Texture2D;
        right_button_Style.active.background = Resources.Load(PathOfMainGUIResource + "Next_down", typeof(Texture2D)) as Texture2D;

        food_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Grain", typeof(Texture2D)) as Texture2D;
        wood_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "PinePlanks", typeof(Texture2D)) as Texture2D;
        stone_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "StoneBlock", typeof(Texture2D)) as Texture2D;
        copper_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "CopperIngot", typeof(Texture2D)) as Texture2D;
        armor_icon = Resources.Load(PathOfGameItemTextures + "Armor", typeof(Texture2D)) as Texture2D;
		weapon_icon = Resources.Load(PathOfGameItemTextures + "Weapon", typeof(Texture2D)) as Texture2D;
		
        gold_icon = Resources.Load(PathOfMainGUIResource + "GoldIngot", typeof(Texture2D)) as Texture2D;
        employee_icon = Resources.Load(PathOfMainGUIResource + "Employee_icon", typeof(Texture2D)) as Texture2D;
		
		domination_icon = Resources.Load(PathOfMainGUIResource + "Domination", typeof(Texture2D)) as Texture2D;
		agriculture_icon = Resources.Load(PathOfMainGUIResource + "Agriculture", typeof(Texture2D)) as Texture2D;
		industry_icon = Resources.Load(PathOfMainGUIResource + "Industry", typeof(Texture2D)) as Texture2D;
		commerce_icon = Resources.Load(PathOfMainGUIResource + "Commerce", typeof(Texture2D)) as Texture2D;
		military_icon = Resources.Load(PathOfMainGUIResource + "Military", typeof(Texture2D)) as Texture2D;
        map_icon = Resources.Load(PathOfMainGUIResource + "Map_Texture", typeof(Texture2D)) as Texture2D;
		setting_icon = Resources.Load(PathOfMainGUIResource + "Setting", typeof(Texture2D)) as Texture2D;

        marketTradingIcon = Resources.Load(PathOfMainGUIResource + "Market", typeof(Texture2D)) as Texture2D;

        elder_advisor = Resources.Load(Advisor_ResourcePath + "VillageElder", typeof(Texture2D)) as Texture2D;

        yield return 0;
    }

    public GameObject joystick_base_obj;
    public GameObject joystick_obj;
	public JoystickManager joystickManager;
	private float moveCamSpeed;
    private IEnumerator InitializeJoystick()
    {
        joystick_base_obj = Instantiate(Resources.Load(Mz_BaseScene.ResourcePathName.PathOfGUI_PREFABS + "GUI_Joystickbase", typeof(GameObject))) as GameObject;
        joystick_obj = Instantiate(Resources.Load(Mz_BaseScene.ResourcePathName.PathOfGUI_PREFABS + "GUI_Joystick", typeof(GameObject))) as GameObject;
		
        yield return 0;
    }
	
	// Update is called once per frame
	void Update () {
		if(joystick_obj != null) {
			if(joystickManager != null)
				this.UpdateJoystick();
			else
				joystickManager = joystick_obj.GetComponent<JoystickManager>();
		}

        #region <!-- Detech when used keybroad input.

        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (Camera.main.transform.position.x > -640)
                Camera.main.transform.Translate(Vector3.left * moveCamSpeed);
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            if (Camera.main.transform.position.x < 640)
                Camera.main.transform.Translate(Vector3.right * moveCamSpeed);
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            if (Camera.main.transform.position.y < 400)
                Camera.main.transform.Translate(Vector3.up * moveCamSpeed);
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            if (Camera.main.transform.position.y > -400)
                Camera.main.transform.Translate(Vector3.down * moveCamSpeed);
        }

        #endregion
	}
	
	void UpdateJoystick() {
		moveCamSpeed = Time.deltaTime * 360f;
		
		if(joystickManager.joystick.touchCount != 0) {
			if(joystickManager.joystick._isMoveGUI) {
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
	
	public void OnInput(string Inputname) {
		Debug.Log("GUI_Manager.OnInput : " + Inputname);
		
		if(Inputname == "Left_button") {
			if(Camera.main.transform.position.x > -640)
				Camera.main.transform.Translate(Vector3.left * moveCamSpeed);
		}
		else if(Inputname == "Right_button") {
			if(Camera.main.transform.position.x < 640)
				Camera.main.transform.Translate(Vector3.right * moveCamSpeed);
		}
		
		if(Inputname == "Up_button") {
            if(Camera.main.transform.position.y < 400)
                Camera.main.transform.Translate(Vector3.up * moveCamSpeed);
		}
		else if(Inputname == "Down_button") {
            if(Camera.main.transform.position.y > -400)
                Camera.main.transform.Translate(Vector3.down * moveCamSpeed);
        }
    }

    void OnGUI()
    {		
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));
		
		#region <!-- Header group.

        Rect first_rect = new Rect(0, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        Rect second_rect = new Rect((header_button_rect.width) * 1, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        Rect third_rect = new Rect((header_button_rect.width) * 2, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        Rect fourth_rect = new Rect((header_button_rect.width) * 3, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        Rect fifth_rect = new Rect((header_button_rect.width) * 4, header_button_rect.y, header_button_rect.width, header_button_rect.height);
        Rect sixth_rect = new Rect((header_button_rect.width) * 5, header_button_rect.y, header_button_rect.width, header_button_rect.height);

        GUI.BeginGroup(header_group_rect);
        {
            GUI.Box(first_rect, new GUIContent(StoreHouse.sumOfFood + "/" + StoreHouse.SumOfMaxCapacity, food_icon), taskbarUI_Skin.button);
            GUI.Box(second_rect, new GUIContent(StoreHouse.sumOfWood + "/" + StoreHouse.SumOfMaxCapacity, wood_icon), taskbarUI_Skin.button);
            GUI.Box(third_rect, new GUIContent(StoreHouse.sumOfStone + "/" + StoreHouse.SumOfMaxCapacity, stone_icon), taskbarUI_Skin.button);
            GUI.Box(fourth_rect, new GUIContent(StoreHouse.sumOfGold.ToString(), gold_icon), taskbarUI_Skin.button);
            GUI.Box(fifth_rect, new GUIContent(HouseBeh.SumOfPopulation.ToString(), employee_icon), taskbarUI_Skin.button);
        }
        GUI.EndGroup();
		
		#endregion
		
		this.DrawRightSidebar();
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
            else if (currentRightSideState == RightSideState.show_commerce)
            {
                DrawCommerce_tab();
            }
            else if (currentRightSideState == RightSideState.show_setting)
            {
                this.DrawSettingTab();
            }

            #region <!--- show_Map.

            if(currentRightSideState == RightSideState.show_map) {                
				GUI.BeginGroup(sidebarContentGroup_rect, GUIContent.none, GUI.skin.box);
				{
					float label_width = sidebarContentGroup_rect.width - 20;
                    GUI.Box(new Rect(10, 10, label_width, 40), "Map", taskbarUI_Skin.box);

                    //GUI.Box(new Rect(10, 100, label_width, 40), "Population : " + HouseBeh.SumOfPopulation, taskbarUI_Skin.box);
                    //GUI.Box(new Rect(10, 145, label_width, 40), "Employee : " + HouseBeh.SumOfEmployee, taskbarUI_Skin.box);
                    //GUI.Box(new Rect(10, 190, label_width, 40), "Unemployee : " + HouseBeh.SumOfUnemployed, taskbarUI_Skin.box);
				}
				GUI.EndGroup();
            }

            #endregion
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
			else if(Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer) {
				if(GUI.Button(new Rect(5, 145, label_width, 32), "Fullscreen")) {
					Screen.fullScreen = !Screen.fullScreen;
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
    void DrawMilitaryTab() {
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
                    new GUIContent("Food : " + StoreHouse.sumOfFood + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
            }
            else {
                GUI.Box(sidebarContentBox_rect,
                    new GUIContent("Food : " + StoreHouse.sumOfFood + "/" + StoreHouse.SumOfMaxCapacity), taskbarUI_Skin.box);
            }
            //<!-- Wood.
            if (MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[1])) {
                GUI.Box(new Rect(sidebarContentBox_rect.x, (sidebarContentBox_rect.y + label_height) + 5, label_width, label_height),
                    new GUIContent("Wood : " + StoreHouse.sumOfWood + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
            }
            else {
                GUI.Box(new Rect(sidebarContentBox_rect.x, (sidebarContentBox_rect.y + label_height) + 5, label_width, label_height),
                    new GUIContent("Wood : " + StoreHouse.sumOfWood + "/" + StoreHouse.SumOfMaxCapacity), taskbarUI_Skin.box);
            }
			//<!-- Stone.
			if(MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[2])) {
				GUI.Box(new Rect(sidebarContentBox_rect.x, (sidebarContentBox_rect.y + (label_height + 5) * 2), label_width, label_height),
				new GUIContent("Stone : " + StoreHouse.sumOfStone + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
			}
			else {
				GUI.Box(new Rect(sidebarContentBox_rect.x, (sidebarContentBox_rect.y + (label_height + 5) * 2), label_width, label_height),
				"Stone : " + StoreHouse.sumOfStone + "/" + StoreHouse.SumOfMaxCapacity, taskbarUI_Skin.box);
			}
			//<!-- Copper.
			if(MarketBeh.tradingMaterial_List.Count != 0 && MarketBeh.tradingMaterial_List.Contains(stageManager.gameMaterials[3])) {
				GUI.Box(new Rect(sidebarContentBox_rect.x, sidebarContentBox_rect.y + ((label_height + 5) * 3), label_width, label_height),
                new GUIContent("Copper : " + StoreHouse.sumOfCopper + "/" + StoreHouse.SumOfMaxCapacity, marketTradingIcon), taskbarUI_Skin.box);
			}
			else {
				GUI.Box(new Rect(sidebarContentBox_rect.x, sidebarContentBox_rect.y + ((label_height + 5) * 3), label_width, label_height),
                "Copper : " + StoreHouse.sumOfCopper + "/" + StoreHouse.SumOfMaxCapacity, taskbarUI_Skin.box);
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
