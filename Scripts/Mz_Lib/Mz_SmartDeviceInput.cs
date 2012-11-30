using UnityEngine;
using System.Collections;

public class Mz_SmartDeviceInput : MonoBehaviour {

	internal Camera cam;
	/// <summary>
	/// SmartDevice input call with Update or FixUpdate function.
	/// </summary>
	public void ImplementTouchInput () {
		if(cam == null) {
			Debug.LogWarning("Camera is null");
			return;
		}

		if(Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(0);
			Ray cursorRay = cam.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (touch.phase == TouchPhase.Began) {
                if (Physics.Raycast(cursorRay, out hit)) {
                    hit.collider.SendMessage("OnTouchBegan", SendMessageOptions.DontRequireReceiver);
                }
            }

			if (touch.phase == TouchPhase.Stationary) {
				if( Physics.Raycast(cursorRay, out hit)) {
					hit.collider.gameObject.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
				}
			}
			
            if(touch.phase == TouchPhase.Ended) {
				if(Physics.Raycast(cursorRay, out hit)) {
					hit.collider.SendMessage("OnTouchEnded", SendMessageOptions.DontRequireReceiver);
				}	

				return;
			}
			
			if(touch.phase == TouchPhase.Moved) {
				if(Physics.Raycast(cursorRay, out hit)) {
					hit.collider.SendMessage("OnTouchDrag", SendMessageOptions.DontRequireReceiver);
				}
			}
        
            Debug.DrawRay(cursorRay.origin, cursorRay.direction, Color.red);
		}
	}	
    
	public Vector3 mousePos;
	public Vector3 originalPos;
	public Vector3 currentPos;   
    public void ImplementMouseInput () {
		if(cam == null) {
			Debug.LogWarning("Camera is null");
			return;
		}

		mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
		Ray cursorRay = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if(Input.GetMouseButtonDown(0)) {
			//Debug.Log("originalPos == " + originalPos);
			originalPos = mousePos;
			currentPos = mousePos;
			if (Physics.Raycast (cursorRay, out hit)) {
				hit.collider.SendMessage ("OnTouchBegan", SendMessageOptions.DontRequireReceiver);
			}
		}
		
		if(Input.GetMouseButton(0)) {
			//Debug.Log("currentPos == " + currentPos);
			currentPos = mousePos;
			if(currentPos != originalPos) {
				if(Physics.Raycast(cursorRay, out hit)) {
					hit.collider.SendMessage("OnTouchDrag", SendMessageOptions.DontRequireReceiver);
				}	
			}
		}
		
		if (Input.GetMouseButtonUp(0)) {
			originalPos = Vector3.zero;
			currentPos = Vector3.zero;

			if(Physics.Raycast(cursorRay, out hit)) {
				hit.collider.SendMessage("OnTouchEnded", SendMessageOptions.DontRequireReceiver);
			}	
		}

        //if (touch.phase == TouchPhase.Stationary) {
        //    if( Physics.Raycast( cursorRay, out hit)) {
        //        hit.collider.gameObject.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
        //    }
        //}
	}
}
