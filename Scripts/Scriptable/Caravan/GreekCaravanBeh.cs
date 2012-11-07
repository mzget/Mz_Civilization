using UnityEngine;
using System.Collections;

public class GreekCaravanBeh : ScriptableObject {
	
	public MarketBeh marketInstance { get; set; }
	public GameResource goods { get; set; }
	
	private int timeToTravel = 3;
	private int dayTravalCounter = 0;

	
	// Use this for initialization
	public GreekCaravanBeh() {
		Debug.Log("Starting :: GreekCaravanBeh");
	}

	public void Traveling ()
	{
		marketInstance.SendingCaravanEvent += Handle_SendingCaravanEvent;
	}

	void Handle_SendingCaravanEvent (object sender, System.EventArgs e)
	{
		dayTravalCounter += 1;

		if (dayTravalCounter == timeToTravel) {
			StoreHouse.sumOfArmor += goods.Armor;
			StoreHouse.sumOfWeapon += goods.Weapon;
			Destroy(this);
		}
	}

	void OnDestroy () {
		marketInstance.SendingCaravanEvent -= Handle_SendingCaravanEvent;
	}
}
