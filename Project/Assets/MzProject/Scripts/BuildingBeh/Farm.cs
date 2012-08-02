using UnityEngine;
using System.Collections;

public class Farm : Buildings
{
    //<!-- Static Data.
    public static GameResource CreateResource = new GameResource(50, 80, 80, 60);
    public static string BuildingName = "Farm";
    public static string Description = "อาหารเพื่อเลี้ยงประชากรของคุณผลิตขึ้นที่นี่ เพิ่มระดับฟาร์มเพื่อเพิ่มกำลังการผลิตธัญพืช";

    private int productionRate = 1;        // produce food per second.
    private float timeInterval = 0;



    void Awake() {
        sprite = this.gameObject.GetComponent<OTSprite>();
    }

    // Use this for initialization
    void Start()
	{
        this.name = BuildingName;
		base.buildingType = Buildings.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);
		
        this.level = 1;
        this.buildingStatus = Buildings.BuildingStatus.onBuildingProcess;
        this.OnBuildingProcess(this);
    }

    #region Building Processing.

    protected override void OnBuildingProcess(Buildings obj)
    {
        base.OnBuildingProcess(obj);
    }
    protected override void CreateProcessBar()
    {
        base.CreateProcessBar();
    }
    protected override void DestroyBuildingProcess(Buildings obj)
    {
        base.DestroyBuildingProcess(obj);

        Destroy(base.processbar_Obj_parent);

        if (this.buildingStatus != Buildings.BuildingStatus.buildingComplete)
            this.buildingStatus = Buildings.BuildingStatus.buildingComplete;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {		
		if(buildingStatus == Buildings.BuildingStatus.buildingComplete) {
	        timeInterval += Time.deltaTime;
	        if (timeInterval >= 1f) {
	            timeInterval = 0;
	
	            StoreHouse.sumOfFood += productionRate;
	        }
		}
    }
	
    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);
		
		GUI.Box(new Rect(48, 24, 256, 48), base.buildingStatus.ToString(), standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height), scrollPosition, 
            new Rect(0, 0, base.windowRect.width, base.background_Rect.height), false, false);
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
