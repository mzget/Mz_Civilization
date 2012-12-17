using UnityEngine;
using System.Collections;

public class Smelter : BuildingBeh {
	
    //<!-- Static Data.
    public static GameResource[] RequireResource = new GameResource[10] {
        new GameResource(50, 0, 80, 60, 3),
        new GameResource(200, 0, 200, 200, 5),    //...2
        new GameResource(300, 0, 300, 300, 8),    //...3
        new GameResource(400, 0, 400, 400, 12),   //...4
        new GameResource(500, 0, 500, 500, 17),
        new GameResource(600, 0, 600, 600, 23),
        new GameResource(700, 0, 700, 700, 30),   //...7
        new GameResource(800, 0, 800, 800, 38),   //...8
        new GameResource(900, 0, 900, 900, 47),   //...9
        new GameResource(1000, 0, 1000, 1000, 60),   //...10
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
		base.Awake();
		
        if (sprite == null)
            sprite = this.GetComponent<OTSprite>();
		
        this.name = Smelter.BuildingName;
        base.buildingType = BuildingBeh.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
	}

	// Use this for initialization
    void Start() {
		this.InitializeTexturesResource();
		
		base.stageManager.resourceCycle_Event += this.HaveResourceCycle_Event;
	}
	
	protected override void InitializeTexturesResource ()
	{
		base.InitializeTexturesResource ();
		
        //<!-- Load textures resource.
		buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "CopperIngot", typeof(Texture2D)) as Texture2D;
	}

    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, p_indexPosition, p_level);

        BuildingBeh.SmelterInstance.Add(this);
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

        Destroy(processbar_Obj_parent);
		
		if(this.currentBuildingStatus != BuildingBeh.BuildingStatus.none)
			this.currentBuildingStatus = BuildingBeh.BuildingStatus.none;
    }

    #endregion
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();
		
		stageManager.resourceCycle_Event -= HaveResourceCycle_Event;
		BuildingBeh.SmelterInstance.Remove(this);
	}
	
	#region <!-- Events handle.
	
    void HaveResourceCycle_Event(object sender, System.EventArgs e)
    {
        if (this.currentBuildingStatus == BuildingBeh.BuildingStatus.none) {
            StoreHouse.Add_sumOfCopper(this.productionRate[this.Level]);
        }
    }
	
	#endregion

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        GUI.Box(new Rect(48, 24, 256, 48), base.currentBuildingStatus.ToString(), standard_Skin.box);

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
                        GUI.Label(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), 
                            base.stageManager.taskManager.food_icon), standard_Skin.box);
                        //GUI.Label(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), 
                        //    base.stageManager.taskbarManager.wood_icon), standard_Skin.box);
                        GUI.Label(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                            base.stageManager.taskManager.stone_icon), standard_Skin.box);
                        GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), 
                            base.stageManager.taskManager.gold_icon), standard_Skin.box);
                        GUI.Label(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), 
                            base.stageManager.taskManager.employee_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();
                }
                GUI.EndGroup();

                #endregion

                #region <!--- Upgrade Button mechanichm.

                bool enableUpgrade = false;
                if (base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level]))
                {
                    enableUpgrade = true;
                }

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
        GUI.EndScrollView();
    }
}
