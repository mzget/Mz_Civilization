using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GroupCaravan : ScriptableObject {
    
    public MarketBeh MarketInstance { get; set; }
    public List<CaravanBeh> GroupList = new List<CaravanBeh>();
	
    private int travelingDay = 0;
    private int travelDayCounter = 0;
	private int collectGold = 0;
	

    public void OnEnable() {
    	Debug.Log("OnEnable GroupCaravan");
    }

	public void TravelingCaravan(int dayToTarget, int p_collectGold) {
		this.travelingDay = dayToTarget;
		this.collectGold = p_collectGold;
		
		MarketInstance.SendingCaravanEvent += HandleMarketSendingCaravanEvent;
		
		foreach(CaravanBeh caravan in GroupList)
			caravan.currentCaravanState = CaravanBeh.CaravanBehState.traveling;
	}

	public void HandleMarketSendingCaravanEvent (object sender, System.EventArgs e) {
		travelDayCounter += 1;
		
		//<!-- reach to target.
		if(travelDayCounter == travelingDay) 
		{
			StoreHouse.sumOfGold += collectGold;
			
			MarketInstance.SendingCaravanEvent -= HandleMarketSendingCaravanEvent;
		
			foreach(CaravanBeh caravan in GroupList) {
				caravan.currentCaravanState = CaravanBeh.CaravanBehState.idle;
				MarketInstance.CheckingIdleCaravan();
			}
		
			Dispose();
			MarketInstance.TradingMechanism();
			
			Debug.Log("Group caravan reach to target.");
        }
	}
	
	private void Dispose() {
		collectGold = 0;
		travelingDay = 0;
		travelDayCounter = 0;
	}
}