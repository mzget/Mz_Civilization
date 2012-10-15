using UnityEngine;
using System.Collections;

public class Mz_BaseScene : MonoBehaviour {

    public class ResourcePathName {
        public const string PathOfGUI_PREFABS = "GUI_objects/";
    }

    public enum ScenesInstance { 
        LoadingScreen = 0,
        Startup = 1,
        MainMenu,
        Town,
    }
    //public static ScenesInstance SceneInstance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
