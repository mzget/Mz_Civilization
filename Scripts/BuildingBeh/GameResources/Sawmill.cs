using UnityEngine;
using System.Collections;

public class Sawmill : BuildingBeh {
	
	//<!--- Static Data.
    public static GameMaterialDatabase[] RequireResource = new GameMaterialDatabase[10] {
        new GameMaterialDatabase(80, 60, 0, 120, 3),
        new GameMaterialDatabase(160, 120, 0, 240, 5),
        new GameMaterialDatabase(320, 240, 0, 480, 8),
        new GameMaterialDatabase(640, 480, 0, 960, 12),
        new GameMaterialDatabase(1280, 960, 0, 1920, 17),
        new GameMaterialDatabase(2560, 1920, 0, 3840, 23),
        new GameMaterialDatabase(3840, 2880, 0, 5760, 30),
        new GameMaterialDatabase(5760, 4320, 0, 7680, 38),
        new GameMaterialDatabase(7680, 6240, 0, 9600, 47),
        new GameMaterialDatabase(9600, 8640, 0, 12000, 60),
    };

    //<!--- Data.
	public static string BuildingName = "Sawmill";
    private static string Description_TH = "โรงตัดไม้ ตัดต้นไม้เพื่อนำมาทำท่อนไม้ ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้ไม้มากขึ้นไปด้วย";
    private static string Description_EN = "Wood can only be gathered by cutting down trees. It is used to build almost all Structures.";
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
		sprite = this.gameObject.GetComponent<OTSprite>();
		
        this.gameObject.name = BuildingName;
        base.buildingType = BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
	}
	
	// Use this for initialization
    void Start()
    {
		this.InitializeTexturesResource();
		
		base.sceneController.resourceCycle_Event += this.HaveResourceCycle_Event;
        base.NotEnoughResource_Notification_event += Sawmill_NotEnoughResource_Notification_event;
    }

	#region <@-- Events Handle.

    void HaveResourceCycle_Event(object sender, System.EventArgs e)
    {
		if(this.currentBuildingStatus == BuildingBeh.BuildingStatus.none) {		
			StoreHouse.Add_sumOfWood(this.productionRate[this.Level]);
		}
    }

    private void Sawmill_NotEnoughResource_Notification_event(object sender, NoEnoughResourceNotificationArg e)
    {
        base.notificationText = e.notification_args;
    }

	#endregion
	
	protected override void InitializeTexturesResource ()
	{
		base.InitializeTexturesResource ();
		
        //<!-- Load textures resource.
		buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "PinePlanks", typeof(Texture2D)) as Texture2D;
	}

    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, p_indexPosition, p_level);
		
		this.sprite.position += Vector2.up * 20f;
        BuildingBeh.Sawmill_Instance.Add(this);
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
    
    /// <summary>
    /// Building Processing.
    /// </summary>
    /// <param name="obj"></param>
    protected override void BuildingProcessComplete(BuildingBeh obj)
    {
        base.BuildingProcessComplete(obj);

		Destroy(processbar_Obj_parent);

        if (QuestSystemManager.arr_isMissionComplete[1] == false)
            sceneController.taskManager.questManager.MissionComplete(1);
    }
    /// <summary>
    /// Destruction building.
    /// </summary>
    protected override void DecreaseBuildingLevel()
    {
        base.DecreaseBuildingLevel();
        buildingLevel_textmesh.text = this.Level.ToString();
    }
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();
		
		base.sceneController.resourceCycle_Event -= HaveResourceCycle_Event;
        base.NotEnoughResource_Notification_event -= Sawmill_NotEnoughResource_Notification_event;

		BuildingBeh.Sawmill_Instance.Remove(this);
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if(currentBuildingStatus == BuildingBeh.BuildingStatus.onBuildingProcess
			|| currentBuildingStatus == BuildingBeh.BuildingStatus.onUpgradeProcess) {
			buildingLevel_textmesh.text = base.notificationText;
		}
	}
	
    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);
        
//        if(base.notificationText == "") {
//            base.notificationText = base.currentBuildingStatus.ToString();
//        }
        GUI.Box(base.notificationBox_rect, base.notificationText, base.notification_Style);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.windowRect.width, base.background_Rect.height));
        {
            GUI.BeginGroup(base.building_background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

                #region <!--- Content group.

                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {   //<!-- group draw order.

                    //<!-- Current Production rate.
                    GUI.Label(currentJob_Rect, "Current production rate per minute : " + productionRate[Level] * 6, base.job_style);
                    GUI.Label(nextJob_Rect, "Next production rate per minute : " + productionRate[Level + 1] * 6, base.job_style);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Label(GameMaterialDatabase.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), 
                            base.sceneController.taskManager.food_icon), standard_Skin.box);
                        GUI.Label(GameMaterialDatabase.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), 
                            base.sceneController.taskManager.wood_icon), standard_Skin.box);
                        //GUI.Label(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), 
                        //    base.stageManager.taskbarManager.stone_icon), standard_Skin.box);
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

				GUI.enabled = (base.CheckingCanUpgradeLevel() && base.CheckingEnoughUpgradeResource(RequireResource[Level])) ? true:false;
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
