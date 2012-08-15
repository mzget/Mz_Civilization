using UnityEngine;
using System.Collections;

public class MargetBeh : Buildings {

    public static string BuildingName = "Market";
	public static GameResource CreateResource = new GameResource(100, 120, 120, 60);

    private static string Description_TH = "สร้างและฝึกฝนกองคาราวาน ซื้อขายและแลกเปลี่ยนสินค้า \n วิจัยและพัฒนากลไกการตลาด";
    private static string Description_EN = "The Market can be built to buy and sell resources for gold. Upgrade market to train more Caravan.";
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

    void Awake() {
        base.sprite = this.gameObject.GetComponent<OTSprite>();
    }

	// Use this for initialization
	void Start () {
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);

        base.level = 1;
        base.buildingStatus = BuildingStatus.onBuildingProcess;
        base.OnBuildingProcess(this);
    }

    #region Building Processing.

    public override void OnBuildingProcess(Buildings obj)
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
	void Update () {
	
	}
}
