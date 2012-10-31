using UnityEngine;
using System.Collections;

public class Base_ObjectBeh : MonoBehaviour {

    protected bool _OnTouchBegin = false;
    protected bool _OnTouchRelease = false;

	// Use this for initialization
	void Start () {
	
	}
	
	protected virtual void Update() {       
        //Debug.Log(this.name + " : update");

        if (_OnTouchBegin && _OnTouchRelease) {
            OnMouseDown();
        }
		        
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Moved) {
                _OnTouchBegin = false;
                _OnTouchRelease = false;
            }
        }
	}
	
	IEnumerator WaitForEndUpdate() {
		yield return new WaitForFixedUpdate();
	}

	#region <!-- OnInput Events.
    
	protected virtual void OnTouchBegan() {
        if(_OnTouchBegin == false)
            _OnTouchBegin = true;
	}
    protected virtual void OnMouseDown()
    {
        /// do something.
		
        _OnTouchBegin = false;
        _OnTouchRelease = false;
    }
    protected virtual void OnMouseExit() {
        if(_OnTouchRelease == false)
            _OnTouchRelease = true;
    }

    #endregion
}
