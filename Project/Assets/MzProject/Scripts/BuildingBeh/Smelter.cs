using UnityEngine;
using System.Collections;

public class Smelter : Buildings {
	
    //<!-- Static Data.
    public static GameResource CreateResource = new GameResource(80, 120, 50, 60);
    public GameResource[] UpgradeResource = new GameResource[10] {
        new GameResource(100, 100, 100, 100),
        new GameResource(200, 200, 200, 200),
        new GameResource(300, 300, 300, 300),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
        new GameResource(200, 200, 200, 200),
	};
    public int Level { get { return base.level; } set { base.level = value; } }

    //<!-- Data.
    public static string BuildingName = "Smelter";
    private static string Description_TH = "โรงหลอมแร่ แร่ทองแดงถูกหลอมขึ้นที่นี่ อาวุธและชุดเกราะในกองทัพของคุณจำเป็นต้องใช้มัน \n" + " อัพเกรดเพื่อเพิ่มกำลังการผลิต";
    private static string Description_EN = "Copper can be gathered from mining. Upgrade Smelter to increase copper ingot production.";
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

    private int[] productionRate = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };       // produce food per second.
    private float timeInterval = 0;

	protected override void Awake() {
		base.Awake();
		
        if (sprite == null)
            sprite = this.GetComponent<OTSprite>();
		
        this.name = BuildingName;
        base.buildingType = Buildings.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
	}

	// Use this for initialization
    void Start() {
		
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
    protected override void DestroyBuildingProcess(Buildings building)
    {
        base.DestroyBuildingProcess(building);

        Destroy(processbar_Obj_parent);
		
		if(this.currentBuildingStatus != Buildings.BuildingStatus.none)
			this.currentBuildingStatus = Buildings.BuildingStatus.none;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (this.currentBuildingStatus == Buildings.BuildingStatus.none) {
            timeInterval += Time.deltaTime;
			
            if (timeInterval >= 1f && StoreHouse.sumOfGold < StoreHouse.SumOfCapacity) {
                timeInterval = 0;
                StoreHouse.sumOfGold += this.productionRate[this.level];
            }
        }
    }

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.background_Rect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height));
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);

            GUI.BeginGroup(base.background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.buildingIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.level, standard_Skin.box);
                GUI.BeginGroup(base.description_Rect, CurrentDescription, building_Skin.textArea);
                {   //<!-- group draw order.

                    //<!-- Current Production rate.
                    GUI.Label(currentProduction_Rect, "Current production rate : " + productionRate[level], building_Skin.label);
                    GUI.Label(nextProduction_Rect, "Next production rate : " + productionRate[level + 1], building_Skin.label);

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Label(GameResource.Food_Rect, new GUIContent(this.UpgradeResource[level].Food.ToString(), base.food_icon), standard_Skin.box);
                        GUI.Label(GameResource.Wood_Rect, new GUIContent(this.UpgradeResource[level].Wood.ToString(), base.wood_icon), standard_Skin.box);
                        GUI.Label(GameResource.Copper_Rect, new GUIContent(this.UpgradeResource[level].Gold.ToString(), base.copper_icon), standard_Skin.box);
                        GUI.Label(GameResource.Stone_Rect, new GUIContent(this.UpgradeResource[level].Stone.ToString(), base.stone_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();

                    //<!-- Upgrade Button.
                    if (StoreHouse.sumOfFood >= this.UpgradeResource[level].Food && StoreHouse.sumOfWood >= this.UpgradeResource[level].Wood &&
                        StoreHouse.sumOfGold >= this.UpgradeResource[level].Gold && StoreHouse.sumOfStone >= this.UpgradeResource[level].Stone)
                    {
                        Buildings._CanUpgradeLevel = this.CheckingCanUpgradeLevel();

                        if (Buildings._CanUpgradeLevel)
                        {
                            if (GUI.Button(upgradeButton_Rect, new GUIContent("Upgrade"), GUI.skin.button))
                            {
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
