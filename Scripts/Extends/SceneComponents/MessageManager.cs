using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour {

    private TaskManager taskManager;
    

	// Use this for initialization
    void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        taskManager = gameController.GetComponent<TaskManager>();
        taskManager.displayTroopsActivity.troopsReachTOTarget_Event += displayTroopsActivity_displayMessageUI_Event;
	}

    private void displayTroopsActivity_displayMessageUI_Event(object sender, System.EventArgs e)
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {    
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));

        this.DrawGUI_MessageIcon();
    }

	void DrawGUI_MessageIcon ()
	{
		if (GUI.Button (taskManager.notificationRect_1, taskManager.messageFormSystem_icon)) {

		}
	}
}
