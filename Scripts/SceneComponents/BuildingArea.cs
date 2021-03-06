using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingArea : Base_ObjectBeh 
{
    Rect RequireResource_Rect = new Rect(10, 125, 500, 40);
    Rect requireDescription_rect = new Rect(10, 80, 300, 40);
	
    /// GUI : Texture && Skin.
    private GUIStyle tagname_Style;
    public GUISkin standard_skin;
    public GUISkin buildingArea_Skin;
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
    private Texture2D farm_Icon;
    public Texture2D sawmill_Icon;
    public Texture2D millStone_Icon;
    public Texture2D smelter_icon;
    //<!--- Militaly.
    public Texture2D barracks_icon;

    internal tk2dSprite Sprite;
	private int indexOfAreaPosition;
    public int IndexOfAreaPosition { get { return indexOfAreaPosition; } set { indexOfAreaPosition = value; } }
    private enum GUIState { none = 0, ShowMainUI, ShowUtiltyUI, ShowEconomyUI, ShowMilitaryUI, ShowBuyArea, };
    //public enum AreaState { Active = 1, UnActive = 0, };
    //public AreaState areaState; 
    internal bool areaActiveState = false;
    private GUIState guiState;
    private CapitalCity sceneController;
	private static BuildingBeh buildingBeh;
	
	private Vector2 scrollPosition = Vector2.zero;
    private Rect window_rect;
    private Rect closeBuildingArea_rect;
    private Rect background_Rect;
    private Rect image_rect;
    private Rect tagName_rect;
    private Rect content_rect;
    private Rect createButton_rect;
    private Rect economy_rect;
    private Rect utility_rect;
    private Rect military_rect;
    private Rect buyArea_rect;
    private Rect closeBuyArea_rect;
    private Rect advisor_drawRect;
    private Rect buyAreaDescription_rect;
	
	Rect positionOfScrolling;
	Rect viewRectOfScrolling;
	
    float frameHeight = 210;
	Rect firstBuilding_rect;
	Rect secondBuilding_rect;
	Rect thirdBuilding_rect;
	Rect fourthBuilding_rect;
	Rect fifthBuilding_rect;
	Rect sixthBuilding_rect;

    private Rect requirementGroup_rect;
    private GUIStyle requirementGroup_style;
    private GUIStyle requirementBox_style;

	
	void Awake() {		
        Sprite = this.gameObject.GetComponent<tk2dSprite>();
	}
	
	// Use this for initialization
	IEnumerator Start () {
		while (CapitalCity._StageInitialized == false) {
            yield return null;
		}
		
        sceneController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CapitalCity>();
        StartCoroutine(this.LoadTextureResource());
        this.InitializeGUI();

        if(!areaActiveState) {
			Sprite.color = Color.grey;
        }

        //<!--- Static building type.
        if (buildingBeh == null) {
            GameObject towncenter = GameObject.Find(TownCenter.BuildingName);
            buildingBeh = towncenter.GetComponent<BuildingBeh>();
        }
	}
	
    private void InitializeGUI()
    {
        tagname_Style = new GUIStyle();
        tagname_Style.alignment = TextAnchor.MiddleCenter;
        tagname_Style.normal.textColor = Color.white;
        tagname_Style.font = buildingArea_Skin.font;
        tagname_Style.fontStyle = FontStyle.Bold;

        requirementGroup_style = new GUIStyle(standard_skin.box);
        requirementGroup_style.alignment = TextAnchor.UpperCenter;
        requirementBox_style = new GUIStyle(standard_skin.box);
        requirementBox_style.alignment = TextAnchor.MiddleLeft;

		window_rect = new Rect((Main.FixedGameWidth * 3 / 8) - 350, Main.FixedGameHeight / 2 - 250, 700, 500);
        closeBuildingArea_rect = new Rect(window_rect.width - 50, 2, 48, 48);
        background_Rect = new Rect(0, 0, 680, 420);
        image_rect = new Rect(40, 50, 80, 80);
        tagName_rect = new Rect(0, 15, 160, 30);
        content_rect = new Rect(140, 20, background_Rect.width - 160, 170);
        createButton_rect = new Rect(30, 145, 100, 40);
        economy_rect = new Rect(window_rect.width / 2 - 90, 25, 180, 50);
        utility_rect = new Rect(economy_rect.x - 200, economy_rect.y, economy_rect.width, economy_rect.height);
        military_rect = new Rect((economy_rect.x + economy_rect.width) + 20, economy_rect.y, economy_rect.width, economy_rect.height);
		
		positionOfScrolling = new Rect(0, 80, window_rect.width, background_Rect.height);
		viewRectOfScrolling = new Rect(0, 0, background_Rect.width, 2100);
		
		firstBuilding_rect = new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight);
		secondBuilding_rect = new Rect(0, 1 * frameHeight, background_Rect.width, frameHeight);
		thirdBuilding_rect = new Rect(0, 2 * frameHeight, background_Rect.width, frameHeight);
        fourthBuilding_rect = new Rect(0, 3 * frameHeight, background_Rect.width, frameHeight);
        fifthBuilding_rect = new Rect(0, 4 * frameHeight, background_Rect.width, frameHeight);
        sixthBuilding_rect = new Rect(0, 5 * frameHeight, background_Rect.width, frameHeight);
        
        buyArea_rect = new Rect((Main.FixedGameWidth * 3 / 8) - 300, Main.FixedGameHeight / 2 - 300, 600, 600);
        closeBuyArea_rect = new Rect(buyArea_rect.width - 50, 48, 48, 48);
        advisor_drawRect = new Rect(10, (buyArea_rect.height / 2) - (sceneController.taskManager.advisor_villageElder.height/2), sceneController.taskManager.advisor_villageElder.width, sceneController.taskManager.advisor_villageElder.height);
        float descriptionPosX = (advisor_drawRect.x + advisor_drawRect.width) + 10;
        buyAreaDescription_rect = new Rect(descriptionPosX, 50, buyArea_rect.width - descriptionPosX, 500);
	}

    private IEnumerator LoadTextureResource()
    {
		farm_Icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Farm", typeof(Texture2D)) as Texture2D;
		sawmill_Icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Woodsman", typeof(Texture2D)) as Texture2D;
		millStone_Icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "StoneCarver", typeof(Texture2D)) as Texture2D;
		smelter_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Miner", typeof(Texture2D)) as Texture2D;

        house_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "House", typeof(Texture2D)) as Texture2D;
        academy_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Academy", typeof(Texture2D)) as Texture2D;

        storeHouse_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Storehouse", typeof(Texture2D)) as Texture2D;
        market_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Market", typeof(Texture2D)) as Texture2D;

		barracks_icon = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Barracks", typeof(Texture2D)) as Texture2D;

        yield return 0;
    }
    
	
    protected override void OnTouchDown()
    {
        sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.displayUI_clip);

        if(areaActiveState) {
		    if(TaskManager.IsShowInteruptGUI == false) {
                guiState = GUIState.ShowUtiltyUI;
                TaskManager.IsShowInteruptGUI = true;
            }
        }
        else if(areaActiveState == false) {
            if (TaskManager.IsShowInteruptGUI == false) {
                guiState = GUIState.ShowBuyArea;
                TaskManager.IsShowInteruptGUI = true;
            }
        }

        base.OnTouchDown();
    }

    
    void DeActivateArea()
	{
        TaskManager.IsShowInteruptGUI = false;
    }

    private bool CheckBasicRequire()
    {
        if (BuildingBeh.TownCenter.Level >= 3 && StoreHouse.sumOfGold >= 500)
            return true;
        else
            return false;
    }

    private void buyArea()
    {
        StoreHouse.sumOfGold -= 500;

        this.areaActiveState = true;
        CapitalCity.arr_buildingAreaState[indexOfAreaPosition] = true;

//        Sprite.tintColor = Color.white;
//        Sprite.frameIndex = 3;
		Sprite.color = Color.white;
		
		if(QuestSystemManager.arr_isMissionComplete[8] == false)
			sceneController.taskManager.questManager.MissionComplete(8);

        this.CloseGUIWindow();
    }
	
    //<!--- Economy, Military, Utility.
