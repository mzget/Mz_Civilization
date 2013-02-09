using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSystemManager : NotificationSystem {

    public static bool[] arr_isMissionComplete = new bool[16];
    public static int CurrentMissionTopic_ID = 0;

    public List<QuestBeh> list_questBeh = new List<QuestBeh>();

    public enum QuestManagerStateBeh { none = 0, DrawMissionActivity = 1, DrawCompleteMissionActivity, };
    public QuestManagerStateBeh currentQuestManagerStateBeh;
    private Rect rewardBoxRect;
    private Rect rewardItemRect_0;
    private Rect rewardItemRect_1;
	private Rect rewardItemRect_2;


	// Use this for initialization
    void Start()
    {
        StartCoroutine(this.InitializeMissionList());
        //<@-- Notification when user have a remaining mission in list.
        StartCoroutine(this.CheckingTodoMission());
        StartCoroutine(this.InitializeDataFields());
	}

    private IEnumerator InitializeMissionList()
    {
        list_questBeh.Add(new QuestBeh() { QuestName = MissionMessageData.NULL_MISSION_MESSAGE, QuestDescription = "", });

        QuestBeh level_1 = new QuestBeh()
        {
            QuestName = MissionMessageData.TOPIC_CREATE_SAWMILL,
            QuestDescription = MissionMessageData.CREATE_SAWMILL_DESCRIPTION,
            reward = new List<GameMaterial>(3), 
            _IsComplete = arr_isMissionComplete[1],
        };
        level_1.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        level_1.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        level_1.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(level_1);

        var level_2 = new QuestBeh()
        {
            QuestName = MissionMessageData.TOPIC_CREATE_FARM,
            QuestDescription = MissionMessageData.CREATE_FARM_DESCRIPTION,
            reward = new List<GameMaterial>(3),
            _IsComplete = arr_isMissionComplete[2],
        };
        level_2.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        level_2.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        level_2.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(level_2);

        QuestBeh quest_3 = new QuestBeh()
        {
            QuestName = MissionMessageData.TOPIC_CREATE_HOUSE,
            QuestDescription = MissionMessageData.CREATE_HOUSE_DESCRIPTION,
            reward = new List<GameMaterial>(3),
            _IsComplete = arr_isMissionComplete[3],
        };
        quest_3.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        quest_3.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        quest_3.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(quest_3);
		
		QuestBeh quest_4 = new QuestBeh() {
			QuestName = MissionMessageData.TOPIC_CREATE_STOREHOUSE, 
			QuestDescription = MissionMessageData.CREATE_STOREHOUSE_DESCRIPTION, 
            reward = new List<GameMaterial>(3),
            _IsComplete = arr_isMissionComplete[4],
		};
        quest_4.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        quest_4.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        quest_4.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(quest_4);

		QuestBeh quest_5 = new QuestBeh() { 
			QuestName = MissionMessageData.TOPIC_CREATE_MARKET, 
			QuestDescription = MissionMessageData.CREATE_MARKET_DESCRIPTION,
			reward = new List<GameMaterial>(3),
			_IsComplete = arr_isMissionComplete[5],
		};
		quest_5.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
		quest_5.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
		quest_5.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
		list_questBeh.Add(quest_5);

		QuestBeh quest_6 = new QuestBeh() { 
			QuestName = MissionMessageData.TOPIC_UPGRADE_TOWNCENTER, 
			QuestDescription = MissionMessageData.UPGRADE_TOWNCENTER_DESCRIPTION,
			reward = new List<GameMaterial>(3),
			_IsComplete = arr_isMissionComplete[6],
		};
		quest_6.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
		quest_6.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
		quest_6.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
		list_questBeh.Add(quest_6);

		QuestBeh quest_7 = new QuestBeh() {
			QuestName = MissionMessageData.LV7_TOPIC, 
			QuestDescription = MissionMessageData.LV7_DESCRIPTION,
			reward = new List<GameMaterial>(3),
			_IsComplete = arr_isMissionComplete[7],
		};
		quest_7.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
		quest_7.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
		quest_7.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
		list_questBeh.Add(quest_7);

        QuestBeh quest_8 = new QuestBeh() {
            QuestName = MissionMessageData.LV8_TOPIC,
            QuestDescription = MissionMessageData.LV8_DESCRIPTION,
            reward = new List<GameMaterial>(3),
            _IsComplete = arr_isMissionComplete[8],
        };
        quest_8.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        quest_8.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        quest_8.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(quest_8);

        QuestBeh quest_9 = new QuestBeh()
        {
            QuestName = MissionMessageData.LV9_TOPIC,
            QuestDescription = MissionMessageData.LV9_DESCRIPTION,
            reward = new List<GameMaterial>(3),
            _IsComplete = arr_isMissionComplete[9],
        };
        quest_9.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        quest_9.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        quest_9.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(quest_9);

        QuestBeh quest_10 = new QuestBeh()
        {
            QuestName = MissionMessageData.LV10_TOPIC,
            QuestDescription = MissionMessageData.LV10_DESCRIPTION,
            reward = new List<GameMaterial>(3),
            _IsComplete = arr_isMissionComplete[10],
        };
        quest_10.reward.Add(new GameMaterial() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        quest_10.reward.Add(new GameMaterial() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        quest_10.reward.Add(new GameMaterial() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(quest_10);

        yield return null;
    }

    private IEnumerator InitializeDataFields()
    {
        rewardBoxRect = new Rect(210 * Mz_OnGUIManager.Extend_heightScale, 200, 120 * Mz_OnGUIManager.Extend_heightScale, 40);
        rewardItemRect_0 = new Rect(210 * Mz_OnGUIManager.Extend_heightScale, 250, 100 * Mz_OnGUIManager.Extend_heightScale, 40);
        rewardItemRect_1 = new Rect(rewardItemRect_0.x + (110 * Mz_OnGUIManager.Extend_heightScale), 250, 100 * Mz_OnGUIManager.Extend_heightScale, 40);
        rewardItemRect_2 = new Rect(rewardItemRect_1.x + (110 * Mz_OnGUIManager.Extend_heightScale), 250, 100 * Mz_OnGUIManager.Extend_heightScale, 40);

        yield return null;
    }

    private IEnumerator CheckingTodoMission ()
	{
		Debug.Log("QuestSystemManager.CheckingTodoMission()");

		if (Mz_SaveData.Username != string.Empty)
		{
			if (CurrentMissionTopic_ID != 0 && CurrentMissionTopic_ID < list_questBeh.Count) {
				if (list_questBeh [CurrentMissionTopic_ID]._IsComplete)
					this.ActiveBeh_NoticeButton ();
			} 
			else if (CurrentMissionTopic_ID == 0 && list_questBeh [1]._IsComplete == false) {
				MessageManager.CurrentMessageManagerState = MessageManager.MessageManagetStateBeh.drawNewPlayerMessage;
			}
		}
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {

	}

    internal void InitializeMessageMechanism(QuestManagerStateBeh stateBeh)
    {
		currentQuestManagerStateBeh = stateBeh;

        if (QuestSystemManager.CurrentMissionTopic_ID == 0) {
			QuestSystemManager.CurrentMissionTopic_ID = 0;
        }
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));

        if (currentQuestManagerStateBeh == QuestManagerStateBeh.DrawMissionActivity) {
			if(CurrentMissionTopic_ID < list_questBeh.Count) {
	            this.DrawQuestListsWindow();
	            TaskManager.IsShowInteruptGUI = true;
			}
			else {
                currentQuestManagerStateBeh = QuestManagerStateBeh.none;
				Debug.Log("No mission available.");
			}
        }
		else if(currentQuestManagerStateBeh == QuestManagerStateBeh.DrawCompleteMissionActivity) {
			this.DrawMissionCompleteWindow();
			TaskManager.IsShowInteruptGUI = true;
		}
    }

    internal void DrawQuestNoticeIcon ()
	{
		GUI.enabled = TaskManager.IsShowInteruptGUI ? false : true; 
		{
			if (GUI.Button (taskManager.notificationRect_2, new GUIContent ("Mission", quest_icon), NoticeButton_style))
            {
                sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

				if (currentQuestManagerStateBeh == QuestManagerStateBeh.none)
                {
					if(CurrentMissionTopic_ID != 0 && CurrentMissionTopic_ID < list_questBeh.Count) 
					{
						if(list_questBeh[CurrentMissionTopic_ID]._IsComplete)
							taskManager.MoveOutLeftSidebar(TaskManager.DISPLAY_MISSION_COMPLETE_ACTIVITY);
						else
							taskManager.MoveOutLeftSidebar (TaskManager.DISPLAY_QUEST_ACTIVITY);
					}
					else if(CurrentMissionTopic_ID == 0) {
						MessageManager.CurrentMessageManagerState = MessageManager.MessageManagetStateBeh.drawNewPlayerMessage;
					}
					else {
						Debug.Log("No mission available.");
					}
				}
			}
		}
		GUI.enabled = true;
    }

    private void DrawQuestListsWindow ()
    {
        GUI.BeginGroup(taskManager.standardWindow_rect, "Mission", taskManager.taskbarUI_Skin.window);
        {
            //<!-- Exit Button.
            if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6]))
            {
                sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);
                CloseGUIWindow();
            }

            GUI.Box(drawMessageRect, taskManager.messageFormSystem_icon);
			GUI.DrawTexture(drawAdvisorRect, taskManager.newQuestAdvisor_img, ScaleMode.ScaleToFit);
            GUI.Box(base.drawNoticeTopicRect, list_questBeh[QuestSystemManager.CurrentMissionTopic_ID].QuestName, base.taskManager.taskbarUI_Skin.box);
            GUI.Box(base.drawNoticeMessageContentRect, list_questBeh[QuestSystemManager.CurrentMissionTopic_ID].QuestDescription, base.noticeMessageContent_boxStyle);
            GUI.Box(rewardBoxRect, "Reward", taskManager.taskbarUI_Skin.box);
            GUI.Box(rewardItemRect_0, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[0].materialNumber.ToString(),
                list_questBeh[CurrentMissionTopic_ID].reward[0].materialIcon), taskManager.taskbarUI_Skin.box);
            GUI.Box(rewardItemRect_1, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[1].materialNumber.ToString(),
                list_questBeh[CurrentMissionTopic_ID].reward[1].materialIcon), taskManager.taskbarUI_Skin.box);
            GUI.Box(rewardItemRect_2, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[2].materialNumber.ToString(),
                list_questBeh[CurrentMissionTopic_ID].reward[2].materialIcon), taskManager.taskbarUI_Skin.box);
        }
        GUI.EndGroup();
    }

	private void DrawMissionCompleteWindow ()
	{
		GUI.BeginGroup(taskManager.standardWindow_rect, "Mission", taskManager.taskbarUI_Skin.window);
		{			
			GUI.Box(drawMessageRect, taskManager.messageFormSystem_icon);
			GUI.DrawTexture(drawAdvisorRect, taskManager.completeQuestAdvisor_img, ScaleMode.ScaleToFit);
			GUI.Box(base.drawNoticeTopicRect, list_questBeh[QuestSystemManager.CurrentMissionTopic_ID].QuestName, base.taskManager.taskbarUI_Skin.box);
			GUI.Box(base.drawNoticeMessageContentRect, list_questBeh[QuestSystemManager.CurrentMissionTopic_ID].QuestDescription, base.noticeMessageContent_boxStyle);
			GUI.Box(rewardBoxRect, "Reward", taskManager.taskbarUI_Skin.box);
			GUI.Box(rewardItemRect_0, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[0].materialNumber.ToString(),
                list_questBeh[CurrentMissionTopic_ID].reward[0].materialIcon), taskManager.taskbarUI_Skin.box);
			GUI.Box(rewardItemRect_1, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[1].materialNumber.ToString(),
                list_questBeh[CurrentMissionTopic_ID].reward[1].materialIcon), taskManager.taskbarUI_Skin.box);
			GUI.Box(rewardItemRect_2, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[2].materialNumber.ToString(),
                list_questBeh[CurrentMissionTopic_ID].reward[2].materialIcon), taskManager.taskbarUI_Skin.box);

            //Get reward and go to next mission
            GUI.Box(new Rect(210 * Mz_OnGUIManager.Extend_heightScale, 320, 460 * Mz_OnGUIManager.Extend_heightScale, 40), "Get reward and go to next mission", taskManager.taskbarUI_Skin.box);

			if(GUI.Button(base.completeSessionMessage_Rect, "Get reward", completeSessionMessage_buttonStyle))
            {
                sceneController.audioEffect.PlayOnecSound(sceneController.audioEffect.buttonDown_Clip);

                if (QuestSystemManager.CurrentMissionTopic_ID < list_questBeh.Count)
                {
                    this.GetRewardSystem();
                    taskManager.MoveOutLeftSidebar(TaskManager.DISPLAY_QUEST_ACTIVITY);
                    QuestSystemManager.CurrentMissionTopic_ID += 1;
                    this.CloseGUIWindow();
                }
                else
                {
                    Debug.LogError("CurrentMissionID is more than questBeh_List");
                }
			}
		}
		GUI.EndGroup();
	}

    private void GetRewardSystem()
    {
        foreach (var reward in list_questBeh[CurrentMissionTopic_ID].reward)
	    {
            if (reward.name == "Food")
                StoreHouse.Add_sumOfFood(reward.materialNumber);
            else if (reward.name == "Wood")
                StoreHouse.Add_sumOfWood(reward.materialNumber);
            else if (reward.name == "Gold")
                StoreHouse.sumOfGold += reward.materialNumber;
	    }

        Debug.Log("GetRewardSystem");
    }

    private void CloseGUIWindow()
    {
        currentQuestManagerStateBeh = QuestManagerStateBeh.none;
        taskManager.MoveInLeftSidebar();

		if(CurrentMissionTopic_ID < list_questBeh.Count && list_questBeh[CurrentMissionTopic_ID]._IsComplete) {
			ActiveBeh_NoticeButton();
		}
		else {
			UnActive_NoticeButton();
		}
    }
	
	internal void MissionComplete (int mission_id)
	{		
		QuestSystemManager.arr_isMissionComplete[mission_id] = true;
		sceneController.taskManager.questManager.list_questBeh[mission_id]._IsComplete = true;
			
        if (list_questBeh[CurrentMissionTopic_ID]._IsComplete == true) 
            this.ActiveBeh_NoticeButton();
	}
}
