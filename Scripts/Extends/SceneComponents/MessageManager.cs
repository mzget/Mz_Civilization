using UnityEngine;
using System;
using System.Collections;

public class MessageManager : NotificationManager {

	public enum MessageManagetStateBeh { none = 0, drawNewPlayerMessage = 1, drawActivity = 2,};
	public static MessageManagetStateBeh CurrentMessageManagerState;


	// Use this for initialization
    void Start()
    {

	}

    internal static void Handle_MainMenu_NewPlayer_Event(object sender, System.EventArgs e)
    {
        CurrentMessageManagerState = MessageManager.MessageManagetStateBeh.drawNewPlayerMessage;
        Debug.Log("MessageManager :: Handle_MainMenu_NewPlayer_Event");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI ()
	{
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, Screen.height / Main.FixedGameHeight, 1));

		if (CurrentMessageManagerState == MessageManagetStateBeh.drawActivity) {
			taskManager.standardWindow_rect = GUI.Window(0, taskManager.standardWindow_rect, this.DrawMessageListsWindow, 
                new GUIContent("Message"), taskManager.taskbarUI_Skin.window);

            TaskManager.IsShowInteruptGUI = true;
		}
        else if (CurrentMessageManagerState == MessageManagetStateBeh.drawNewPlayerMessage)
        {
            taskManager.standardWindow_rect = GUI.Window(0, taskManager.standardWindow_rect, this.Draw_NewPlayerMessageWindow,
                new GUIContent("Message"), taskManager.taskbarUI_Skin.window);

            TaskManager.IsShowInteruptGUI = true;
        }
	}

    private void Draw_NewPlayerMessageWindow(int id)
    {
        GUI.Box(drawMessageRect, taskManager.messageFormSystem_icon);
        GUI.DrawTexture(drawAdvisorRect, questAdvisor_icon, ScaleMode.ScaleToFit);
        GUI.Box(base.drawNoticeTopicRect, MessageDataStore.NEW_PLAYER_GREETING_MESSAGE_TOPIC, base.taskManager.taskbarUI_Skin.box);
		GUI.Box(base.drawNoticeMessageContentRect, base.messageDataStore.newPlayerGreetingMessage, base.noticeMessageContent_boxStyle);

		if(GUI.Button(base.completeSessionMessage_Rect, "The first mission", base.completeSessionMessage_buttonStyle)) {
			CloseGUIWindow();
			if (base.taskManager.questManager.currentQuestManagerStateBeh == QuestManager.QuestManagerStateBeh.none) {
				taskManager.MoveOutLeftSidebar (TaskManager.DISPLAY_QUEST_ACTIVITY);
			}
		}
    }

	internal void DrawGUI_MessageIcon ()
	{
		GUI.enabled = !TaskManager.IsShowInteruptGUI ? true : false;
		{
			if (GUI.Button (taskManager.notificationRect_1,new GUIContent("Message",taskManager.messageFormSystem_icon), noticeButton_style)) {
				taskManager.MoveOutLeftSidebar (TaskManager.DISPLAY_MESSAGE_ACTIVITY);
			}
		}
		GUI.enabled = true;
	}

	void DrawMessageListsWindow (int id)
	{
		//<!-- Exit Button.
		if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6]))
		{
			CloseGUIWindow();
        }
		
		GUI.Box(drawMessageRect, taskManager.messageFormSystem_icon);
		GUI.DrawTexture(drawAdvisorRect, questAdvisor_icon, ScaleMode.ScaleToFit);
		GUI.Box(base.drawNoticeTopicRect, MessageDataStore.NEW_PLAYER_GREETING_MESSAGE_TOPIC, base.taskManager.taskbarUI_Skin.box);
		GUI.Box(base.drawNoticeMessageContentRect, base.messageDataStore.newPlayerGreetingMessage, base.noticeMessageContent_boxStyle);
	}

	void CloseGUIWindow ()
	{
		CurrentMessageManagerState = MessageManagetStateBeh.none;
		taskManager.MoveInLeftSidebar();
	}

}