/*    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width/ Main.FixedGameWidth, Screen.height / Main.FixedGameHeight, 1));
        		
        if(guiState != GUIState.none && guiState != GUIState.ShowBuyArea) {
            window_rect = GUI.Window(0, window_rect, this.CreateWindow, new GUIContent("Building Area", "GUI window"), standard_skin.window);
        }
		else if(guiState == GUIState.ShowBuyArea) {
			this.DrawShowBuyArea();
		}
    }
*/
    void DrawShowBuyArea()
    {
        GUI.BeginGroup(buyArea_rect);
        {
            //<!--- Draw advisor.
            GUI.DrawTexture(advisor_drawRect, sceneController.taskManager.advisor_villageElder);
            //<!--- Draw Description.
            GUI.BeginGroup(buyAreaDescription_rect, "Expand empire.", buildingArea_Skin.window);
            {           
                requirementGroup_rect = new Rect(5, buyAreaDescription_rect.height / 2, buyAreaDescription_rect.width - 10, (buyAreaDescription_rect.height / 2) -5);
                GUI.BeginGroup(requirementGroup_rect, "Requirement", requirementGroup_style);
                {
                    GUI.Box(new Rect(5, 50, requirementGroup_rect.width - 10, 30), "Town center level 3.", requirementBox_style);
                    GUI.Box(new Rect(5, 85, requirementGroup_rect.width - 10, 30), "Gold :: 500", requirementBox_style);

                    GUI.enabled = CheckBasicRequire();
                    if (GUI.Button(new Rect(requirementGroup_rect.width / 2 - 60, requirementGroup_rect.height - 40, 120, 35), "Expand !"))
                    { 
						buyArea();
                    }
                    GUI.enabled = true;
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            //<!-- Close button.
            if (GUI.Button(closeBuyArea_rect, new GUIContent(string.Empty, "Close Button"), buildingArea_Skin.customStyles[0]))
            {
                this.CloseGUIWindow();
            }
        }
        GUI.EndGroup();
    }

    void CloseGUIWindow()
    {
        if (guiState != GUIState.none)
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            guiState = GUIState.none;
            TaskManager.IsShowInteruptGUI = false;
        }
    }

/*    void CreateWindow(int windowID)
    {
		#region <!--- Main Data flow.
		
        if (GUI.Button(closeBuildingArea_rect, new GUIContent(string.Empty, "Close Button"), buildingArea_Skin.customStyles[0])) {
            this.CloseGUIWindow();
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
                GUI.BeginGroup(firstBuilding_rect, GUIContent.none, buildingArea_Skin.box);
                {
                    this.DrawIntroduceHouse();
                }
                GUI.EndGroup();

                GUI.BeginGroup(secondBuilding_rect, GUIContent.none, buildingArea_Skin.box);
                {
                    this.DrawIntroduce_Academy();
                }
                GUI.EndGroup();

                GUI.BeginGroup(thirdBuilding_rect, GUIContent.none, buildingArea_Skin.box);
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
 */

    #region <!-- Utility section.
/*
    private void DrawIntroduceHouse()
    {
        GUI.DrawTexture(image_rect, house_icon);   //<<-- "Draw icon texture".
        GUI.Box(tagName_rect, new GUIContent(HouseBeh.BuildingName, "Tagname"), tagname_Style);
		
        GUI.BeginGroup(content_rect, new GUIContent(HouseBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(HouseBeh.RequireResource[0].Food.ToString(),
                    sceneController.taskManager.food_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(HouseBeh.RequireResource[0].Wood.ToString(),
                    sceneController.taskManager.wood_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(HouseBeh.RequireResource[0].Stone.ToString(),
                    sceneController.taskManager.stone_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(HouseBeh.RequireResource[0].Gold.ToString(), 
                    sceneController.taskManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();

        #region <!--- Build Button mechanichm.

		GUI.enabled = (BuildingBeh.CheckingOnBuildingList() &&
		               buildingBeh.CheckingEnoughUpgradeResource(HouseBeh.RequireResource[0])) ? true:false;
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(HouseBeh.RequireResource[0]);

            GameObject temp_House = Instantiate(sceneController.house_prefab) as GameObject;
            HouseBeh housebeh = temp_House.GetComponent<HouseBeh>();
            housebeh.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
            housebeh.OnBuildingProcess(housebeh);

            DeActivateArea();
        }
        GUI.enabled = true;

        #endregion
    }
*/
/*
    private void DrawIntroduce_Academy()
    {    
        GUI.DrawTexture(image_rect, academy_icon);   //<<-- "Draw icon texture".
        GUI.Box(tagName_rect, new GUIContent(AcademyBeh.BuildingName, "Tagname"), tagname_Style);
		
        GUI.BeginGroup(content_rect, new GUIContent(AcademyBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
			GUI.Box(requireDescription_rect, AcademyBeh.RequireDescription, standard_skin.box);
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(AcademyBeh.RequireResource[0].Food.ToString(),
                    sceneController.taskManager.food_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(AcademyBeh.RequireResource[0].Wood.ToString(),
                    sceneController.taskManager.wood_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(AcademyBeh.RequireResource[0].Gold.ToString(),
                    sceneController.taskManager.gold_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(AcademyBeh.RequireResource[0].Employee.ToString(), 
                    sceneController.taskManager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();

        #region <!-- Build Button mechanichm.

        GUI.enabled = (BuildingBeh.CheckingOnBuildingList()
            && buildingBeh.CheckingEnoughUpgradeResource(AcademyBeh.RequireResource[0])
            && BuildingBeh.TownCenter.Level >= 5
            && BuildingBeh.AcademyInstance == null) ? true : false;
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(AcademyBeh.RequireResource[0]);

            GameObject temp_Academy = Instantiate(sceneController.academy_prefab) as GameObject;
            AcademyBeh academy = temp_Academy.GetComponent<AcademyBeh>();
            academy.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
            academy.OnBuildingProcess(academy);

            DeActivateArea();
        }
        GUI.enabled = true;

        #endregion
    }
*/
    #endregion

    #region <!-- Economy section.
/*
    private void DrawIntroduceFarmGUI()
    {
        GUI.DrawTexture(image_rect, farm_Icon);
        GUI.Label(tagName_rect, new GUIContent(Farm.BuildingName), tagname_Style);	

        #region <!--- Build Button mechanichm.

        GUI.enabled = (BuildingBeh.CheckingOnBuildingList() &&
            buildingBeh.CheckingEnoughUpgradeResource(Farm.RequireResource[0])) ? true : false;
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(Farm.RequireResource[0]);

            GameObject temp = Instantiate(sceneController.farm_prefab) as GameObject;				
			Farm farm = temp.GetComponent<Farm>();
            farm.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
			farm.OnBuildingProcess((BuildingBeh)farm);

            DeActivateArea();
        }
		GUI.enabled = true;
		
		#endregion
		
        GUI.BeginGroup(content_rect, new GUIContent(Farm.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(Farm.RequireResource[0].Food.ToString(), 
                    sceneController.taskManager.food_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(Farm.RequireResource[0].Wood.ToString(), 
                    sceneController.taskManager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(Farm.RequireResource[0].Stone.ToString(), 
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(Farm.RequireResource[0].Gold.ToString(), 
                    sceneController.taskManager.gold_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(Farm.RequireResource[0].Employee.ToString(), 
                    sceneController.taskManager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

	private void DrawIntroduceSawMill()
	{
        GUI.Box(image_rect, new GUIContent(sawmill_Icon, "IconTexture"));
        GUI.Label(tagName_rect, new GUIContent(Sawmill.BuildingName), tagname_Style);	

        #region <!--- Build Button mechanichm.

        GUI.enabled = (BuildingBeh.CheckingOnBuildingList() &&
            buildingBeh.CheckingEnoughUpgradeResource(Sawmill.RequireResource[0])) ? true : false;
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(Sawmill.RequireResource[0]);

            GameObject new_sawmill = Instantiate(sceneController.sawmill_prefab) as GameObject;
            Sawmill sawmill = new_sawmill.GetComponent<Sawmill>();
            sawmill.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
            sawmill.OnBuildingProcess((BuildingBeh)sawmill);

            DeActivateArea();
        }
        GUI.enabled = true;

        #endregion
		
        GUI.BeginGroup(content_rect, new GUIContent(Sawmill.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(Sawmill.RequireResource[0].Food.ToString(),
                    sceneController.taskManager.food_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(Sawmill.RequireResource[0].Wood.ToString(),
                    sceneController.taskManager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(Sawmill.RequireResource[0].Stone.ToString(),
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(Sawmill.RequireResource[0].Gold.ToString(),
                    sceneController.taskManager.gold_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(Sawmill.RequireResource[0].Employee.ToString(),
                    sceneController.taskManager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
	}

	private void DrawIntroduceMillStone()
    {
        GUI.Box(image_rect, new GUIContent(millStone_Icon, "IconTexture"));
        GUI.Label(tagName_rect, new GUIContent(StoneCrushingPlant.BuildingName), tagname_Style);

        #region <!--- Build Button mechanichm.

        GUI.enabled = (BuildingBeh.CheckingOnBuildingList() &&
            buildingBeh.CheckingEnoughUpgradeResource(StoneCrushingPlant.RequireResource[0])) ? true : false;
		/// Have to research stonecutter before building.
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(StoneCrushingPlant.RequireResource[0]);

            GameObject new_building = Instantiate(sceneController.millstone_prefab) as GameObject;
            StoneCrushingPlant stoneCrushing = new_building.GetComponent<StoneCrushingPlant>();
            stoneCrushing.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
            stoneCrushing.OnBuildingProcess((BuildingBeh)stoneCrushing);

            DeActivateArea();
        }
        GUI.enabled = true;

        #endregion
		
        GUI.BeginGroup(content_rect, new GUIContent(StoneCrushingPlant.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(StoneCrushingPlant.RequireResource[0].Food.ToString(), 
                    sceneController.taskManager.food_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(StoneCrushingPlant.RequireResource[0].Wood.ToString(),
                    sceneController.taskManager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(MillStone.RequireResource[0].Stone.ToString(),
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(StoneCrushingPlant.RequireResource[0].Gold.ToString(),
                    sceneController.taskManager.gold_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(StoneCrushingPlant.RequireResource[0].Employee.ToString(),
                    sceneController.taskManager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

	private void DrawIntroduceSmelter()
    {
        GUI.Box(image_rect, new GUIContent(smelter_icon, "IconTexture"));
        GUI.Label(tagName_rect, new GUIContent(Smelter.BuildingName), tagname_Style);

        #region <!--- Build Button mechanichm.

        GUI.enabled = (BuildingBeh.CheckingOnBuildingList() &&
            buildingBeh.CheckingEnoughUpgradeResource(Smelter.RequireResource[0]) &&
            BuildingBeh.TownCenter.Level >= 5) ? true : false;
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(Smelter.RequireResource[0]);

            GameObject new_smelter = Instantiate(sceneController.smelter_prefab) as GameObject;
            Smelter smelter = new_smelter.GetComponent<Smelter>();
            smelter.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
            smelter.OnBuildingProcess((BuildingBeh)smelter);

            DeActivateArea();
        }
        GUI.enabled = true;

        #endregion
		
        GUI.BeginGroup(content_rect, new GUIContent(Smelter.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            GUI.Box(requireDescription_rect, Smelter.RequireDescription, standard_skin.box);
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(Smelter.RequireResource[0].Food.ToString(),
                    sceneController.taskManager.food_icon), standard_skin.box);
                //GUI.Label(GameResource.Second_Rect, new GUIContent(Smelter.RequireResource[0].Wood.ToString(), 
                //    stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(Smelter.RequireResource[0].Stone.ToString(), 
                    sceneController.taskManager.stone_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(Smelter.RequireResource[0].Gold.ToString(),
                    sceneController.taskManager.gold_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(Smelter.RequireResource[0].Employee.ToString(),
                    sceneController.taskManager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    private void DrawIntroduceMarKet()
    {
        GUI.DrawTexture(image_rect, market_icon);     ///Market icon texture.
        GUI.Label(tagName_rect, new GUIContent(MarketBeh.BuildingName), tagname_Style);     /// Market label tagname.

        #region <!--- Build Button mechanichm.

        GUI.enabled = (BuildingBeh.CheckingOnBuildingList() && buildingBeh.CheckingEnoughUpgradeResource(MarketBeh.RequireResource[0]) 
            && BuildingBeh.MarketInstance == null) ? true : false;
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(MarketBeh.RequireResource[0]);

            GameObject market_obj = Instantiate(sceneController.market_prefab) as GameObject;
            MarketBeh market = market_obj.GetComponent<MarketBeh>();
            market.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
            market.OnBuildingProcess(market);

            DeActivateArea();
        }
        GUI.enabled = true;

        #endregion
		
        GUI.BeginGroup(content_rect, new GUIContent(MarketBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(MarketBeh.RequireResource[0].Food.ToString(),
                    sceneController.taskManager.food_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(MarketBeh.RequireResource[0].Wood.ToString(), 
                    sceneController.taskManager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(MarketBeh.RequireResource[0].Stone.ToString(), 
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(MarketBeh.RequireResource[0].Gold.ToString(),
                    sceneController.taskManager.gold_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(MarketBeh.RequireResource[0].Employee.ToString(),
                    sceneController.taskManager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    private void DrawIntroduceStoreHouse()
    {
        GUI.DrawTexture(image_rect, storeHouse_icon);
        GUI.Label(tagName_rect, new GUIContent(StoreHouse.BuildingName), tagname_Style);

        #region <!--- Build Button mechanichm.

        GUI.enabled = (BuildingBeh.CheckingOnBuildingList() &&
            buildingBeh.CheckingEnoughUpgradeResource(StoreHouse.RequireResource[0])) ? true : false;
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(StoreHouse.RequireResource[0]);

            GameObject new_storehouse = Instantiate(sceneController.storehouse_prefab) as GameObject;
            StoreHouse storeHouse = new_storehouse.GetComponent<StoreHouse>();
            storeHouse.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
            storeHouse.OnBuildingProcess(storeHouse);

            DeActivateArea();
        }
        GUI.enabled = true;

        #endregion
		
        GUI.BeginGroup(content_rect, new GUIContent(StoreHouse.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(StoreHouse.RequireResource[0].Food.ToString(), 
                    sceneController.taskManager.food_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(StoreHouse.RequireResource[0].Wood.ToString(),
                    sceneController.taskManager.wood_icon), standard_skin.box);
                //GUI.Label(GameResource.Third_Rect, new GUIContent(StoreHouse.RequireResource[0].Stone.ToString(),
                //    stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(StoreHouse.RequireResource[0].Gold.ToString(),
                    sceneController.taskManager.gold_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(StoreHouse.RequireResource[0].Employee.ToString(),
                    sceneController.taskManager.employee_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }
*/
    #endregion

	#region <!-- Millitary section.
/*	
    private void DrawIntroduceBarracks()
    {
        GUI.DrawTexture(image_rect, barracks_icon);   //<<-- "Draw icon texture".
        GUI.Label(tagName_rect, new GUIContent(BarracksBeh.BuildingName), tagname_Style);
		
        GUI.BeginGroup(content_rect, new GUIContent(BarracksBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            GUI.Box(requireDescription_rect, BarracksBeh.RequireDescription, standard_skin.box);
            //<!-- Requirements Resource.
            GUI.BeginGroup(RequireResource_Rect);
            {
                GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(BarracksBeh.RequireResource[0].Food.ToString(), sceneController.taskManager.food_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(BarracksBeh.RequireResource[0].Wood.ToString(), sceneController.taskManager.wood_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(BarracksBeh.RequireResource[0].Stone.ToString(), sceneController.taskManager.stone_icon), standard_skin.box);
                GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(BarracksBeh.RequireResource[0].Gold.ToString(), sceneController.taskManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();

        #region <!-- Build Button mechanichm.

		bool enableBuildingButton  = false;
		if(BuildingBeh.CheckingOnBuildingList() && 
            buildingBeh.CheckingEnoughUpgradeResource(BarracksBeh.RequireResource[0]) && 
            BuildingBeh.Barrack_Instance == null && BuildingBeh.AcademyInstance != null) 
        {
			enableBuildingButton = (BuildingBeh.AcademyInstance.Level >= 3) ? true : false;
		}

        GUI.enabled = enableBuildingButton;
        if (GUI.Button(createButton_rect, "Build", standard_skin.button))
        {
            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

            GameMaterialDatabase.UsedResource(BarracksBeh.RequireResource[0]);

            GameObject barracks_obj = Instantiate(sceneController.barracks_prefab) as GameObject;
            BarracksBeh barracks = barracks_obj.GetComponent<BarracksBeh>();
            barracks.InitializingBuildingBeh(BuildingBeh.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
            barracks.OnBuildingProcess(barracks);

            DeActivateArea();
        }
        GUI.enabled = true;

        #endregion
    }
*/
	#endregion
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                