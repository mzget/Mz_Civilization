using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaravanBeh : ScriptableObject {
	
//	public static int Caravan_Level = 0;	
	
	public enum CaravanBehState { idle = 0, traveling = 1, }
    public CaravanBehState currentCaravanState;
	
	// Use this for initialization
	public void OnEnable () {
		Debug.Log("OnEnable CaravanBeh");	
	}
}
