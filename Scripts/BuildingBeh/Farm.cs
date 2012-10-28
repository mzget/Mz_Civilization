using UnityEngine;
using System.Collections;

public class Farm : BuildingBeh
{
    public static GameResource[] RequireResource = new GameResource[10] {
        new GameResource(40, 80, 0, 50, 3),
        new GameResource() {Food = 80, Wood = 120, Gold = 100, Employee = 5},
        new GameResource() {Food = 120, Wood = 240, Gold = 150, Employee = 8},
        new GameResource() {Food = 240, Wood = 480, Gold = 300, Employee = 12},
        new GameResource() {Food = 480, Wood = 960, Gold = 600, Employee = 18},
        new GameResource() {Food = 960, Wood = 1920, Gold = 1200, Employee = 24},
        new GameResource() {Food = 1920, Wood = 3840, Gold = 2400, Employee = 32},
        new GameResource() {Food = 3840, Wood = 7680, Gold = 4800, Employee = 40},
        new GameResource() {Food = 7680, Wood = 15000, Gold = 9600, Employee = 50},
        new GameResource() {Food = 15000, Wood = 30000, Gold = 19000, Employee = 64},
    };
	
    //<!-- Data.
    public static string BuildingName = "Farm";
    private static string Description_TH = "อาหารเพื่อเลี้ยงประชากรของคุณผลิตขึ้นที่นี่ เพิ่มระดับฟาร์มเพื่อเพิ่มกำลังการผลิตธัญพืช";
    private static string Description_EN = "Food for your poppulation made form here, Upgrade farm to increase grain production.";
    public static string CurrentDescription {
        get {
            string temp = "";
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
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

        this.gameObject.name = BuildingName;
        base.buildingType = BuildingBeh.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
    }

    // Use this for initialization
    void Start()
    {
		this.InitializeTexturesResource();

        base.stageManager.resourceCycle_Event += HaveResourceCycle_Event;
        base.NotEnoughResource_Notification_event += Farm_NotEnoughResource_Notification_event;
    }
    
    void HaveResourceCycle_Event(object sender, System.EventArgs e)
    {
        if (currentBuildingStatus == BuildingBeh.BuildingStatus.none) {
            if (StoreHouse.sumOfFood < StoreHouse.SumOfMaxCapacity)
                StoreHouse.sumOfFood += this.productionRate[this.Level];
        }
    }

    void Farm_NotEnoughResource_Notification_event(object sender, NoEnoughResourceNotificationArg e)
    {
        base.notificationText = e.notification_args;
    }

    protected override void InitializeTexturesResource()
    {
        base.InitializeTexturesResource();

        base.buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Grain", typeof(Texture2D)) as Texture2D;
    }

    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, p_indexPosition, p_level);

        BuildingBeh.Farm_Instance.Add(this);
    }

    #region <!--- Building Processing.

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
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        base.stageManager.resourceCycle_Event -= this.HaveResourceCycle_Event;
        base.NotEnoughResource_Notification_event -= this.Farm_NotEnoughResource_Notification_event;

		BuildingBeh.Farm_Instance.Remove(this);
	}

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
	
    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);
        
        if(base.notificationText == "") {
            base.notificationText = base.currentBuildingStatus.ToString();
        }
        GUI.Box(base.notificationBox_rect, base.notificationText, standard_Skin.box);

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
                    GUI.Label(currentJob_Rect, "Current production rate per minute : " + productionRate[Level] * 6, base.job_style);
                    GUI.Label(nextJob_Rect, "Next production rate per minute : " + productionRate[Level + 1] * 6, base.job_style);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Label(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), 
                            stageManager.taskManager.food_icon), standard_Skin.box);
                        GUI.Label(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), 
                            stageManager.taskManager.wood_icon), standard_Skin.box);
                        //GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                        //    stageManager.taskbarManager.stone_icon), standard_Skin.box);
                        GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), 
                            stageManager.taskManager.gold_icon), standard_Skin.box);
                        GUI.Label(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), 
                            stageManager.taskManager.employee_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();
                }
                GUI.EndGroup();

                #endregion

                #region <!--- Upgrade Button mechanichm.

                bool enableUpgrade = false;
                if (base.CheckingCanUpgradeLevel() && base.CheckingEnoughUpgradeResource(RequireResource[Level]))
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
