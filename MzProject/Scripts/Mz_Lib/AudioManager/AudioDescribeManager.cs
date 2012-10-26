using UnityEngine;
using System.Collections;

public class AudioDescribeManager : MonoBehaviour {

    public const string PathOfAudioDescriptions = "AudioClips/DescriptionClips/";
    public const string PathOfAudioExtra = "AudioClips/extra/";

    public AudioClip selectedGameplayLevel_Clip;
    //<!-- Mainmenu.
    public AudioClip gameStarting_Clip;
    public AudioClip newgame_Clip;
    //<!-- Dressing room.
    public AudioClip dressingRoomStarting_Clip;
    public AudioClip formal_clip;
    public AudioClip casual_Clip;
    public AudioClip sport_Clip;
    public AudioClip sleep_Clip;
    public AudioClip hair_Clip;
    public AudioClip dress_Clip;
    public AudioClip shirt_Clip;
    public AudioClip pants_Clip;
    public AudioClip skirt_Clip;
    public AudioClip shoes_Clip;
    //<!-- Claire house.
    public AudioClip claireHouse_Clip;
    public AudioClip dressingRoom_Clip;
    //<!-- City.
    public AudioClip pushNotification_Clip;
    public AudioClip introCity_Clip;
    public AudioClip hall;
    public AudioClip house_Clip;
    public AudioClip fitness_Clip;
    public AudioClip studio_Clip;
    //<!---- Fitness.
    public AudioClip yoga_Clip;
    public AudioClip yogaDes_Clip;
    public AudioClip treadmill_Clip;
    public AudioClip treadmillDes_Clip;
	//<!---- Studio.
	public AudioClip studioIntro_Clip;
	public AudioClip wallpaper_Clip;
	public AudioClip props_Clip;
	public AudioClip clothes_Clip;
	public AudioClip postures_Clip;
	public AudioClip capture_Clip;
	public AudioClip album_Clip;
	//<!---- Hall
	public AudioClip danceTutor_0001;
	public AudioClip danceTutor_0002;
	public AudioClip catwalkTutor_0001;
	public AudioClip catwalkTutor_0002;

    //<!----------- Sound Appreciated.
    public AudioClip levelUp_Clip;
	//<!------ Warnning -------->>



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void PlayOnecSound(AudioClip sound)
    {
        this.audio.Stop();
        this.audio.PlayOneShot(sound);
    }

    public void PlayOnecWithOutStop(AudioClip sound)
    {
        this.audio.PlayOneShot(sound);
    }
}
