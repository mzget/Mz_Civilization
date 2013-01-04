using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class UnitBeh {
	public string name = string.Empty;
    public TimeSpan TimeTraining;
    public UnitAbility ability;


	// Use this for initialization
    public UnitBeh() {
        Debug.Log("UnitBeh :: Starting... ");
	}
	
	// Update is called once per frame
	void OnDestroy() {
        Debug.Log("UnitBeh :: OnDestroy");
    }
}
