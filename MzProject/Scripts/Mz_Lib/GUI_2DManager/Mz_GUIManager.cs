using UnityEngine;
using System.Collections;

public class Mz_GUIManager : MonoBehaviour {
	
	private Mz_BaseScene baseScene;

    public static Rect viewPort_rect;
	public static Rect midcenterGroup_rect = new Rect(0, 0, Main.GAMEWIDTH, Main.GAMEHEIGHT);	
	//<!--- Equation finding scale == x = screen.height/ main.fixed.
    public static float Extend_heightScale = 0;
    public static void CalculateViewportScreen() {    
		// Calculation height of screen.
		if(Screen.height == Main.FixedGameHeight) {
            Extend_heightScale = 1;
		}
		else {
			Extend_heightScale =  Screen.height / Main.FixedGameHeight;
			
			midcenterGroup_rect.height = Main.FixedGameHeight * Extend_heightScale;
			midcenterGroup_rect.width = Main.FixedGameWidth * Extend_heightScale;
		}
		
        viewPort_rect = new Rect(((Screen.width / 2) - (midcenterGroup_rect.width / 2)), 0, midcenterGroup_rect.width, Main.FixedGameHeight);
    }


    void Awake() {
        CalculateViewportScreen();
    } 

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height/ Main.GAMEHEIGHT, 1));

		GUI.BeginGroup(viewPort_rect);
		{

		}
		GUI.EndGroup();
	}
}
