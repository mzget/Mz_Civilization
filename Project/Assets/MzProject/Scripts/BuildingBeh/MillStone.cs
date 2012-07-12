using UnityEngine;
using System.Collections;

public class MillStone : Buildings {

    //<!-- Static Data.
    public static GameResource CreateResource = new GameResource(50, 80, 40, 40);
    public static string BuildingName = "MillStone";
    public static string Description = "โรงโม่หิน มีช่างหินเป็นผู้เชี่ยวชาญในการตัดหิน ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้หินมากขึ้นไปด้วย";

    public Texture2D icon_Texture;
    [System.NonSerialized]
    public OTSprite sprite;

    private int productionRate = 1;        // produce food per second.
    private float timeInterval = 0;



	// Use this for initialization
	void Start () {
        sprite = this.GetComponent<OTSprite>();
        this.name = BuildingName;
        base.buildingType = BuildingType.resource;
        base.buildingTimeData = new BuildingsTimeData(base.buildingType);

        this.level = 1;
        this.buildingStatus = Buildings.BuildingStatus.buildingProcess;
        this.OnBuildingProcess();
    }

    #region Building Processing.

    public override void OnBuildingProcess()
    {
        base.OnBuildingProcess();
        Debug.Log(processbar_Obj_parent);

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
        scaleData.Add("oncompletetarget", this.gameObject);

        iTween.ValueTo(this.gameObject, scaleData);
    }

    protected override void BuildingProcess(Vector2 Rvalue)
    {
        base.BuildingProcess(Rvalue);

        processBar_Scolling.size = Rvalue;
    }

    protected override void DestroyBuildingProcess()
    {
        base.DestroyBuildingProcess();

        Destroy(processbar_Obj_parent);
    }

    #endregion
	
	// Update is called once per frame
	void Update () {
        timeInterval += Time.deltaTime;
        if (timeInterval >= 1f)
        {
            timeInterval = 0;

            StoreHouse.sumOfStone += productionRate;
        }
	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 100, 800, 480), scrollPosition, new Rect(0, 0, 800, 480));
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);

            GUI.BeginGroup(background_Rect, new GUIContent(Description), building_Skin.box);
            {
                GUI.DrawTexture(new Rect(24, 38, 80, 80), icon_Texture, ScaleMode.ScaleToFit);
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
