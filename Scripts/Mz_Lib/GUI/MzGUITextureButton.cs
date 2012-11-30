using UnityEngine;
using System.Collections;

public class MzGUITextureButton : MonoBehaviour {
	
	private TaskManager taskbarManager;
    private AudioEffectManager audioEffect; 
	
	public GUITexture guitexture;
    private Vector3 originalScale;
    private bool _OnHover = false;


    void Awake()
    {
        taskbarManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TaskManager>();
    }

	// Use this for initialization
    void Start ()
	{
        originalScale = this.transform.localScale;

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


        if (_OnHover) {
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
		taskbarManager.OnInput(this.gameObject.name);
	}
	
	void OnMouseDrag() {		
		taskbarManager.OnInput(this.gameObject.name);
	}
	
    void OnMouseEnter()
    {
        _OnHover = true;
//        audioEffect.PlayOnecWithOutStop(sound_Hover);
        this.transform.localScale = originalScale * 1.1f;
    }

    void OnMouseExit()
    {
        _OnHover = false;
        this.transform.localScale = originalScale;
    }
}
