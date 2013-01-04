using UnityEngine;
using System.Collections;

public class StoneCrushingPlant : BuildingBeh {

    //<!-- Static Data.
    public static GameMaterialDatabase[] RequireResource = new GameMaterialDatabase[10] {
        new GameMaterialDatabase(80, 80, 0, 120, 3),
        new GameMaterialDatabase(160, 160, 0, 240, 5),
        new GameMaterialDatabase(320, 320, 0, 480, 8),
        new GameMaterialDatabase(640, 640, 0, 960, 12),
        new GameMaterialDatabase(1280, 1280, 0, 1920, 17),
        new GameMaterialDatabase(2560, 2560, 0, 3840, 23),
        new GameMaterialDatabase(3840, 3840, 0, 5760, 30),
        new GameMaterialDatabase(5760, 5760, 0, 7680, 38),
        new GameMaterialDatabase(7680, 7680, 0, 9600, 47),
        new GameMaterialDatabase(9600, 9600, 0, 12000, 60),
    };

    //<!-- Data.
    public static string BuildingName = "Stone crushing plant";
    private static string Description_TH = "โรงโม่หิน มีช่างหินเป็นผู้เชี่ยวชาญในการตัดหิน ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้หินมากขึ้นไปด้วย";
    private static string Description_EN = "Stone block can be gathered from stone mining. Upgrade millstone to increase stone block production.";
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
    

    protected override void Awake()
    {
        base.Awake(); 
		sprite = this.GetComponent<OTSprite>();
		
        this.gameObject.name = BuildingName;
        base.buildingType = BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
    }
	
	// Use this for initialization
	void Start () {
        this.InitializeTexturesResource();

        base.sceneController.resourceCycle_Event += HaveResourceCycle_Event;
        base.NotEnoughResource_Notification_event += MillStone_NotEnoughResource_Notification_event;
    }

    #region <!--- Events Handle.

    void HaveResourceCycle_Event(object sender, System.EventArgs e)
    {
        if (this.currentBuildingStatus == BuildingBeh.BuildingStatus.none) {
			StoreHouse.Add_sumOfStone(this.productionRate[this.Level]);
        }
    }

    private void MillStone_NotEnoughResource_Notification_event(object sender, NoEnoughResourceNotificationArg e)
    {
        base.notificationText = e.notification_args;
    }

    #endregion

    protected override void InitializeTexturesResource()
    {
        base.InitializeTexturesResource();

        buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "StoneBlock", typeof(Texture2D)) as Texture2D;
    }
    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, p_indexPosition, p_level);

        BuildingBeh.MillStoneInstance.Add(this);
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

    #region <!-- Building Processing.

    public override void OnBuildingProcess(BuildingBeh building)
    {
        base.OnBuildingProcess(building);
    }
    protected override void CreateProcessBar(BuildingBeh.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void BuildingProcessComplete(BuildingBeh obj)
    {
        base.BuildingProcessComplete(obj);

        Destroy(processbar_Obj_parent);
    }

    #endregion
	
	protected override void ClearStorageData ()
	{
		base.ClearStorageData ();

        sceneController.resourceCycle_Event -= HaveResourceCycle_Event;
		BuildingBeh.MillStoneInstance.Remove(this);
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

				/// Have to research stonecutter before upgrade.
				GUI.enabled = (base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level])) ? true : false;
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
