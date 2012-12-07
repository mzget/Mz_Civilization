using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour {

    private TaskManager taskManager;
    private DisplayTroopsActivity displayTroop;

    public enum StateBeh { None = 0, DrawMessage = 1, }
    public StateBeh currentStateBeh;


    void Awake() {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        taskManager = gameController.GetComponent<TaskManager>();
        displayTroop = gameController.GetComponent<DisplayTroopsActivity>();
    }

	// Use this for initialization
	void Start () {
        displayTroop.troopsReachTOTarget_Event += displayTroopsActivity_displayMessageUI_Event;
	}

    private void displayTroopsActivity_displayMessageUI_Event(object sender, System.EventArgs e)
    {
        currentStateBeh = StateBeh.DrawMessage;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {    
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));

        if(currentStateBeh == StateBeh.DrawMessage)
            this.DrawGUI_MessageIcon();
    }

	void DrawGUI_MessageIcon ()
	{
		if (GUI.Button (new Rect (10 * Mz_OnGUIManager.Extend_heightScale, 50, 80 * Mz_OnGUIManager.Extend_heightScale, 80), taskManager.messageFormSystem_icon)) {
		}
	}
}
