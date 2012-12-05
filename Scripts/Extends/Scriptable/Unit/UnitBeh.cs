using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBeh : ScriptableObject {

    public System.TimeSpan TimeTraining;

	// Use this for initialization
    void OnEnable() {
        Debug.Log("UnitBeh :: Starting... ");
	}
	
	// Update is called once per frame
	void OnDestroy() {
        Debug.Log("UnitBeh :: OnDestroy");
    }
}

public class GroupOFUnitBeh {

    public List<string> unitName = new List<string>();
    public List<int> member = new List<int>();
	
	public GroupOFUnitBeh() {
		Debug.Log("GroupOFUnitBeh");
	}
}
