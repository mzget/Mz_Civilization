using UnityEngine;
using System.Collections;

public class BarracksBeh : Buildings
{
    public const string PathOf_TroopIcons = "Textures/Troop_Icons/";

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

    public int Level { get { return base.Level; } set { base.Level = value; } }

	public GUISkin mainBuildingSkin;
	public Texture2D spearmanTex;
	public Texture2D hypaspistTex;
	public Texture2D hopliteTex;
	public Texture2D ToxotesTex;

    private Rect troopsIcon_Rect;
	

    protected override void Awake() {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);

        buildingIcon_Texture = Resources.Load(PathOf_BuildingIcons + "Barracks", typeof(Texture2D)) as Texture2D;
        //<!-- Load troop icon.
        spearmanTex = Resources.Load(PathOf_TroopIcons + "Spearman", typeof(Texture2D)) as Texture2D;
        hypaspistTex = Resources.Load(PathOf_TroopIcons + "Hypaspist", typeof(Texture2D)) as Texture2D;
        hopliteTex = Resources.Load(PathOf_TroopIcons + "Hoplite", typeof(Texture2D)) as Texture2D;
        ToxotesTex = Resources.Load(PathOf_TroopIcons + "Toxotai", typeof(Texture2D)) as Texture2D;

        troopsIcon_Rect = new Rect(base.imgIcon_Rect.x, base.imgIcon_Rect.y + 16, base.imgIcon_Rect.width, base.imgIcon_Rect.height);
    }
    
	// Use this for initialization
	void Start () {
		
    }

    public override void InitializeData(BuildingStatus p_buildingState, int p_indexPosition, int p_level) {
        base.InitializeData(p_buildingState, p_indexPosition, p_level);

        Buildings.Barrack_Instances.Add(this);
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

        if (this.currentBuildingStatus != Buildings.BuildingStatus.none)
            this.currentBuildingStatus = Buildings.BuildingStatus.none;
    }

    #endregion

    protected override void ClearStorageData()
    {
        base.ClearStorageData();

        Buildings.Barrack_Instances.Remove(this);
    }
	
	// Update is called once per frame
	void Update ()
	{

	}

    private Rect buttonRect = new Rect(460, 140, 100, 30);
    float height = 160f;
	Rect soldierDescriptionRect;
    
	protected override void CreateWindow (int windowID)
	{
        base.CreateWindow(windowID);
		
		Rect new_background_Rect =
		    new Rect(base.building_background_Rect.x, base.building_background_Rect.y, base.building_background_Rect.width - 16, base.building_background_Rect.height);
		Rect new_descriptionGroupRect =
		    new Rect(base.descriptionGroup_Rect.x, base.descriptionGroup_Rect.y, base.descriptionGroup_Rect.width - 16, base.descriptionGroup_Rect.height);
		soldierDescriptionRect = new Rect(new_descriptionGroupRect.x, 10, new_descriptionGroupRect.width, 140);
		
		
        GUI.Box(new Rect(48, 24, 256, 48), base.currentBuildingStatus.ToString(), standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height), 
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height * 3));
        {
            GUI.BeginGroup(new_background_Rect, new GUIContent("", "content background"), building_Skin.box);
            {
                //<!-- building icon.
                GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture);
                //<!-- building level.
                GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);
                //<!-- description group rect.
                //<!-- group draw order.
                GUI.BeginGroup(new_descriptionGroupRect, CurrentDescription, building_Skin.textArea);
                {   
                    if (base.Level < 10)
                    {
                        //GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                        //GUI.Label(nextProduction_Rect, "Next Max Capacity : " + this.maxCapacities[base.level], base.job_style);

                        //<!-- Requirements Resource.
                        GUI.BeginGroup(update_requireResource_Rect);
                        {
                            GUI.Box(GameResource.Food_Rect, new GUIContent(UpgradeResource[Level].Food.ToString(), base.food_icon), standard_Skin.box);
                            GUI.Box(GameResource.Wood_Rect, new GUIContent(UpgradeResource[Level].Wood.ToString(), base.wood_icon), standard_Skin.box);
                            GUI.Box(GameResource.Copper_Rect, new GUIContent(UpgradeResource[Level].Gold.ToString(), base.copper_icon), standard_Skin.box);
                            GUI.Box(GameResource.Stone_Rect, new GUIContent(UpgradeResource[Level].Stone.ToString(), base.stone_icon), standard_Skin.box);
                        }
                        GUI.EndGroup();

                        //<!-- Upgrade Button.
                        if (StoreHouse.sumOfFood >= UpgradeResource[Level].Food && StoreHouse.sumOfWood >= UpgradeResource[Level].Wood &&
                            StoreHouse.sumOfGold >= UpgradeResource[Level].Gold && StoreHouse.sumOfStone >= UpgradeResource[Level].Stone)
                        {
                            bool enableUpgrade = base.CheckingCanUpgradeLevel();

                            GUI.enabled = enableUpgrade;
                            if (GUI.Button(base.upgradeButton_Rect, new GUIContent("Upgrade")))
                            {
                                StoreHouse.UsedResource(UpgradeResource[Level]);

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

            this.DrawGroupOfSpearman();
            this.DrawGroupOfHypasist();
            this.DrawGroupOfHoplist();
            this.DrawGroupOfToxotes();
		}
		// End the scroll view that we began above.
		GUI.EndScrollView ();
	}

    private void DrawGroupOfSpearman()
    {
        GUI.BeginGroup(new Rect(0, 2 * height, background_Rect.width, height), new GUIContent("", "Background"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Spearman", "สเปียร์แมน"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, spearmanTex);      //<!-- Spearman Images.
            GUI.Box(soldierDescriptionRect, new GUIContent("Spearman เป็นหน่วยที่มีพรสวรรค์และมีความชำนาญ \n ในการเอาชนะกองทหารม้า", "content"), mainBuildingSkin.textArea);
            if (GUI.Button(buttonRect, "Create"))
            {

            }
        }
        GUI.EndGroup();
    }
    private void DrawGroupOfHypasist()
    {
        GUI.BeginGroup(new Rect(0, 3 * height, background_Rect.width, height), new GUIContent("", "Background"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Hypaspist", "พลดาบสองมือ"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, hypaspistTex);    //<!-- Hypaspist Images.
            GUI.Box(soldierDescriptionRect, new GUIContent("Hypaspist เป็นนักรบถืออาวุธครบมือ \n พวกเขาคือองค์รักษ์คุ้มกันจักรพรรดิด้วยชีวิต \n มีความชำนาญในการต่อต้านกองทหารราบอื่นๆ ", "content"), mainBuildingSkin.textArea);
            if (GUI.Button(buttonRect, "Create"))
            {

            }
        }
        GUI.EndGroup();
    }
    private void DrawGroupOfHoplist()
    {
        GUI.BeginGroup(new Rect(0, 4 * height, background_Rect.width, height), new GUIContent("", "Backgroud"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Hoplite", "พลหุ้มเกราะ"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, hopliteTex);      //<!-- Hoplite Images.
            GUI.Box(soldierDescriptionRect, new GUIContent("Hoplite เป็นหน่วยทหารราบหุ้มเกราะ พวกเขามีความชำนาญในการผลักดันข้าศึก ถืออาวุธหลักเป็นหอกยาวและโล่ มีรูปแบบการต่อสู้ร่วมกันเป็นกลุ่ม", "content"), mainBuildingSkin.textArea);
            if (GUI.Button(buttonRect, "Create"))
            {

            }
        }
        GUI.EndGroup();
    }
    private void DrawGroupOfToxotes()
    {
        GUI.BeginGroup(new Rect(0, 5 * height, background_Rect.width, height), new GUIContent("", "background"), mainBuildingSkin.box);
        {
            GUI.Label(tagName_Rect, new GUIContent("Toxotes", "พลธนู"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, ToxotesTex);    //<!-- Toxotes images icons.
            GUI.Box(soldierDescriptionRect, new GUIContent("Toxotes เป็นหน่วยจู่โจมด้วยธนู \n พวกเขามีความชำนาญในการโจมตีจากระยะไกล", "content"), mainBuildingSkin.textArea);
            if (GUI.Button(buttonRect, "Create"))
            {

            }
        }
        GUI.EndGroup();
    }
}
