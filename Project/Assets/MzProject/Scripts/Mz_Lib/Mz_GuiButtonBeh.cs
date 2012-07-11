using UnityEngine;
using System.Collections;

public class Mz_GuiButtonBeh : MonoBehaviour {

    private AudioEffectManager audioEffect;    

	// Use this for initialization
    void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {

	}

    private IEnumerator OnMouseDown ()
	{
        audioEffect.PlaySoundClickButton();

		this.OnMouseEnter ();
		yield return new WaitForSeconds (0.1f);
		this.OnMouseExit ();
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
