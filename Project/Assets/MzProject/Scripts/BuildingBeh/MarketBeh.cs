using UnityEngine;
using System.Collections;

public class MarketBeh : Buildings {

    public static GameResource CreateResource = new GameResource(100, 120, 120, 60);
    public GameResource[] UpgradeResource = new GameResource[10] {
        new GameResource(80, 120, 50, 60),
        new GameResource(200, 200, 200, 200),
        new GameResource(300, 300, 300, 300),
        new GameResource(400, 400, 400, 400),
        new GameResource(500, 500, 500, 500),
        new GameResource(600, 600, 600, 600),
        new GameResource(700, 700, 700, 700),
        new GameResource(800, 800, 800, 800),
        new GameResource(900, 900, 900, 900),
        new GameResource(1000, 1000, 1000, 1000),
	};

    public static string BuildingName = "Market";
    private static string Description_TH = "สร้างและฝึกฝนกองคาราวาน ซื้อขายและแลกเปลี่ยนสินค้า \n วิจัยและพัฒนากลไกการตลาด";
    private static string Description_EN = "The Market can be built to buy and sell resources for gold. Upgrade market to train more Caravan.";
    public static string CurrentDescription {
        get 
        {
            string temp = Description_EN;

            if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.Thai)
                temp = Description_TH;

            return temp;
        }
    }

    protected override void Awake() {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = MarketBeh.BuildingName;
        base.buildingType = Buildings.BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
		
		//<!-- Load textures.
        buildingIcon_Texture = Resources.Load(Buildings.PathOf_BuildingIcons + "Market", typeof(Texture2D)) as Texture2D;
    }

	// Use this for initialization
	void Start () {

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
            //<!-- description group rect.
            GUI.BeginGroup(new_descriptionGroupRect, CurrentDescription, building_Skin.textArea);
            {   //<!-- group draw order.

                //<!-- Current Production rate.
                //GUI.Label(currentProduction_Rect, "Current production rate : " + productionRate[level], building_Skin.label);
                //GUI.Label(nextProduction_Rect, "Next production rate : " + productionRate[level + 1], building_Skin.label);

                //<!-- Requirements Resource.
                GUI.BeginGroup(update_requireResource_Rect);
                {
                    GUI.Box(GameResource.Food_Rect, new GUIContent(this.UpgradeResource[Level].Food.ToString(), base.food_icon), standard_Skin.box);
                    GUI.Box(GameResource.Wood_Rect, new GUIContent(this.UpgradeResource[Level].Wood.ToString(), base.wood_icon), standard_Skin.box);
                    GUI.Box(GameResource.Copper_Rect, new GUIContent(this.UpgradeResource[Level].Gold.ToString(), base.copper_icon), standard_Skin.box);
                    GUI.Box(GameResource.Stone_Rect, new GUIContent(this.UpgradeResource[Level].Stone.ToString(), base.stone_icon), standard_Skin.box);
                }
                GUI.EndGroup();

                //<!-- Upgrade Button.
                if (StoreHouse.sumOfFood >= this.UpgradeResource[Level].Food && StoreHouse.sumOfWood >= this.UpgradeResource[Level].Wood &&
                    StoreHouse.sumOfGold >= this.UpgradeResource[Level].Gold && StoreHouse.sumOfStone >= this.UpgradeResource[Level].Stone)
                {
                    bool enableUpgrade = base.CheckingCanUpgradeLevel();

                    GUI.enabled = enableUpgrade;
                    if (GUI.Button(base.upgradeButton_Rect, new GUIContent("Upgrade")))
                    {
                        StoreHouse.UsedResource(UpgradeResource[Level]);

                        base.currentBuildingStatus = Buildings.BuildingStatus.onUpgradeProcess;
                        base.OnUpgradeProcess(this);
                        base._isShowInterface = false;
                    }
                    GUI.enabled = true;
                }
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    private void DrawGreekTradeUI()
    {
        GUI.BeginGroup(new Rect(0, 1 * base.background_Rect.height, background_Rect.width, base.background_Rect.height), GUIContent.none, building_Skin.box);
        {
            GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture, ScaleMode.ScaleToFit);
            GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);
            //<!-- description group rect.
            GUI.BeginGroup(new_descriptionGroupRect, CurrentDescription, building_Skin.textArea);
            {   //<!-- group draw order.

                //<!-- Current Production rate.
                //GUI.Label(currentProduction_Rect, "Current production rate : " + productionRate[level], building_Skin.label);
                //GUI.Label(nextProduction_Rect, "Next production rate : " + productionRate[level + 1], building_Skin.label);

                //<!-- Requirements Resource.
                GUI.BeginGroup(update_requireResource_Rect);
                {
                    GUI.Box(GameResource.Food_Rect, new GUIContent(this.UpgradeResource[Level].Food.ToString(), base.food_icon), standard_Skin.box);
                    GUI.Box(GameResource.Wood_Rect, new GUIContent(this.UpgradeResource[Level].Wood.ToString(), base.wood_icon), standard_Skin.box);
                    GUI.Box(GameResource.Copper_Rect, new GUIContent(this.UpgradeResource[Level].Gold.ToString(), base.copper_icon), standard_Skin.box);
                    GUI.Box(GameResource.Stone_Rect, new GUIContent(this.UpgradeResource[Level].Stone.ToString(), base.stone_icon), standard_Skin.box);
                }
                GUI.EndGroup();

                //<!-- Upgrade Button.
                if (StoreHouse.sumOfFood >= this.UpgradeResource[Level].Food && StoreHouse.sumOfWood >= this.UpgradeResource[Level].Wood &&
                    StoreHouse.sumOfGold >= this.UpgradeResource[Level].Gold && StoreHouse.sumOfStone >= this.UpgradeResource[Level].Stone)
                {
                    bool enableUpgrade = base.CheckingCanUpgradeLevel();

                    GUI.enabled = enableUpgrade;
                    if (GUI.Button(base.upgradeButton_Rect, new GUIContent("Upgrade")))
                    {
                        StoreHouse.UsedResource(UpgradeResource[Level]);

                        base.currentBuildingStatus = Buildings.BuildingStatus.onUpgradeProcess;
                        base.OnUpgradeProcess(this);
                        base._isShowInterface = false;
                    }
                    GUI.enabled = true;
                }
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }
}
