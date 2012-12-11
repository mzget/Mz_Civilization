using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class Startup : Mz_BaseScene {

    public Texture2D mzLogo_Icon;

	// Use this for initialization
    IEnumerator Start()
    {
        Mz_OnGUIManager.CalculateViewportScreen();

        Application.runInBackground = true;
        QualitySettings.SetQualityLevel(3);

        yield return new WaitForSeconds(1);
		
        Application.LoadLevel(Mz_BaseScene.ScenesInstance.MainMenu.ToString());
	}
	
	// Update is called once per frame
	new void Update () { }

    private new void OnGUI() {
        GUI.DrawTexture(new Rect(Screen.width / 2 - (mzLogo_Icon.width / 2), Screen.height / 2 - (mzLogo_Icon.height / 2), 
            mzLogo_Icon.width, mzLogo_Icon.height), mzLogo_Icon);
    }
}
