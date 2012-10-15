using UnityEngine;
using System.Collections;

public class AudioEffectManager : MonoBehaviour {

    public AudioClip sound_ClickButton;
	public AudioClip sound_ClickPop;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySoundClickButton() {
        audio.Stop();
        audio.PlayOneShot(sound_ClickButton);
    }
	
	public void PlaySoundClickPop() {
		audio.Stop();
		audio.PlayOneShot(sound_ClickPop);
	}
}
