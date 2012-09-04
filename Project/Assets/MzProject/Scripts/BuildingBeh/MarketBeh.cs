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
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        GUI.Box(new Rect(48, 24, 256, 48), base.currentBuildingStatus.ToString(), building_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.background_Rect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height), false, false);
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);

            GUI.BeginGroup(base.background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture, ScaleMode.ScaleToFit);
                GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);
                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {   //<!-- group draw order.

                    //<!-- Current Production rate.
                    //GUI.Label(currentProduction_Rect, "Current production rate : " + productionRate[level], building_Skin.label);
                    //GUI.Label(nextProduction_Rect, "Next production rate : " + productionRate[level + 1], building_Skin.label);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Label(GameResource.Food_Rect, new GUIContent(this.UpgradeResource[Level].Food.ToString(), base.food_icon), building_Skin.label);
                        GUI.Label(GameResource.Wood_Rect, new GUIContent(this.UpgradeResource[Level].Wood.ToString(), base.wood_icon), building_Skin.label);
                        GUI.Label(GameResource.Copper_Rect, new GUIContent(this.UpgradeResource[Level].Gold.ToString(), base.copper_icon), building_Skin.label);
                        GUI.Label(GameResource.Stone_Rect, new GUIContent(this.UpgradeResource[Level].Stone.ToString(), base.stone_icon), building_Skin.label);
                    }
                    GUI.EndGroup();

                    //<!-- Upgrade Button.
                    if (StoreHouse.sumOfFood >= this.UpgradeResource[Level].Food && StoreHouse.sumOfWood >= this.UpgradeResource[Level].Wood &&
                        StoreHouse.sumOfGold >= this.UpgradeResource[Level].Gold && StoreHouse.sumOfStone >= this.UpgradeResource[Level].Stone)
                    {
                        if (base.CheckingCanUpgradeLevel())
                        {
                            if (GUI.Button(upgradeButton_Rect, new GUIContent("Upgrade")))
                            {
                                StoreHouse.UsedResource(this.UpgradeResource[base.Level]);

                                base.currentBuildingStatus = Buildings.BuildingStatus.onUpgradeProcess;
                                base.OnUpgradeProcess(this);
                                base._isShowInterface = false;
                            }
                        }
                    }
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
