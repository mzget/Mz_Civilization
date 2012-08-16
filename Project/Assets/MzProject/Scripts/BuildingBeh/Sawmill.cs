using UnityEngine;
using System.Collections;

public class Sawmill : Buildings {
	
	//<!-- Static Data.
    public static GameResource CreateResource = new GameResource(80, 120, 80, 60);
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
        //		set{}
    }

    private int productionRate = 1;        // produce food per second.
    private float timeInterval = 0;

	
	
	protected override void Awake() {
        base.Awake();
		sprite = this.gameObject.GetComponent<OTSprite>();
	}
	
	// Use this for initialization
    void Start()
    {
        this.name = BuildingName;
        base.buildingType = BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);

        this.currentBuildingStatus = Buildings.BuildingStatus.onBuildingProcess;
        this.OnBuildingProcess(this);
		
		string position_Data = this.transform.position.x + "|" + this.transform.position.y + "|" + this.transform.position.z;
		Debug.Log(position_Data);
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

        if (this.currentBuildingStatus != Buildings.BuildingStatus.buildingComplete)
            this.currentBuildingStatus = Buildings.BuildingStatus.buildingComplete;
    }

    #endregion
	
	// Update is called once per frame
	void Update () {
		
		if(this.currentBuildingStatus == Buildings.BuildingStatus.buildingComplete) {
	        timeInterval += Time.deltaTime;
	        if (timeInterval >= 1f)
	        {
	            timeInterval = 0;
	
	            StoreHouse.sumOfWood += productionRate;
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
            GUI.BeginGroup(background_Rect, GUIContent.none, building_Skin.box);
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
