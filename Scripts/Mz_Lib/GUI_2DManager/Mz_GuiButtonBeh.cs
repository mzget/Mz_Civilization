using UnityEngine;
using System.Collections;

public class Mz_GuiButtonBeh : MonoBehaviour {
	
	private TaskManager GUI_manager;
    private AudioEffectManager audioEffect; 
	
	public GUITexture guitexture;
    private Vector3 originalScale;
    private bool _OnClick = false;

	// Use this for initialization
    void Start ()
	{
        originalScale = this.transform.localScale;

		GUI_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TaskManager>();

        try{
            guitexture = this.gameObject.GetComponent<GUITexture>();
        }
        catch{}
	}
	
	// Update is called once per frame
	void Update () {
        if(guitexture) {
            if(Input.touchCount >= 1) {
                Touch touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Stationary) {
                    if (guitexture.HitTest(touch.position)) {
						OnMouseDrag();
					}
                }
            }
        }


        if (_OnClick) {
            if (Input.GetMouseButtonDown(0)) {
                OnMouseDown();
            }

            if (Input.GetMouseButtonUp(0)) {
                OnMouseExit();
            }
        }
	}

    void OnMouseDown ()
	{
//        audioEffect.PlaySoundClickButton();

		GUI_manager.OnInput(this.gameObject.name);
	}
	
	void OnMouseDrag() {		
		GUI_manager.OnInput(this.gameObject.name);
	}
	
    void OnMouseEnter()
    {
//        audioEffect.PlayOnecWithOutStop(sound_Hover);
        this.transform.localScale = originalScale * 1.1f;
        _OnClick = true;
    }

    void OnMouseExit()
    {
        this.transform.localScale = originalScale;
        _OnClick = false;
    }
}
