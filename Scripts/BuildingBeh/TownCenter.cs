using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownCenter : BuildingBeh {

    public GUISkin mainBuildingSkin;

    // Technology Point.
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        scrollPosition = GUI.BeginScrollView(
			new Rect(0, base.windowRect.height - base.background_Rect.height, base.background_Rect.width, base.background_Rect.height), 
			scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height));
        {
            GUI.BeginGroup(background_Rect, new GUIContent("Technology Tree", "Technology Tree Graph."), mainBuildingSkin.box);
            {

            }
            GUI.EndGroup();
        }
        GUI.EndScrollView();
    }
}
