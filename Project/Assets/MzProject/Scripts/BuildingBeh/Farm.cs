using UnityEngine;
using System.Collections;

public class Farm : Buildings
{
    //<!-- Static Data.
	
    //<!-- Resource.
    public static GameResource CreateResource = new GameResource(50, 80, 80, 60);
    public GameResource[] UpgradeResource = new GameResource[10];
	
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
        //		set{}
    }

    private int[] productionRate = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };       // produce food per second.
    private float timeInterval = 0;



    protected override void Awake() {
        base.Awake();
        sprite = this.gameObject.GetComponent<OTSprite>();
		
        this.name = BuildingName;
		base.buildingType = Buildings.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
    }

    // Use this for initialization
    void Start()
    {
        UpgradeResource[0] = new GameResource(100, 100, 100, 100);
        UpgradeResource[1] = new GameResource(200, 200, 200, 200);
        UpgradeResource[2] = new GameResource(300, 300, 300, 300);
        UpgradeResource[3] = new GameResource(200, 200, 200, 200);
        UpgradeResource[4] = new GameResource(200, 200, 200, 200);
        UpgradeResource[5] = new GameResource(200, 200, 200, 200);
        UpgradeResource[6] = new GameResource(200, 200, 200, 200);
        UpgradeResource[7] = new GameResource(200, 200, 200, 200);
        UpgradeResource[8] = new GameResource(200, 200, 200, 200);
        UpgradeResource[9] = new GameResource(200, 200, 200, 200);
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

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != Buildings.BuildingStatus.none)
            this.currentBuildingStatus = Buildings.BuildingStatus.none;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {		
		if(currentBuildingStatus == Buildings.BuildingStatus.none) {
	        timeInterval += Time.deltaTime;
	        if (timeInterval >= 1f) {
	            timeInterval = 0;
	
	            StoreHouse.sumOfFood += productionRate[level];
	        }
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

            GUI.BeginGroup(base.background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.buildingIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.level, standard_Skin.box);
                GUI.BeginGroup(base.description_Rect, CurrentDescription, building_Skin.textArea);
                {   //<!-- group draw order.

                    //<!-- Current Production rate.
                    Rect currentProduction_Rect = new Rect(10, 64, 200, 32);
                    Rect nextProduction_Rect = new Rect(10, 100, 200, 32);
                    Rect update_requireResource_Rect = new Rect(10, 140, 400, 32);
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
