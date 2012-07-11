using UnityEngine;
using System.Collections;

public class BarrackBeh : Buildings
{
	//<!-- Static Data.
	public static GameResource CreateResource = new GameResource(40, 180, 120, 80);
    public static string Description = "ในค่ายทหารนี้คุณสามารถเกณฑ์ทหารราบได้ ยิ่งระดับค่ายทหารมากเท่าไร \n " + "กองกำลังก็จะเข็มแข็งมากขึ้น";


	public GUISkin mainBuildingSkin;
	public Texture2D spearmanTex;
	public Texture2D hypaspistTex;
	public Texture2D hoplistTex;
	public Texture2D ToxotesTex;

	
    
	// Use this for initialization
	void Start ()
	{
        name = "Barrack";
        Debug.Log("BarrackBeh:: " + "Level ::" + level);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

    #region All Mouse Event.

	void OnMouseOver ()
	{

	}

	void OnMouseDown ()
	{
		_clicked = true;
	}

	void OnMouseExit ()
	{

	}

    #endregion
    
	
	private Rect buttonRect = new Rect (460, 140, 100, 30);
    
	protected override void CreateWindow (int windowID)
	{		
        base.CreateWindow(windowID);
        
		// An absolute-positioned example: We make a scrollview that has a really large client
		// rect and put it in a small rect on the screen.
		scrollPosition = GUI.BeginScrollView (new Rect (0, 100, windowRect.width, 400), scrollPosition, new Rect (0, 0, 780, 1000));
		{
			float height = 200F;

            GUI.BeginGroup(new Rect (0, 0 * height, background_Rect.width, height), new GUIContent(string.Empty, "Barrack Header"), GUIStyle.none);
            {
                Rect headerContentRect = new Rect(5, 0 * height, background_Rect.width - 10, height);
                GUI.Box(headerContentRect, GUIContent.none, standard_Skin.box);
                GUI.Box(new Rect(200, 2, headerContentRect.width - 200, height - 4), Description, standard_Skin.textArea);
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
