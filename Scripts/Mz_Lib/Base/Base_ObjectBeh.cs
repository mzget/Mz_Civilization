using UnityEngine;
using System.Collections;

public class Base_ObjectBeh : MonoBehaviour {

    protected bool _OnTouchBegin = false;
    protected bool _OnTouchRelease = false;
	
	protected virtual void Update() {       
//        Debug.Log(this.name + " : update");

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
//		_OnTouchMove = false;
    }
    protected virtual void OnTouchEnded()
    {
        if (_OnTouchRelease == false && _OnTouchBegin)
            _OnTouchRelease = true;
    }
	
	protected virtual void OnMouseExit() {
		_OnTouchBegin = false;
		_OnTouchRelease = false;
	}
}
