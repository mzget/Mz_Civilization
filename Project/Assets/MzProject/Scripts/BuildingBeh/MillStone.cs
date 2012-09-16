using UnityEngine;
using System.Collections;

public class MillStone : Buildings {

    //<!-- Static Data.
    public static GameResource[] RequireResource = new GameResource[10] {
        new GameResource(80, 80, 60, 120, 3),
        new GameResource(160, 160, 120, 240, 5),
        new GameResource(320, 320, 240, 480, 8),
        new GameResource(640, 640, 480, 960, 12),
        new GameResource(1280, 1280, 960, 1920, 17),
        new GameResource(2560, 2560, 1920, 3840, 23),
        new GameResource(3840, 3840, 2880, 5760, 30),
        new GameResource(5760, 5760, 4320, 7680, 38),
        new GameResource(7680, 7680, 6240, 9600, 47),
        new GameResource(9600, 9600, 8640, 12000, 60),
    };

    //<!-- Data.
    public static string BuildingName = "MillStone";
    private static string Description_TH = "โรงโม่หิน มีช่างหินเป็นผู้เชี่ยวชาญในการตัดหิน ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้หินมากขึ้นไปด้วย";
    private static string Description_EN = "Stone block can be gathered from stone mining. Upgrade millstone to increase stone block production.";
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
	//<!--- produce food per second.
    private int[] productionRate = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, };       



    protected override void Awake()
    {
        base.Awake(); 
		sprite = this.GetComponent<OTSprite>();
		
        this.name = BuildingName;
        base.buildingType = BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
    }
	
	// Use this for initialization
	void Start () {
        this.LoadTextureResource();

        stageManager.dayCycle += ReachDayCycle;
    }

    protected override void LoadTextureResource()
    {
        base.LoadTextureResource();

        buildingIcon_Texture = Resources.Load("Textures/Building_Icons/StoneBlock", typeof(Texture2D)) as Texture2D;
    }

    public override void InitializeData(Buildings.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializeData(p_buildingState, p_indexPosition, p_level);

        Buildings.MillStoneInstance.Add(this);
    }

    #region Building Processing.

    public override void OnBuildingProcess(Buildings building)
    {
        base.OnBuildingProcess(building);
    }
    protected override void CreateProcessBar(Buildings.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void BuildingProcessComplete(Buildings obj)
    {
        base.BuildingProcessComplete(obj);

        Destroy(processbar_Obj_parent);
		
		if(this.currentBuildingStatus != Buildings.BuildingStatus.none)
			this.currentBuildingStatus = Buildings.BuildingStatus.none;
    }

    #endregion
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        stageManager.dayCycle -= ReachDayCycle;
		Buildings.MillStoneInstance.Remove(this);
	}
	
	// Update is called once per frame
	void Update () 
	{	

	}

    void ReachDayCycle(object sender, System.EventArgs e)
    {
        if (this.currentBuildingStatus == Buildings.BuildingStatus.none) {
            if (StoreHouse.sumOfStone < StoreHouse.SumOfCapacity)
                StoreHouse.sumOfStone += this.productionRate[this.Level];
        }
    }

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
                    GUI.Label(currentProduction_Rect, "Current production rate per minute : " + productionRate[Level] * 6, base.job_style);
                    GUI.Label(nextProduction_Rect, "Next production rate per minute : " + productionRate[Level + 1] * 6, base.job_style);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Label(GameResource.Food_Rect, new GUIContent(RequireResource[Level].Food.ToString(), base.stageManager.taskbarManager.food_icon), GUI.skin.box);
                        GUI.Label(GameResource.Wood_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), base.stageManager.taskbarManager.wood_icon), GUI.skin.box);
                        GUI.Label(GameResource.Stone_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), base.stageManager.taskbarManager.stone_icon), GUI.skin.box);
                        GUI.Label(GameResource.Gold_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), base.stageManager.taskbarManager.gold_icon), GUI.skin.box);
                        GUI.Label(GameResource.Employee_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), base.stageManager.taskbarManager.employee_icon), GUI.skin.box);
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
                    StoreHouse.UsedResource(RequireResource[Level]);

                    base.currentBuildingStatus = Buildings.BuildingStatus.onUpgradeProcess;
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
