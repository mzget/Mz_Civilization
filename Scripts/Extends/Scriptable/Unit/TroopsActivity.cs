using UnityEngine;
using System;
using System.Collections;

public class TroopsActivity : ScriptableObject {

	public enum TroopsStatus {
		LeaveOfTown = 0, BackToTown = 1, Pillage = 2, Conquer = 3,
	}
	public TroopsStatus currentTroopsStatus;
    public AICities targetCity;

	public TimeSpan timeToTravel;
	public TimeSpan RemainingTime;
	
    public DateTime startTime;
	public GroupOFUnitBeh groupUnits;

	// Use this for initialization
	public TroopsActivity() {
    
    }
	
	public void OnDestroy() {
    
    }
}
