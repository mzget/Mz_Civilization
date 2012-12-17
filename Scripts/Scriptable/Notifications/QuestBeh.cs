using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class QuestBeh {
	// Quest name.
	// Quest Objectives.
	// Rewards.

	public string QuestName;
	public string QuestObjectives;
	public bool _IsComplete = false;
    public List<GameMaterialData> reward;
    //public Texture2D reward_2 = null;
    //public Texture2D reward_3 = null;
   

	// Use this for initialization
    public QuestBeh() { }
	
	void OnDestroy() {	}
}
