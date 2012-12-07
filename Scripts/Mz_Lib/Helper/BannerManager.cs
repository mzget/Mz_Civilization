using UnityEngine;
using System.Collections;

public class BannerManager : MonoBehaviour {

	private ADBannerView banner = null;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator ShowBanner() {
		while (!banner.loaded && banner.error == null)
			yield return null;
		
		if (banner.error == null)
			banner.Show();
		else banner = null;
	}
	
	void OnGUI() {
		GUI.enabled = (banner == null ? true : false);
		
		if (GUILayout.Button("Show Banner")) {
			banner = new ADBannerView();
			banner.autoSize = true;
			banner.autoPosition = ADPosition.Bottom;
			StartCoroutine(ShowBanner());
		}
	}
}
