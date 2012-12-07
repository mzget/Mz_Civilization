using UnityEngine;
using System.Collections;

public class ForeignManager : MonoBehaviour
{
	public enum ForeignTabStatus
	{
		None = 0, DrawActivity = 1,
	};
	public ForeignTabStatus currentForeignTabStatus;

	private TaskManager taskManager;
	private DisplayTroopsActivity displayTroopsActivity;
	
	private Rect citiesSymbol_rect = new Rect(24 * Mz_OnGUIManager.Extend_heightScale, 24, 100 * Mz_OnGUIManager.Extend_heightScale, 100);
	private Rect citiesTagName_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 130, 120 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect sendButton_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 170, 120 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect selectTroopBox_rect = new Rect(150 * Mz_OnGUIManager.Extend_heightScale, 40, 545 * Mz_OnGUIManager.Extend_heightScale, 450);
	private Rect drawUnit_00_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 10, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
	private Rect selectUnitBoxRect_00 = new Rect(70 * Mz_OnGUIManager.Extend_heightScale, 24, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect maxUnitButtonRect_00 = new Rect(130 * Mz_OnGUIManager.Extend_heightScale, 20, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
	private Rect drawUnitRect_01 = new Rect(240 * Mz_OnGUIManager.Extend_heightScale, 10, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
	private Rect selectUnitBoxRect_01 = new Rect(300 * Mz_OnGUIManager.Extend_heightScale, 24, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect maxUnitButtonRect_01 = new Rect(360 * Mz_OnGUIManager.Extend_heightScale, 20, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
	private Rect drawUnitRect_10 = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 80, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
	private Rect selectUnitBoxRect_10 = new Rect(70 * Mz_OnGUIManager.Extend_heightScale, 94, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect maxUnitButtonRect_10 = new Rect(130 * Mz_OnGUIManager.Extend_heightScale, 90, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
	private Rect drawUnitRect_11 = new Rect(240 * Mz_OnGUIManager.Extend_heightScale, 80, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
	private Rect selectUnitBoxRect_11 = new Rect(300 * Mz_OnGUIManager.Extend_heightScale, 94, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect maxUnitButtonRect_11 = new Rect(360 * Mz_OnGUIManager.Extend_heightScale, 90, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
	private string numberOFUnit_00 = string.Empty; 
	private string numberOFUnit_01 = string.Empty;
	private string numberOFUnit_02 = string.Empty;

	// Use this for initialization
	void Start ()
	{
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		taskManager = gameController.GetComponent<TaskManager>();
		displayTroopsActivity = gameController.GetComponent<DisplayTroopsActivity>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGUI() {		
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));
		
		if(currentForeignTabStatus == ForeignManager.ForeignTabStatus.DrawActivity) {
			taskManager.standardWindow_rect = GUI.Window(0, taskManager.standardWindow_rect, DrawActivityWindow, new GUIContent("Select troops"));
		}		
		if(taskManager.currentRightSideState != TaskManager.RightSideState.show_ForeignTab && currentForeignTabStatus != ForeignManager.ForeignTabStatus.None) {
			currentForeignTabStatus = ForeignManager.ForeignTabStatus.None;
			TaskManager.IsShowInteruptGUI = false;
		}
	}
	
	private void DrawActivityWindow(int id)
	{
		//<!-- Exit Button.
		if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6])) {
			CloseGUIWindow();
		}

		GUI.BeginGroup(selectTroopBox_rect, "Pillage", taskManager.taskbarUI_Skin.box); {
			GUI.DrawTexture(drawUnit_00_rect, taskManager.spearmanUnitIcon);
			numberOFUnit_00 = GUI.TextField(selectUnitBoxRect_00, numberOFUnit_00, 3, taskManager.taskbarUI_Skin.textField);
			if (GUI.Button(maxUnitButtonRect_00, BarracksBeh.AmountOfSpearman.ToString())) {
				numberOFUnit_00 = BarracksBeh.AmountOfSpearman.ToString();
			}
			
			GUI.DrawTexture(drawUnitRect_01, taskManager.hypaspistUnitIcon);
			GUI.TextField(selectUnitBoxRect_01, "0", 3, taskManager.taskbarUI_Skin.textField);
			if (GUI.Button(maxUnitButtonRect_01, BarracksBeh.AmountOfHapaspist.ToString())) {
				numberOFUnit_01 = BarracksBeh.AmountOfHapaspist.ToString();
			}
			
			GUI.DrawTexture(drawUnitRect_10, taskManager.hopliteUnitIcon);
			GUI.TextField(selectUnitBoxRect_10, "0", 3, taskManager.taskbarUI_Skin.textField);
			if (GUI.Button(maxUnitButtonRect_10, BarracksBeh.AmountOfHoplite.ToString())) {
				numberOFUnit_02 = BarracksBeh.AmountOfHoplite.ToString();
			}
			
//			GUI.DrawTexture(drawUnitRect_11, taskManager.ToxotesUnitIcon);
//			GUI.TextField(selectUnitBoxRect_11, "0", 3, taskManager.taskbarUI_Skin.textField);
//			GUI.Button(maxUnitButtonRect_11, "Max");
		}
		GUI.EndGroup();
		
		/// Draw cities symbol.
		GUI.DrawTexture(citiesSymbol_rect, StageManager.list_AICity[0].symbols);
		GUI.Box(citiesTagName_rect, StageManager.list_AICity[0].name);
		
		if (GUI.Button(sendButton_rect, "Send")) {
			try{
				int unit_0 = numberOFUnit_00 != string.Empty ? int.Parse(numberOFUnit_00) : 0;
				int unit_1 = numberOFUnit_01 != string.Empty ? int.Parse(numberOFUnit_01) : 0;
				int unit_2 = numberOFUnit_02 != string.Empty ? int.Parse(numberOFUnit_02) : 0;
				
				GroupOFUnitBeh groupTemp = new GroupOFUnitBeh();
				groupTemp.unitName.Add(UnitDataStore.GreekUnitData.Spearman);
				groupTemp.unitName.Add(UnitDataStore.GreekUnitData.Hapaspist);
				groupTemp.unitName.Add(UnitDataStore.GreekUnitData.Hoplite);
				groupTemp.member.Add(unit_0);
				groupTemp.member.Add(unit_1);
				groupTemp.member.Add(unit_2);
				
				if(unit_0 + unit_1 + unit_2 > 0) {
					displayTroopsActivity.MilitaryActivityList.Add(new TroopsActivity() {
						currentTroopsStatus = TroopsActivity.TroopsStatus.Pillage,
						targetCity = StageManager.list_AICity[0],
						timeToTravel = System.TimeSpan.FromSeconds(StageManager.list_AICity[0].distance),
						startTime = System.DateTime.UtcNow,
						groupUnits = groupTemp,
					});
					
					Debug.Log ("displayTroopsActivity.MilitaryActivityList.Count : " + displayTroopsActivity.MilitaryActivityList.Count);
					
					CloseGUIWindow();
				}
			}catch {
				
			}finally {
				numberOFUnit_00 = string.Empty;
				numberOFUnit_01 = string.Empty;
				numberOFUnit_02 = string.Empty;
			}
		}
	}
	private void CloseGUIWindow()
	{
		currentForeignTabStatus = ForeignManager.ForeignTabStatus.None;
		TaskManager.IsShowInteruptGUI = false;
	}
}

