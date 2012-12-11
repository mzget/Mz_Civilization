using UnityEngine;
using System.Collections;

public class Mz_LoadingScreen : MonoBehaviour
{
    public Texture2D background_images;
    Rect viewport_rect = new Rect(0, 0, Screen.width, Screen.height);

	public static string TargetSceneName = string.Empty;

	private AsyncOperation async;
	
	
	void Awake() {
		Resources.UnloadUnusedAssets();
	}
	
    IEnumerator Start()
    {
		yield return new WaitForSeconds(.01f);
		Application.LoadLevel(TargetSceneName);
		
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
	
//		async = Application.LoadLevelAsync(sceneName);
//		
//		if(Application.isLoadingLevel) {
//			while(async.isDone == false) {
//				yield return 0;
//			}
//		}
		
#endif	
    }

    void Update()
    {

    }

    void OnGUI()
    {
        //<!-- Draw background images.
        GUI.DrawTexture(viewport_rect, background_images, ScaleMode.ScaleToFit);
     
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.FixedGameWidth, Screen.height / Main.FixedGameHeight, 1));

        //GUI.DrawTexture(new Rect(0, 0, Main.GAMEWIDTH, Main.GAMEHEIGHT), background);
		
		GUI.skin.box.fontSize = 32;
		GUI.skin.box.alignment = TextAnchor.MiddleCenter;
//        float process = async.progress * 100f;
        GUI.Box(new Rect(Main.FixedGameWidth / 2 - 200, Main.FixedGameHeight / 2 - 40, 400, 80), "Loading..." /*+ process.ToString("F1") + " %"*/, GUI.skin.box);
    }
}