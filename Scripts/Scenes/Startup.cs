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
        StartCoroutine(AutomaticSetup_QualitySetting());

        yield return new WaitForSeconds(1);
		
        Application.LoadLevel(Mz_BaseScene.ScenesInstance.MainMenu.ToString());
	}

    private IEnumerator AutomaticSetup_QualitySetting()
    {
#if UNITY_IPHONE

		if(iPhone.generation == iPhoneGeneration.iPad1Gen ||
			iPhone.generation == iPhoneGeneration.iPhone3G || iPhone.generation == iPhoneGeneration.iPhone3GS) {
			QualitySettings.SetQualityLevel(0);	
		    Application.targetFrameRate = 30;
		}
		else {
			QualitySettings.SetQualityLevel(1);
		    Application.targetFrameRate = 60;
		}

#elif UNITY_ANDROID

        if (Screen.height < Main.HD_HEIGHT)
        {
            QualitySettings.SetQualityLevel(0);
            Application.targetFrameRate = 30;
        }
        else
        {
            QualitySettings.SetQualityLevel(1);
            Application.targetFrameRate = 60;
        }

#else 

		QualitySettings.SetQualityLevel(3);
		Application.targetFrameRate = 60;

#endif

        yield return null;
    }
	
	// Update is called once per frame
	new void Update () { }

    private void OnGUI() {
        GUI.DrawTexture(new Rect(Screen.width / 2 - (mzLogo_Icon.width / 2), Screen.height / 2 - (mzLogo_Icon.height / 2), 
            mzLogo_Icon.width, mzLogo_Icon.height), mzLogo_Icon);
    }
}
