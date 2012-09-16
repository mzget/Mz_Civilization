using UnityEngine;
using System.Collections;

public class MarketBeh : Buildings {

    public const string PathOfTribes_Texture = "Textures/Tribes_Icons/";

    public static GameResource[] RequireResource = new GameResource[10] {
        new GameResource(80, 120, 50, 60, 10),
        new GameResource(200, 200, 200, 200, 20),
        new GameResource(300, 300, 300, 300, 30),
        new GameResource(400, 400, 400, 400, 40),
        new GameResource(500, 500, 500, 500, 50),
        new GameResource(600, 600, 600, 600, 60),
        new GameResource(700, 700, 700, 700, 70),
        new GameResource(800, 800, 800, 800, 80),
        new GameResource(900, 900, 900, 900, 90),
        new GameResource(1000, 1000, 1000, 1000, 100),
	};

    public const string BuildingName = "Market";

    private const string Description_TH = "���ҧ��н֡���ͧ�����ҹ ���͢������š����¹�Թ��� \n �Ԩ����оѲ�ҡ�䡡�õ�Ҵ";
    private const string Description_EN = "The Market can be built to buy and sell resources for gold. Upgrade market to train more Caravan.";
    public static string CurrentDescription {
        get {
            string temp = Description_EN;

            if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.defualt_En) temp = Description_EN;
            else if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.Thai) temp = Description_TH;

