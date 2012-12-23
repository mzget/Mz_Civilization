using UnityEngine;
using System.Collections;

public class AudioEffectManager : MonoBehaviour {
	
    public AudioSource alternativeEffect_source;
	
    public AudioClip buttonDown_Clip;
	public AudioClip buttonUp_Clip;
	public AudioClip buttonHover_Clip;
//	public AudioClip correct_Clip;
//	public AudioClip wrong_Clip;
//	public AudioClip tick_Clip;
//	public AudioClip mutter_clip;
    public AudioClip displayUI_clip;
    public AudioClip storageCart_clip;
	
	
	void Awake() {
		GameObject source = new GameObject("alternativeEffect", typeof(AudioSource));
		source.transform.parent = this.gameObject.transform;
		
		if(alternativeEffect_source == null)
			alternativeEffect_source = source.gameObject.GetComponent<AudioSource>();
	}
	
	// Use this for initialization
	void Start () {
        audio.volume = 1;
		alternativeEffect_source.volume = 1;
	}
	
	public void PlayOnecSound(AudioClip sound) {
        this.audio.Stop();
		this.audio.PlayOneShot(sound);
	}
	
	public void PlayOnecWithOutStop(AudioClip sound) {
        this.alternativeEffect_source.PlayOneShot(sound);
	}
}
