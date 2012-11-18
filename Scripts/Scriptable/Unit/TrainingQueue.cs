using UnityEngine;
using System.Collections;


public class TrainingQueue : ScriptableObject
{
    public UnitBeh Unit { get; set; }
    public int Number { get; set; }
    public System.TimeSpan RemainingTime;
	public System.TimeSpan ToTalQueueTime;
    public System.DateTime startTime;

    void OnEnable() {
		Debug.Log("Starting... :: TrainingQueue");
	}

    void OnDestroy() {
        Debug.Log("TrainingQueue :: OnDestroing...");
    }
};
