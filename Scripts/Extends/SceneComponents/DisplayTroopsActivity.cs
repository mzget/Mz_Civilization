using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DisplayTroopsActivity : MonoBehaviour {

	public List<TroopsActivity> MilitaryActivityList = new List<TroopsActivity>();
	
	private Rect activityWindowsRect = new Rect(0, 40, 400 * Mz_OnGUIManager.Extend_heightScale, 200);
	
	private Rect[] arr_activityRect = new Rect[4];
    private Rect[] arr_labelRect = new Rect[4];

    /// <summary>
    /// Draw Troops activity details.
    /// </summary>
	private Rect drawRemainingTime;
    private Rect[] arr_showGroupUnitBox = new Rect[4];


    private TaskManager taskManager;
    private GUIStyle labelStyle;
    public enum DrawGUIState { None = 0, DrawDetailWindow = 1, };
    public DrawGUIState currentDrawGUIState;
    

    //private DateTime counterTimer;
    //private DateTime startingCounterTimer;

	
	// Use this for initialization
	void Start () {
        this.InitializeDataFields();
        taskManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TaskManager>();
	}

    private void InitializeDataFields()
    {
		arr_activityRect[0] = new Rect(0 * Mz_OnGUIManager.Extend_heightScale, 30, 180 * Mz_OnGUIManager.Extend_heightScale, 40);
		arr_activityRect[1] = arr_activityRect[0]; arr_activityRect[1].y += 40;
        arr_activityRect[2] = arr_activityRect[1]; arr_activityRect[2].y += 40;
        arr_activityRect[3] = arr_activityRect[2]; arr_activityRect[3].y += 40;

        arr_labelRect[0] = new Rect(180 * Mz_OnGUIManager.Extend_heightScale, 30, 200 * Mz_OnGUIManager.Extend_heightScale, 40);
        arr_labelRect[1] = arr_labelRect[0]; arr_labelRect[1].y += 40;
        arr_labelRect[2] = arr_labelRect[1]; arr_labelRect[2].y += 40;
        arr_labelRect[3] = arr_labelRect[2]; arr_labelRect[3].y += 40;

		drawRemainingTime  =  new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 30, 500 * Mz_OnGUIManager.Extend_heightScale, 40);
        arr_showGroupUnitBox[0] = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 70, 500 * Mz_OnGUIManager.Extend_heightScale, 40);
        arr_showGroupUnitBox[1] = arr_showGroupUnitBox[0]; arr_showGroupUnitBox[1].y += 40;
        arr_showGroupUnitBox[2] = arr_showGroupUnitBox[1]; arr_showGroupUnitBox[2].y += 40;
        arr_showGroupUnitBox[3] = arr_showGroupUnitBox[2]; arr_showGroupUnitBox[3].y += 40;
    }
	
	// Update is called once per frame
	void Update () {
        if(MilitaryActivityList.Count > 0) {
            for (int i = 0; i < MilitaryActivityList.Count; i++)
            {
			    TimeSpan elapsedTime = DateTime.UtcNow - MilitaryActivityList[i].startTime;
			    TimeSpan remainTime = MilitaryActivityList[i].timeToTravel - elapsedTime;
     
			    MilitaryActivityList[i].RemainingTime = remainTime;
                if(MilitaryActivityList[i].RemainingTime.Ticks <= 0) {
                    currentDrawGUIState = DrawGUIState.None;
                }

                //counterTimer = DateTime.UtcNow;
                //TimeSpan counter = counterTimer - startingCounterTimer;
                //if (counter.TotalSeconds >= list_trainingUnit[0].Unit.TimeTraining.TotalSeconds) {
                //    startingCounterTimer = DateTime.UtcNow;

                //    AmountOfSpearman += 1;
                //    list_trainingUnit[0].Number -= 1;
                //}
			
                //if(list_trainingUnit[0].Number == 0) { 
                //    list_trainingUnit.RemoveAt(0); 
                //    this.TrainingUnitMechanism();
                //}
            }
        }
    }
	
	void OnGUI() { 
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));

        if(currentDrawGUIState == DrawGUIState.None) {
            if(labelStyle == null) {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.normal.textColor = Color.green;
            }
		
		    if(MilitaryActivityList.Count > 0) {
			    GUI.BeginGroup(activityWindowsRect, "Military activities", GUIStyle.none);
			    {
				    for (int i = 0; i < MilitaryActivityList.Count; i++) {
                        if(GUI.Button(arr_activityRect[i], MilitaryActivityList[i].currentTroopsStatus + " : " + MilitaryActivityList[i].targetCity.name)) {
                            currentDrawGUIState = DrawGUIState.DrawDetailWindow;
                        }
					
					    if(MilitaryActivityList[i].RemainingTime.Ticks > 0) {
                    	    string queueTime = new DateTime(MilitaryActivityList[i].RemainingTime.Ticks).ToString("HH:mm:ss");
                    	    GUI.Label(arr_labelRect[i], queueTime, labelStyle);
					    }
					    else {
						    MilitaryActivityList.RemoveAt(i);

                            Debug.Log("MilitaryActivityList.Count : " + MilitaryActivityList.Count);
					    }
				    }
			    }
			    GUI.EndGroup();
		    }
        }
        else if(currentDrawGUIState == DrawGUIState.DrawDetailWindow) {
			taskManager.standardWindow_rect = GUI.Window(0, taskManager.standardWindow_rect, DrawActivityWindow, new GUIContent("Troops activity detail."));
        }
	}

    private void DrawActivityWindow(int id)
    {
        //<!-- Exit Button.
        if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6])) {
            CloseGUIWindow();
        }
		
		string queueTime = new DateTime(MilitaryActivityList[0].RemainingTime.Ticks).ToString("HH:mm:ss");
        GUI.Box(drawRemainingTime, MilitaryActivityList[0].currentTroopsStatus + " : " + MilitaryActivityList[0].targetCity.name + " in " + queueTime, taskManager.taskbarUI_Skin.box);
        for (int i = 0; i < MilitaryActivityList[0].groupUnits.unitName.Count; i++)
        {
            GUI.Box(arr_showGroupUnitBox[i], MilitaryActivityList[0].groupUnits.unitName[i] + " : " + MilitaryActivityList[0].groupUnits.member[i], taskManager.taskbarUI_Skin.box);
        }
    }

    private void CloseGUIWindow()
    {
        currentDrawGUIState = DrawGUIState.None;
    }
}
