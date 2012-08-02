using UnityEngine;
using System.Collections;

public class MillStone : Buildings {

    //<!-- Static Data.
    public static GameResource CreateResource = new GameResource(50, 80, 40, 40);
    public static string BuildingName = "MillStone";
    public static string Description = "โรงโม่หิน มีช่างหินเป็นผู้เชี่ยวชาญในการตัดหิน ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้หินมากขึ้นไปด้วย";

    private int productionRate = 1;        // produce food per second.
    private float timeInterval = 0;


	
	void Awake() {
        sprite = this.GetComponent<OTSprite>();
	}
	
	
	// Use this for initialization
	void Start () {		
        this.name = BuildingName;
        base.buildingType = BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);

        this.level = 1;
        this.buildingStatus = Buildings.BuildingStatus.onBuildingProcess;
        this.OnBuildingProcess(this);
    }

    #region Building Processing.

    protected override void OnBuildingProcess(Buildings building)
    {
        base.OnBuildingProcess(building);
    }
    protected override void CreateProcessBar()
    {
        base.CreateProcessBar();
    }
    protected override void DestroyBuildingProcess(Buildings obj)
    {
        base.DestroyBuildingProcess(obj);

        Destroy(processbar_Obj_parent);
		
		if(this.buildingStatus == Buildings.BuildingStatus.buildingComplete)
			this.buildingStatus = Buildings.BuildingStatus.buildingComplete;
    }

    #endregion
	
	// Update is called once per frame
	void Update () {
		
		if(this.buildingStatus == Buildings.BuildingStatus.buildingComplete) {
	        timeInterval += Time.deltaTime;
	        if (timeInterval >= 1f)
	        {
	            timeInterval = 0;
	
	            StoreHouse.sumOfStone += productionRate;
	        }
		}
	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 100, 800, 480), scrollPosition, new Rect(0, 0, 800, 480));
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);

            GUI.BeginGroup(base.background_Rect, GUIContent.none, building_Skin.box);
            {
                GUI.DrawTexture(base.buildingIcon_Rect, buildingIcon_Texture);
                GUI.Label(base.levelLable_Rect, "Level " + this.level, standard_Skin.box);
                GUI.TextArea(base.discription_Rect, Description, standard_Skin.textArea);
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
