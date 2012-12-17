using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : NotificationManager {

    public static bool[] arr_isMissionComplete = new bool[8];
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
        list_questBeh.Add(new QuestBeh() { QuestName = MissionMassageDataStore.NULL_MISSION_MESSAGE, QuestObjectives = "", });

        QuestBeh level_1 = new QuestBeh()
        {
            QuestName = MissionMassageDataStore.TOPIC_CREATE_SAWMILL,
            QuestObjectives = MissionMassageDataStore.CREATE_SAWMILL_DESCRIPTION,
            reward = new List<GameMaterialData>(3), 
            _IsComplete = arr_isMissionComplete[1],
        };
        level_1.reward.Add(new GameMaterialData() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        level_1.reward.Add(new GameMaterialData() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        level_1.reward.Add(new GameMaterialData() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(level_1);

        var level_2 = new QuestBeh()
        {
            QuestName = MissionMassageDataStore.TOPIC_CREATE_FARM,
            QuestObjectives = MissionMassageDataStore.CREATE_FARM_DESCRIPTION,
            reward = new List<GameMaterialData>(3),
            _IsComplete = arr_isMissionComplete[2],
        };
        level_2.reward.Add(new GameMaterialData() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        level_2.reward.Add(new GameMaterialData() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        level_2.reward.Add(new GameMaterialData() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(level_2);

        QuestBeh quest_3 = new QuestBeh()
        {
            QuestName = MissionMassageDataStore.TOPIC_CREATE_HOUSE,
            QuestObjectives = MissionMassageDataStore.CREATE_HOUSE_DESCRIPTION,
            reward = new List<GameMaterialData>(3),
            _IsComplete = arr_isMissionComplete[3],
        };
        quest_3.reward.Add(new GameMaterialData() { name = "Food", materialIcon = taskManager.food_icon, materialNumber = 20 });
        quest_3.reward.Add(new GameMaterialData() { name = "Wood", materialIcon = taskManager.wood_icon, materialNumber = 10 });
        quest_3.reward.Add(new GameMaterialData() { name = "Gold", materialIcon = taskManager.gold_icon, materialNumber = 20 });
        list_questBeh.Add(quest_3);

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

    private IEnumerator CheckingTodoMission()
    {
        if (CurrentMissionTopic_ID != 0) {
            this.ActiveBeh_NoticeButton();
        }

        yield return null;
    }
	
	// Update is called once per frame
	void Update () {

	}

    internal void InitializeMessageMechanism(QuestManagerStateBeh stateBeh)
    {
		currentQuestManagerStateBeh = stateBeh;

        if (QuestManager.CurrentMissionTopic_ID == 0) {
			QuestManager.CurrentMissionTopic_ID = 0;
        }
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));

        if (currentQuestManagerStateBeh == QuestManagerStateBeh.DrawMissionActivity) {
            this.DrawQuestListsWindow();
            TaskManager.IsShowInteruptGUI = true;
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
			if (GUI.Button (taskManager.notificationRect_2, new GUIContent ("Mission", quest_icon), NoticeButton_style)) {
				if (currentQuestManagerStateBeh == QuestManagerStateBeh.none) {
					if(CurrentMissionTopic_ID != 0) {
						if(list_questBeh[CurrentMissionTopic_ID]._IsComplete)
							taskManager.MoveOutLeftSidebar(TaskManager.DISPLAY_MISSION_COMPLETE_ACTIVITY);
						else
							taskManager.MoveOutLeftSidebar (TaskManager.DISPLAY_QUEST_ACTIVITY);
					}
					else {
						MessageManager.CurrentMessageManagerState = MessageManager.MessageManagetStateBeh.drawNewPlayerMessage;
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
                CloseGUIWindow();
				this.UnActive_NoticeButton();
            }

            GUI.Box(drawMessageRect, taskManager.messageFormSystem_icon);
            GUI.DrawTexture(drawAdvisorRect, questAdvisor_icon, ScaleMode.ScaleToFit);
            GUI.Box(base.drawNoticeTopicRect, list_questBeh[QuestManager.CurrentMissionTopic_ID].QuestName, base.taskManager.taskbarUI_Skin.box);
            GUI.Box(base.drawNoticeMessageContentRect, list_questBeh[QuestManager.CurrentMissionTopic_ID].QuestObjectives, base.noticeMessageContent_boxStyle);
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

	void DrawMissionCompleteWindow ()
	{
		GUI.BeginGroup(taskManager.standardWindow_rect, "Mission", taskManager.taskbarUI_Skin.window);
		{			
			GUI.Box(drawMessageRect, taskManager.messageFormSystem_icon);
			GUI.DrawTexture(drawAdvisorRect, questAdvisor_icon, ScaleMode.ScaleToFit);
			GUI.Box(base.drawNoticeTopicRect, list_questBeh[QuestManager.CurrentMissionTopic_ID].QuestName, base.taskManager.taskbarUI_Skin.box);
			GUI.Box(base.drawNoticeMessageContentRect, list_questBeh[QuestManager.CurrentMissionTopic_ID].QuestObjectives, base.noticeMessageContent_boxStyle);
			GUI.Box(rewardBoxRect, "Reward", taskManager.taskbarUI_Skin.box);
			GUI.Box(rewardItemRect_0, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[0].materialNumber.ToString(),
			                                         list_questBeh[CurrentMissionTopic_ID].reward[0].materialIcon), taskManager.taskbarUI_Skin.box);
			GUI.Box(rewardItemRect_1, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[1].materialNumber.ToString(),
			                                         list_questBeh[CurrentMissionTopic_ID].reward[1].materialIcon), taskManager.taskbarUI_Skin.box);
			GUI.Box(rewardItemRect_2, new GUIContent(list_questBeh[CurrentMissionTopic_ID].reward[2].materialNumber.ToString(),
			                                         list_questBeh[CurrentMissionTopic_ID].reward[2].materialIcon), taskManager.taskbarUI_Skin.box);

            //Get reward and go to next mission
            GUI.Box(new Rect(210 * Mz_OnGUIManager.Extend_heightScale, 300, 460 * Mz_OnGUIManager.Extend_heightScale, 40), "Get reward and go to next mission", taskManager.taskbarUI_Skin.box);

			if(GUI.Button(base.completeSessionMessage_Rect, "Complete", completeSessionMessage_buttonStyle)) {
                taskManager.MoveOutLeftSidebar(TaskManager.DISPLAY_QUEST_ACTIVITY);
                QuestManager.CurrentMissionTopic_ID += 1;
                taskManager.questManager.ActiveBeh_NoticeButton();
                this.CloseGUIWindow();
			}
		}
		GUI.EndGroup();
	}

    private void CloseGUIWindow()
    {
        currentQuestManagerStateBeh = QuestManagerStateBeh.none;
        taskManager.MoveInLeftSidebar();
    }
}
