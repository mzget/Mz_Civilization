using UnityEngine;
using System.Collections;

public class MargetBeh : Buildings {

    public static string BuildingName = "Market";
	public static GameResource CreateResource = new GameResource(100, 120, 120, 60);


    void Awake() {
        base.sprite = this.gameObject.GetComponent<OTSprite>();
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);

        base.level = 1;
        base.buildingStatus = BuildingStatus.onBuildingProcess;
        base.OnBuildingProcess(this);
    }

	// Use this for initialization
	void Start () {

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
	void Update () {
	
	}
}
