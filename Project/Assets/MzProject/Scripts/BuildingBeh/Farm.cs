using UnityEngine;
using System.Collections;

public class Farm : Buildings
{
    public static GameResource[] RequireResource = new GameResource[10] {
        new GameResource(50, 80, 80, 60, 3),
        new GameResource(200, 200, 200, 200, 5),    //...2
        new GameResource(300, 300, 300, 300, 8),    //...3
        new GameResource(400, 400, 400, 400, 12),   //...4
        new GameResource(500, 500, 500, 500, 17),
        new GameResource(600, 600, 600, 600, 23),
        new GameResource(700, 700, 700, 700, 30),   //...7
        new GameResource(800, 800, 800, 800, 38),   //...8
        new GameResource(900, 900, 900, 900, 47),   //...9
        new GameResource(1000, 1000, 1000, 1000, 60),   //...10
    };
	
    //<!-- Data.
    public static string BuildingName = "Farm";
    private static string Description_TH = "อาหารเพื่อเลี้ยงประชากรของคุณผลิตขึ้นที่นี่ เพิ่มระดับฟาร์มเพื่อเพิ่มกำลังการผลิตธัญพืช";
    private static string Description_EN = "Food for your poppulation made form here, Upgrade farm to increase grain production.";
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
        sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = BuildingName;
        base.buildingType = Buildings.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
    }

    // Use this for initialization
    void Start()
    {
		this.LoadTextureResource();
        base.stageManager.dayCycle += ReachDayCycle;
    }

    protected override void LoadTextureResource()
    {
        base.LoadTextureResource();

        buildingIcon_Texture = Resources.Load("Textures/Building_Icons/Grain", typeof(Texture2D)) as Texture2D;
    }

    public override void InitializeData(Buildings.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializeData(p_buildingState, p_indexPosition, p_level);

        Buildings.Farm_Instance.Add(this);
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
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        base.stageManager.dayCycle -= this.ReachDayCycle;
		Buildings.Farm_Instance.Remove(this);
	}

    // Update is called once per frame
    void Update()
    {

    }

    void ReachDayCycle(object sender, System.EventArgs e)
    {
        if (currentBuildingStatus == Buildings.BuildingStatus.none)
        {
            if (StoreHouse.sumOfFood < StoreHouse.SumOfCapacity)
                StoreHouse.sumOfFood += this.productionRate[this.Level];
        }
    }
	
    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        GUI.Box(new Rect(48, 24, 256, 48), base.currentBuildingStatus.ToString(), standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height), 
			scrollPosition, new Rect(0, 0, base.windowRect.width, base.background_Rect.height), false, false);
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);

            GUI.BeginGroup(base.building_background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture, ScaleMode.ScaleToFit);
                GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

                #region <<!--- Content group.

                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {   
                    //<!-- Current Production rate.
                    GUI.Label(currentProduction_Rect, "Current production rate per minute : " + productionRate[Level] * 6, base.job_style);
                    GUI.Label(nextProduction_Rect, "Next production rate per minute : " + productionRate[Level + 1] * 6, base.job_style);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Label(GameResource.Food_Rect, new GUIContent(RequireResource[Level].Food.ToString(), stageManager.taskbarManager.food_icon), standard_Skin.box);
                        GUI.Label(GameResource.Wood_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), stageManager.taskbarManager.wood_icon), standard_Skin.box);
                        GUI.Label(GameResource.Stone_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), stageManager.taskbarManager.stone_icon), standard_Skin.box);
                        GUI.Label(GameResource.Gold_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), stageManager.taskbarManager.gold_icon), standard_Skin.box);
                        GUI.Label(GameResource.Employee_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), stageManager.taskbarManager.employee_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();
                }
                GUI.EndGroup();

                #endregion

                #region <!--- Upgrade Button mechanichm.

                bool enableUpgrade = false;
                if (base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level]))
                    enableUpgrade = true;

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
                    base.DestructionBuilding();
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
