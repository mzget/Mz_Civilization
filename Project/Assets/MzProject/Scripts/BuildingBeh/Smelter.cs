using UnityEngine;
using System.Collections;

public class Smelter : Buildings {
	
    //<!-- Static Data.
    public static GameResource CreateResource = new GameResource(80, 120, 50, 60);
    public static string BuildingName = "Smelter";
    public static string Description = "โรงหลอมแร่ แร่ทองแดงถูกหลอมขึ้นที่นี่ อาวุธและชุดเกราะในกองทัพของคุณจำเป็นต้องใช้มัน \n" + " อัพเกรดเพื่อเพิ่มกำลังการผลิต";

    private int productionRate = 1;        // produce food per second.
    private float timeInterval = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timeInterval += Time.deltaTime;
        if (timeInterval >= 1f)
        {
            timeInterval = 0;

            StoreHouse.sumOfGold += productionRate;
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
                GUI.DrawTexture(new Rect(24, 38, 80, 80), buildingIcon_Texture);
            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
