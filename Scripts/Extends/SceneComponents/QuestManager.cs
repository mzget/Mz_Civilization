using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class QuestManager : MonoBehaviour {

    public Texture2D quest_icon;
    internal Texture2D questAdvisor_icon;

    private GUIStyle noticeButton_style;

    public enum QuestManagerStateBeh { none = 0, DrawActivity = 1, };
    public QuestManagerStateBeh currentQuestManagerStateBeh;

    private TaskManager taskManager;


	// Use this for initialization
	void Start () {
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		taskManager = gameController.GetComponent<TaskManager>();

		this.InitializeDataFields();
	}

	void InitializeDataFields ()
	{
		quest_icon = Resources.Load(TaskManager.PathOfMainGUI + "Quest_icon", typeof(Texture2D)) as Texture2D;
        questAdvisor_icon = Resources.Load(TaskManager.Advisor_ResourcePath + "VillageElder", typeof(Texture2D)) as Texture2D;
        if (noticeButton_style == null)
        {
            noticeButton_style = new GUIStyle(taskManager.taskbarUI_Skin.button);
            noticeButton_style.normal.textColor = Color.red;
            noticeButton_style.alignment = TextAnchor.LowerCenter;
            noticeButton_style.imagePosition = ImagePosition.ImageAbove;
        }
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

    internal void DrawQuestNoticeIcon()
    {
        if (GUI.Button(taskManager.notificationRect_2,new GUIContent("Quest", quest_icon), noticeButton_style)) {
            Debug.Log("QuestManager.DrawQuestNoticeIcon");
            if (currentQuestManagerStateBeh == QuestManagerStateBeh.none)
            {
                taskManager.MoveOutLeftSidebar(TaskManager.DISPLAY_QUEST_ACTIVITY);
            }
        }
    }

    private void DrawQuestListsWindow(int id)
    {
        //<!-- Exit Button.
        if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6]))
        {
            CloseGUIWindow();
        }

        GUI.DrawTexture(new Rect(0, (taskManager.standardWindow_rect.height / 2) - 100, 200 * Mz_OnGUIManager.Extend_heightScale, 200), questAdvisor_icon, ScaleMode.ScaleToFit);
    }

    private void CloseGUIWindow()
    {
        currentQuestManagerStateBeh = QuestManagerStateBeh.none;
        taskManager.MoveInLeftSidebar();
    }
}
