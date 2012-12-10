using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class QuestManager : MonoBehaviour {
	
	private TaskManager taskManager;


    public Texture2D quest_icon;

	// Use this for initialization
	void Start () {
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		taskManager = gameController.GetComponent<TaskManager>();

		this.InitializeDataFields();
	}

	void InitializeDataFields ()
	{
		quest_icon = Resources.Load(TaskManager.PathOfMainGUI + "Quest_icon", typeof(Texture2D)) as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));

		if(GUI.Button(taskManager.notificationRect_2, quest_icon)) {}
    }
}
