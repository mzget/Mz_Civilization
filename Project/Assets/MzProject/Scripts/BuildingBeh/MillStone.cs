using UnityEngine;
using System.Collections;

public class MillStone : Buildings {

    //<!-- Static Data.
    public static GameResource CreateResource = new GameResource(50, 80, 40, 40);
    public static string BuildingName = "MillStone";
    public static string Description = "โรงโม่หิน มีช่างหินเป็นผู้เชี่ยวชาญในการตัดหิน ยิ่งคุณอัพเกรดมันมากเท่าไหร่ \n คุณก็จะได้หินมากขึ้นไปด้วย";

    public Texture2D icon_Texture;

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
