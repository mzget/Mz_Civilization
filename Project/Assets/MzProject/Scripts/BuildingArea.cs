using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingArea : MonoBehaviour {
    /// <summary>
    /// GUI : Texture && Skin.
    /// </summary>
    public GUISkin standard_skin;
    public GUISkin buildingArea_Skin;
	public GUISkin taskbarUI_Skin;
    private GUIStyle tagname_Style;
    /// <summary>
    /// Static GameObject //Prefabs, Texture2D,
    /// </summary>
    public Texture2D utility_icon;
    public Texture2D economy_icon;
    public Texture2D military_icon;
    //<!-- Utility.
    public GameObject houseObj;
    public Texture2D house_Notation;
    public GameObject guardTower_Obj;
    public Texture2D guardTower_icon;
    public GameObject academy_Obj;
    public Texture2D academy_icon;
    //<!-- Economic.
    public GameObject marketObj;
    public Texture2D marketNotation;
    public GameObject storeHouse_Obj;
    public Texture2D storeHouse_Icon;
    public GameObject farm_Obj;
    public Texture2D farm_Icon;
    public GameObject sawmill_Obj;
    public Texture2D sawmill_Icon;
    public GameObject millStone_Obj;
    public Texture2D millStone_Icon;
    public GameObject smelter_Obj;
    public Texture2D smelter_icon;
    //<!-- Militaly.
    public GameObject barrackOBJ;
    public Texture2D barrackNotation;
	
    private OTSprite sprite;
    private ObjectName objectname;
	private int indexOfArea;
	public int IndexOfArea { get {return indexOfArea;} set {indexOfArea = value;}}
    private enum GUIState { none = 0, ShowMainUI, ShowUtiltyUI, ShowEconomyUI, ShowMilitaryUI };
    private GUIState guiState;
	
	public class DrawOrder {
		public string nameofOrder;
		public int order;
		
		public DrawOrder(string name, int order) {
			nameofOrder = name;
		}
	};
	private static List<DrawOrder> drawOrderList = new List<DrawOrder>();
    private static DrawOrder drawOrder_farm = new DrawOrder("farm", 0);
    private static DrawOrder drawOrder_sawmill = new DrawOrder("sawmill", 1);
    private static DrawOrder drawOrder_millstone = new DrawOrder("millstone", 2);
    private static DrawOrder drawOrder_smelter = new DrawOrder("smelter", 3);
    private static DrawOrder drawOrder_market = new DrawOrder("market", 4); 
	private static DrawOrder drawOrder_storehouse = new DrawOrder("storehouse", 5);


    void Awake() {
        tagname_Style = buildingArea_Skin.customStyles[1];
    }

	// Use this for initialization
	void Start () {
        sprite = this.gameObject.GetComponent<OTSprite>();
        objectname = this.gameObject.GetComponent<ObjectName>();

		if(drawOrderList.Count == 0) {
			drawOrderList.Add(drawOrder_farm);
			drawOrderList.Add(drawOrder_sawmill);
			drawOrderList.Add(drawOrder_millstone);
			drawOrderList.Add(drawOrder_smelter);
            drawOrderList.Add(drawOrder_market);
			drawOrderList.Add(drawOrder_storehouse);
		}
		Debug.Log("BuildingArea :: " + "OnStart");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    #region Incloud Mouse Event.

    void OnMouseOver() { 
    }

    void OnMouseDown() {
        if (guiState != GUIState.ShowUtiltyUI) {
            guiState = GUIState.ShowUtiltyUI;
        }
    }

    void OnMouseExit() { 
		
    }

    #endregion
    
    void ActiveManager() {
        this.gameObject.SetActive(false);
    }
	

    private Rect windowRect;
    protected Rect exitButton_Rect;
    private Vector2 scrollPosition = Vector2.zero;
    protected Rect background_Rect;
    protected Rect imgRect;
    private Rect tagName_Rect;
    private Rect contentRect;
    private Rect creatButton_Rect;
    //<!-- Economy.
    //<!-- Military.
    //<!-- Utility.
    void OnGUI()
    {
        windowRect = new Rect(Main.GAMEWIDTH / 2 - 350, Main.GAMEHEIGHT / 2 - 200, 700, 400);
        exitButton_Rect = new Rect(windowRect.width - 34, 2, 32, 32);
        background_Rect = new Rect(0, 0, windowRect.width - 16, 320);
        imgRect = new Rect(40, 48, 80, 80);
        tagName_Rect = new Rect(0, 12, 160, 30);
        contentRect = new Rect(140, 10, background_Rect.width - 160, 140);
        creatButton_Rect = new Rect(420, 100, 100, 32);

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Main.FixedWidthRatio, Main.FixedHeightRatio, 1));
        		
        if(guiState != GUIState.none) {
            windowRect = GUI.Window(0, windowRect, this.CreateWindow, new GUIContent("Building Area", "GUI window"), standard_skin.window);
        }
    }

    void CreateWindow(int windowID)
    {
        Rect economy_Rect = new Rect(windowRect.width / 2 - 90, 24, 180, 48);
        Rect utility_Rect = new Rect(economy_Rect.x - 200, economy_Rect.y, economy_Rect.width, economy_Rect.height);
        Rect military_Rect = new Rect((economy_Rect.x + economy_Rect.width) +  20, economy_Rect.y, economy_Rect.width, economy_Rect.height);
        float frameHeight = 160;
		
		#region <!-- Mian Data flow.
		
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), buildingArea_Skin.customStyles[0])) {
            if (guiState != GUIState.none) {
                guiState = GUIState.none;
            }
        }

        if(GUI.Button(utility_Rect, new GUIContent("Utility", utility_icon), buildingArea_Skin.button)) {
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
				#region Utility Sections.
				
                //<!-- House.
                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), new GUIContent("House", "Utility"), buildingArea_Skin.box);
                {
                    GUI.Box(imgRect, new GUIContent(house_Notation, "Texture"));
                    GUI.Box(contentRect, new GUIContent("The house can be built to increase your maximum population by 50.", "Utility"), buildingArea_Skin.textArea);
                    if (GUI.Button(creatButton_Rect, "Build"))
                    {
                        GameObject house = Instantiate(houseObj) as GameObject;
                        house.transform.position = sprite.position;
                        ActiveManager();
                    }
                }
                GUI.EndGroup();

                //<!-- Guard Tower.
                GUI.BeginGroup(new Rect(0, 1 * frameHeight, background_Rect.width, frameHeight), new GUIContent("Guard Tower", "Utility"), buildingArea_Skin.box);
                {
                    GUI.Box(imgRect, new GUIContent(guardTower_icon, "Texture"));
                    GUI.Box(contentRect, new GUIContent("The Guard Tower it is an offensive building \n " +
                        "that provides Line of Sight and fires arrows at enemy units.", "content"), buildingArea_Skin.textArea);
                    if (GUI.Button(creatButton_Rect, "Create"))
                    {
                        GameObject building = Instantiate(guardTower_Obj) as GameObject;
                        float posX = sprite.position.x;
                        float posY = sprite.position.y + sprite.size.y / 4; 
                        building.transform.position = new Vector2(posX, posY);
                        ActiveManager();
                    }
                }
                GUI.EndGroup();

                //<!-- Academy(สถานศึกษา).
                GUI.BeginGroup(new Rect(0, 2 * frameHeight, background_Rect.width, frameHeight), new GUIContent("Academy", "Utility"), buildingArea_Skin.box);
                {
                    GUI.Box(imgRect, new GUIContent(academy_icon, "Utility Texture"));
                    GUI.Box(contentRect, new GUIContent("The Academy is a building in which you can \n" +
                        "research technologies to increase the power of your city and troops.", "content"), buildingArea_Skin.textArea);
                    if (GUI.Button(creatButton_Rect, "Create"))
                    {
                        GameObject building = Instantiate(academy_Obj) as GameObject;
                        building.transform.position = sprite.position;
                        ActiveManager();
                    }
                }
                GUI.EndGroup();
				
				#endregion
            }
            else if (guiState == GUIState.ShowEconomyUI)
            {
				#region <!-- Economy Sections.
				
                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
                    if (drawOrderList.Count >= 1)
                    {
						CalculationDrawOrderGUI(0);
                    }
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 1 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
                    if (drawOrderList.Count >= 2)
                    {
						CalculationDrawOrderGUI(1);
                    }
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 2 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
                    if (drawOrderList.Count >= 3)
                    {
						CalculationDrawOrderGUI(2);
                    }
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 3 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
                    if (drawOrderList.Count >= 4)
                    {
						CalculationDrawOrderGUI(3);
                    }
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 4 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
                {
					if(drawOrderList.Count >= 5) {
						CalculationDrawOrderGUI(4);
					}
                }
                GUI.EndGroup();        
				
				GUI.BeginGroup(new Rect(0, 5 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, buildingArea_Skin.box);
		        {
		            if(drawOrderList.Count >= 6)
						CalculationDrawOrderGUI(5);
		        }
		        GUI.EndGroup();
				
				#endregion
            }
            else if (guiState == GUIState.ShowMilitaryUI)
            {
				#region Millitary Sections.
				
                //<!-- Barrack.
                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), new GUIContent(BarrackBeh.BuildingName, "ค่ายทหาร"), buildingArea_Skin.box);
                {
                    GUI.DrawTexture(imgRect, barrackNotation, ScaleMode.ScaleToFit);
                    GUI.BeginGroup(contentRect, new GUIContent(BarrackBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
					{
						//<!-- Requirements Resource.
                        GUI.BeginGroup(GameResource.RequireResource_Rect);
						{
                            GUI.Label(GameResource.Food_Rect, new GUIContent(BarrackBeh.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                            GUI.Label(GameResource.Wood_Rect, new GUIContent(BarrackBeh.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                            GUI.Label(GameResource.Copper_Rect, new GUIContent(BarrackBeh.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                            GUI.Label(GameResource.Stone_Rect, new GUIContent(BarrackBeh.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
						}
						GUI.EndGroup();
						
						//<!-- Build Button.
						if(StoreHouse.sumOfFood >= BarrackBeh.CreateResource.Food && StoreHouse.sumOfWood >= BarrackBeh.CreateResource.Wood &&
							StoreHouse.sumOfGold >= BarrackBeh.CreateResource.Gold && StoreHouse.sumOfStone >= BarrackBeh.CreateResource.Stone)
						{
                            Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
                            if (Buildings._CanCreateBuilding)
                            {
                                if (GUI.Button(creatButton_Rect, "Build"))
                                {
                                    StoreHouse.UsedResource(BarrackBeh.CreateResource);

                                    GameObject barrack = Instantiate(barrackOBJ) as GameObject;
                                    barrack.transform.position = sprite.position;
                                    ActiveManager();
                                }
                            }
						}
					}
					GUI.EndGroup();
                }
                GUI.EndGroup();
				
				#endregion
            }
        }
        // End the scroll view that we began above.
        GUI.EndScrollView();
    }
	
	private void CalculationDrawOrderGUI(int indexUIorder) 
	{		
        for (int i = 0; i < drawOrderList.Count; i++) 
		{
            if (drawOrderList[indexUIorder].nameofOrder == drawOrderList[i].nameofOrder) 
			{
                drawOrderList[i].order = i;
                if (drawOrderList[i].nameofOrder == drawOrder_farm.nameofOrder) {
					DrawIntroduceFarmGUI(); 
					drawOrder_farm.order = i;
				}
                else if (drawOrderList[i].nameofOrder == drawOrder_sawmill.nameofOrder) {
					DrawIntroduceSawMill();
					drawOrder_sawmill.order = i;
				}
                else if (drawOrderList[i].nameofOrder == drawOrder_millstone.nameofOrder) {
					DrawIntroduceMillStone();
					drawOrder_millstone.order = i;
				}
                else if (drawOrderList[i].nameofOrder == drawOrder_smelter.nameofOrder) {
					DrawIntroduceSmelter();
					drawOrder_smelter.order = i;
				}
                else if (drawOrderList[i].nameofOrder == drawOrder_market.nameofOrder) {
					DrawIntroMarKet();
					drawOrder_market.order = i;
				}
                else if (drawOrderList[i].nameofOrder == drawOrder_storehouse.nameofOrder) {
					DrawIntroduceStoreHouse();
					drawOrder_storehouse.order = i;
				}
            }
        }
	}
	
    void DrawIntroduceFarmGUI()
    {
        GUI.DrawTexture(imgRect, farm_Icon);
        GUI.Label(tagName_Rect, new GUIContent(Farm.BuildingName), tagname_Style);		
        GUI.BeginGroup(contentRect, new GUIContent(Farm.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(Farm.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(Farm.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(GameResource.Copper_Rect, new GUIContent(Farm.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(Farm.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= Farm.CreateResource.Food && StoreHouse.sumOfWood >= Farm.CreateResource.Wood &&
                StoreHouse.sumOfGold >= Farm.CreateResource.Gold && StoreHouse.sumOfStone >= Farm.CreateResource.Stone)
            {
				Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
				if(Buildings._CanCreateBuilding) 
                {
	                if (GUI.Button(creatButton_Rect, "Build"))
	                {
	                    StoreHouse.UsedResource(Farm.CreateResource);
	
	                    GameObject building = Instantiate(farm_Obj) as GameObject;
						building.transform.position = StageManager.buildingArea_Pos[this.indexOfArea];
						
						Farm newFarm = building.GetComponent<Farm>();
						newFarm.currentBuildingStatus = Buildings.BuildingStatus.onBuildingProcess;
						newFarm.OnBuildingProcess((Buildings)newFarm);
	                    newFarm.IndexOfPosition = this.indexOfArea;
                        Buildings.Farm_Instance.Add(newFarm);
	
	                    drawOrderList.RemoveAt(drawOrder_farm.order);
	                    ActiveManager();
	                }
				}
            }
        }
        GUI.EndGroup();
    }
	void DrawIntroduceSawMill()
	{
        GUI.Box(imgRect, new GUIContent(sawmill_Icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(Sawmill.BuildingName), tagname_Style);		
        GUI.BeginGroup(contentRect, new GUIContent(Sawmill.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(Sawmill.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(Sawmill.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(GameResource.Copper_Rect, new GUIContent(Sawmill.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(Sawmill.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= Sawmill.CreateResource.Food && StoreHouse.sumOfWood >= Sawmill.CreateResource.Wood &&
                StoreHouse.sumOfGold >= Sawmill.CreateResource.Gold && StoreHouse.sumOfStone >= Sawmill.CreateResource.Stone)
            {
				Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
				if(Buildings._CanCreateBuilding)
				{
	                if (GUI.Button(creatButton_Rect, "Build")) 
					{
	                    StoreHouse.UsedResource(Sawmill.CreateResource);
	
	                    GameObject building = Instantiate(sawmill_Obj) as GameObject;
	                    building.transform.position = StageManager.buildingArea_Pos[this.indexOfArea];
						
						Sawmill new_sawmill = building.GetComponent<Sawmill>();
						new_sawmill.currentBuildingStatus = Buildings.BuildingStatus.onBuildingProcess;
						new_sawmill.OnBuildingProcess((Buildings)new_sawmill);
	                    new_sawmill.IndexOfPosition = this.indexOfArea;
                        Buildings.Sawmill_Instance.Add(new_sawmill);
						
	                    drawOrderList.RemoveAt(drawOrder_sawmill.order);
	                    ActiveManager();
	                }
				}
            }
        }
        GUI.EndGroup();
	}
	void DrawIntroduceMillStone()
	{
        GUI.Box(imgRect, new GUIContent(millStone_Icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(MillStone.BuildingName), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(MillStone.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(MillStone.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(MillStone.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(GameResource.Copper_Rect, new GUIContent(MillStone.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(MillStone.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= MillStone.CreateResource.Food && StoreHouse.sumOfWood >= MillStone.CreateResource.Wood &&
                StoreHouse.sumOfGold >= MillStone.CreateResource.Gold && StoreHouse.sumOfStone >= MillStone.CreateResource.Stone)
            {
				Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
				if(Buildings._CanCreateBuilding) {
	                if (GUI.Button(creatButton_Rect, "Build"))
	                {
	                    StoreHouse.UsedResource(MillStone.CreateResource);
	
	                    GameObject building = Instantiate(millStone_Obj) as GameObject;
	                    building.transform.position = StageManager.buildingArea_Pos[this.indexOfArea];
						
						MillStone millstone = building.GetComponent<MillStone>();
						millstone.currentBuildingStatus = Buildings.BuildingStatus.onBuildingProcess;
						millstone.OnBuildingProcess((Buildings)millstone);
	                    millstone.IndexOfPosition = this.indexOfArea;
                        Buildings.MillStoneInstance.Add(millstone);
	
	                    drawOrderList.RemoveAt(drawOrder_millstone.order);
	                    ActiveManager();
	                }
				}
            }
        }
        GUI.EndGroup();
	}
	void DrawIntroduceSmelter() 
	{
        GUI.Box(imgRect, new GUIContent(smelter_icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(Smelter.BuildingName), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(Smelter.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(Smelter.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(Smelter.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(GameResource.Copper_Rect, new GUIContent(Smelter.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(Smelter.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= Smelter.CreateResource.Food && StoreHouse.sumOfWood >= Smelter.CreateResource.Wood &&
                StoreHouse.sumOfGold >= Smelter.CreateResource.Gold && StoreHouse.sumOfStone >= Smelter.CreateResource.Stone)
            {
				Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
				if(Buildings._CanCreateBuilding) 
				{
	                if (GUI.Button(creatButton_Rect, "Build"))
	                {
	                    StoreHouse.UsedResource(Smelter.CreateResource);
	
	                    GameObject building = Instantiate(smelter_Obj) as GameObject;
	                    building.transform.position = StageManager.buildingArea_Pos[this.indexOfArea];
						
						Smelter smelter = building.GetComponent<Smelter>();
						smelter.currentBuildingStatus = Buildings.BuildingStatus.onBuildingProcess;
						smelter.OnBuildingProcess((Buildings)smelter);
	                    smelter.IndexOfPosition = this.indexOfArea;
						
                        Buildings.SmelterInstance.Add(smelter);
	
	                    drawOrderList.RemoveAt(drawOrder_smelter.order);
	                    ActiveManager();
	                }
				}
            }
        }
        GUI.EndGroup();
	}

    private void DrawIntroMarKet()
    {
        GUI.Box(imgRect, new GUIContent(marketNotation, "Market Texture"));
        GUI.Label(tagName_Rect, new GUIContent(MargetBeh.BuildingName), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(MargetBeh.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(MargetBeh.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(MargetBeh.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(GameResource.Copper_Rect, new GUIContent(MargetBeh.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(MargetBeh.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create Button.
            if (StoreHouse.sumOfFood >= MargetBeh.CreateResource.Food && StoreHouse.sumOfWood >= MargetBeh.CreateResource.Wood &&
                StoreHouse.sumOfGold >= MargetBeh.CreateResource.Gold && StoreHouse.sumOfStone >= MargetBeh.CreateResource.Stone)
            {
				Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
				if(Buildings._CanCreateBuilding) {
		            if (GUI.Button(creatButton_Rect, "Create"))
		            {
		                StoreHouse.UsedResource(MargetBeh.CreateResource);
		
		                GameObject market = Instantiate(marketObj) as GameObject;
		                market.transform.position = sprite.position;
		                ActiveManager();
		            }
				}
            }
        }
        GUI.EndGroup();
    }

    private void DrawIntroduceStoreHouse()
    {
        GUI.Box(imgRect, new GUIContent(storeHouse_Icon, "Texture"));
        GUI.Label(tagName_Rect, new GUIContent(StoreHouse.BuildingName), tagname_Style);
        GUI.BeginGroup(contentRect, new GUIContent(StoreHouse.CurrentDescription, "content"), buildingArea_Skin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(GameResource.RequireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(GameResource.Food_Rect, new GUIContent(StoreHouse.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(GameResource.Wood_Rect, new GUIContent(StoreHouse.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(GameResource.Copper_Rect, new GUIContent(StoreHouse.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(GameResource.Stone_Rect, new GUIContent(StoreHouse.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= StoreHouse.CreateResource.Food && StoreHouse.sumOfWood >= StoreHouse.CreateResource.Wood &&
                StoreHouse.sumOfGold >= StoreHouse.CreateResource.Gold && StoreHouse.sumOfStone >= StoreHouse.CreateResource.Stone)
            {
                Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
                if (Buildings._CanCreateBuilding)
                {
                    if (GUI.Button(creatButton_Rect, "Build"))
                    {
                        StoreHouse.UsedResource(StoreHouse.CreateResource);

                        GameObject building = Instantiate(storeHouse_Obj) as GameObject;
                        building.transform.position = StageManager.buildingArea_Pos[this.indexOfArea];

                        StoreHouse storeHouse = building.GetComponent<StoreHouse>();
						storeHouse.currentBuildingStatus = Buildings.BuildingStatus.onBuildingProcess;
						storeHouse.OnBuildingProcess(storeHouse);
						storeHouse.IndexOfPosition = this.indexOfArea;
						
                        Buildings.StoreHouseInstance.Add(storeHouse);
						
                        storeHouse.ID = Buildings.StoreHouseInstance.Count;

                        ActiveManager();
                    }
                }
            }
        }
        GUI.EndGroup();
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           