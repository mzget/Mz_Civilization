using UnityEngine;
using System.Collections;

public class LordsHouse : BuildingBeh {
	
	public const string BuildingName = "Lords house";

	private int[] maxDweller = new int[5] { 10, 20, 30, 40, 50, };
	private int currentMaxDweller;


	protected override void Awake ()
	{
		base.Awake ();
		base.sprite = this.gameObject.GetComponent<tk2dSprite>();
		
		this.name = BuildingName;
		base.buildingType = BuildingType.general;
		base.buildingTimeData = new BuildingsTimeData(buildingType);
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		base._canMovable = true;
		constructionArea = new TileArea() { x = 3, y = 8, numSlotWidth = 2, numSlotHeight = 3 };
		this.transform.position = Tile.GetAreaPosition(constructionArea);
		originalPosition = this.transform.position;
        Tile.SetNoEmptyArea(constructionArea);
		
		this.InitializeTexturesResource();
		this.CalculationCurrentDweller();
		
		base.NotEnoughResource_Notification_event += Handle_NotEnoughResource_Notification_event;;
		
//		if(_IsAddEvent == false) {
//			sceneController.dayCycle_Event += Handle_dayCycle_Event;
//			_IsAddEvent = true;
//		}
	}

	void Handle_NotEnoughResource_Notification_event (object sender, NoEnoughResourceNotificationArg e)
	{
		base.notificationText = e.notification_args;
	}

	private void CalculationCurrentDweller()
	{
		for (int i = 1; i <= maxDweller.Length; i++) {
			if (base.Level == i) {
				this.currentMaxDweller = this.maxDweller[i - 1];
				HouseBeh.CalculationSumOfPopulation();
				return;
			}
		}
		
		Debug.Log("CalculationCurrentDweller");
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
}
