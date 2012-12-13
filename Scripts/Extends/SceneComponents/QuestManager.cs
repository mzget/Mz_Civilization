using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class QuestManager : NotificationManager {
    

    public enum QuestManagerStateBeh { none = 0, DrawActivity = 1, };
    public QuestManagerStateBeh currentQuestManagerStateBeh;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));

        if(currentQuestManagerStateBeh == QuestManagerStateBeh.DrawActivity)
            taskManager.standardWindow_rect = GUI.Window(0, taskManager.standardWindow_rect, this.DrawQuestListsWindow, 
			                                             new GUIContent("Quest list."), taskManager.taskbarUI_Skin.window);
    }

    internal void DrawQuestNoticeIcon ()
	{
		GUI.enabled = TaskManager.IsShowInteruptGUI ? false : true; 
		{
			if (GUI.Button (taskManager.notificationRect_2, new GUIContent ("Quest", quest_icon), noticeButton_style)) {
				if (currentQuestManagerStateBeh == QuestManagerStateBeh.none) {
						taskManager.MoveOutLeftSidebar (TaskManager.DISPLAY_QUEST_ACTIVITY);
				}
			}
		}
		GUI.enabled = true;
    }

    private void DrawQuestListsWindow(int id)
    {
        //<!-- Exit Button.
        if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6]))
        {
            CloseGUIWindow();
        }

        GUI.DrawTexture(drawAdvisorRect, questAdvisor_icon, ScaleMode.ScaleToFit);
    }

    private void CloseGUIWindow()
    {
        currentQuestManagerStateBeh = QuestManagerStateBeh.none;
        taskManager.MoveInLeftSidebar();
    }
}
