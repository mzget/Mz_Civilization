using UnityEngine;
using System.Collections;

public class Smelter : Buildings {
	
    //<!-- Static Data.
    public static GameResource CreateResource = new GameResource(80, 120, 50, 60);
    public static string BuildingName = "Smelter";

    private static string Description_TH = "โรงหลอมแร่ แร่ทองแดงถูกหลอมขึ้นที่นี่ อาวุธและชุดเกราะในกองทัพของคุณจำเป็นต้องใช้มัน \n" + " อัพเกรดเพื่อเพิ่มกำลังการผลิต";
    private static string Description_EN = "Copper can be gathered from mining. Upgrade Smelter to increase copper ingot production.";
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
        //		set{}
    }

    private int productionRate = 1;        // produce food per second.
    private float timeInterval = 0;

	
	
	void Awake() {
        if (sprite == null)
            sprite = this.GetComponent<OTSprite>();
	}

	// Use this for initialization
	void Start () {
        this.name = BuildingName;
        base.buildingType = Buildings.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);

        this.level = 1;
        this.buildingStatus = Buildings.BuildingStatus.onBuildingProcess;
        this.OnBuildingProcess(this);
		
		string position_Data = this.transform.position.x + "|" + this.transform.position.y + "|" + this.transform.position.z;
		Debug.Log(position_Data);
	}

    #region Building Processing.

    public override void OnBuildingProcess(Buildings building)
    {
        base.OnBuildingProcess(building);
    }
    protected override void CreateProcessBar()
    {
        base.CreateProcessBar();
    }
    protected override void DestroyBuildingProcess(Buildings building)
    {
        base.DestroyBuildingProcess(building);

        Destroy(processbar_Obj_parent);
		
		if(this.buildingStatus != Buildings.BuildingStatus.buildingComplete)
			this.buildingStatus = Buildings.BuildingStatus.buildingComplete;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {

        if (this.buildingStatus == Buildings.BuildingStatus.buildingComplete)
        {
            timeInterval += Time.deltaTime;
            if (timeInterval >= 1f)
            {
                timeInterval = 0;

                StoreHouse.sumOfGold += productionRate;
            }
        }
    }

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        scrollPosition = GUI.BeginScrollView(
            new Rect(0, 80, base.background_Rect.width, base.background_Rect.height),
            scrollPosition,
            new Rect(0, 0, base.background_Rect.width, base.background_Rect.height));
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);

            GUI.BeginGroup(base.background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.buildingIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.level, standard_Skin.box);
                GUI.TextArea(base.description_Rect, CurrentDescription, standard_Skin.textArea);
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
