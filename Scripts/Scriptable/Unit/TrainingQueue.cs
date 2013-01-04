using UnityEngine;
using System.Collections;

[System.Serializable]
public class TrainingQueue
{
    public UnitBeh unitBeh { get; set; }
    public int number { get; set; }
    public System.TimeSpan remainingTime;
	public System.TimeSpan totalQueueTime;
    public System.DateTime startTime;

    public TrainingQueue() {
		Debug.Log("Starting... :: TrainingQueue");
	}

    void OnDestroy() {
        Debug.Log("TrainingQueue :: OnDestroing...");
    }
};
