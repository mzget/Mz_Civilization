using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DisplayTroopsActivity : MonoBehaviour {
	public List<TroopsActivity> MilitaryActivityList = new List<TroopsActivity>();
	
	private Rect activityWindowsRect = new Rect(0, 40, 400 * Mz_OnGUIManager.Extend_heightScale, 200);
	
	private Rect[] arr_activityRect = new Rect[4];
    private Rect[] arr_labelRect = new Rect[4];

    private GUIStyle labelStyle;

	
	// Use this for initialization
	void Start () {
        this.InitializeDataFields();
	}

    private void InitializeDataFields()
    {
		arr_activityRect[0] = new Rect(0 * Mz_OnGUIManager.Extend_heightScale, 30, 180 * Mz_OnGUIManager.Extend_heightScale, 40);
		arr_activityRect[1] = arr_activityRect[0]; arr_activityRect[1].y += 40;
		arr_activityRect[2] = arr_activityRect[1];		arr_activityRect[2].y += 40;
		arr_activityRect[3] = arr_activityRect[2];		arr_activityRect[3].y += 40;

        arr_labelRect[0] = new Rect(180 * Mz_OnGUIManager.Extend_heightScale, 30, 200 * Mz_OnGUIManager.Extend_heightScale, 40);
        arr_labelRect[1] = arr_labelRect[0]; arr_labelRect[1].y += 40;
        arr_labelRect[2] = arr_labelRect[1]; arr_labelRect[2].y += 40;
        arr_labelRect[3] = arr_labelRect[2]; arr_labelRect[3].y += 40;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() { 
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));

        if(labelStyle == null) {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = Color.black;
        }
		
		if(MilitaryActivityList.Count > 0) {
			GUI.BeginGroup(activityWindowsRect, "Military activities", GUIStyle.none);
			{
				for (int i = 0; i < MilitaryActivityList.Count; i++) {
                    GUI.Button(arr_activityRect[i], MilitaryActivityList[i].currentTroopsStatus + " : " + MilitaryActivityList[i].targetCity.name);
                    GUI.Label(arr_labelRect[i], "timespan", labelStyle);
				}
			}
			GUI.EndGroup();
		}
	}
}
