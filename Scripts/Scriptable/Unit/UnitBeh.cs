using UnityEngine;
using System.Collections;

public class UnitBeh : ScriptableObject {

    public System.TimeSpan TimeTraining;

	// Use this for initialization
    void OnEnable() {
		Debug.Log("Starting... UnitBeh");
	}
	
	// Update is called once per frame
	void OnDestroy() { }
}
