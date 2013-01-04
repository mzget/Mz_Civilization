using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DisplayTroopsActivity : MonoBehaviour {

	public List<TroopsActivity> MilitaryActivityList = new List<TroopsActivity>();
	
	private Rect activityWindowsRect;
	
	private Rect[] arr_activityRect = new Rect[4];

    /// <summary>
    /// Draw Troops activity details.
    /// </summary>
	private Rect drawRemainingTime;
    private Rect[] arr_showGroupUnitBox = new Rect[4];

    /// <summary>
    /// Occurs when troops reach TO target_ event.
    /// </summary>
    public event System.EventHandler<MilitaryActivity_EventArg> troopsReachTOTarget_Event;
    private void OnTroopsReachToTargetEvent(MilitaryActivity_EventArg e) {
        if (troopsReachTOTarget_Event != null)
            troopsReachTOTarget_Event(this, e);
    }
	public class MilitaryActivity_EventArg : EventArgs {
		public int activity_id = 0;
	};

    private TaskManager taskManager;
    private GUIStyle labelStyle;
    public enum DrawGUIState { None = 0, DrawDetailWindow = 1, };
    public DrawGUIState currentDrawGUIState;
  


	void Awake ()
	{
		taskManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TaskManager>();
	}

	// Use this for initialization
	void Start () {
        this.InitializeDataFields();
		this.troopsReachTOTarget_Event += Handle_troopsReachTOTarget_Event;
	}

	void Handle_troopsReachTOTarget_Event (object sender, MilitaryActivity_EventArg e)
	{
		Debug.Log("Handle_troopsReachTOTarget_Event");

        WarfareSystem warfare = new WarfareSystem();
        warfare.WarfareProcessing(MilitaryActivityList[e.activity_id]);

		BarracksBeh.AmountOfSpearman += MilitaryActivityList[e.activity_id].groupOfUnitBeh.members[0];
		BarracksBeh.AmountOfHapaspist += MilitaryActivityList[e.activity_id].groupOfUnitBeh.members[1];
		BarracksBeh.AmountOfHoplite += MilitaryActivityList[e.activity_id].groupOfUnitBeh.members[2];
	}

    private void InitializeDataFields()
    {
		activityWindowsRect = new Rect(taskManager.baseSidebarGroup_rect.x - (300 * Mz_OnGUIManager.Extend_heightScale), 40, 300 * Mz_OnGUIManager.Extend_heightScale, 200);

		arr_activityRect[0] = new Rect(0 * Mz_OnGUIManager.Extend_heightScale, 30, 300 * Mz_OnGUIManager.Extend_heightScale, 40);
		arr_activityRect[1] = arr_activityRect[0]; arr_activityRect[1].y += 40;
        arr_activityRect[2] = arr_activityRect[1]; arr_activityRect[2].y += 40;
        arr_activityRect[3] = arr_activityRect[2]; arr_activityRect[3].y += 40;

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
					this.OnTroopsReachToTargetEvent(new MilitaryActivity_EventArg() { activity_id = i });
                }
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
			    GUI.BeginGroup(activityWindowsRect, "Military activities list.", GUIStyle.none);
			    {
				    for (int i = 0; i < MilitaryActivityList.Count; i++) {	
						string queueTime = "";
						if(MilitaryActivityList[i].RemainingTime.Ticks >= 0) {
							queueTime = new DateTime(MilitaryActivityList[i].RemainingTime.Ticks).ToString("HH:mm:ss");

                            if(GUI.Button(arr_activityRect[i], MilitaryActivityList[i].currentTroopsStatus 
						                  + " : " + MilitaryActivityList[i].targetCity.name
						                  + " in " + queueTime.ToString())) {
                                currentDrawGUIState = DrawGUIState.DrawDetailWindow;
                            }
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
			taskManager.standardWindow_rect = GUI.Window(0, taskManager.standardWindow_rect, DrawActivityWindow, new GUIContent("Troops status detail."), taskManager.foreignActivityStyle);
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
        for (int i = 0; i < MilitaryActivityList[0].groupOfUnitBeh.unitBehs.Count; i++)
        {
            GUI.Box(arr_showGroupUnitBox[i], MilitaryActivityList[0].groupOfUnitBeh.unitBehs[i] + " : " + MilitaryActivityList[0].groupOfUnitBeh.members[i], taskManager.taskbarUI_Skin.box);
        }
    }

    private void CloseGUIWindow()
    {
        currentDrawGUIState = DrawGUIState.None;
    }
}
