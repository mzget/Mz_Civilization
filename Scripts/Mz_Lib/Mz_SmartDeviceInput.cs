using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mz_SmartDeviceInput : MonoBehaviour {
	
	/// <summary>
	/// SmartDevice input call with Update or FixUpdate function.
	/// </summary>
	public void ImplementTouchInput () {
		if(Input.touchCount >= 1)
        {
            Camera spaceCam = this.camera;

            Touch touch = Input.GetTouch(0);
            Ray cursorRay = spaceCam.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (touch.phase == TouchPhase.Began) {
                if (Physics.Raycast(cursorRay, out hit)) {
                    hit.collider.SendMessage("OnTouchBegan", SendMessageOptions.DontRequireReceiver);
                }
            }

			if (touch.phase == TouchPhase.Stationary) {
				if( Physics.Raycast(cursorRay, out hit)) {
					hit.collider.gameObject.SendMessage("OnTouchOver", SendMessageOptions.DontRequireReceiver);
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
    
	private Vector3 mousePos;
	private Vector3 originalPos;
	private Vector3 currentPos;   
    public void ImplementMouseInput () {
        Camera spaceCam = this.camera;

		mousePos = spaceCam.ScreenToViewportPoint(Input.mousePosition);
		Ray cursorRay = spaceCam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if(Input.GetMouseButtonDown(0)) {
			//Debug.Log("originalPos == " + originalPos);
			originalPos = mousePos;
			currentPos = mousePos;
			if (Physics.Raycast (cursorRay, out hit)) {
				hit.collider.SendMessage ("OnTouchBegan", this.tag, SendMessageOptions.DontRequireReceiver);
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
			else {
				if(Physics.Raycast(cursorRay, out hit)) {
					hit.collider.SendMessage("OnTouchOver", SendMessageOptions.DontRequireReceiver);
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
