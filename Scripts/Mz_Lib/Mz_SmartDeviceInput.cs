using UnityEngine;
using System.Collections;

public class Mz_SmartDeviceInput : MonoBehaviour {
	
	/// <summary>
	/// SmartDevice input call with Update or FixUpdate function.
	/// </summary>
	public static void IOS_INPUT () {
		RaycastHit hit = new RaycastHit();
		if(Input.touchCount >= 1) {	
			Ray cursorRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			
			if (Input.GetTouch(0).phase == TouchPhase.Stationary) {
				if( Physics.Raycast( cursorRay, out hit)) {
					hit.collider.gameObject.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
				}
				return;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Moved) {
				if(Physics.Raycast(cursorRay, out hit)) {
					hit.collider.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
				}	
				return;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Began) {
	            if (Physics.Raycast(cursorRay, out hit)) {
	                hit.collider.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
				}
				return;
			}
		}
	}
	
	/// <summary>
	/// Override of iOS_input not avialiable.
	/// </summary>
	/// <param name='characters'>
	/// Characters.
	/// </param>
	public static void IOS_INPUT ( GameObject[] characters ) {
		if(Input.touchCount >= 1 ) {
			Ray cursorRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hit;
			foreach (GameObject character in characters) {
				if( Physics.Raycast( cursorRay, out hit, 100F)) {
					if(hit.collider == character.collider) {
						hit.collider.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
		// Code for OnMouseDown in the iPhone. Unquote to test.
        for (int i = 0; i < Input.touchCount; ++i) {
			RaycastHit hit_1 = new RaycastHit();
            if (Input.GetTouch(i).phase ==  TouchPhase.Began) {
	            // Construct a ray from the current touch coordinates
	            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
	            if ( Physics.Raycast(ray, out hit_1, 100F )) {
	                hit_1.transform.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
	
	public static void IOS_GUITouch()
    {
        Camera camera_GUI = Camera.main;
		RaycastHit hit;

		if(Input.touchCount >= 1)
        {
			Touch touch = Input.GetTouch(0);
			Ray ray = camera_GUI.camera.ScreenPointToRay(touch.position);
			
			if(touch.phase == TouchPhase.Began) {			
	            if ( Physics.Raycast(ray, out hit)) {
	                hit.collider.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
				}
			}
		    
            if (touch.phase == TouchPhase.Moved) {				
				if( Physics.Raycast( ray, out hit)) {
					hit.collider.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
				}
			}

            if(touch.phase == TouchPhase.Stationary) {
                if(Physics.Raycast(ray, out hit)) {
                    hit.collider.SendMessage("OnMouseDrag", SendMessageOptions.DontRequireReceiver);
                }
			}
		}
	}
	
	public static void IOS_GUITouchPause() 
	{
		GameObject camera_Pause = GameObject.FindGameObjectWithTag("Camera_Pause");
		RaycastHit hit = new RaycastHit();
		
		if(Input.GetMouseButtonDown(0)) 
		{
			if(Input.touchCount == 1) 
			{
				Ray ray = camera_Pause.camera.ScreenPointToRay(Input.GetTouch(0).position);
				
	            if ( Physics.Raycast(ray, out hit)) {
	                hit.collider.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
				}
				return;
			}
		}
		else if(Input.touchCount >= 1) 
		{
			if (Input.GetTouch(0).phase == TouchPhase.Moved) 
			{
				Ray cursorRay = camera_Pause.camera.ScreenPointToRay(Input.GetTouch(0).position);
				
				if( Physics.Raycast( cursorRay, out hit)) {
					hit.collider.gameObject.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
				}
				return;
			}
		}
	}
}
