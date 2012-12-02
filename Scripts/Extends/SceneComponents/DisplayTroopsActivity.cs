using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DisplayTroopsActivity : MonoBehaviour {
	public List<TroopsActivity> MilitaryActivityList = new List<TroopsActivity>();
	
	private Rect activityWindowsRect = new Rect(0, 40, 250 * Mz_OnGUIManager.Extend_heightScale, 200);
	private Rect activityRect_2;
	private Rect activityRect_3;
	
	private Rect[] arr_activityRect = new Rect[4];
	
	// Use this for initialization
	void Start () {
		arr_activityRect[0] = new Rect(5 * Mz_OnGUIManager.Extend_heightScale, 30, 240 * Mz_OnGUIManager.Extend_heightScale, 40);
		arr_activityRect[1] = new Rect(5 * Mz_OnGUIManager.Extend_heightScale, 70, 240 * Mz_OnGUIManager.Extend_heightScale, 40);
		arr_activityRect[2] = arr_activityRect[1];
		arr_activityRect[2].y += 40;
		arr_activityRect[3] = arr_activityRect[2];
		arr_activityRect[3].y += 40;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() { 
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));
		
		if(MilitaryActivityList.Count > 0) {
			GUI.BeginGroup(activityWindowsRect, "Military activities", GUI.skin.window);
			{
				for (int i = 0; i < MilitaryActivityList.Count; i++) {
					GUI.Button(arr_activityRect[i], "activityRect_" + i);
				}
			}
			GUI.EndGroup();
		}
	}
}
