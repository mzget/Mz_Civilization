using UnityEngine;
using System.Collections;

public class BarracksBeh : BuildingBeh
{
    public const string PathOf_TroopIcons = "Textures/Troop_Icons/";

	//<!-- Static Data.
    public static GameResource[] RequireResource = new GameResource[10] {
        new GameResource() { Food = 300, Wood = 120, Copper = 500, Gold = 250 },
        new GameResource() { Food = 800, Wood = 300, Copper = 1200, Gold = 500, Employee = 5},
        new GameResource() { Food = 1800, Wood = 800, Copper = 2200, Gold = 1000, Employee = 12},
        new GameResource() { Food = 3000, Wood = 1600, Copper =  3400, Gold = 2000, Employee = 22},
        new GameResource(500, 500, 500, 500, 50),
        new GameResource(600, 600, 600, 600, 60),
        new GameResource(700, 700, 700, 700, 70),
        new GameResource(800, 800, 800, 800, 80),
        new GameResource(900, 900, 900, 900, 90),
        new GameResource(1000, 1000, 1000, 1000, 100),
    };
	
    public static string BuildingName = "Barrack";
    private static string Description_TH = "ในค่ายทหารนี้คุณสามารถเกณฑ์ทหารราบได้ ยิ่งระดับค่ายทหารมากเท่าไร \n " + "กองกำลังก็จะเข็มแข็งมากขึ้น";
    private static string Description_EN = "Trains infantry units. Also researches technologies related to infantry units";
	public static string CurrentDescription {
        get {
            string temp = Description_EN;
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                temp = Description_TH;
            return temp;
        }
	}

	public GUISkin mainBuildingSkin;
	public Texture2D spearmanTex;
    private string spearman_describe;
	public Texture2D hypaspistTex;
    public string hypaspist_describe;
	public Texture2D hopliteTex;
    public string hoplite_describe;
	public Texture2D ToxotesTex;

    private Rect troopsIcon_Rect;


    protected override void Awake()
    {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
    }
    
	// Use this for initialization
	void Start () {
        this.InitializingData();
		this.InitializeTexturesResource();
		
		base.NotEnoughResource_Notification_event += HandleBaseNotEnoughResourceNotification_event;
    }

	void HandleBaseNotEnoughResourceNotification_event (object sender, BuildingBeh.NoEnoughResourceNotificationArg e)
	{
		base.notificationText = e.notification_args;
	}
	
	protected override void InitializeTexturesResource ()
	{
		base.InitializeTexturesResource ();

        buildingIcon_Texture = Resources.Load(BuildingIcons_TextureResourcePath + "Barracks", typeof(Texture2D)) as Texture2D;
        //<!-- Load troop icon.
        spearmanTex = Resources.Load(PathOf_TroopIcons + "Spearman", typeof(Texture2D)) as Texture2D;
        hypaspistTex = Resources.Load(PathOf_TroopIcons + "Hypaspist", typeof(Texture2D)) as Texture2D;
        hopliteTex = Resources.Load(PathOf_TroopIcons + "Hoplite", typeof(Texture2D)) as Texture2D;
        ToxotesTex = Resources.Load(PathOf_TroopIcons + "Toxotai", typeof(Texture2D)) as Texture2D;
	}
    protected override void InitializingData()
    {
        base.InitializingData();

        troopsIcon_Rect = new Rect(base.imgIcon_Rect.x, base.imgIcon_Rect.y + 16, base.imgIcon_Rect.width, base.imgIcon_Rect.height);

        spearman_describe = UnitDataStore.GreekUnitData.Get_Spearman_describe;
        hypaspist_describe = UnitDataStore.GreekUnitData.Get_Hypaspist_describe;
        hoplite_describe = UnitDataStore.GreekUnitData.Get_Hoplite_describe;
    }
    public override void InitializeBuildingBeh(BuildingStatus p_buildingState, int p_indexPosition, int p_level) {
        base.InitializeBuildingBeh(p_buildingState, p_indexPosition, p_level);

        BuildingBeh.Barrack_Instances.Add(this);
    }

    #region Building Processing.

    public override void OnBuildingProcess(BuildingBeh obj)
    {
        base.OnBuildingProcess(obj);
    }
    protected override void CreateProcessBar(BuildingBeh.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void BuildingProcessComplete(BuildingBeh obj)
    {
        base.BuildingProcessComplete(obj);

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != BuildingBeh.BuildingStatus.none)
            this.currentBuildingStatus = BuildingBeh.BuildingStatus.none;
    }

    #endregion

    protected override void ClearStorageData()
    {
        base.ClearStorageData();
		
		base.NotEnoughResource_Notification_event -= HandleBaseNotEnoughResourceNotification_event;
        BuildingBeh.Barrack_Instances.Remove(this);
    }
	
