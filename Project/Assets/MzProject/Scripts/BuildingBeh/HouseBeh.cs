using UnityEngine;
using System.Collections;

public class HouseBeh : Buildings {

    //<!-- Requirements Resource.
    public static GameResource[] RequireResource = new GameResource[10] {
		new GameResource(80, 120, 40, 60, 0),
        new GameResource(200, 200, 200, 200, 0),
        new GameResource(300, 300, 300, 300, 0),
        new GameResource(400, 400, 400, 400, 0),
        new GameResource(500, 500, 500, 500, 0),
        new GameResource(600, 600, 600, 600, 0),
        new GameResource(700, 700, 700, 700, 0),
        new GameResource(800, 800, 800, 800, 0),
        new GameResource(900, 900, 900, 900, 0),
        new GameResource(1000, 1000, 1000, 1000, 0),
	};

    public const string BuildingName = "House";
    private const string Description_TH = "��ҹ�ѡ���ҧ������������ӹǹ��Ъҡ��٧�ش�ͧ�س";
    private const string Description_EN = "The house can be built to increase your maximum population.";
    public static string CurrentDescription {
        get {
            string temp = Description_EN;

            if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (MainMenu.CurrentAppLanguage == MainMenu.AppLanguage.Thai)
                temp = Description_TH;

            return temp;
        }
    }
    public static int SumOfPopulation = 5;

    private int[] maxPopulation = new int[5] { 10, 20, 30, 40, 50, };
    private int currentMaxPopulation;


    public static void CalculationSumOfPopulation()
    {
        SumOfPopulation = 5;
        foreach (HouseBeh house in Buildings.House_Instances)
            SumOfPopulation += house.currentMaxPopulation;
    }



    //<!-------------- Awake ------------------>>
    protected override void Awake()
    {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
    }

	// Use this for initialization
	IEnumerator Start () {
        this.LoadTextureResource();
        this.ReCalculatePopulationData();

        yield return 0;
    }

    protected override void LoadTextureResource()
    {
        base.LoadTextureResource();

        //<!-- Load textures resources.
        buildingIcon_Texture = Resources.Load(Buildings.PathOf_BuildingIcons + "House", typeof(Texture2D)) as Texture2D;
    }

    private void ReCalculatePopulationData()
    {
        for (int i = 1; i <= maxPopulation.Length; i++)
        {
            if (base.Level == i)
            {
                this.currentMaxPopulation = this.maxPopulation[i - 1];
                CalculationSumOfPopulation();
                break;
            }
        }
    }
	
	public override void InitializeData (BuildingStatus p_buildingState, int p_indexPosition, int p_level)
	{
		base.InitializeData (p_buildingState, p_indexPosition, p_level);

        Buildings.House_Instances.Add(this);
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
        {
            ReCalculatePopulationData();

            this.currentBuildingStatus = Buildings.BuildingStatus.none;
        }
    }

    #endregion
	
	// Update is called once per frame
	void Update () {

	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        GUI.Box(new Rect(48, 24, 256, 48), base.currentBuildingStatus.ToString(), standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.windowRect.width, base.background_Rect.height));
        {
            GUI.BeginGroup(base.building_background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

                #region <!--- Description group.

                GUI.BeginGroup(base.descriptionGroup_Rect, CurrentDescription, building_Skin.textArea);
                {
                    //<!-- Current Production rate.
                    if (base.Level < 10)
                    {
                        GUI.Label(currentProduction_Rect, "Current max population. : " + this.currentMaxPopulation, base.job_style);
                        GUI.Label(nextProduction_Rect, "Next max population. : " + this.maxPopulation[base.Level], base.job_style);

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
                    else
                    {
                        GUI.Label(currentProduction_Rect, "Current max population. : " + this.currentMaxPopulation, base.job_style);
                        GUI.Label(nextProduction_Rect, "Max upgrade building.", base.job_style);
                    }
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
        GUI.EndScrollView();
    }
}
