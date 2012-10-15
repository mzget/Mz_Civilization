using UnityEngine;
using System.Collections;

public class Mz_GuiButtonBeh : MonoBehaviour {
	
	private GUI_Manager GUI_manager;
    private AudioEffectManager audioEffect; 
	
	public GUITexture guitexture;

	// Use this for initialization
    void Start ()
	{
		GUI_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GUI_Manager>();

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
	}

    private IEnumerator OnMouseDown ()
	{
//        audioEffect.PlaySoundClickButton();

		this.OnMouseEnter ();
		yield return new WaitForSeconds (0.1f);
		this.OnMouseExit ();
		
		GUI_manager.OnInput(this.gameObject.name);
	}
	
	void OnMouseDrag() {		
		GUI_manager.OnInput(this.gameObject.name);
	}
	
    void OnMouseEnter()
    {
//        audioEffect.PlayOnecWithOutStop(sound_Hover);
        this.transform.localScale = Vector3.one * 1.1f;
    }

    void OnMouseExit()
    {
        this.transform.localScale = Vector3.one;
    }
}
