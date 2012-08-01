using UnityEngine;
using System.Collections;

public class Sawmill : Buildings {
	
	//<!-- Static Data.
	public static GameResource CreateResource = new GameResource(0,0,0,0);
	public static string BuildingName = "Sawmill";
    public static string Description = "โรงตัดไม้ ตัดต้นไม้เพื่อนำมาทำท่อนไม้ ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้ไม้มากขึ้นไปด้วย";

    [System.NonSerialized]
    public OTSprite sprite;

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

        if (base.processbar_Obj_parent == null)
        {
            processbar_Obj_parent = Instantiate(Resources.Load("Processbar_Group", typeof(GameObject)),
            new Vector3(this.sprite.position.x, this.sprite.position.y + this.sprite.size.y, 0),
            Quaternion.identity) as GameObject;

            OTSprite backgroundSprite = processbar_Obj_parent.GetComponentInChildren<OTSprite>();
            backgroundSprite.size = new Vector2(128, 24);

            if (processBar_Scolling == null)
            {
                var scrolling = Instantiate(Resources.Load("processbar_scroll", typeof(GameObject))) as GameObject;
                scrolling.transform.parent = processbar_Obj_parent.transform;

                processBar_Scolling = scrolling.GetComponent<OTSprite>();
                processBar_Scolling.pivot = OTObject.Pivot.Left;
                processBar_Scolling.position = new Vector2((-backgroundSprite.size.x / 2) + 2, 0);
                processBar_Scolling.size = new Vector2(12, 24);
            }
        }

        Hashtable scaleData = new Hashtable();
        scaleData.Add("from", new Vector2(12, 24));
        scaleData.Add("to", new Vector2(124, 24));
        scaleData.Add("time", base.buildingTimeData.arrBuildingTimesData[level - 1]);
        scaleData.Add("onupdate", "BuildingProcess");
        scaleData.Add("easetype", iTween.EaseType.linear);
        scaleData.Add("oncomplete", "DestroyBuildingProcess");
        scaleData.Add("oncompleteparams", this);
        scaleData.Add("oncompletetarget", this.gameObject);

        iTween.ValueTo(this.gameObject, scaleData);
    }
	protected override void BuildingProcess (Vector2 Rvalue)
	{
		base.BuildingProcess (Rvalue);
		
		processBar_Scolling.size = Rvalue;
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
