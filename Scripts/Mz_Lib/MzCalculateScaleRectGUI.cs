using UnityEngine;
using System.Collections;

public struct MzReCalculateScaleRectGUI {

    public static float ScaleHeightRatio = Screen.height / Main.GAMEHEIGHT;

    public static Rect CalculateScale_MiddleCenter_GUIRect(Rect p_rect) {        
        Rect newRect = p_rect;
        newRect.width *= ScaleHeightRatio;
        newRect.x += (p_rect.width - newRect.width) / 2;
        return newRect;
    }
    
    public static Rect CalculateScale_TopRight_GUIRect(Rect p_rect) {        
        Rect newRect = p_rect;
        newRect.height *= Screen.height / Main.GAMEHEIGHT;
        newRect.width = newRect.width * (Screen.height / Main.GAMEHEIGHT);
        newRect.x = Screen.width - newRect.width;
        newRect.y = 0;
        return newRect;
    }

    public static Rect CalculateScale_TopLeft_GUIRect(Rect p_rect) {        
        Rect newRect = p_rect;
        newRect.height *= Screen.height / Main.GAMEHEIGHT;
        newRect.width = newRect.width * (Screen.height / Main.GAMEHEIGHT);
        newRect.x *= ScaleHeightRatio;
        newRect.y *= ScaleHeightRatio;
        return newRect;
    }
	
	public static Rect ReCalulateWidth(Rect p_rect) {		     
        Rect newRect = p_rect;
        newRect.width = newRect.width * ScaleHeightRatio;
        newRect.x *= ScaleHeightRatio;
        return newRect;
	}
}
