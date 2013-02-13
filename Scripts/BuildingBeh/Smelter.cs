using UnityEngine;
using System.Collections;

public class Smelter : BuildingBeh {
	
    //<!-- Static Data.
    public static GameMaterialDatabase[] RequireResource = new GameMaterialDatabase[10] {
        new GameMaterialDatabase(50, 0, 80, 60, 3),
        new GameMaterialDatabase(200, 0, 200, 200, 5),    //...2
        new GameMaterialDatabase(300, 0, 300, 300, 8),    //...3
        new GameMaterialDatabase(400, 0, 400, 400, 12),   //...4
        new GameMaterialDatabase(500, 0, 500, 500, 17),
        new GameMaterialDatabase(600, 0, 600, 600, 23),
        new GameMaterialDatabase(700, 0, 700, 700, 30),   //...7
        new GameMaterialDatabase(800, 0, 800, 800, 38),   //...8
        new GameMaterialDatabase(900, 0, 900, 900, 47),   //...9
        new GameMaterialDatabase(1000, 0, 1000, 1000, 60),   //...10
	};

    //<!-- Data.
    public static string BuildingName = "Smelter";
	public const string RequireDescription = "Require :: Towncenter level 5.";
    private static string Description_TH = "โรงหลอมแร่ แร่ทองแดงถูกหลอมขึ้นที่นี่ อาวุธและชุดเกราะในกองทัพของคุณจำเป็นต้องใช้มัน \n" + " อัพเกรดเพื่อเพิ่มกำลังการผลิต";
    private static string Description_EN = "Copper, Iron can be gathered from mining. Upgrade Smelter to increase ingot production.";
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
	//<!--- produce food per second.
    private int[] productionRate = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, };       
	
	
	
	
	protected override void Awake() {
        this.name = Smelter.BuildingName;
        base.buildingType = BuildingBeh.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);

        base.processbar_offsetPos = Vector3.up * 60;

        base.Awake();
	}

	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        this.InitializeTexturesResource();
        base.sceneController.resourceCycle_Event += this.HaveResourceCycle_Event;
    }
	
	protected override void InitializeTexturesResource ()
	{
		base.InitializeTexturesResource ();
		
        //<!-- Load textures resource.
		buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "CopperIngot", typeof(Texture2D)) as Texture2D;
	}

    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, TileArea area, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, area, p_level);

        BuildingBeh.Smelter_Instances.Add(this);
		this.CalculateNumberOfEmployed(p_level);
    }
	protected override void CalculateNumberOfEmployed (int p_level)
	{
		int sumOfEmployed = 0;
		for (int i = 0; i < p_level; i++) {
			sumOfEmployed += RequireResource[i].Employee;
		}
		
		HouseBeh.SumOfEmployee += sumOfEmployed;
	}

    #region <-- Building Processing.

    public override void OnBuildingProcess(BuildingBeh building)
    {
        base.OnBuildingProcess(building);
    }
    protected override void CreateProcessBar(BuildingBeh.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void BuildingProcessComplete(BuildingBeh building)
    {
        base.BuildingProcessComplete(building);
    }

    #endregion
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();
		
		sceneController.resourceCycle_Event -= HaveResourceCycle_Event;
		BuildingBeh.Smelter_Instances.Remove(this);
	}
	
	#region <!-- Events handle.
	
    void HaveResourceCycle_Event(object sender, System.EventArgs e)
    {
        if (this.currentBuildingStatus == BuildingBeh.BuildingStatus.none) {
            StoreHouse.Add_sumOfCopper(this.productionRate[this.Level]);
        }
    }
	
	#endregion
	
	protected override void Update ()
	{
		base.Update ();
		
		if(currentBuildingStatus == BuildingBeh.BuildingStatus.onBuildingProcess
			|| currentBuildingStatus == BuildingBeh.BuildingStatus.onUpgradeProcess) {
			buildingStatus_textmesh.text = base.notificationText;
		}
	}

    protected override void CreateWindow()
    {
        base.CreateWindow();

        GUI.Box(base.notificationBox_rect, base.notificationText, base.notification_Style);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.windowRect.width, base.background_Rect.height));
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);

            GUI.BeginGroup(base.building_background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

                #region <!--- Content group.

                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {
                    //<!-- Current Production rate.
                    GUI.Label(currentJob_Rect, "Current production rate per minute : " + productionRate[Level] * 6, base.job_style);
                    GUI.Label(nextJob_Rect, "Next production rate per minute : " + productionRate[Level + 1] * 6, base.job_style);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), 
                            base.sceneController.taskManager.food_icon), standard_Skin.box);
                        //GUI.Label(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), 
                        //    base.stageManager.taskbarManager.wood_icon), standard_Skin.box);
                        GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                            base.sceneController.taskManager.stone_icon), standard_Skin.box);
                        GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), 
                            base.sceneController.taskManager.gold_icon), standard_Skin.box);
                        GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), 
                            base.sceneController.taskManager.employee_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();
                }
                GUI.EndGroup();

                #endregion

                #region <!--- Upgrade Button mechanichm.

				GUI.enabled = (base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level])) ? true:false;
                if (GUI.Button(base.upgrade_Button_Rect, new GUIContent("Upgrade")))
                {
                    GameMaterialDatabase.UsedResource(RequireResource[Level]);

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
        GUI.EndScrollView();
    }
}
