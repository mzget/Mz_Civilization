using UnityEngine;
using System.Collections;

public class Sawmill : Buildings {
	
	//<!-- Static Data.
    public static GameResource CreateResource = new GameResource(80, 120, 80, 60);
    public GameResource[] UpgradeResource = new GameResource[10] {
        new GameResource(80, 120, 80, 60),
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
    public int Level { get { return base.level; } set { base.level = value; } }

    //<!-- Data.
	public static string BuildingName = "Sawmill";
    private static string Description_TH = "โรงตัดไม้ ตัดต้นไม้เพื่อนำมาทำท่อนไม้ ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้ไม้มากขึ้นไปด้วย";
    private static string Description_EN = "Wood can only be gathered by cutting down trees. It is used to build almost all Structures.";
    public static string CurrentDescription
    {
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
		sprite = this.gameObject.GetComponent<OTSprite>();
		
        this.name = BuildingName;
        base.buildingType = BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
	}
	
	// Use this for initialization
    void Start()
    {
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
    protected override void DestroyBuildingProcess(Buildings obj)
    {
        base.DestroyBuildingProcess(obj);

        Destroy(processbar_Obj_parent);

        if (this.currentBuildingStatus != Buildings.BuildingStatus.none)
            this.currentBuildingStatus = Buildings.BuildingStatus.none;
    }

    #endregion
	
	// Update is called once per frame
	void Update () 
	{		
		if(this.currentBuildingStatus == Buildings.BuildingStatus.none) {
	        timeInterval += Time.deltaTime;
			
	        if (timeInterval >= 1f && StoreHouse.sumOfWood < StoreHouse.SumOfCapacity) {
	            timeInterval = 0;
	
	            StoreHouse.sumOfWood += this.productionRate[this.level];
	        }
		}
	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.background_Rect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height));
        {
            GUI.BeginGroup(background_Rect, GUIContent.none, building_Skin.box);
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
