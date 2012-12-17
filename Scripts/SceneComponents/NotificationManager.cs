using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour {

	private Texture2D noticeMessageBox_img;

    protected Texture2D quest_icon;
    protected Texture2D message_icon;
    protected Texture2D questAdvisor_icon;
	
    protected GUIStyle NoticeButton_style;
	protected GUIStyle noticeMessageContent_boxStyle;
	protected GUIStyle completeSessionMessage_buttonStyle;

    protected Rect drawAdvisorRect;
    protected Rect drawMessageRect;
    protected Rect drawNoticeTopicRect;
	protected Rect drawNoticeMessageContentRect;
	protected Rect completeSessionMessage_Rect;

    protected TaskManager taskManager;
	protected MessageDataStore messageDataStore;
    protected MissionMassageDataStore questMessageData;


    void Awake()
    {
        taskManager = this.gameObject.GetComponent<TaskManager>();
		
		messageDataStore = new MessageDataStore();
        questMessageData = new MissionMassageDataStore();
        //messageDataStore.list_questBeh.Add(new QuestBeh() { QuestName = "" });

        noticeMessageBox_img = Resources.Load(TaskManager.PathOfMainGUI + "GUI_box", typeof(Texture2D)) as Texture2D;
        quest_icon = Resources.Load(TaskManager.PathOfMainGUI + "Quest_icon", typeof(Texture2D)) as Texture2D;
        questAdvisor_icon = Resources.Load(TaskManager.Advisor_ResourcePath + "VillageElder", typeof(Texture2D)) as Texture2D;

        drawMessageRect = new Rect(40 * Mz_OnGUIManager.Extend_heightScale, 40, 100 * Mz_OnGUIManager.Extend_heightScale, 100);
        drawAdvisorRect = new Rect(0, (taskManager.standardWindow_rect.height / 2) - 100, 200 * Mz_OnGUIManager.Extend_heightScale, 200);
        drawNoticeTopicRect = new Rect(225 * Mz_OnGUIManager.Extend_heightScale, 40, 400 * Mz_OnGUIManager.Extend_heightScale, 40);
		drawNoticeMessageContentRect = new Rect(190 * Mz_OnGUIManager.Extend_heightScale, 90, 500 * Mz_OnGUIManager.Extend_heightScale, 300);
		completeSessionMessage_Rect = new Rect(190 * Mz_OnGUIManager.Extend_heightScale, 440, 200 * Mz_OnGUIManager.Extend_heightScale, 40);
		
        if (NoticeButton_style == null) {
            NoticeButton_style = new GUIStyle(taskManager.taskbarUI_Skin.button);
            NoticeButton_style.normal.textColor = Color.white;
            NoticeButton_style.alignment = TextAnchor.LowerCenter;
            NoticeButton_style.imagePosition = ImagePosition.ImageAbove;
        }

		if(noticeMessageContent_boxStyle == null) {
			noticeMessageContent_boxStyle = new GUIStyle(taskManager.taskbarUI_Skin.box);
			noticeMessageContent_boxStyle.normal.background = noticeMessageBox_img;
			noticeMessageContent_boxStyle.alignment = TextAnchor.UpperLeft;
			noticeMessageContent_boxStyle.wordWrap = true;
			noticeMessageContent_boxStyle.fontStyle = FontStyle.Italic;
			noticeMessageContent_boxStyle.fontSize = 16;
			noticeMessageContent_boxStyle.richText = true;
            noticeMessageContent_boxStyle.contentOffset = new Vector2(10, 20);
            noticeMessageContent_boxStyle.normal.textColor = Color.white;
		}

		if(completeSessionMessage_buttonStyle == null) {
			completeSessionMessage_buttonStyle = new GUIStyle(taskManager.taskbarUI_Skin.button);
			completeSessionMessage_buttonStyle.fontSize = 14;
			completeSessionMessage_buttonStyle.normal.textColor = Color.green;
			completeSessionMessage_buttonStyle.hover.textColor = Color.cyan;
			completeSessionMessage_buttonStyle.active.textColor = Color.grey;
		}
    }

    internal virtual void ActiveBeh_NoticeButton() {
        NoticeButton_style.normal.textColor = Color.red;
    }

	internal virtual void UnActive_NoticeButton() {
		NoticeButton_style.normal.textColor = Color.white;
	}
}
