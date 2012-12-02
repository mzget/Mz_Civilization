using UnityEngine;
using System.Collections;

public class TroopsActivity : ScriptableObject {

	public enum TroopsStatus
	{
		LeaveOfTown = 0, BackToTown = 1,
	} 
	public TroopsStatus currentTroopsStatus;
    public AICities targetCity;


	// Use this for initialization
	public TroopsActivity() {
    
    }
	
	public void OnDestroy() {
    
    }
}
