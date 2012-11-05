using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public const string RequireDescription = "Require :: Academy level 3.";
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

    float height = 210f;
    Rect new_background_Rect;
    Rect new_descriptionGroupRect;
    Rect soldierDescriptionRect;
    Rect requireResource_rect;
    Rect createButton_rect;
    private Rect amountLabel_rect;
    private Rect amountTextbox_rect;
	Rect maxAmountLabel_rect;
    public string numberOfSpearman;


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

        new_background_Rect = new Rect(base.building_background_Rect.x, base.building_background_Rect.y, base.building_background_Rect.width - 16, base.building_background_Rect.height);
        new_descriptionGroupRect = new Rect(base.descriptionGroup_Rect.x, base.descriptionGroup_Rect.y, base.descriptionGroup_Rect.width - 16, base.descriptionGroup_Rect.height);
        soldierDescriptionRect = new Rect(new_descriptionGroupRect.x, 10, new_descriptionGroupRect.width, 190);
        requireResource_rect = new Rect(5, soldierDescriptionRect.height - 45, soldierDescriptionRect.width - 10, 40);
        amountLabel_rect = new Rect(5, soldierDescriptionRect.height - 90, 100, 40);
        amountTextbox_rect = new Rect(amountLabel_rect.x + amountLabel_rect.width, amountLabel_rect.y, 50, 40);
		maxAmountLabel_rect = new Rect(amountTextbox_rect.x + amountTextbox_rect.width, amountTextbox_rect.y, 50, 40);
        createButton_rect = new Rect(30, 150, 100, 35);
    }

    public override void InitializingBuildingBeh(BuildingStatus p_buildingState, int p_indexPosition, int p_level) {
        base.InitializingBuildingBeh(p_buildingState, p_indexPosition, p_level);

        BuildingBeh.Barrack_Instances.Add(this);

		this.CalculateNumberOfEmployed(p_level);
    }
	protected override void CalculateNumberOfEmployed (int p_level)
	{
//		base.CalculateNumberOfEmployed (p_level);
		int sumOfEmployed = 0;
		for (int i = 0; i < p_level; i++) {
			sumOfEmployed += RequireResource[i].Employee;
		}
		
		HouseBeh.SumOfEmployee += sumOfEmployed;
	}

    #region <!-- Building Processing.

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

	#region <!-- Events Handle.

	void HandleBaseNotEnoughResourceNotification_event (object sender, BuildingBeh.NoEnoughResourceNotificationArg e)
	{
		base.notificationText = e.notification_args;
	}

	#endregion

	protected override void CreateWindow (int windowID)
	{
        base.CreateWindow(windowID);
		
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
                        GUI.Box(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), base.stageManager.taskManager.food_icon), standard_Skin.box);
                        GUI.Box(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), base.stageManager.taskManager.wood_icon), standard_Skin.box);
                        GUI.Box(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Copper.ToString(), base.stageManager.taskManager.copper_icon), standard_Skin.box);
                        GUI.Box(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), base.stageManager.taskManager.gold_icon), standard_Skin.box);
                        GUI.Box(GameResource.Fifth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), base.stageManager.taskManager.employee_icon), standard_Skin.box);
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

    private int amountOfSpearman = 0;
    private void DrawGroupOfSpearman()
    {
        GUI.BeginGroup(new Rect(0, 2 * height, background_Rect.width, height), new GUIContent("", "Background"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Spearman", "สเปียร์แมน"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, spearmanTex);      //<!-- Spearman Images.

            GUI.BeginGroup(soldierDescriptionRect, new GUIContent(spearman_describe, "content"), mainBuildingSkin.textArea);
            {
                GUI.Label(amountLabel_rect, "Amount", standard_Skin.box);
                numberOfSpearman = GUI.TextField(amountTextbox_rect, numberOfSpearman, 3, standard_Skin.textField);
				if(GUI.Button(maxAmountLabel_rect, "Max", standard_Skin.button)) {
                    amountOfSpearman = CalculationCanCreateSpearman();
				}

                GUI.BeginGroup(requireResource_rect, GUIContent.none, standard_Skin.box);
                {
                    GUI.Box(GameResource.First_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Food.ToString(), stageManager.taskManager.food_icon), standard_Skin.box);
                    GUI.Box(GameResource.Second_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Armor.ToString(), stageManager.taskManager.armor_icon), standard_Skin.box);
                    GUI.Box(GameResource.Third_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Weapon.ToString(), stageManager.taskManager.weapon_icon), standard_Skin.box);
                    GUI.Box(GameResource.Fourth_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Gold.ToString(), stageManager.taskManager.gold_icon), standard_Skin.box);
                    GUI.Box(GameResource.Fifth_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanTraining_timer.ToString("mm:ss")), standard_Skin.box);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            if (GUI.Button(createButton_rect, "Train"))
            {
                if(int.Parse(numberOfSpearman) > 0) {
					
				}
            }
        }
        GUI.EndGroup();
    }

    private int CalculationCanCreateSpearman()
    {
        int maximumCanCreate = 0;
        if (StoreHouse.sumOfFood >= UnitDataStore.GreekUnitData.SpearmanResource.Food &&
            StoreHouse.sumOfArmor >= UnitDataStore.GreekUnitData.SpearmanResource.Armor &&
            StoreHouse.sumOfWeapon >= UnitDataStore.GreekUnitData.SpearmanResource.Weapon &&
            StoreHouse.sumOfGold >= UnitDataStore.GreekUnitData.SpearmanResource.Gold)
        {
            List<int> spearmanRequireResource = new List<int>()
            { 
                UnitDataStore.GreekUnitData.SpearmanResource.Food, 
                UnitDataStore.GreekUnitData.SpearmanResource.Armor, 
                UnitDataStore.GreekUnitData.SpearmanResource.Weapon, 
                UnitDataStore.GreekUnitData.SpearmanResource.Gold,
            };
            spearmanRequireResource.Sort();
            int minimumRequireResource = spearmanRequireResource[0];
            if (minimumRequireResource == UnitDataStore.GreekUnitData.SpearmanResource.Food) { 
                maximumCanCreate = StoreHouse.sumOfFood / minimumRequireResource;
                return maximumCanCreate;
            }
            else if (minimumRequireResource == UnitDataStore.GreekUnitData.SpearmanResource.Armor) {
                maximumCanCreate = StoreHouse.sumOfArmor / minimumRequireResource;
                return maximumCanCreate;
            }
            else if (minimumRequireResource == UnitDataStore.GreekUnitData.SpearmanResource.Weapon) {
                maximumCanCreate = StoreHouse.sumOfWeapon / minimumRequireResource;
                return maximumCanCreate;
            }
            else if (minimumRequireResource == UnitDataStore.GreekUnitData.SpearmanResource.Gold) {
                maximumCanCreate = StoreHouse.sumOfGold / minimumRequireResource;
                return maximumCanCreate;
            }
            else return 0;
        }
        else return 0;
    }

    private void DrawGroupOfHypasist()
    {
        GUI.BeginGroup(new Rect(0, 3 * height, background_Rect.width, height), new GUIContent("", "Background"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Hypaspist", "พลดาบสองมือ"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, hypaspistTex);    //<!-- Hypaspist Images.
            GUI.Box(soldierDescriptionRect, new GUIContent(hypaspist_describe, "content"), mainBuildingSkin.textArea);
            if (GUI.Button(createButton_rect, "Create"))
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
            if (GUI.Button(createButton_rect, "Create"))
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
            if (GUI.Button(createButton_rect, "Create"))
            {

            }
        }
        GUI.EndGroup();
    }
}
