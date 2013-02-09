using System.Collections;
using UnityEngine;

public class Base_ObjectBeh : MonoBehaviour {

    protected bool _OnTouchBegin = false;
    protected bool _OnTouchRelease = false;
	
	protected virtual void Update() {       
        if (_OnTouchBegin && _OnTouchRelease) {
            OnTouchDown();
        }
	}

    protected virtual void OnTouchBegan()
    {
        if (_OnTouchBegin == false)
            _OnTouchBegin = true;
    }

    protected virtual void OnTouchDown()
    {
        //Debug.Log("Class : Base_ObjectBeh." + "OnTouchDown");

        /// do something.
		
        _OnTouchBegin = false;
        _OnTouchRelease = false;
    }

	protected virtual void OnTouchOver() {
		//Debug.Log("Class : Base_ObjectBeh." + "OnTouchOver"); 
	}

    protected virtual void OnTouchDrag()
    {
//		Debug.Log("Class : Base_ObjectBeh." + "OnTouchDrag");
    }

    protected virtual void OnTouchEnded()
    {
        if (_OnTouchRelease == false && _OnTouchBegin)
        {
            _OnTouchRelease = true;
            print("Call OnTouchEnded_Function() : " + _OnTouchRelease);
        }
    }
	
	protected virtual void OnMouseExit() {
		_OnTouchBegin = false;
		_OnTouchRelease = false;
	}
}
