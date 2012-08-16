using UnityEngine;
using System.Collections;

public class GuardTower : Buildings {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Main.FixedWidthRatio, Main.FixedHeightRatio, 1));

        if (_isShowInterface)
        {
            windowRect = GUI.Window(0, windowRect, CreateWindow, new GUIContent("Guard Tower", "GUI window"), building_Skin.window);
        }
    }

    protected override void CreateWindow(int windowID)
    {
        if (GUI.Button(new Rect(565, 5, 30, 30), new GUIContent(string.Empty, "Close Button"), building_Skin.customStyles[0]))
        {
            _isShowInterface = false;
        }

        scrollPosition = GUI.BeginScrollView(new Rect(0, 40, 600, 380), scrollPosition, new Rect(0, 0, 580, 380));
        {
            GUI.BeginGroup(new Rect(0, 0, 588, 380), new GUIContent("Guard Tower", ""), building_Skin.box);
            {

            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();

        base.CreateWindow(windowID);
    }
}
