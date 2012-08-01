using UnityEngine;
using System.Collections;

public class Smelter : Buildings {
	
    //<!-- Static Data.
    public static GameResource CreateResource = new GameResource(80, 120, 50, 60);
    public static string BuildingName = "Smelter";
    public static string Description = "โรงหลอมแร่ แร่ทองแดงถูกหลอมขึ้นที่นี่ อาวุธและชุดเกราะในกองทัพของคุณจำเป็นต้องใช้มัน \n" + " อัพเกรดเพื่อเพิ่มกำลังการผลิต";

    [System.NonSerialized]
    public OTSprite sprite;

    private int productionRate = 1;        // produce food per second.
    private float timeInterval = 0;



	// Use this for initialization
	void Start () {
        if (sprite == null)
            sprite = this.GetComponent<OTSprite>();

        this.name = BuildingName;
        base.buildingType = Buildings.BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);

        this.level = 1;
        this.buildingStatus = Buildings.BuildingStatus.buildingProcess;
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

        if (processbar_Obj_parent == null)
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
    protected override void BuildingProcess(Vector2 Rvalue)
    {
        base.BuildingProcess(Rvalue);

        processBar_Scolling.size = Rvalue;
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
                GUI.TextArea(base.discription_Rect, Description, standard_Skin.textArea);
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
