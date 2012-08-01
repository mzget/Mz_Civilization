using UnityEngine;
using System.Collections;

public class Sawmill : Buildings {
	
	//<!-- Static Data.
	public static GameResource CreateResource = new GameResource(0,0,0,0);
	public static string BuildingName = "Sawmill";
    public static string Description = "โรงตัดไม้ ตัดต้นไม้เพื่อนำมาทำท่อนไม้ ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้ไม้มากขึ้นไปด้วย";

    private int productionRate = 1;        // produce food per second.
    private float timeInterval = 0;

	
	
	void Awake() {
		sprite = this.gameObject.GetComponent<OTSprite>();
	}
	
	// Use this for initialization
	void Start () {
        this.name = BuildingName;
        base.buildingType = BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);

        this.level = 1;
        this.buildingStatus = Buildings.BuildingStatus.buildingProcess;
        this.OnBuildingProcess(this);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(this.buildingStatus == Buildings.BuildingStatus.buildingComplete) {
	        timeInterval += Time.deltaTime;
	        if (timeInterval >= 1f)
	        {
	            timeInterval = 0;
	
	            StoreHouse.sumOfWood += productionRate;
	        }
		}
	}
	
	#region Building Processing.
	
	protected override void OnBuildingProcess (Buildings obj)
	{
		base.OnBuildingProcess (obj);
	}
    protected override void CreateProcessBar()
    {
        base.CreateProcessBar();
    }
	protected override void BuildingProcess (Vector2 Rvalue)
	{
		base.BuildingProcess (Rvalue);
	}
	protected override void DestroyBuildingProcess (Buildings obj)
	{
		base.DestroyBuildingProcess (obj);
		
		Destroy(processbar_Obj_parent);
		
		if(this.buildingStatus != Buildings.BuildingStatus.buildingComplete) 
			this.buildingStatus = Buildings.BuildingStatus.buildingComplete;
	}
	
	#endregion

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
                GUI.TextArea(base.discription_Rect, Description, standard_Skin.textArea);
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
