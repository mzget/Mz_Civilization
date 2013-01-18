using UnityEngine;
using System.Collections;


public class BuildingsTimeData {
	
	public float[] arrBuildingTimesData = new float[BuildingBeh.MAX_LEVEL];
	
	
	
	public BuildingsTimeData() {}
	
    public BuildingsTimeData(BuildingBeh.BuildingType r_buildingType)
    {
        if (r_buildingType == BuildingBeh.BuildingType.general)
        {
            float[] time_generalType = { 30f, 50f, 90f, 120f, 180f, 220f, 250f, 300f, 400f, 500f, };

            arrBuildingTimesData = time_generalType;
        }
        else if (r_buildingType == BuildingBeh.BuildingType.resource)
        {
            float[] time_resourceType = { 30f, 50f, 90f, 120f, 180f, 220f, 250f, 300f, 400f, 500f, };

            arrBuildingTimesData = time_resourceType;
        }
        else if (r_buildingType == BuildingBeh.BuildingType.storehouse)
        {
            return;
        }
        else if (r_buildingType == BuildingBeh.BuildingType.barrack) {
            float[] time_resourceType = { 30f, 50f, 90f, 120f, 180f, 220f, 250f, 300f, 400f, 500f, };

            arrBuildingTimesData = time_resourceType;
        }
    }
};