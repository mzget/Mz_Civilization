using UnityEngine;
using System.Collections;

public class BarrackBeh : Buildings
{
	//<!-- Static Data.
    public static GameResource CreateResource = new GameResource(40, 180, 120, 80);
    public static GameResource[] UpgradeResource = new GameResource[10] {
		new GameResource(80, 120, 40, 60),
        new GameResource(200, 200, 200, 200),
        new GameResource(300, 300, 300, 300),
        new GameResource(400, 400, 400, 400),
        new GameResource(500, 500, 500, 500),
        new GameResource(600, 600, 600, 600),
        new GameResource(700, 700, 700, 700),
        new GameResource(800, 800, 800, 800),
        new GameResource(900, 900, 900, 900),
        new GameResource(1000, 1000, 1000, 1000),
	};
    public static string BuildingName = "Barrack";
    private static string Description_TH = "ในค่ายทหารนี้คุณสามารถเกณฑ์ทหารราบได้ ยิ่งระดับค่ายทหารมากเท่าไร \n " + "กองกำลังก็จะเข็มแข็งมากขึ้น";
    private static string Description_EN = "Trains infantry units. Also researches technologies related to infantry units";
	public static string CurrentDescription {
        get {
            string temp = Description_EN;

            if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.Thai)
                temp = Description_TH;

            return temp;
        }
	}

	public GUISkin mainBuildingSkin;
	public Texture2D spearmanTex;
	public Texture2D hypaspistTex;
	public Texture2D hoplistTex;
	public Texture2D ToxotesTex;	
	

    protected override void Awake() {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
		
		buildingIcon_Texture = Resources.Load("Textures/Building_Icons/Barracks", typeof(Texture2D)) as Texture2D;
    }
    
	// Use this for initialization
	void Start () {
		
    }

    #region Building Processing.

    public override void OnBuildingProcess(Buildings obj)
    {
        base.OnBuildingProcess(obj);
    }
    protected override void CreateProcessBar(Buildings.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void BuildingProcessComplete(Buildings obj)
    {
        base.BuildingProcessComplete(obj);

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != Buildings.BuildingStatus.buildingComplete)
            this.currentBuildingStatus = Buildings.BuildingStatus.buildingComplete;
    }

    #endregion

    protected override void ClearStorageData()
    {
        base.ClearStorageData();

        Buildings.barrack_Instances.Remove(this);
    }
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	private Rect buttonRect = new Rect (460, 140, 100, 30);
    
	protected override void CreateWindow (int windowID)
	{
        base.CreateWindow(windowID);

        float height = 160f;
        GUI.Box(new Rect(48, 24, 256, 48), base.currentBuildingStatus.ToString(), standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height), 
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height * 5));
        {
            GUI.BeginGroup(base.building_background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.buildingIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.level, base.status_style);
                GUI.BeginGroup(base.description_Rect, CurrentDescription, building_Skin.textArea);
                {   //<!-- group draw order.

                    //<!-- Current Production rate.
                    if (base.level < 10)
                    {
                        //GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                        //GUI.Label(nextProduction_Rect, "Next Max Capacity : " + this.maxCapacities[base.level], base.job_style);

                        //<!-- Requirements Resource.
                        GUI.BeginGroup(update_requireResource_Rect);
                        {
                            GUI.Box(GameResource.Food_Rect, new GUIContent(UpgradeResource[level].Food.ToString(), base.food_icon), standard_Skin.box);
                            GUI.Box(GameResource.Wood_Rect, new GUIContent(UpgradeResource[level].Wood.ToString(), base.wood_icon), standard_Skin.box);
                            GUI.Box(GameResource.Copper_Rect, new GUIContent(UpgradeResource[level].Gold.ToString(), base.copper_icon), standard_Skin.box);
                            GUI.Box(GameResource.Stone_Rect, new GUIContent(UpgradeResource[level].Stone.ToString(), base.stone_icon), standard_Skin.box);
                        }
                        GUI.EndGroup();

                        //<!-- Upgrade Button.
                        if (StoreHouse.sumOfFood >= UpgradeResource[level].Food && StoreHouse.sumOfWood >= UpgradeResource[level].Wood &&
                            StoreHouse.sumOfGold >= UpgradeResource[level].Gold && StoreHouse.sumOfStone >= UpgradeResource[level].Stone)
                        {
                            bool enableUpgrade = base.CheckingCanUpgradeLevel();

                            GUI.enabled = enableUpgrade;
                            if (GUI.Button(base.upgradeButton_Rect, new GUIContent("Upgrade")))
                            {
                                StoreHouse.UsedResource(UpgradeResource[level]);

                                base.currentBuildingStatus = Buildings.BuildingStatus.onUpgradeProcess;
                                base.OnUpgradeProcess(this);
                                base._isShowInterface = false;
                            }
                            GUI.enabled = true;
                        }
                    }
                    else
                    {
                        //GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                        GUI.Label(nextProduction_Rect, "Max upgrade building.", base.job_style);
                    }
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            //GUI.BeginGroup (new Rect (0, 1 * height, background_Rect.width, height), new GUIContent ("Spearman", "พลหอก"), mainBuildingSkin.box);
            //{
            //    GUI.Box (imgContent_Rect, new GUIContent (spearmanTex, "Spearman Images"));
            //    GUI.Box (contentRect, new GUIContent ("Spearman เป็นหน่วยที่มีพรสวรรค์และมีความชำนาญ \n ในการเอาชนะกองทหารม้า", "content"), mainBuildingSkin.textArea);
            //    if (GUI.Button (buttonRect, "Create")) {

            //    }
            //}
            //GUI.EndGroup ();

			GUI.BeginGroup (new Rect (0, 2 * height, background_Rect.width, height), new GUIContent ("Hypaspist", "พลถือโล่"), mainBuildingSkin.box);
			{
				GUI.Box (imgContent_Rect, new GUIContent (hypaspistTex, "Hypaspist Images"));
				GUI.Box (contentRect, new GUIContent ("Hypaspist เป็นนักรบถือโล่่และมีอาวุธครบมือ \n พวกเขาคือองค์รักษ์คุ้มกันจักรพรรดิด้วยชีวิต \n มีความชำนาญในการต่อต้านกองทหารราบอื่นๆ ", "content"), mainBuildingSkin.textArea);
				if (GUI.Button (buttonRect, "Create")) {

				}
			}
			GUI.EndGroup ();

			GUI.BeginGroup (new Rect (0, 3 * height, background_Rect.width, height), new GUIContent ("Hoplist", "พลหุ้มเกราะ"), mainBuildingSkin.box);
			{
				GUI.Box (imgContent_Rect, new GUIContent (hoplistTex, "Hoplist Images"));
				GUI.Box (contentRect, new GUIContent ("Hoplist เป็นหน่วยทหารราบหุ้มเกราะ \n พวกเขามีความชำนาญในการผลักดันและบุกฝ่าทะลวงข้าศึก", "content"), mainBuildingSkin.textArea);
				if (GUI.Button (buttonRect, "Create")) {

				}
			}
			GUI.EndGroup ();

            GUI.BeginGroup(new Rect(0, 4 * height, background_Rect.width, height), new GUIContent("Toxotes", "พลธนู"), mainBuildingSkin.box);
			{
				GUI.Box (imgContent_Rect, new GUIContent (ToxotesTex, "Toxotes Images"));
				GUI.Box (contentRect, new GUIContent ("Toxotes เป็นหน่วยจู่โจมด้วยธนู \n พวกเขามีความชำนาญในการโจมตีจากระยะไกล", "content"), mainBuildingSkin.textArea);
				if (GUI.Button (buttonRect, "Create")) {

				}
			}
			GUI.EndGroup ();    
		}
		// End the scroll view that we began above.
		GUI.EndScrollView ();
	}
}
