using UnityEngine;
using System;
using System.Collections;

public class TroopsActivity {

    public enum TroopsStatus { LeaveOfTown = 0, BackToTown = 1, Pillage = 2, Conquer = 3, };
	public TroopsStatus currentTroopsStatus;

    public enum ResultOfBattle { None, Win, Lose, };
    internal ResultOfBattle battleResult;

    public AICities targetCity;

	public TimeSpan timeToTravel;
	public TimeSpan RemainingTime;
	
    public DateTime startTime;
	public GroupOFUnitBeh groupOfUnitBeh;
    
    public int totalCapacity;
    internal GameMaterialDatabase reward;

    public float attackBonus;
    public int totalAttackScore;

	// Use this for initialization
	public TroopsActivity() { }
	
	public void OnDestroy() { }
}
