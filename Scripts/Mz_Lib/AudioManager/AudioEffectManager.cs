using UnityEngine;
using System.Collections;

public class AudioEffectManager : MonoBehaviour {
	
	public AudioSource alternativeEffect_source;
	
    public AudioClip buttonDown_Clip;
	public AudioClip buttonUp_Clip;
	public AudioClip buttonHover_Clip;
	public AudioClip correct_Clip;
	public AudioClip wrong_Clip;
	
	
	void Awake() {
        //GameObject source = Instantiate(new GameObject()) as GameObject;
        //source.name = "alternativeEffect";
        //source.transform.parent = this.gameObject.transform;
        //source.gameObject.AddComponent<AudioSource>();
		
        //if(alternativeEffect_source == null)
        //    alternativeEffect_source = source.gameObject.GetComponent<AudioSource>();
	}
	
	// Use this for initialization
	void Start () {
        audio.volume = 1;
        //alternativeEffect_source.volume = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void PlayOnecSound(AudioClip sound) {
        this.audio.Stop();
		this.audio.PlayOneShot(sound);
	}
	
	public void PlayOnecWithOutStop(AudioClip sound) {
        this.audio.PlayOneShot(sound);
	}
}
