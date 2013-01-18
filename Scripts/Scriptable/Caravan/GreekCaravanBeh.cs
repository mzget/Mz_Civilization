using UnityEngine;
using System.Collections;

[System.Serializable]
public class GreekCaravanBeh {
	
	public MarketBeh marketInstance { get; set; }
	public GameMaterialDatabase goods { get; set; }
	
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
            Debug.Log("GreekCaravanBeh reach to this town.");

			StoreHouse.sumOfArmor += goods.Armor;
			StoreHouse.sumOfWeapon += goods.Weapon;
            this.OnDestroy();
		}
	}

	void OnDestroy () {
		marketInstance.SendingCaravanEvent -= Handle_SendingCaravanEvent;
        //marketInstance.CaravanArriveToVillage();
	}
}