	// Update is called once per frame
	void Update ()
	{

	}

    private Rect buttonRect = new Rect(460, 140, 100, 30);
    float height = 160f;
    Rect new_background_Rect;
    Rect new_descriptionGroupRect;
	Rect soldierDescriptionRect;
    Rect requireResource_rect;
	protected override void CreateWindow (int windowID)
	{
        base.CreateWindow(windowID);

        new_background_Rect = new Rect(base.building_background_Rect.x, base.building_background_Rect.y, base.building_background_Rect.width - 16, base.building_background_Rect.height);
        new_descriptionGroupRect = new Rect(base.descriptionGroup_Rect.x, base.descriptionGroup_Rect.y, base.descriptionGroup_Rect.width - 16, base.descriptionGroup_Rect.height);
		soldierDescriptionRect = new Rect(new_descriptionGroupRect.x, 10, new_descriptionGroupRect.width, 140);
        requireResource_rect = new Rect(5, soldierDescriptionRect.height - 32, 240, 30);
		
        GUI.Box(base.notificationBox_rect, base.notificationText, standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height), 
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height * 3));
        {
            this.DrawMainBuildingContent();         
            this.DrawGroupOfSpearman();
            this.DrawGroupOfHypasist();
            this.DrawGroupOfHoplist();
            this.DrawGroupOfToxotes();
		}
		GUI.EndScrollView ();   // End the scroll view that we began above.
	}

    private void DrawMainBuildingContent()
    {     
        GUI.BeginGroup(new_background_Rect, new GUIContent("", "content background"), building_Skin.box);
        {
            //<!-- building icon.
            GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture);
            //<!-- building level.
            GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

            #region <!--- Contents Group.

            GUI.BeginGroup(new_descriptionGroupRect, CurrentDescription, building_Skin.textArea);
            {
                if (base.Level < RequireResource.Length)
                {
                    //GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                    //GUI.Label(nextProduction_Rect, "Next Max Capacity : " + this.maxCapacities[base.level], base.job_style);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Box(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), base.stageManager.gui_Manager.food_icon), standard_Skin.box);
                        GUI.Box(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), base.stageManager.gui_Manager.wood_icon), standard_Skin.box);
                        GUI.Box(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Copper.ToString(), base.stageManager.gui_Manager.copper_icon), standard_Skin.box);
                        GUI.Box(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), base.stageManager.gui_Manager.gold_icon), standard_Skin.box);
                        GUI.Box(GameResource.Fifth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), base.stageManager.gui_Manager.employee_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();
                }
                else
                {
                    //GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                    GUI.Label(nextJob_Rect, "Max upgrade building.", base.job_style);
                }
            }
            GUI.EndGroup();

            #endregion

            #region <!--- Upgrade Button mechanichm.

            bool enableUpgrade = false;
            if (base.Level < RequireResource.Length && base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(base.upgrade_Button_Rect, new GUIContent("Upgrade")))
            {
                GameResource.UsedResource(RequireResource[Level]);

                base.currentBuildingStatus = BuildingBeh.BuildingStatus.onUpgradeProcess;
                base.OnUpgradeProcess(this);
                base.CloseGUIWindow();
            }
            GUI.enabled = true;

            #endregion

            #region <!--- Destruction button.

            GUI.enabled = this.CheckingCanDestructionBuilding();
            if (GUI.Button(destruction_Button_Rect, new GUIContent("Destruct")))
            {
                this.currentBuildingStatus = BuildingStatus.OnDestructionProcess;
                this.DestructionBuilding();
                base.CloseGUIWindow();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    private void DrawGroupOfSpearman()
    {
        GUI.BeginGroup(new Rect(0, 2 * height, background_Rect.width, height), new GUIContent("", "Background"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Spearman", "สเปียร์แมน"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, spearmanTex);      //<!-- Spearman Images.

            GUI.BeginGroup(soldierDescriptionRect, new GUIContent(spearman_describe, "content"), mainBuildingSkin.textArea);
            {
                GUI.BeginGroup(requireResource_rect, GUIContent.none, standard_Skin.box);
                {
                    GUI.Box(GameResource.First_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Food.ToString(), stageManager.gui_Manager.food_icon), standard_Skin.box);
                    GUI.Box(GameResource.Third_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Gold.ToString(), stageManager.gui_Manager.gold_icon), standard_Skin.box);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

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
            GUI.Box(soldierDescriptionRect, new GUIContent(hypaspist_describe, "content"), mainBuildingSkin.textArea);
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
            GUI.Box(soldierDescriptionRect, new GUIContent(hoplite_describe, "content"), mainBuildingSkin.textArea);
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
