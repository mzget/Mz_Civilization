using UnityEngine;
using System.Collections;

public class BarrackBeh : Buildings
{
	//<!-- Static Data.
    public static string BuildingName = "Barrack";
	public static GameResource CreateResource = new GameResource(40, 180, 120, 80);

    private static string Description_TH = "ในค่ายทหารนี้คุณสามารถเกณฑ์ทหารราบได้ ยิ่งระดับค่ายทหารมากเท่าไร \n " + "กองกำลังก็จะเข็มแข็งมากขึ้น";
    private static string Description_EN = "Trains infantry units. Also researches technologies related to infantry units";
	public static string CurrentDescription {
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



	public GUISkin mainBuildingSkin;
	public Texture2D spearmanTex;
	public Texture2D hypaspistTex;
	public Texture2D hoplistTex;
	public Texture2D ToxotesTex;

	
	

    void Awake() {
        base.sprite = this.gameObject.GetComponent<OTSprite>();
    }
    
	// Use this for initialization
	void Start ()
	{
        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);

        base.currentBuildingStatus = BuildingStatus.onBuildingProcess;
        base.OnBuildingProcess(this);
    }

    #region Building Processing.

    public override void OnBuildingProcess(Buildings obj)
    {
        base.OnBuildingProcess(obj);
    }
    protected override void CreateProcessBar(Buildings.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void DestroyBuildingProcess(Buildings obj)
    {
        base.DestroyBuildingProcess(obj);

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != Buildings.BuildingStatus.buildingComplete)
            this.currentBuildingStatus = Buildings.BuildingStatus.buildingComplete;
    }

    #endregion
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	private Rect buttonRect = new Rect (460, 140, 100, 30);
    
	protected override void CreateWindow (int windowID)
	{
        base.CreateWindow(windowID);

        float height = 160f;

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height), 
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height * 5));
		{
            GUI.BeginGroup(new Rect (0, 0 * height, background_Rect.width, height), new GUIContent(string.Empty, "Barrack Header"), GUIStyle.none);
            {
                Rect headerContentRect = new Rect(5, 0 * height, background_Rect.width - 10, height);
                GUI.Box(headerContentRect, GUIContent.none, standard_Skin.box);
                GUI.Box(new Rect(200, 2, headerContentRect.width - 200, height - 4), Description_TH, standard_Skin.textArea);
                GUI.DrawTexture(buildingIcon_Rect, buildingIcon_Texture);
			}
			GUI.EndGroup ();

			GUI.BeginGroup (new Rect (0, 1 * height, background_Rect.width, height), new GUIContent ("Spearman", "พลหอก"), mainBuildingSkin.box);
			{
				GUI.Box (imgContent_Rect, new GUIContent (spearmanTex, "Spearman Images"));
				GUI.Box (contentRect, new GUIContent ("Spearman เป็นหน่วยที่มีพรสวรรค์และมีความชำนาญ \n ในการเอาชนะกองทหารม้า", "content"), mainBuildingSkin.textArea);
				if (GUI.Button (buttonRect, "Create")) {

				}
			}
			GUI.EndGroup ();

			GUI.BeginGroup (new Rect (0, 2 * height, background_Rect.width, height), new GUIContent ("Hypaspist", "พลถือโล่"), mainBuildingSkin.box);
			{
				GUI.Box (imgContent_Rect, new GUIContent (hypaspistTex, "Hypaspist Images"));
				GUI.Box (contentRect, new GUIContent ("Hypaspist เป็นนักรบถือโล่่และมีอาวุธครบมือ \n พวกเขาคือองค์รักษ์คุ้มกันจักรพรรดิด้วยชีวิต \n มีความชำนาญในการต่อต้านกองทหารราบอื่นๆ ", "content"), mainBuildingSkin.textArea);
				if (GUI.Button (buttonRect, "Create")) {

				}
			}
			GUI.EndGroup ();

			GUI.BeginGroup (new Rect (0, 3 * height, background_Rect.width, height), new GUIContent ("Hoplist", "พลหุ้มเกราะ"), mainBuildingSkin.box);
			{
				GUI.Box (imgContent_Rect, new GUIContent (hoplistTex, "Hoplist Images"));
				GUI.Box (contentRect, new GUIContent ("Hoplist เป็นหน่วยทหารราบหุ้มเกราะ \n พวกเขามีความชำนาญในการผลักดันและบุกฝ่าทะลวงข้าศึก", "content"), mainBuildingSkin.textArea);
				if (GUI.Button (buttonRect, "Create")) {
				}
			}
			GUI.EndGroup ();

            GUI.BeginGroup(new Rect(0, 4 * height, background_Rect.width, height), new GUIContent("Toxotes", "พลธนู"), mainBuildingSkin.box);
			{
				GUI.Box (imgContent_Rect, new GUIContent (ToxotesTex, "Toxotes Images"));
				GUI.Box (contentRect, new GUIContent ("Toxotes เป็นหน่วยจู่โจมด้วยธนู \n พวกเขามีความชำนาญในการโจมตีจากระยะไกล", "content"), mainBuildingSkin.textArea);
				if (GUI.Button (buttonRect, "Create")) {
				}
			}
			GUI.EndGroup ();    
		}
		// End the scroll view that we began above.
		GUI.EndScrollView ();
	}
}
