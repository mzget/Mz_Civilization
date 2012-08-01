using UnityEngine;
using System.Collections;

public class Mz_LoadingScreen : MonoBehaviour
{
    public Texture2D background;

	private static string sceneName = string.Empty;
	public static string SceneName {
		get{return sceneName;}
		set{sceneName = value;}
	}

	private AsyncOperation async;
	
	
	void Awake() {
		Resources.UnloadUnusedAssets();
	}
	
    IEnumerator Start()
    {
		yield return new WaitForSeconds(.1f);
		Application.LoadLevel(sceneName);
		
#if UNITY_FLASH || UNITY_WEBPLAYER
//        async =  Application.LoadLevelAsync(sceneName);
//
//        if (Application.isLoadingLevel) {
//            while (!async.isDone)
//            {
//                yield return 0;
//            }
//        }
#endif
		
#if UNITY_IPHONE || UNITY_ANDROID
	
		async = Application.LoadLevelAsync(sceneName);
		
		if(Application.isLoadingLevel) {
			while(async.isDone == false) {
				yield return 0;
			}
		}
		
#endif	
    }

    void Update()
    {

    }

    void OnGUI()
    {
        GUI.depth = 0;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GameWidth, Screen.height / Main.GameHeight, 1));

        GUI.DrawTexture(new Rect(0, 0, Main.GameWidth, Main.GameHeight), background);
		
		GUI.skin.box.fontSize = 32;
		GUI.skin.box.alignment = TextAnchor.MiddleCenter;
//        float process = async.progress * 100f;
        GUI.Box(new Rect(Main.GameWidth / 2 - 200, Main.GameHeight / 2 - 40, 400, 80), "Loading..." /*+ process.ToString("F1") + " %"*/, GUI.skin.box);
    }
}