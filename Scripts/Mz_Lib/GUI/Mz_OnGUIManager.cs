using UnityEngine;
using System.Collections;


/// <summary>
/// Return Extend_heightScale as float for use to multiply with Rectangle.x, Rectangle.width.
/// </summary>
public class Mz_OnGUIManager {
	
	private Mz_BaseScene baseScene;
    
    /// <summary>
    /// Return Static Fields. 
    /// </summary>
    public static Rect viewPort_rect;
	public static Rect midcenterGroup_rect = new Rect(0, 0, Main.FixedGameWidth, Main.FixedGameHeight);	
	//<!--- Equation finding scale == x = screen.height/ main.fixed.
    public static float Extend_heightScale = 1;

    /// <summary>
    /// Call onec on application startup.
    /// </summary>
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
}
