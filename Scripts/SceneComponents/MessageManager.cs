using UnityEngine;
using System;
using System.Collections;

public class MessageManager : NotificationSystem {

	public enum MessageManagetStateBeh { none = 0, drawNewPlayerMessage = 1, drawActivity = 2,};
	public static MessageManagetStateBeh CurrentMessageManagerState;


	// Use this for initialization
    void Start() {

	}

    internal static void Handle_MainMenu_NewPlayer_Event(object sender, System.EventArgs e)
    {
		Debug.Log("MessageManager :: Handle_MainMenu_NewPlayer_Event");
        CurrentMessageManagerState = MessageManager.MessageManagetStateBeh.drawNewPlayerMessage;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void InitializeMessageMechanism()
    {
        CurrentMessageManagerState = MessageManagetStateBeh.drawActivity;

        if (messageDataStore.currentMessageTopic == string.Empty) {
            messageDataStore.currentMessageTopic = MessageDataStore.NULL_MESSAGE_TOPIC;
        } 
    }

	void OnGUI ()
	{
		GUI.depth = 0;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, Screen.height / Main.FixedGameHeight, 1));

		if (CurrentMessageManagerState == MessageManagetStateBeh.drawActivity)
		{
            TaskManager.IsShowInteruptGUI = true;
            this.DrawMessageListsWindow();
		}
        else if (CurrentMessageManagerState == MessageManagetStateBeh.drawNewPlayerMessage)
		{
            TaskManager.IsShowInteruptGUI = true;
			this.Draw_NewPlayerMessageWindow();
        }
	}

    internal void Draw_NewPlayerMessageWindow()
    {
		GUI.BeginGroup(taskManager.standardWindow_rect, "Message", taskManager.taskbarUI_Skin.window);
		{
	        GUI.Box(drawMessageRect, taskManager.messageFormSystem_icon);
	        GUI.DrawTexture(drawAdvisorRect, taskManager.newQuestAdvisor_img, ScaleMode.ScaleToFit);
	        GUI.Box(base.drawNoticeTopicRect, MessageDataStore.NEW_PLAYER_GREETING_MESSAGE_TOPIC, base.taskManager.taskbarUI_Skin.box);
			GUI.Box(base.drawNoticeMessageContentRect, base.messageDataStore.newPlayerGreetingMessage, base.noticeMessageContent_boxStyle);
	
			if(GUI.Button(base.completeSessionMessage_Rect, "The first mission", base.completeSessionMessage_buttonStyle)) {
				if (base.taskManager.questManager.currentQuestManagerStateBeh == QuestSystemManager.QuestManagerStateBeh.none) {
					taskManager.MoveOutLeftSidebar (TaskManager.DISPLAY_QUEST_ACTIVITY);
	                QuestSystemManager.CurrentMissionTopic_ID = 1;
	                taskManager.questManager.ActiveBeh_NoticeButton();
	            }
	            sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);
	            CloseGUIWindow();
			}
		}
		GUI.EndGroup();
    }

	internal void DrawGUI_MessageIcon()
	{
		GUI.enabled = !TaskManager.IsShowInteruptGUI ? true : false;
		{
			if (GUI.Button (taskManager.notificationRect_1,new GUIContent("Message",taskManager.messageFormSystem_icon), NoticeButton_style)) {
				taskManager.MoveOutLeftSidebar (TaskManager.DISPLAY_MESSAGE_ACTIVITY);
                sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);
			}
		}
		GUI.enabled = true;
	}

	void DrawMessageListsWindow ()
    {
        GUI.BeginGroup(taskManager.standardWindow_rect, "Message", taskManager.taskbarUI_Skin.window);
        {
            //<!-- Exit Button.
            if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6]))
            {
                sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);
                CloseGUIWindow();
            }

            GUI.Box(drawMessageRect, taskManager.messageFormSystem_icon);
			GUI.DrawTexture(drawAdvisorRect, taskManager.newQuestAdvisor_img, ScaleMode.ScaleToFit);
            GUI.Box(base.drawNoticeTopicRect, base.messageDataStore.currentMessageTopic, base.taskManager.taskbarUI_Skin.box);
            GUI.Box(base.drawNoticeMessageContentRect, base.messageDataStore.currentMessage, base.noticeMessageContent_boxStyle);
        }
        GUI.EndGroup();
    }

	void CloseGUIWindow ()
	{
		CurrentMessageManagerState = MessageManagetStateBeh.none;
		taskManager.MoveInLeftSidebar();
	}
}
