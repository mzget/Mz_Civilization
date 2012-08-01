using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingArea : MonoBehaviour {
    /// <summary>
    /// GUI : Texture && Skin.
    /// </summary>
    public GUISkin standard_skin;
    public GUISkin mainBuildingSkin;
    public GUISkin buildingArea_Skin;
	public GUISkin taskbarUI_Skin;
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
    private static DrawOrder farm = new DrawOrder("farm", 0);
    private static DrawOrder sawmill = new DrawOrder("sawmill", 1);
    private static DrawOrder millstone = new DrawOrder("millstone", 2);
    private static DrawOrder smelter = new DrawOrder("smelter", 3);
    private static DrawOrder market = new DrawOrder("market", 4); 
	private static DrawOrder storehouse = new DrawOrder("storehouse", 5);
    
	// Use this for initialization
	void Start () {
        sprite = this.gameObject.GetComponent<OTSprite>();
        objectname = this.gameObject.GetComponent<ObjectName>();

		if(drawOrderList.Count == 0) {
			drawOrderList.Add(farm);
			drawOrderList.Add(sawmill);
			drawOrderList.Add(millstone);
			drawOrderList.Add(smelter);
            drawOrderList.Add(market);
			drawOrderList.Add(storehouse);
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
    
    void DestroyManager() 
    {
        Destroy(this.gameObject);
    }
	

    private Rect windowRect;
    protected Rect exitButton_Rect;
    private Vector2 scrollPosition = Vector2.zero;
    protected Rect background_Rect;
    protected Rect imgRect;
    private Rect tagName_Rect;
    private Rect contentRect;
    private Rect creatButton_Rect;
	private Rect requireResource_Rect = new Rect(10, 100, 400, 32);
	private Rect food_Rect = new Rect(0,1,100,30);
	private Rect wood_Rect = new Rect(100,1,100,30);
	private Rect gold_Rect = new Rect(200,1,100,30);
	private Rect stone_Rect= new Rect(300,1,100,30);
    //<!-- Economy.
    //<!-- Military.
    //<!-- Utility.
    void OnGUI()
    {
        windowRect = new Rect(Main.GameWidth / 2 - 350, Main.GameHeight / 2 - 200, 700, 400);
        exitButton_Rect = new Rect(windowRect.width - 34, 2, 32, 32);
        background_Rect = new Rect(0, 0, windowRect.width - 16, 320);
        imgRect = new Rect(40, 48, 80, 80);
        tagName_Rect = new Rect(0, 12, 160, 30);
        contentRect = new Rect(140, 10, background_Rect.width - 160, 140);
        creatButton_Rect = new Rect(420, 100, 100, 32);

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GameWidth, Screen.height / Main.GameHeight, 1));
        		
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
		
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), mainBuildingSkin.customStyles[0])) {
            if (guiState != GUIState.none) {
                guiState = GUIState.none;
            }
        }

        if(GUI.Button(utility_Rect, new GUIContent("Utility", utility_icon), mainBuildingSkin.button)) {
            if (guiState != GUIState.ShowUtiltyUI) {
                guiState = GUIState.ShowUtiltyUI;
            }
			
			scrollPosition = Vector2.zero;
        }
        else if (GUI.Button(economy_Rect, new GUIContent("Economy",economy_icon), mainBuildingSkin.button)) {
            if (guiState != GUIState.ShowEconomyUI) {
                guiState = GUIState.ShowEconomyUI;
            }
			
			scrollPosition = Vector2.zero;
        }
        else if (GUI.Button(military_Rect, new GUIContent("Military", military_icon), mainBuildingSkin.button)) {
            if (guiState != GUIState.ShowMilitaryUI) {
                guiState = GUIState.ShowMilitaryUI;
            }
			
			scrollPosition = Vector2.zero;
        }
		
		#endregion

        // An absolute-positioned example: We make a scrollview that has a really large client
        // rect and put it in a small rect on the screen.
        scrollPosition = GUI.BeginScrollView(
            new Rect(0, 80, windowRect.width, background_Rect.height),
            scrollPosition,
            new Rect(0, 0, background_Rect.width, 1200), false, true); 
        {
            mainBuildingSkin.box.contentOffset = new Vector2(50, 20);
            mainBuildingSkin.textArea.wordWrap = true;

            //<!-- Utility.
            if (guiState == GUIState.ShowUtiltyUI)          
            {
				#region Utility Sections.
				
                //<!-- House.
                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), new GUIContent("House", "Utility"), mainBuildingSkin.box);
                {
                    GUI.Box(imgRect, new GUIContent(house_Notation, "Texture"));
                    GUI.Box(contentRect, new GUIContent("The house can be built to increase your maximum population by 50.", "Utility"), mainBuildingSkin.textArea);
                    if (GUI.Button(creatButton_Rect, "Build"))
                    {
                        GameObject house = Instantiate(houseObj) as GameObject;
                        house.transform.position = sprite.position;
                        DestroyManager();
                    }
                }
                GUI.EndGroup();

                //<!-- Guard Tower.
                GUI.BeginGroup(new Rect(0, 1 * frameHeight, background_Rect.width, frameHeight), new GUIContent("Guard Tower", "Utility"), mainBuildingSkin.box);
                {
                    GUI.Box(imgRect, new GUIContent(guardTower_icon, "Texture"));
                    GUI.Box(contentRect, new GUIContent("The Guard Tower it is an offensive building \n " +
                        "that provides Line of Sight and fires arrows at enemy units.", "content"), mainBuildingSkin.textArea);
                    if (GUI.Button(creatButton_Rect, "Create"))
                    {
                        GameObject building = Instantiate(guardTower_Obj) as GameObject;
                        float posX = sprite.position.x;
                        float posY = sprite.position.y + sprite.size.y / 4; 
                        building.transform.position = new Vector2(posX, posY);
                        DestroyManager();
                    }
                }
                GUI.EndGroup();

                //<!-- Academy(สถานศึกษา).
                GUI.BeginGroup(new Rect(0, 2 * frameHeight, background_Rect.width, frameHeight), new GUIContent("Academy", "Utility"), mainBuildingSkin.box);
                {
                    GUI.Box(imgRect, new GUIContent(academy_icon, "Utility Texture"));
                    GUI.Box(contentRect, new GUIContent("The Academy is a building in which you can \n" +
                        "research technologies to increase the power of your city and troops.", "content"), mainBuildingSkin.textArea);
                    if (GUI.Button(creatButton_Rect, "Create"))
                    {
                        GameObject building = Instantiate(academy_Obj) as GameObject;
                        building.transform.position = sprite.position;
                        DestroyManager();
                    }
                }
                GUI.EndGroup();
				
				#endregion
            }
            else if (guiState == GUIState.ShowEconomyUI)
            {
				#region <!-- Economy Sections.
				
                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, mainBuildingSkin.box);
                {
                    if (drawOrderList.Count >= 1)
                    {
						CalculationDrawOrderGUI(0);
                    }
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 1 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, mainBuildingSkin.box);
                {
                    if (drawOrderList.Count >= 2)
                    {
						CalculationDrawOrderGUI(1);
                    }
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 2 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, mainBuildingSkin.box);
                {
                    if (drawOrderList.Count >= 3)
                    {
						CalculationDrawOrderGUI(2);
                    }
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 3 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, mainBuildingSkin.box);
                {
                    if (drawOrderList.Count >= 4)
                    {
						CalculationDrawOrderGUI(3);
                    }
                }
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, 4 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, mainBuildingSkin.box);
                {
					if(drawOrderList.Count >= 5) {
						CalculationDrawOrderGUI(4);
					}
                }
                GUI.EndGroup();        
				
				GUI.BeginGroup(new Rect(0, 5 * frameHeight, background_Rect.width, frameHeight), GUIContent.none, mainBuildingSkin.box);
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
                GUI.BeginGroup(new Rect(0, 0 * frameHeight, background_Rect.width, frameHeight), new GUIContent("Barrack", "ค่ายทหาร"), mainBuildingSkin.box);
                {
                    GUI.DrawTexture(imgRect, barrackNotation, ScaleMode.ScaleToFit);
                    GUI.BeginGroup(contentRect, new GUIContent("ในค่ายทหารแห่งนี้ คุณสามารถเกณฑ์กองทหารราบได้ \n ยิ่งระดับค่ายทหารมากเท่าไร ก็จะสร้างกองกำลังได้เร็วขึ้นเท่านั้น", "content"), mainBuildingSkin.textArea);
					{
						//<!-- Requirements Resource.
						GUI.BeginGroup(requireResource_Rect);
						{
							GUI.Label(food_Rect, new GUIContent(BarrackBeh.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
							GUI.Label(wood_Rect, new GUIContent(BarrackBeh.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
							GUI.Label(gold_Rect, new GUIContent(BarrackBeh.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
							GUI.Label(stone_Rect, new GUIContent(BarrackBeh.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
						}
						GUI.EndGroup();
						
						//<!-- Build Button.
						if(StoreHouse.sumOfFood >= BarrackBeh.CreateResource.Food &&
							StoreHouse.sumOfWood >= BarrackBeh.CreateResource.Wood &&
							StoreHouse.sumOfGold >= BarrackBeh.CreateResource.Gold &&
							StoreHouse.sumOfStone >= BarrackBeh.CreateResource.Stone)
						{
		                    if (GUI.Button(creatButton_Rect, "Build"))
		                    {
								StoreHouse.UsedResource(BarrackBeh.CreateResource);
								
		                        GameObject barrack = Instantiate(barrackOBJ) as GameObject;
		                        barrack.transform.position = sprite.position;
		                        DestroyManager();
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
                if (drawOrderList[i].nameofOrder == farm.nameofOrder) {
					DrawIntroduceFarmGUI(); 
					farm.order = i;
				}
                else if (drawOrderList[i].nameofOrder == sawmill.nameofOrder) {
					DrawIntroduceSawMill();
					sawmill.order = i;
				}
                else if (drawOrderList[i].nameofOrder == millstone.nameofOrder) {
					DrawIntroduceMillStone();
					millstone.order = i;
				}
                else if (drawOrderList[i].nameofOrder == smelter.nameofOrder) {
					DrawIntroduceSmelter();
					smelter.order = i;
				}
                else if (drawOrderList[i].nameofOrder == market.nameofOrder) {
					DrawIntroMarKet();
					market.order = i;
				}
                else if (drawOrderList[i].nameofOrder == storehouse.nameofOrder) {
					DrawIntroduceStoreHouse();
					storehouse.order = i;
				}
            }
        }
	}
	
    void DrawIntroduceFarmGUI()
    {
        GUI.DrawTexture(imgRect, farm_Icon);
        GUI.Label(tagName_Rect, new GUIContent(Farm.BuildingName), buildingArea_Skin.label);
		
        GUI.BeginGroup(contentRect, new GUIContent(Farm.Description, "content"), mainBuildingSkin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(requireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(food_Rect, new GUIContent(Farm.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(wood_Rect, new GUIContent(Farm.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(gold_Rect, new GUIContent(Farm.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(stone_Rect, new GUIContent(Farm.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= Farm.CreateResource.Food && StoreHouse.sumOfWood >= Farm.CreateResource.Wood &&
                StoreHouse.sumOfGold >= Farm.CreateResource.Gold && StoreHouse.sumOfStone >= Farm.CreateResource.Stone)
            {
				Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
				if(Buildings._CanCreateBuilding) {
	                if (GUI.Button(creatButton_Rect, "Build"))
	                {
	                    StoreHouse.UsedResource(Farm.CreateResource);
	
	                    GameObject building = Instantiate(farm_Obj) as GameObject;
	                    building.transform.position = sprite.position;
	
	                    drawOrderList.RemoveAt(farm.order);
	                    DestroyManager();
	                }
				}
            }
        }
        GUI.EndGroup();
    }
	
	void DrawIntroduceSawMill()
	{
        GUI.Box(imgRect, new GUIContent(sawmill_Icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(Sawmill.BuildingName), buildingArea_Skin.label);
		
        GUI.BeginGroup(contentRect, new GUIContent(Sawmill.Description, "content"), mainBuildingSkin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(requireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(food_Rect, new GUIContent(Sawmill.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(wood_Rect, new GUIContent(Sawmill.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(gold_Rect, new GUIContent(Sawmill.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(stone_Rect, new GUIContent(Sawmill.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= Sawmill.CreateResource.Food && StoreHouse.sumOfWood >= Sawmill.CreateResource.Wood &&
                StoreHouse.sumOfGold >= Sawmill.CreateResource.Gold && StoreHouse.sumOfStone >= Sawmill.CreateResource.Stone)
            {
				Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
				if(Buildings._CanCreateBuilding) {
	                if (GUI.Button(creatButton_Rect, "Build")) {
	                    StoreHouse.UsedResource(Sawmill.CreateResource);
	
	                    GameObject building = Instantiate(sawmill_Obj) as GameObject;
	                    building.transform.position = sprite.position;
						
	                    drawOrderList.RemoveAt(sawmill.order);
	                    DestroyManager();
	                }
				}
            }
        }
        GUI.EndGroup();
	}
	
	void DrawIntroduceMillStone()
	{
        GUI.Box(imgRect, new GUIContent(millStone_Icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(MillStone.BuildingName), buildingArea_Skin.label);
        GUI.BeginGroup(contentRect, new GUIContent(MillStone.Description, "content"), mainBuildingSkin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(requireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(food_Rect, new GUIContent(MillStone.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(wood_Rect, new GUIContent(MillStone.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(gold_Rect, new GUIContent(MillStone.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(stone_Rect, new GUIContent(MillStone.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
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
	                    building.transform.position = sprite.position;
	
	                    drawOrderList.RemoveAt(millstone.order);
	                    DestroyManager();
	                }
				}
            }
        }
        GUI.EndGroup();
	}
	
	void DrawIntroduceSmelter() 
	{
        GUI.Box(imgRect, new GUIContent(smelter_icon, "IconTexture"));
        GUI.Label(tagName_Rect, new GUIContent(Smelter.BuildingName), buildingArea_Skin.label);
        GUI.BeginGroup(contentRect, new GUIContent(Smelter.Description, "content"), mainBuildingSkin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(requireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(food_Rect, new GUIContent(Smelter.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(wood_Rect, new GUIContent(Smelter.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(gold_Rect, new GUIContent(Smelter.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(stone_Rect, new GUIContent(Smelter.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= Smelter.CreateResource.Food && StoreHouse.sumOfWood >= Smelter.CreateResource.Wood &&
                StoreHouse.sumOfGold >= Smelter.CreateResource.Gold && StoreHouse.sumOfStone >= Smelter.CreateResource.Stone)
            {
				Buildings._CanCreateBuilding = Buildings.CheckingCanCreateBuilding();
				if(Buildings._CanCreateBuilding) {
	                if (GUI.Button(creatButton_Rect, "Build"))
	                {
	                    StoreHouse.UsedResource(Smelter.CreateResource);
	
	                    GameObject building = Instantiate(smelter_Obj) as GameObject;
	                    building.transform.position = sprite.position;
	
	                    //Buildings.SmelterInstance = building.GetComponent<Smelter>();
	                    drawOrderList.RemoveAt(smelter.order);
	                    DestroyManager();
	                }
				}
            }
        }
        GUI.EndGroup();
	}

    private void DrawIntroMarKet()
    {
        GUI.Box(imgRect, new GUIContent(marketNotation, "Market Texture"));
        GUI.BeginGroup(contentRect, new GUIContent("สร้างและฝึกฝนกองคาราวาน ซื้อขายและแลกเปลี่ยนสินค้า \n วิจัยและพัฒนากลไกการตลาด", "content"), mainBuildingSkin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(requireResource_Rect);
            {
                GUI.Label(food_Rect, new GUIContent(MargetBeh.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(wood_Rect, new GUIContent(MargetBeh.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(gold_Rect, new GUIContent(MargetBeh.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(stone_Rect, new GUIContent(MargetBeh.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create Button.
            if (StoreHouse.sumOfFood >= MargetBeh.CreateResource.Food &&
                StoreHouse.sumOfWood >= MargetBeh.CreateResource.Wood &&
                StoreHouse.sumOfGold >= MargetBeh.CreateResource.Gold &&
                StoreHouse.sumOfStone >= MargetBeh.CreateResource.Stone)
            {
                if (GUI.Button(creatButton_Rect, "Create"))
                {
                    StoreHouse.UsedResource(MargetBeh.CreateResource);

                    GameObject market = Instantiate(marketObj) as GameObject;
                    market.transform.position = sprite.position;
                    DestroyManager();
                }
            }
        }
        GUI.EndGroup();
    }
	
	private void DrawIntroduceStoreHouse()
	{
		GUI.Box(imgRect, new GUIContent(storeHouse_Icon, "Market Texture"));
        GUI.BeginGroup(contentRect, new GUIContent(StoreHouse.description, "content"), mainBuildingSkin.textArea);
        {
            //<!-- Requirements Resource.
            GUI.BeginGroup(requireResource_Rect, GUIContent.none, GUIStyle.none);
            {
                GUI.Label(food_Rect, new GUIContent(StoreHouse.CreateResource.Food.ToString(), taskbarUI_Skin.customStyles[0].normal.background), standard_skin.box);
                GUI.Label(wood_Rect, new GUIContent(StoreHouse.CreateResource.Wood.ToString(), taskbarUI_Skin.customStyles[1].normal.background), standard_skin.box);
                GUI.Label(gold_Rect, new GUIContent(StoreHouse.CreateResource.Gold.ToString(), taskbarUI_Skin.customStyles[2].normal.background), standard_skin.box);
                GUI.Label(stone_Rect, new GUIContent(StoreHouse.CreateResource.Stone.ToString(), taskbarUI_Skin.customStyles[3].normal.background), standard_skin.box);
            }
            GUI.EndGroup();
            //<!-- Create button.
            if (StoreHouse.sumOfFood >= StoreHouse.CreateResource.Food &&
                StoreHouse.sumOfWood >= StoreHouse.CreateResource.Wood &&
                StoreHouse.sumOfGold >= StoreHouse.CreateResource.Gold &&
                StoreHouse.sumOfStone >= StoreHouse.CreateResource.Stone)
            {
                if (GUI.Button(creatButton_Rect, "Build"))
                {
                    StoreHouse.UsedResource(StoreHouse.CreateResource);

                    GameObject building = Instantiate(storeHouse_Obj) as GameObject;
                    building.transform.position = sprite.position;
                    DestroyManager();

                    StoreHouse storeHouse = building.GetComponent<StoreHouse>();
                    Buildings.storeHouseId.Add(storeHouse);
                    storeHouse.ID = Buildings.storeHouseId.Count;
                }
            }
        }
        GUI.EndGroup();
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           