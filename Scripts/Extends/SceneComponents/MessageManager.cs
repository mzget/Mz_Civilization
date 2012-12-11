using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour {

    private TaskManager taskManager;

	public enum MessageManagetStateBeh { none = 0, drawActivity = 1,};
	public MessageManagetStateBeh currentMessageManagerState;
    

	// Use this for initialization
    void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        taskManager = gameController.GetComponent<TaskManager>();
        //taskManager.displayTroopsActivity.troopsReachTOTarget_Event += displayTroopsActivity_displayMessageUI_Event;
	}

    private void displayTroopsActivity_displayMessageUI_Event(object sender, System.EventArgs e)
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, Screen.height / Main.FixedGameHeight, 1));

		if (currentMessageManagerState == MessageManagetStateBeh.drawActivity) {
			taskManager.standardWindow_rect = GUI.Window(0, taskManager.standardWindow_rect, this.DrawMessageListsWindow, 
			                                             new GUIContent("Message list."), taskManager.taskbarUI_Skin.window);
		}
	}

	internal void DrawGUI_MessageIcon ()
	{
		if (GUI.Button (taskManager.notificationRect_1, taskManager.messageFormSystem_icon)) {
			taskManager.MoveOutLeftSidebar(TaskManager.DISPLAY_MESSAGE_ACTIVITY);
		}
	}

	void DrawMessageListsWindow (int id)
	{
		//<!-- Exit Button.
		if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6]))
		{
			CloseGUIWindow();
		}
	}

	void CloseGUIWindow ()
	{
		currentMessageManagerState = MessageManagetStateBeh.none;
		taskManager.MoveInLeftSidebar();
	}

}