            return temp;
        }
    }

    private const string GreekDescription_TH = "";
    private const string GreekDescription_EN = "Greece: ancient land of beauty, reason, passion ... and war.";
    private static string CurrentGreekDescription {
        get {
            string temp = "";

            if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.defualt_En) temp = GreekDescription_EN;
            else if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.Thai) temp = GreekDescription_TH;

            return temp;
        }
    }

    public Texture2D GreekIcon_Texture;
    public Texture2D EgyptianIcon_Texture;
    public Texture2D PersianIcon_Texture;
    public Texture2D CelticIcon_Texture;

	/// <summary>
	/// Awake this instance.
	/// </summary>
    protected override void Awake() {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = MarketBeh.BuildingName;
        base.buildingType = Buildings.BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
		
    }

	// Use this for initialization
	IEnumerator Start () {
        StartCoroutine(LoadtexturesResources());
        yield return 0;
    }

    IEnumerator LoadtexturesResources()
    {
        //<!-- Load textures.
        buildingIcon_Texture = Resources.Load(Buildings.PathOf_BuildingIcons + "Market", typeof(Texture2D)) as Texture2D;
        GreekIcon_Texture = Resources.Load(PathOfTribes_Texture + "Greek", typeof(Texture2D)) as Texture2D;

        yield return 0;
    }

    public override void InitializeData(Buildings.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializeData(p_buildingState, p_indexPosition, p_level);

        Buildings.MarketInstances.Add(this);
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

    protected override void ClearStorageData()
    {
        base.ClearStorageData();

        Buildings.MarketInstances.Remove(this);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private Rect new_descriptionGroupRect;

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        new_descriptionGroupRect = new Rect(base.descriptionGroup_Rect.x, base.descriptionGroup_Rect.y, base.descriptionGroup_Rect.width - 16, base.descriptionGroup_Rect.height);

        GUI.Box(new Rect(48, 24, 256, 48), base.currentBuildingStatus.ToString(), standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height * 4), false, false);
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);
			
			this.DrawMainBuildingContent();
            this.DrawGreekTradeUI();
        }
        GUI.EndScrollView();
    }

    private void DrawMainBuildingContent()
    {
        GUI.BeginGroup(base.background_Rect, GUIContent.none, building_Skin.box);
        {
            GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture, ScaleMode.ScaleToFit);
            GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

            #region <!--- description group.

            GUI.BeginGroup(new_descriptionGroupRect, CurrentDescription, building_Skin.textArea);
            {
                //<!-- Current Production rate.
                //GUI.Label(currentProduction_Rect, "Current production rate : " + productionRate[level], building_Skin.label);
                //GUI.Label(nextProduction_Rect, "Next production rate : " + productionRate[level + 1], building_Skin.label);

                //<!-- Requirements Resource.
                GUI.BeginGroup(update_requireResource_Rect);
                {
                    GUI.Box(GameResource.Food_Rect, new GUIContent(RequireResource[Level].Food.ToString(), base.food_icon), standard_Skin.box);
                    GUI.Box(GameResource.Wood_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), base.wood_icon), standard_Skin.box);
                    GUI.Box(GameResource.Stone_Rect, new GUIContent(RequireResource[Level].Stone.ToString(), base.stone_icon), standard_Skin.box);
                    GUI.Box(GameResource.Gold_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), base.gold_icon), standard_Skin.box);
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
                this.DestructionBuilding();
                base.CloseGUIWindow();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }
    //<<!-- Greek trade.
    int numberOfMeats = 0;
    int numberOfOliveOil = 0;
    private void DrawGreekTradeUI()
    {
        GUI.BeginGroup(new Rect(0, 1 * base.background_Rect.height, background_Rect.width, base.background_Rect.height), GUIContent.none, building_Skin.box);
        {
            GUI.DrawTexture(base.imgIcon_Rect, GreekIcon_Texture);
            GUI.Label(base.levelLable_Rect, "Greek", base.status_style);
            //<!-- description group rect.
            GUI.BeginGroup(new_descriptionGroupRect, MarketBeh.CurrentGreekDescription, building_Skin.textArea);
            {   //<!-- group draw order.
                //<<!-- purchase  group.
                Rect importGroupRect = new Rect(10, 40, new_descriptionGroupRect.width - 20, 110);
                GUI.BeginGroup(importGroupRect, GUIContent.none, standard_Skin.textArea);
                {
                    GUI.Label(new Rect(0, 0, 100, 30), "Purchase");
                    GUI.Box(new Rect(20, 35, 100, 30), new GUIContent("Weapon"), standard_Skin.textField);
                    GUI.Box(new Rect(20, 70, 100, 30), new GUIContent("Armor"), standard_Skin.textField);
                }
                GUI.EndGroup();
                //<<!-- sell group.
                Rect exportGroupRect = new Rect(10, 160, new_descriptionGroupRect.width - 20, 110);
                GUI.BeginGroup(exportGroupRect, GUIContent.none, standard_Skin.textArea);
                {
                    GUI.Label(new Rect(0, 0, 100, 30), "Sell");
                    //<!-- "Meats".
                    GUI.Box(new Rect(20, 35, 100, 30), new GUIContent("Meats"), standard_Skin.textField);
                    GUI.Box(new Rect(160, 35, 50, 30), numberOfMeats.ToString(), GUI.skin.textField);
                    if (GUI.Button(new Rect(130, 42, 16, 16), GUIContent.none, base.stageManager.taskbarManager.left_button_Style)) {
                        if (numberOfMeats > 0)
                            numberOfMeats -= 8;
                    }
                    else if (GUI.Button(new Rect(225, 42, 16, 16), GUIContent.none, base.stageManager.taskbarManager.right_button_Style)) {
                        if (numberOfMeats < 64)
                            numberOfMeats += 8;
                    }
                    //<!-- "Olive Oil".
                    GUI.Box(new Rect(20, 70, 100, 30), new GUIContent("Olive Oil"), standard_Skin.textField);
                    GUI.Box(new Rect(160, 70, 50, 30), numberOfOliveOil.ToString(), GUI.skin.textField);
                    if (GUI.Button(new Rect(130, 77, 16, 16), GUIContent.none, base.stageManager.taskbarManager.left_button_Style)) {
                        if (numberOfOliveOil > 0)
                            numberOfOliveOil -= 8;
                    }
                    else if (GUI.Button(new Rect(225, 77, 16, 16), GUIContent.none, base.stageManager.taskbarManager.right_button_Style)) {
                        if (numberOfOliveOil < 64)
                            numberOfOliveOil += 8;
                    }
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }
}
