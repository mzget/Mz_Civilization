using UnityEngine;
using System.Collections;

public class Mz_SettingManager : MonoBehaviour {

    public enum SettingUIState { normal = 0, showSetting, };
    private SettingUIState CurrentUIState;
	
	private string language = "En";
    private Rect settingGroup_Rect;
    private Rect setting_Rect;

    public GUISkin guiSkin;
    private GUIStyle settingIcon_Style;
    private GUIStyle toggleScreen_Style;


	// Use this for initialization
	void Start () {
        settingIcon_Style = guiSkin.customStyles[4];
        toggleScreen_Style = guiSkin.customStyles[5];
		
		InitializeLanguage();
	}
	
	private void InitializeLanguage() {
		if(Main.CurrentAppLanguage == Main.AppLanguage.defualt_En) {
			language = "En";
		}
		else if(Main.CurrentAppLanguage == Main.AppLanguage.Thai) {
			language = "Th";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        settingGroup_Rect = new Rect(Main.FixedGameWidth - 128, 0, 128, 34);
        setting_Rect = new Rect(Main.FixedGameWidth - 35, 1, 32, 32);


        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.FixedGameWidth, Screen.height / Main.FixedGameHeight, 1));

        #region Setting Button and relative mechanism.

        if (GUI.Button(setting_Rect, GUIContent.none, settingIcon_Style))
        {
            if (CurrentUIState == SettingUIState.showSetting)
                CurrentUIState = SettingUIState.normal;
            else if (CurrentUIState == SettingUIState.normal)
                CurrentUIState = SettingUIState.showSetting;
        }

        if (CurrentUIState == SettingUIState.showSetting)
        {
            this.DrawShowSettingUI();
        }

        #endregion
    }

    private void DrawShowSettingUI()
    {
        GUI.BeginGroup(settingGroup_Rect, GUIContent.none, GUI.skin.box);
        {
            if (GUI.Button(new Rect(2, 4, 32, 25), new GUIContent("", "Toggle Screen"), toggleScreen_Style))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
            else if (GUI.Button(new Rect(36, 4, 32, 25), this.language))
            {
				
            }
        }
        GUI.EndGroup();
    }
}
