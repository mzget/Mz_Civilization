using UnityEngine;
using System.Collections;

public class Base_ObjectBeh : MonoBehaviour {

    protected bool _OnTouchBegin = false;
    protected bool _OnTouchRelease = false;
	
	protected virtual void Update() {       
//        Debug.Log(this.name + " : update");

        if (_OnTouchBegin && _OnTouchRelease) {
            OnMouseDown();
        }
	}

    protected virtual void OnTouchBegan()
    {
        if (_OnTouchBegin == false)
            _OnTouchBegin = true;
    }
    protected virtual void OnMouseDown()
    {
        /// do something.
		
        _OnTouchBegin = false;
        _OnTouchRelease = false;
    }
    protected virtual void OnTouchEnded()
    {
        if (_OnTouchRelease == false)
            _OnTouchRelease = true;
    }
}
