using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class Startup : MonoBehaviour {

    public Texture2D mzLogo_Icon;

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(1);
		
        Application.LoadLevel(Scenes.ScenesInstance.MainMenu.ToString());
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI() {
        GUI.DrawTexture(new Rect(Screen.width / 2 - (mzLogo_Icon.width / 2), Screen.height / 2 - (mzLogo_Icon.height / 2), 
            mzLogo_Icon.width, mzLogo_Icon.height), mzLogo_Icon, ScaleMode.ScaleToFit);
    }
}
