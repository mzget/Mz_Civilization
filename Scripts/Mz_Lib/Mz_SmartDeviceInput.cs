using UnityEngine;
using System.Collections;

public class Mz_SmartDeviceInput : MonoBehaviour {
	
	/// <summary>
	/// SmartDevice input call with Update or FixUpdate function.
	/// </summary>
	public static void IOS_INPUT () {
		if(Input.touchCount >= 1) {
            Touch touch = Input.GetTouch(0);
            Ray cursorRay = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (touch.phase == TouchPhase.Began) {
                if (Physics.Raycast(cursorRay, out hit)) {
                    hit.collider.SendMessage("OnTouchBegan", SendMessageOptions.DontRequireReceiver);
                }
            }

			if (touch.phase == TouchPhase.Stationary) {
				if( Physics.Raycast( cursorRay, out hit)) {
					hit.collider.gameObject.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
				}
			}
			
            if(Input.GetTouch(0).phase == TouchPhase.Ended) {
				if(Physics.Raycast(cursorRay, out hit)) {
					hit.collider.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
				}	
			}
		}
	}
}
