using UnityEngine;
using System.Collections;

public class Mz_ResizeScale : MonoBehaviour {
	
	public static void CalculationScale(Transform obj) 
	{
		Mz_GuiTextDebug.debug(Screen.width + "::" + Screen.height);
		
        float currentRatio = (float)Screen.width / (float)Screen.height;
		
		if(Screen.width == 1024) {
			obj.localScale = Vector3.one;
		}
		else {
			obj.localScale = new Vector3((Screen.width / Main.GameWidth), obj.localScale.y, obj.localScale.z);
		}
		
		if(Screen.height == 768) {
			obj.localScale = Vector3.one;
		}
		else {
			obj.localScale = new Vector3(obj.localScale.x, (Screen.height / Main.GameHeight), obj.localScale.z);
		}
	}
}
