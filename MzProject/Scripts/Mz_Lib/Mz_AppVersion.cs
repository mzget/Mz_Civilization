using UnityEngine;
using System.Collections;

public class Mz_AppVersion : MonoBehaviour {

	public enum AppVersion {Free = 0, Pro = 1};
	public AppVersion appVersion;
	public static AppVersion getAppVersion;
	
	public Transform backgroundObj;
	public Transform logoSprite;
	
	// Use this for initialization
	void Start () {
		getAppVersion = appVersion;
		
		Mz_ResizeScale.CalculationScale(backgroundObj.transform);
		Mz_ResizeScale.CalculationScale(logoSprite.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
