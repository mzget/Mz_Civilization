using UnityEngine;
using System.Collections;

public class HouseBeh : Buildings {

	// Use this for initialization
	void Start () {
        name = "House";
	}
	
	// Update is called once per frame
	void Update () {

	}

    protected override void DestructionBuilding()
    {
        base.DestructionBuilding();
    }

    protected override void CreateWindow(int windowID) 
    {
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), building_Skin.customStyles[0]))
        {
            _clicked = false;
        }

        scrollPosition = GUI.BeginScrollView(new Rect(0, 100, windowRect.width, windowRect.height - 40), scrollPosition, new Rect(0, 0, windowRect.width - 20, windowRect.height - 40));
        {
            GUI.BeginGroup(background_Rect, new GUIContent("House"), building_Skin.box);
            {

            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();

        base.CreateWindow(windowID);
    }
}
