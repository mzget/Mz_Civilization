using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingArea : MonoBehaviour {

    public const string PathOf_Notation_Texture = "Textures/Building_Icons/";

    /// GUI : Texture && Skin.
    public GUISkin standard_skin;
    public GUISkin buildingArea_Skin;
	public GUISkin taskbarUI_Skin;
    private GUIStyle tagname_Style;
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
    private enum GUIState { none = 0, ShowMainUI, ShowUtiltyUI, ShowEconomyUI, ShowMilitaryUI };
    private GUIState guiState;
    private StageManager stageManager;

    private Rect windowRect;
    protected Rect exitButton_Rect;
    private Vector2 scrollPosition = Vector2.zero;
    protected Rect background_Rect;
    protected Rect imgRect;
    private Rect tagName_Rect;
    private Rect contentRect;
    private Rect creatButton_Rect;
    private Rect economy_Rect;
    private Rect utility_Rect;
    private Rect military_Rect;


    void Awake()
	{
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        sprite = this.gameObject.GetComponent<OTSprite>();
		
		this.InitializeGUI();
    }

    private void InitializeGUI()
    {
        tagname_Style = new GUIStyle();
        tagname_Style.alignment = TextAnchor.MiddleCenter;
        tagname_Style.normal.textColor = Color.white;
        tagname_Style.font = buildingArea_Skin.font;
        tagname_Style.fontStyle = FontStyle.Bold;		

        windowRect = new Rect(Main.GAMEWIDTH / 2 - 350, Main.GAMEHEIGHT / 2 - 180, 700, 400);
        exitButton_Rect = new Rect(windowRect.width - 34, 2, 32, 32);
        background_Rect = new Rect(0, 0, windowRect.width - 16, 320);
        imgRect = new Rect(40, 48, 80, 80);
        tagName_Rect = new Rect(0, 12, 160, 30);
        contentRect = new Rect(140, 15, background_Rect.width - 160, 140);
        creatButton_Rect = new Rect(420, 100, 100, 32);
        economy_Rect = new Rect(windowRect.width / 2 - 90, 24, 180, 48);
        utility_Rect = new Rect(economy_Rect.x - 200, economy_Rect.y, economy_Rect.width, economy_Rect.height);
        military_Rect = new Rect((economy_Rect.x + economy_Rect.width) + 20, economy_Rect.y, economy_Rect.width, economy_Rect.height);
	}

	// Use this for initialization
	IEnumerator Start () 
	{
        StartCoroutine(this.LoadTextureResource());

        yield return 0;
	}

    private IEnumerator LoadTextureResource()
    {
        house_icon = Resources.Load(PathOf_Notation_Texture + "House", typeof(Texture2D)) as Texture2D;

        storeHouse_icon = Resources.Load(PathOf_Notation_Texture + "Storehouse", typeof(Texture2D)) as Texture2D;
        market_icon = Resources.Load(PathOf_Notation_Texture + "Market", typeof(Texture2D)) as Texture2D;

        yield return 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    #region <!-- Incloud Mouse Event.

    void OnMouseOver() { 
    }

    void OnMouseDown() {
        if (TaskbarManager.IsShowInteruptGUI == false) {
            guiState = GUIState.ShowUtiltyUI;
            TaskbarManager.IsShowInteruptGUI = true;
        }
    }

    void OnMouseExit() { 
		
    }

    #endregion
    
    void ActiveManager() {
        this.gameObject.active = false;
        TaskbarManager.IsShowInteruptGUI = false;
    }
	
    //<!-- Economy, Military, Utility.
    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GAMEWIDTH, Screen.height / Main.GAMEHEIGHT, 1));
        		
        if(guiState != GUIState.none) {
            windowRect = GUI.Window(0, windowRect, this.CreateWindow, new GUIContent("Building Area", "GUI window"), standard_skin.window);
        }
    }

    void CreateWindow(int windowID)
    {
        float frameHeight = 160;
		
		#region <!-- Mian Data flow.
		
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), buildingArea_Skin.customStyles[0])) {
            if (guiState != GUIState.none) {
                guiState = GUIState.none;
                TaskbarManager.IsShowInteruptGUI = false;
            }
        }
		else if(GUI.Button(utility_Rect, new GUIContent("Utility", utility_icon), buildingArea_Skin.button)) {
            if (guiState != GUIState.ShowUtiltyUI) {
                guiState = GUIState.ShowUtiltyUI;
            }
			
			scrollPosition = Vector2.zero;
        }
        else if (GUI.Button(economy_Rect, new GUIContent("Economy",economy_icon), buildingArea_Skin.button)) {
            if (guiState != GUIState.ShowEconomyUI) {
                guiState = GUIState.ShowEconomyUI;
            }
			
			scrollPosition = Vector2.zero;
        }
        else if (GUI.Button(military_Rect, new GUIContent("Military", military_icon), buildingArea_Skin.button)) {
            if (guiState != GUIState.ShowMilitaryUI) {
                guiState = GUIState.ShowMilitaryUI;
            }
			
			scrollPosition = Vector2.zero;
        }
		
		#endregion

        // An absolute-positioned example: We make a scrollview that has a really large client
        // rect and put it in a small rect on the screen.
        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, windowRect.width, background_Rect.height),
            scrollPosition, new Rect(0, 0, background_Rect.width, 1200), false, true); 
        {
            buildingArea_Skin.box.contentOffset = new Vector2(50, 20);
            buildingArea_Skin.textArea.wordWrap = true;

            //<!-- Utility.
            if (guiState == GUIState.ShowUtiltyUI)          
            {
				#region <!-- Utility Sections.
				
                //<!-- House.
                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), new GUIContent("House", "Utility"), buildingArea_Skin.box);
                {
                    this.DrawIntroduceHouse();
                }
                GUI.EndGroup();

                //<!-- Guard Tower.
                GUI.BeginGroup(new Rect(0, 1 * frameHeight, background_Rect.width, frameHeight), new GUIContent("Guard Tower", "Utility"), buildingArea_Skin.box);
                {
                    //GUI.Box(imgRect, new GUIContent(guardTower_icon, "Texture"));
                    //GUI.Box(contentRect, new GUIContent("The Guard Tower it is an offensive building \n " +
                    //    "that provides Line of Sight and fires arrows at enemy units.", "content"), buildingArea_Skin.textArea);
                    //if (GUI.Button(creatButton_Rect, "Create"))
                    //{
                    //    GameObject building = Instantiate(guardTower_Obj) as GameObject;
                    //    float posX = sprite.position.x;
                    //    float posY = sprite.position.y + sprite.size.y / 4; 
                    //    building.transform.position = new Vector2(posX, posY);
                    //    ActiveManager();
                    //}
                }
                GUI.EndGroup();

                //<!-- Academy(สถานศึกษา).
                GUI.BeginGroup(new Rect(0, 2 * frameHeight, background_Rect.width, frameHeight), new GUIContent("Academy", "Utility"), buildingArea_Skin.box);
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
				
                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceFarmGUI();
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 1 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceSawMill();
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 2 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceMillStone();
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 3 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceSmelter();
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 4 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
					this.DrawIntroduceStoreHouse();
                }
                GUI.EndGroup();        
				
				GUI.BeginGroup(new Rect(0, 5 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
		        {
					this.DrawIntroduceMarKet();
		        }
		        GUI.EndGroup();
				
				#endregion
            }
            else if (guiState == GUIState.ShowMilitaryUI)
            {
				#region <!-- Millitary Sections.

                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
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
        GUI.DrawTexture(imgRect, house_icon);   //<<-- "Draw icon texture".
        GUI.Box(tagName_Rect, new GUIContent(HouseBeh.BuildingName, "Tagname"), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(HouseBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(HouseBeh.RequireResource[0].Food.ToString(), stageManager.taskbarManager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(HouseBeh.RequireResource[0].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(HouseBeh.RequireResource[0].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Gold_Rect, new GUIContent(HouseBeh.RequireResource[0].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (Buildings.CheckingCanCreateBuilding() && Buildings.CheckingEnoughUpgradeResource(HouseBeh.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(creatButton_Rect, "Build"))
            {
                StoreHouse.UsedResource(HouseBeh.RequireResource[0]);

                GameObject temp_House = Instantiate(stageManager.house_prefab) as GameObject;
                HouseBeh housebeh = temp_House.GetComponent<HouseBeh>();
                housebeh.InitializeData(Buildings.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
                housebeh.OnBuildingProcess(housebeh);

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
        GUI.DrawTexture(imgRect, farm_Icon);
        GUI.Label(tagName_Rect, new GUIContent(Farm.BuildingName), tagname_Style);		
        GUI.BeginGroup(contentRect, new GUIContent(Farm.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(Farm.RequireResource[0].Food.ToString(), stageManager.taskbarManager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(Farm.RequireResource[0].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(Farm.RequireResource[0].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Gold_Rect, new GUIContent(Farm.RequireResource[0].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (Buildings.CheckingCanCreateBuilding() && Buildings.CheckingEnoughUpgradeResource(Farm.RequireResource[0]))
                enableUpgrade = true;
			
			GUI.enabled = enableUpgrade;
            if (GUI.Button(creatButton_Rect, "Build"))
            {
                StoreHouse.UsedResource(Farm.RequireResource[0]);

                GameObject temp = Instantiate(stageManager.farm_prefab) as GameObject;				
				Farm farm = temp.GetComponent<Farm>();
                farm.InitializeData(Buildings.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
				farm.OnBuildingProcess((Buildings)farm);

                ActiveManager();
            }
			GUI.enabled = true;
			
			#endregion
        }
        GUI.EndGroup();
    }

	private void DrawIntroduceSawMill()
	{
        GUI.Box(imgRect, new GUIContent(sawmill_Icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(Sawmill.BuildingName), tagname_Style);		
        GUI.BeginGroup(contentRect, new GUIContent(Sawmill.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(Sawmill.RequireResource[0].Food.ToString(), stageManager.taskbarManager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(Sawmill.RequireResource[0].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(Sawmill.RequireResource[0].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Gold_Rect, new GUIContent(Sawmill.RequireResource[0].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (Buildings.CheckingCanCreateBuilding() && Buildings.CheckingEnoughUpgradeResource(Sawmill.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(creatButton_Rect, "Build"))
            {
                StoreHouse.UsedResource(Sawmill.RequireResource[0]);

                GameObject building = Instantiate(stageManager.sawmill_prefab) as GameObject;
                Sawmill sawmill = building.GetComponent<Sawmill>();
                sawmill.InitializeData(Buildings.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
                sawmill.OnBuildingProcess((Buildings)sawmill);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
	}

	private void DrawIntroduceMillStone()
    {
        GUI.Box(imgRect, new GUIContent(millStone_Icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(MillStone.BuildingName), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(MillStone.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(MillStone.RequireResource[0].Food.ToString(), stageManager.taskbarManager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(MillStone.RequireResource[0].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(MillStone.RequireResource[0].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Gold_Rect, new GUIContent(MillStone.RequireResource[0].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (Buildings.CheckingCanCreateBuilding() && Buildings.CheckingEnoughUpgradeResource(MillStone.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(creatButton_Rect, "Build"))
            {
                StoreHouse.UsedResource(MillStone.RequireResource[0]);

                GameObject building = Instantiate(stageManager.millstone_prefab) as GameObject;
                MillStone millstone = building.GetComponent<MillStone>();
                millstone.InitializeData(Buildings.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
                millstone.OnBuildingProcess((Buildings)millstone);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

	private void DrawIntroduceSmelter()
    {
        GUI.Box(imgRect, new GUIContent(smelter_icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(Smelter.BuildingName), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(Smelter.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(Smelter.RequireResource[0].Food.ToString(), stageManager.taskbarManager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(Smelter.RequireResource[0].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(Smelter.RequireResource[0].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Gold_Rect, new GUIContent(Smelter.RequireResource[0].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (Buildings.CheckingCanCreateBuilding() && Buildings.CheckingEnoughUpgradeResource(Smelter.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(creatButton_Rect, "Build"))
            {
                StoreHouse.UsedResource(Smelter.RequireResource[0]);

                GameObject building = Instantiate(stageManager.smelter_prefab) as GameObject;
                Smelter smelter = building.GetComponent<Smelter>();
                smelter.InitializeData(Buildings.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
                smelter.OnBuildingProcess((Buildings)smelter);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    private void DrawIntroduceMarKet()
    {
        GUI.DrawTexture(imgRect, market_icon);     ///Market icon texture.
        GUI.Label(tagName_Rect, new GUIContent(MarketBeh.BuildingName), tagname_Style);     /// Market label tagname.
        GUI.BeginGroup(contentRect, new GUIContent(MarketBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(MarketBeh.RequireResource[0].Food.ToString(), stageManager.taskbarManager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(MarketBeh.RequireResource[0].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(MarketBeh.RequireResource[0].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Gold_Rect, new GUIContent(MarketBeh.RequireResource[0].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (Buildings.CheckingCanCreateBuilding() && Buildings.CheckingEnoughUpgradeResource(MarketBeh.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(creatButton_Rect, "Create"))
            {
                StoreHouse.UsedResource(MarketBeh.RequireResource[0]);

                GameObject market_obj = Instantiate(stageManager.market_prefab) as GameObject;
                MarketBeh market = market_obj.GetComponent<MarketBeh>();
                market.InitializeData(Buildings.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
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
        GUI.DrawTexture(imgRect, storeHouse_icon);
        GUI.Label(tagName_Rect, new GUIContent(StoreHouse.BuildingName), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(StoreHouse.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(StoreHouse.RequireResource[0].Food.ToString(), stageManager.taskbarManager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(StoreHouse.RequireResource[0].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(StoreHouse.RequireResource[0].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Gold_Rect, new GUIContent(StoreHouse.RequireResource[0].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (Buildings.CheckingCanCreateBuilding() && Buildings.CheckingEnoughUpgradeResource(StoreHouse.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(creatButton_Rect, "Build"))
            {
                StoreHouse.UsedResource(StoreHouse.RequireResource[0]);

                GameObject building = Instantiate(stageManager.storehouse_prefab) as GameObject;
                StoreHouse storeHouse = building.GetComponent<StoreHouse>();
                storeHouse.InitializeData(Buildings.BuildingStatus.onBuildingProcess, indexOfAreaPosition, 0);
                storeHouse.OnBuildingProcess(storeHouse);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    #endregion

    #region <!-- Military section.

    private void DrawIntroduceBarracks()
    {
        GUI.DrawTexture(imgRect, barrackNotation);   //<<-- "Draw icon texture".
        GUI.Label(tagName_Rect, new GUIContent(BarracksBeh.BuildingName), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(BarracksBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(BarracksBeh.RequireResource[0].Food.ToString(), stageManager.taskbarManager.food_icon), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(BarracksBeh.RequireResource[0].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(BarracksBeh.RequireResource[0].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_skin.box);
                GUI.Label(GameResource.Gold_Rect, new GUIContent(BarracksBeh.RequireResource[0].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_skin.box);
            }
            GUI.EndGroup();

            #region <!--- Build Button mechanichm.

            bool enableUpgrade = false;
            if (Buildings.CheckingCanCreateBuilding() && Buildings.CheckingEnoughUpgradeResource(BarracksBeh.RequireResource[0]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(creatButton_Rect, "Build"))
            {
                StoreHouse.UsedResource(BarracksBeh.RequireResource[0]);

                GameObject barracks_obj = Instantiate(stageManager.barracks_prefab) as GameObject;
                BarracksBeh barracks = barracks_obj.GetComponent<BarracksBeh>();
                barracks.InitializeData(Buildings.BuildingStatus.onBuildingProcess, this.indexOfAreaPosition, 0);
                barracks.OnBuildingProcess(barracks);

                ActiveManager();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    #endregion
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           