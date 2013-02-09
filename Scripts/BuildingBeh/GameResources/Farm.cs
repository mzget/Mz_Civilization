  using UnityEngine;
using System.Collections;

public class Farm : BuildingBeh
{
    public static GameMaterialDatabase[] RequireResource = new GameMaterialDatabase[10] {
        new GameMaterialDatabase(40, 80, 0, 50, 3),
        new GameMaterialDatabase() {Food = 80, Wood = 120, Gold = 100, Employee = 5},
        new GameMaterialDatabase() {Food = 120, Wood = 240, Gold = 150, Employee = 8},
        new GameMaterialDatabase() {Food = 240, Wood = 480, Gold = 300, Employee = 12},
        new GameMaterialDatabase() {Food = 480, Wood = 960, Gold = 600, Employee = 18},
        new GameMaterialDatabase() {Food = 960, Wood = 1920, Gold = 1200, Employee = 24},
        new GameMaterialDatabase() {Food = 1920, Wood = 3840, Gold = 2400, Employee = 32},
        new GameMaterialDatabase() {Food = 3840, Wood = 7680, Gold = 4800, Employee = 40},
        new GameMaterialDatabase() {Food = 7680, Wood = 15000, Gold = 9600, Employee = 50},
        new GameMaterialDatabase() {Food = 15000, Wood = 30000, Gold = 19000, Employee = 64},
    };
	
    //<!-- Data.
    public static string BuildingName = "Farm";
    private static string Description_TH = "อาหารเพื่อเลี้ยงประชากรของคุณผลิตขึ้นที่นี่ เพิ่มระดับฟาร์มเพื่อเพิ่มกำลังการผลิตธัญพืช";
    private static string Description_EN = "Food for your population made form here, Upgrade farm to increase grain production.";
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

        this.gameObject.name = BuildingName;
        base.buildingType = BuildingBeh.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
		
		base.processbar_offsetPos = Vector3.up * 60;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        this.InitializeTexturesResource();

        base.sceneController.resourceCycle_Event += HaveResourceCycle_Event;
        base.NotEnoughResource_Notification_event += Farm_NotEnoughResource_Notification_event;
    }
  
    protected override void InitializeTexturesResource()
    {
        base.InitializeTexturesResource();

        base.buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Grain", typeof(Texture2D)) as Texture2D;
    }

    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, TileArea area, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, area, p_level);
		
        BuildingBeh.Farm_Instances.Add(this);
		
		this.CalculateNumberOfEmployed(p_level);
    }	
	protected override void CalculateNumberOfEmployed(int p_level) {		
		int sumOfEmployed = 0;
		for (int i = 0; i < p_level; i++) {
			 sumOfEmployed += RequireResource[i].Employee;
		}
		
		HouseBeh.SumOfEmployee += sumOfEmployed;
	}

    /// <summary>
    /// Building Processing.
    /// </summary>
    /// <param name="obj"></param>
    protected override void BuildingProcessComplete(BuildingBeh obj)
    {
        base.BuildingProcessComplete(obj);

        Destroy(base.processbar_Obj_parent);

        if(QuestSystemManager.arr_isMissionComplete[2] == false)
            sceneController.taskManager.questManager.MissionComplete(2);	
    }

    /// <summary>
    /// Destruction building.
    /// </summary>
    protected override void DecreaseBuildingLevel()
    {
        HouseBeh.SumOfEmployee -= RequireResource[this.Level].Employee;
		
        base.DecreaseBuildingLevel();
        buildingLevel_textmesh.text = this.Level.ToString();
    }
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        base.sceneController.resourceCycle_Event -= this.HaveResourceCycle_Event;
        base.NotEnoughResource_Notification_event -= this.Farm_NotEnoughResource_Notification_event;

		BuildingBeh.Farm_Instances.Remove(this);
	}
	
	#region <!-- Events handle.
	
    void HaveResourceCycle_Event(object sender, System.EventArgs e)
    {
        if (currentBuildingStatus == BuildingBeh.BuildingStatus.none) {
                StoreHouse.Add_sumOfFood(this.productionRate[this.Level]);
        }
    }

    void Farm_NotEnoughResource_Notification_event(object sender, NoEnoughResourceNotificationArg e)
    {
        base.notificationText = e.notification_args;
    }
	
	#endregion
	
	protected override void Update ()
	{
		base.Update ();
		
		if(currentBuildingStatus == BuildingBeh.BuildingStatus.onBuildingProcess
			|| currentBuildingStatus == BuildingBeh.BuildingStatus.onUpgradeProcess) {
			buildingLevel_textmesh.text = base.notificationText;
		}
	}
	
    protected override void CreateWindow()
    {
        base.CreateWindow();
        
        //if(base.notificationText == "")
        //    base.notificationText = base.currentBuildingStatus.ToString();
        GUI.Box(base.notificationBox_rect, base.notificationText, base.notification_Style);

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
                        GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), 
                            sceneController.taskManager.food_icon), standard_Skin.box);
                        GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), 
                            sceneController.taskManager.wood_icon), standard_Skin.box);
                        //GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                        //    stageManager.taskbarManager.stone_icon), standard_Skin.box);
                        GUI.Label(GameMaterialDatabase.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), 
                            sceneController.taskManager.gold_icon), standard_Skin.box);
                        GUI.Label(GameMaterialDatabase.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), 
                            sceneController.taskManager.employee_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();
                }
                GUI.EndGroup();

                #endregion

                #region <!--- Upgrade Button mechanichm.

                GUI.enabled = base.CheckingCanUpgradeLevel() && base.CheckingEnoughUpgradeResource(RequireResource[Level]) ? true : false;
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
