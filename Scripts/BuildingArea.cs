using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingArea : MonoBehaviour 
{
    /// GUI : Texture && Skin.
    private GUIStyle tagname_Style;
    public GUISkin standard_skin;
    public GUISkin buildingArea_Skin;
	public GUISkin taskbarUI_Skin;
    //<!--- Static GameObject, Prefabs, Texture2D,
    public Texture2D utility_icon;
    public Texture2D economy_icon;
    public Texture2D military_icon;
    //<!--- Utility.
    public Texture2D house_icon;
    public Texture2D guardTower_icon;
    public Texture2D academy_icon;
    //<!--- Economic.
    public Texture2D market_icon;
    public Texture2D storeHouse_icon;
    public Texture2D farm_Icon;
    public Texture2D sawmill_Icon;
    public Texture2D millStone_Icon;
    public Texture2D smelter_icon;
    //<!--- Militaly.
    public Texture2D barrackNotation;

    private OTSprite sprite;
    public OTSprite Sprite {
        get {
            if (sprite == null) {
                sprite = this.gameObject.GetComponent<OTSprite>();
                return sprite;
            }
            else 
                return sprite;
        }
        set { sprite = value; }
    }
	private int indexOfAreaPosition;
    public int IndexOfAreaPosition { get { return indexOfAreaPosition; } set { indexOfAreaPosition = value; } }
    private enum GUIState { none = 0, ShowMainUI, ShowUtiltyUI, ShowEconomyUI, ShowMilitaryUI, ShowBuyArea, };
    public enum AreaState { In_Active = 0, De_Active, };
    public AreaState areaState; 
    private GUIState guiState;
    private StageManager stageManager;
	private static BuildingBeh building;

    private Rect window_rect;
	Rect buyArea_rect;
    private Rect exitButton_rect;
    private Vector2 scrollPosition = Vector2.zero;
    private Rect background_Rect;
    private Rect image_rect;
    private Rect tagName_rect;
    private Rect content_rect;
    private Rect createButton_rect;
    private Rect economy_rect;
    private Rect utility_rect;
    private Rect military_rect;
	
	Rect positionOfScrolling;
	Rect viewRectOfScrolling;
	
    float frameHeight = 160;
	Rect firstBuilding_rect;
	Rect secondBuilding_rect;
	Rect thirdBuilding_rect;
	Rect fourthBuilding_rect;
	Rect fifthBuilding_rect;
	Rect sixthBuilding_rect;


	// Use this for initialization
	void Start () 
	{
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        sprite = this.gameObject.GetComponent<OTSprite>();
		//<!--- Static building type.
		GameObject towncenter = GameObject.Find("TownCenter");
        building = towncenter.GetComponent<BuildingBeh>();

        this.InitializeGUI();
        StartCoroutine(this.LoadTextureResource());

        if(areaState == AreaState.De_Active) {            
		    sprite = this.gameObject.GetComponent<OTSprite>();
		    sprite.materialReference = "tint";
		    sprite.tintColor = Color.gray;
			
            return;
        }
	}
	
    private void InitializeGUI()
    {
        tagname_Style = new GUIStyle();
        tagname_Style.alignment = TextAnchor.MiddleCenter;
        tagname_Style.normal.textColor = Color.white;
        tagname_Style.font = buildingArea_Skin.font;
        tagname_Style.fontStyle = FontStyle.Bold;

//        window_rect = new Rect((Main.GAMEWIDTH * 3 / 8) - 300, Main.GAMEHEIGHT / 2 - 200, 600, 400);
		window_rect = new Rect((Main.GAMEWIDTH * 3 / 8) - 350, Main.GAMEHEIGHT / 2 - 250, 700, 500);
        buyArea_rect = new Rect((Main.GAMEWIDTH * 3 / 8) - 350, Main.GAMEHEIGHT / 2 - 250, 700, 500);
        background_Rect = new Rect(0, 0, 580, 320);
        exitButton_rect = new Rect(window_rect.width - 34, 2, 32, 32);
        image_rect = new Rect(40, 48, 80, 80);
        tagName_rect = new Rect(0, 12, 160, 30);
        content_rect = new Rect(140, 15, background_Rect.width - 160, 140);
        createButton_rect = new Rect(330, 100, 80, 32);
        economy_rect = new Rect(window_rect.width / 2 - 90, 40, 180, 32);
        utility_rect = new Rect(economy_rect.x - 200, economy_rect.y, economy_rect.width, economy_rect.height);
        military_rect = new Rect((economy_rect.x + economy_rect.width) + 20, economy_rect.y, economy_rect.width, economy_rect.height);
		
		positionOfScrolling = new Rect(0, 80, window_rect.width, background_Rect.height);
		viewRectOfScrolling = new Rect(0, 0, background_Rect.width, 1200);
		
		firstBuilding_rect = new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight);
		secondBuilding_rect = new Rect(0, 1 * frameHeight, background_Rect.width, frameHeight);
		thirdBuilding_rect = new Rect(0, 2 * frameHeight, background_Rect.width, frameHeight);
        fourthBuilding_rect = new Rect(0, 3 * frameHeight, background_Rect.width, frameHeight);
        fifthBuilding_rect = new Rect(0, 4 * frameHeight, background_Rect.width, frameHeight);
        sixthBuilding_rect = new Rect(0, 5 * frameHeight, background_Rect.width, frameHeight);
        
        //if(Screen.height != Main.GAMEHEIGHT) {
        //    window_rect.height *= MzCalculateScaleRectGUI.ScaleHeightRatio;
        //    window_rect.width *= MzCalculateScaleRectGUI.ScaleHeightRatio;
        //    window_rect.x = (Screen.width * 3 / 8) - window_rect.width / 2;
        //    window_rect.y = Screen.height / 2 - window_rect.height / 2;
			
        //    background_Rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(background_Rect);			
        //    exitButton_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(exitButton_rect);
			
        //    utility_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(utility_rect);
        //    economy_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(economy_rect);
        //    military_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(military_rect);
			
        //    positionOfScrolling = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(positionOfScrolling);
        //    viewRectOfScrolling = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(viewRectOfScrolling);
			
        //    firstBuilding_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(firstBuilding_rect);
        //    secondBuilding_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(secondBuilding_rect);
        //    thirdBuilding_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(thirdBuilding_rect);
        //    fourthBuilding_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(fourthBuilding_rect);
        //    fifthBuilding_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(fifthBuilding_rect);
        //    sixthBuilding_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(sixthBuilding_rect);
			
        //    image_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(image_rect);
        //    tagName_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(tagName_rect);
        //    content_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(content_rect);
        //}
	}

    private IEnumerator LoadTextureResource()
    {
        house_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "House", typeof(Texture2D)) as Texture2D;
        academy_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Academy", typeof(Texture2D)) as Texture2D;

        storeHouse_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Storehouse", typeof(Texture2D)) as Texture2D;
        market_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Market", typeof(Texture2D)) as Texture2D;

        yield return 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    #region <!-- OnMouse Event.

    void OnMouseDown() {
        if(areaState == AreaState.In_Active) {
		    if(GUI_Manager.IsShowInteruptGUI == false) {
                guiState = GUIState.ShowUtiltyUI;
                GUI_Manager.IsShowInteruptGUI = true;
            }
        }
        else if(areaState == AreaState.De_Active) {
            if (GUI_Manager.IsShowInteruptGUI == false) {
                guiState = GUIState.ShowBuyArea;
                GUI_Manager.IsShowInteruptGUI = true;
            }
        }
    }

    #endregion
    
    void ActiveManager()
	{
        this.gameObject.active = false;
        GUI_Manager.IsShowInteruptGUI = false;
    }
	
    //<!--- Economy, Military, Utility.
    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width/ Main.GAMEWIDTH, Screen.height / Main.GAMEHEIGHT, 1));
        		
        if(guiState != GUIState.none && guiState != GUIState.ShowBuyArea) {
            window_rect = GUI.Window(0, window_rect, this.CreateWindow, new GUIContent("Building Area", "GUI window"), standard_skin.window);
        }
		else if(guiState == GUIState.ShowBuyArea) {
			this.DrawShowBuyArea();
		}
    }

    void DrawShowBuyArea()
    {
        GUI.BeginGroup(buyArea_rect, "Extend your empire.", GUI.skin.window);
        {
            if (GUI.Button(exitButton_rect, new GUIContent(string.Empty, "Close Button"), buildingArea_Skin.customStyles[0]))
            {
                if (guiState != GUIState.none)
                {
                    guiState = GUIState.none;
                    GUI_Manager.IsShowInteruptGUI = false;
                }
            }
        }
        GUI.EndGroup();
    }
	
    void CreateWindow(int windowID)
    {
		#region <!--- Main Data flow.
		
        if (GUI.Button(exitButton_rect, new GUIContent(string.Empty, "Close Button"), buildingArea_Skin.customStyles[0])) {
            if (guiState != GUIState.none) {
                guiState = GUIState.none;
                GUI_Manager.IsShowInteruptGUI = false;
            }
        }
		else if(GUI.Button(utility_rect, new GUIContent("Utility", utility_icon), buildingArea_Skin.button)) {
            if (guiState != GUIState.ShowUtiltyUI) {
                guiState = GUIState.ShowUtiltyUI;
            }
			
			scrollPosition = Vector2.zero;
        }
        else if (GUI.Button(economy_rect, new GUIContent("Economy",economy_icon), buildingArea_Skin.button)) {
            if (guiState != GUIState.ShowEconomyUI) {
                guiState = GUIState.ShowEconomyUI;
            }
			
			scrollPosition = Vector2.zero;
        }
        else if (GUI.Button(military_rect, new GUIContent("Military", military_icon), buildingArea_Skin.button)) {
            if (guiState != GUIState.ShowMilitaryUI) {
                guiState = GUIState.ShowMilitaryUI;
            }
			
			scrollPosition = Vector2.zero;
        }
		
		#endregion

        // An absolute-positioned example: We make a scrollview that has a really large client
        // rect and put it in a small rect on the screen.
        scrollPosition = GUI.BeginScrollView(positionOfScrolling, scrollPosition, viewRectOfScrolling); 
        {
            buildingArea_Skin.box.contentOffset = new Vector2(50, 20);
            buildingArea_Skin.textArea.wordWrap = true;

            if (guiState == GUIState.ShowUtiltyUI)     
            {
				#region <!-- Utility Sections.
				
                //<!-- House.
                GUI.BeginGroup(firstBuilding_rect, new GUIContent("House", "Utility"), buildingArea_Skin.box);
                {
                    this.DrawIntroduceHouse();
                }
                GUI.EndGroup();

                GUI.BeginGroup(secondBuilding_rect, new GUIContent("Academy", "Utility"), buildingArea_Skin.box);
                {
                    this.DrawIntroduce_Academy();
                }
                GUI.EndGroup();

                GUI.BeginGroup(thirdBuilding_rect, new GUIContent("Guard Tower", "Utility"), buildingArea_Skin.box);
                {
                    //GUI.Box(imgRect, new GUIContent(academy_icon, "Utility Texture"));
                    //GUI.Box(contentRect, new GUIContent("The Academy is a building in which you can \n" +
                    //    "research technologies to increase the power of your city and troops.", "content"), buildingArea_Skin.textArea);
                    //if (GUI.Button(creatButton_Rect, "Create"))
                    //{
                    //    GameObject building = Instantiate(academy_Obj) as GameObject;
                    //    building.transform.position = sprite.position;
                    //    ActiveManager();
                    //}
                }
                GUI.EndGroup();
				
				#endregion
            }
            else if (guiState == GUIState.ShowEconomyUI)
            {
				#region <!-- Economy Sections.
				
                GUI.BeginGroup(firstBuilding_rect, GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceFarmGUI();
                }
                GUI.EndGroup();

                GUI.BeginGroup(secondBuilding_rect, GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceSawMill();
                }
                GUI.EndGroup();

                GUI.BeginGroup(thirdBuilding_rect, GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceMillStone();
                }
                GUI.EndGroup();

                GUI.BeginGroup(fourthBuilding_rect, GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceSmelter();
                }
                GUI.EndGroup();

                GUI.BeginGroup(fifthBuilding_rect, GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceStoreHouse();
                }
                GUI.EndGroup();        
				
				GUI.BeginGroup(sixthBuilding_rect, GUIContent.none, buildingArea_Skin.box);
		        {
					this.DrawIntroduceMarKet();
		        }
		        GUI.EndGroup();
				
				#endregion
            }
            else if (guiState == GUIState.ShowMilitaryUI)
            {
				#region <!-- Millitary Sections.
				
                GUI.BeginGroup(firstBuilding_rect, GUIContent.none, buildingArea_Skin.box);
                {
                    this.DrawIntroduceBarracks();
                }
                GUI.EndGroup();
				
				#endregion
            }
        }
        // End the scroll view that we began above.
        GUI.EndScrollView();
    }

    #region <!-- Utility section.

    private void DrawIntroduceHouse()
    {
        GUI.DrawTexture(image_rect, house_icon);   //<<-- "Draw icon texture".
        GUI.Box(tagName_rect, new GUIContent(HouseBeh.BuildingName, "Tagname"), tagname_Style);
        GUI.BeginGroup(content_rect, new GUIContent(HouseBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(HouseBeh.RequireResource[0].Food.ToString(),
                    stageManager.gui_Manager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(HouseBeh.RequireResource[0].Wood.ToString(),
                    stageManager.gui_Manager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(HouseBeh.RequireResource[0].Stone.ToString(),
                    stageManager.gui_Manager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(HouseBeh.RequireResource[0].Gold.ToString(), 
                    stageManager.gui_Manager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(HouseBeh.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Build"))
            {
                GameResource.UsedResource(HouseBeh.RequireResource[0]);

                GameObject temp_House = Instantiate(stageManager.house_prefab) as GameObject;
                HouseBeh housebeh = temp_House.GetComponent<HouseBeh>();
                housebeh.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
                housebeh.OnBuildingProcess(housebeh);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    private void DrawIntroduce_Academy()
    {    
        GUI.DrawTexture(image_rect, academy_icon);   //<<-- "Draw icon texture".
        GUI.Box(tagName_rect, new GUIContent(AcademyBeh.BuildingName, "Tagname"), tagname_Style);
        GUI.BeginGroup(content_rect, new GUIContent(AcademyBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(AcademyBeh.RequireResource[0].Food.ToString(),
                    stageManager.gui_Manager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(AcademyBeh.RequireResource[0].Wood.ToString(),
                    stageManager.gui_Manager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(AcademyBeh.RequireResource[0].Gold.ToString(),
                    stageManager.gui_Manager.gold_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(AcademyBeh.RequireResource[0].Employee.ToString(), 
                    stageManager.gui_Manager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(AcademyBeh.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Build"))
            {
                GameResource.UsedResource(AcademyBeh.RequireResource[0]);

                GameObject temp_Academy = Instantiate(stageManager.academy_prefab) as GameObject;
                AcademyBeh academy = temp_Academy.GetComponent<AcademyBeh>();
                academy.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
                academy.OnBuildingProcess(academy);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    #endregion

    #region <!-- Economy section.

    private void DrawIntroduceFarmGUI()
    {
        GUI.DrawTexture(image_rect, farm_Icon);
        GUI.Label(tagName_rect, new GUIContent(Farm.BuildingName), tagname_Style);		
        GUI.BeginGroup(content_rect, new GUIContent(Farm.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(Farm.RequireResource[0].Food.ToString(), 
                    stageManager.gui_Manager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(Farm.RequireResource[0].Wood.ToString(), 
                    stageManager.gui_Manager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(Farm.RequireResource[0].Stone.ToString(), 
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(Farm.RequireResource[0].Gold.ToString(), 
                    stageManager.gui_Manager.gold_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(Farm.RequireResource[0].Employee.ToString(), 
                    stageManager.gui_Manager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(Farm.RequireResource[0]))
                enableUpgrade = true;
			
			GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Build"))
            {
                GameResource.UsedResource(Farm.RequireResource[0]);

                GameObject temp = Instantiate(stageManager.farm_prefab) as GameObject;				
				Farm farm = temp.GetComponent<Farm>();
                farm.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
				farm.OnBuildingProcess((BuildingBeh)farm);

                ActiveManager();
            }
			GUI.enabled = true;
			
			#endregion
        }
        GUI.EndGroup();
    }

	private void DrawIntroduceSawMill()
	{
        GUI.Box(image_rect, new GUIContent(sawmill_Icon, "IconTexture"));
        GUI.Label(tagName_rect, new GUIContent(Sawmill.BuildingName), tagname_Style);		
        GUI.BeginGroup(content_rect, new GUIContent(Sawmill.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(Sawmill.RequireResource[0].Food.ToString(),
                    stageManager.gui_Manager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(Sawmill.RequireResource[0].Wood.ToString(),
                    stageManager.gui_Manager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(Sawmill.RequireResource[0].Stone.ToString(),
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(Sawmill.RequireResource[0].Gold.ToString(),
                    stageManager.gui_Manager.gold_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(Sawmill.RequireResource[0].Employee.ToString(),
                    stageManager.gui_Manager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(Sawmill.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Build"))
            {
                GameResource.UsedResource(Sawmill.RequireResource[0]);

                GameObject new_sawmill = Instantiate(stageManager.sawmill_prefab) as GameObject;
                Sawmill sawmill = new_sawmill.GetComponent<Sawmill>();
                sawmill.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
                sawmill.OnBuildingProcess((BuildingBeh)sawmill);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
	}

	private void DrawIntroduceMillStone()
    {
        GUI.Box(image_rect, new GUIContent(millStone_Icon, "IconTexture"));
        GUI.Label(tagName_rect, new GUIContent(MillStone.BuildingName), tagname_Style);
        GUI.BeginGroup(content_rect, new GUIContent(MillStone.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(MillStone.RequireResource[0].Food.ToString(), 
                    stageManager.gui_Manager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(MillStone.RequireResource[0].Wood.ToString(),
                    stageManager.gui_Manager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(MillStone.RequireResource[0].Stone.ToString(),
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(MillStone.RequireResource[0].Gold.ToString(),
                    stageManager.gui_Manager.gold_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(MillStone.RequireResource[0].Employee.ToString(),
                    stageManager.gui_Manager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(MillStone.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Build"))
            {
                GameResource.UsedResource(MillStone.RequireResource[0]);

                GameObject new_millstone = Instantiate(stageManager.millstone_prefab) as GameObject;
                MillStone millstone = new_millstone.GetComponent<MillStone>();
                millstone.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
                millstone.OnBuildingProcess((BuildingBeh)millstone);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

	private void DrawIntroduceSmelter()
    {
        GUI.Box(image_rect, new GUIContent(smelter_icon, "IconTexture"));
        GUI.Label(tagName_rect, new GUIContent(Smelter.BuildingName), tagname_Style);
        GUI.BeginGroup(content_rect, new GUIContent(Smelter.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(Smelter.RequireResource[0].Food.ToString(),
                    stageManager.gui_Manager.food_icon), standard_skin.box);
                //GUI.Label(GameResource.Second_Rect, new GUIContent(Smelter.RequireResource[0].Wood.ToString(), 
                //    stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(Smelter.RequireResource[0].Stone.ToString(), 
                    stageManager.gui_Manager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(Smelter.RequireResource[0].Gold.ToString(),
                    stageManager.gui_Manager.gold_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(Smelter.RequireResource[0].Employee.ToString(),
                    stageManager.gui_Manager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(Smelter.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Build"))
            {
                GameResource.UsedResource(Smelter.RequireResource[0]);

                GameObject new_smelter = Instantiate(stageManager.smelter_prefab) as GameObject;
                Smelter smelter = new_smelter.GetComponent<Smelter>();
                smelter.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
                smelter.OnBuildingProcess((BuildingBeh)smelter);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    private void DrawIntroduceMarKet()
    {
        GUI.DrawTexture(image_rect, market_icon);     ///Market icon texture.
        GUI.Label(tagName_rect, new GUIContent(MarketBeh.BuildingName), tagname_Style);     /// Market label tagname.
        GUI.BeginGroup(content_rect, new GUIContent(MarketBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(MarketBeh.RequireResource[0].Food.ToString(),
                    stageManager.gui_Manager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(MarketBeh.RequireResource[0].Wood.ToString(), 
                    stageManager.gui_Manager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(MarketBeh.RequireResource[0].Stone.ToString(), 
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(MarketBeh.RequireResource[0].Gold.ToString(),
                    stageManager.gui_Manager.gold_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(MarketBeh.RequireResource[0].Employee.ToString(),
                    stageManager.gui_Manager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(MarketBeh.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Create"))
            {
                GameResource.UsedResource(MarketBeh.RequireResource[0]);

                GameObject market_obj = Instantiate(stageManager.market_prefab) as GameObject;
                MarketBeh market = market_obj.GetComponent<MarketBeh>();
                market.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
                market.OnBuildingProcess(market);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    private void DrawIntroduceStoreHouse()
    {
        GUI.DrawTexture(image_rect, storeHouse_icon);
        GUI.Label(tagName_rect, new GUIContent(StoreHouse.BuildingName), tagname_Style);
        GUI.BeginGroup(content_rect, new GUIContent(StoreHouse.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(StoreHouse.RequireResource[0].Food.ToString(), 
                    stageManager.gui_Manager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(StoreHouse.RequireResource[0].Wood.ToString(),
                    stageManager.gui_Manager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(StoreHouse.RequireResource[0].Stone.ToString(),
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(StoreHouse.RequireResource[0].Gold.ToString(),
                    stageManager.gui_Manager.gold_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(StoreHouse.RequireResource[0].Employee.ToString(),
                    stageManager.gui_Manager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(StoreHouse.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Build"))
            {
                GameResource.UsedResource(StoreHouse.RequireResource[0]);

                GameObject new_storehouse = Instantiate(stageManager.storehouse_prefab) as GameObject;
                StoreHouse storeHouse = new_storehouse.GetComponent<StoreHouse>();
                storeHouse.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
                storeHouse.OnBuildingProcess(storeHouse);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    #endregion

	#region <!-- Millitary section.
	
    private void DrawIntroduceBarracks()
    {
        GUI.DrawTexture(image_rect, barrackNotation);   //<<-- "Draw icon texture".
        GUI.Label(tagName_rect, new GUIContent(BarracksBeh.BuildingName), tagname_Style);
        GUI.BeginGroup(content_rect, new GUIContent(BarracksBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.First_Rect, new GUIContent(BarracksBeh.RequireResource[0].Food.ToString(), stageManager.gui_Manager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Second_Rect, new GUIContent(BarracksBeh.RequireResource[0].Wood.ToString(), stageManager.gui_Manager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Third_Rect, new GUIContent(BarracksBeh.RequireResource[0].Copper.ToString(), stageManager.gui_Manager.copper_icon), standard_skin.box);
                GUI.Label(GameResource.Fourth_Rect, new GUIContent(BarracksBeh.RequireResource[0].Gold.ToString(), stageManager.gui_Manager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!-- Build Button mechanichm.

            bool enableUpgrade = false;
            if (BuildingBeh.CheckingCanCreateBuilding() && building.CheckingEnoughUpgradeResource(BarracksBeh.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(createButton_rect, "Build"))
            {
                GameResource.UsedResource(BarracksBeh.RequireResource[0]);

                GameObject barracks_obj = Instantiate(stageManager.barracks_prefab) as GameObject;
                BarracksBeh barracks = barracks_obj.GetComponent<BarracksBeh>();
                barracks.InitializeBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
                barracks.OnBuildingProcess(barracks);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }
	
	#endregion
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                