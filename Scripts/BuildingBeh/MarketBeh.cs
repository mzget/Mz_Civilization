using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarketBeh : BuildingBeh {

    public const string PathOfTribes_Texture = "Textures/Tribes_Icons/";
    //<!--- Constant price of resource.
    public const int pricePerUnitOf_Food = 8;
    public const int pricePerUnitOf_Wood = 15;
	public const int pricePerUnitOf_CopperIngots = 20;
	public const int pricePerUnitOf_StoneBlocks = 18;
    public const int pricePerUnitOf_Armor = 40;
    public const int pricePerUnitOf_Weapon = 50;


    public static GameResource[] RequireResource = new GameResource[10] {
        new GameResource() {Food = 120, Wood = 80, Stone = 80, Employee = 10},
        new GameResource() {Food = 240, Wood = 160, Gold = 160, Employee = 20},
        new GameResource() {Food = 480, Wood = 320, Gold = 320, Employee = 30},
        new GameResource() {Food = 960, Wood = 640, Gold = 640, Employee = 40},
        new GameResource() {Food = 1920, Wood = 1280, Gold = 1280, Employee = 50},
        new GameResource() {Food = 3840, Wood = 2560, Gold = 2560, Employee = 60},
        new GameResource() {Food = 7680, Wood = 5120, Gold = 5120, Employee = 70},
        new GameResource() {Food = 14000, Wood = 10000, Gold = 10000, Employee = 80},
        new GameResource() {Food = 25000, Wood = 20000, Gold = 20000, Employee = 90},
        new GameResource() {Food = 50000, Wood = 40000, Gold = 40000, Employee = 100},
	};

    public const string BuildingName = "Market";
    private const string Description_TH = "���ҧ��н֡���ͧ�����ҹ ���͢������š����¹�Թ��� \n �Ԩ����оѲ�ҡ�䡡�õ�Ҵ";
    private const string Description_EN = "The Market can be built to buy and sell resources for gold. Upgrade market to train more Caravan.";
    public static string CurrentDescription {
        get {
            string temp = "";
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
				temp = Description_EN;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
				temp = Description_TH;
            return temp;
        }
    }

    private const string GreekDescription_TH = "";
    private const string GreekDescription_EN = "Greece: ancient land of beauty, reason, passion and war.";
    private static string CurrentGreekDescription {
        get {
            string temp = "";
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En) 
                temp = GreekDescription_EN;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai) 
                temp = GreekDescription_TH;
            return temp;
        }
    }
	
    public const string TH_persianDescribe = "";
    public const string EN_persianDescribe = "Persian one of the most powerful empires in ancient times.";
    private static string PersiaDescribe {
        get {
            string temp_data = "";
			
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En) 
                temp_data = EN_persianDescribe;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                temp_data = TH_persianDescribe;
			
            return temp_data;
        }
    }

    public List<CaravanBeh> caravanList;
	public List<CaravanBeh> idleCaravanList;
	public GroupCaravan caravan_group; 
	public static int carryAbility = 32;

    public Texture2D GreekIcon_Texture;
    public Texture2D EgyptianIcon_Texture;
    public Texture2D PersianIcon_Texture;
    public Texture2D CelticIcon_Texture;    

    private GUIStyle goods_Label_style;


	
	/// Awake this instance.
    protected override void Awake() {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = MarketBeh.BuildingName;
        base.buildingType = BuildingBeh.BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);


        new_descriptionGroupRect = new Rect(base.descriptionGroup_Rect.x, base.descriptionGroup_Rect.y, base.descriptionGroup_Rect.width - 16, base.descriptionGroup_Rect.height);
        importGroupRect = new Rect(10, 40, new_descriptionGroupRect.width - 20, 110);
        exportGroupRect = new Rect(10, 160, new_descriptionGroupRect.width - 20, 110);
        selectedGoods1Group_rect = new Rect(importGroupRect.width - 100, 35, 100, 30);
        selectedGoods2Group_rect = new Rect(importGroupRect.width - 100, 70, 100, 30);

        goods_Label_style = new GUIStyle(standard_Skin.textField);
        goods_Label_style.imagePosition = ImagePosition.ImageLeft;
        goods_Label_style.alignment = TextAnchor.MiddleCenter;
    }

	// Use this for initialization
	IEnumerator Start () {
        StartCoroutine(LoadtexturesResources());

        base.NotEnoughResource_Notification_event += MarketBeh_NotEnoughResourceNotification_event;
        stageManager.dayCycle_Event += this.ReachDayCycle;
		ReachDayCycle(this, System.EventArgs.Empty);
		
        yield return 0;
    }

    private void MarketBeh_NotEnoughResourceNotification_event(object sender, NoEnoughResourceNotificationArg e)
    {
        base.notificationText = e.notification_args;
    }
	
    protected override void ReachDayCycle(object sender, System.EventArgs e)
    {
        base.ReachDayCycle(sender, e);

        CheckingIdleCaravan();
		if(SendingCaravanEvent != null)
			SendingCaravanEvent(this, System.EventArgs.Empty);
    }

    IEnumerator LoadtexturesResources()
    {
        //<!-- Load textures.
        buildingIcon_Texture = Resources.Load(BuildingBeh.BuildingIcons_TextureResourcePath + "Market", typeof(Texture2D)) as Texture2D;
        GreekIcon_Texture = Resources.Load(PathOfTribes_Texture + "Greek", typeof(Texture2D)) as Texture2D;
        PersianIcon_Texture = Resources.Load(PathOfTribes_Texture + "Persian", typeof(Texture2D)) as Texture2D;

        yield return 0;
    }

    public override void InitializeBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializeBuildingBeh(p_buildingState, p_indexPosition, p_level);

        BuildingBeh.MarketInstances.Add(this);
        for (int i = 0; i < base.Level; i++) {
            caravanList.Add(ScriptableObject.CreateInstance<CaravanBeh>());
//            caravanList.Add(new CaravanBeh());
        }
    }

    #region <!--- Building Processing.

    public override void OnBuildingProcess(BuildingBeh obj)
    {
        base.OnBuildingProcess(obj);
    }
    protected override void CreateProcessBar(BuildingBeh.BuildingStatus buildingState)
    {
        base.CreateProcessBar(buildingState);
    }
    protected override void BuildingProcessComplete(BuildingBeh obj)
    {
        base.BuildingProcessComplete(obj);

        Destroy(base.processbar_Obj_parent);

        if (this.currentBuildingStatus != BuildingBeh.BuildingStatus.none) {
            caravanList.Add(ScriptableObject.CreateInstance<CaravanBeh>());
            CheckingIdleCaravan();
                
            this.currentBuildingStatus = BuildingBeh.BuildingStatus.none;
        }
    }

    #endregion

    protected override void ClearStorageData()
    {
        base.ClearStorageData();
		
		stageManager.dayCycle_Event -= this.ReachDayCycle;
        base.NotEnoughResource_Notification_event -= this.MarketBeh_NotEnoughResourceNotification_event;

        BuildingBeh.MarketInstances.Remove(this);
		caravanList.Clear();
    }

    public void CheckingIdleCaravan() {		
		//<!-- Check free caravan.
        idleCaravanList.Clear();
		foreach(CaravanBeh caravan in caravanList) {
			if(caravan.currentCaravanState == CaravanBeh.CaravanBehState.idle) {
				idleCaravanList.Add(caravan);
            }
		}
    }
	private float CheckUsedCaravan(int var_a, int var_b) {	
		float usedCaravan = 0;	
		float slot_1_used = 0;
		float slot_2_used = 0;
		if(var_a > 0) {
			slot_1_used = var_a / carryAbility;
		}	
		if(var_b > 0) {
			slot_2_used = var_b / carryAbility;
		}
		
		usedCaravan = slot_1_used + slot_2_used;
		
		return usedCaravan;
	}
	
	/// <summary>
	/// Call by treading mechanism method.
	/// </summary>
	/// <returns>
	/// The collecting gold.
	/// </returns>
    private int CheckCollectingGold(int var_number_a, int var_price_a, int var_number_b, int var_price_b)
    {
        int collectGold = 0;
		int slot_1_price = 0;
		int slot_2_price = 0;
		if(var_number_a > 0) {
			slot_1_price = var_number_a * var_price_a;
		}	
		if(var_number_b > 0) {
			slot_2_price = var_number_b * var_price_b;
		}
		
		collectGold = slot_1_price + slot_2_price;
		
		return collectGold;
    }
	
	public event System.EventHandler SendingCaravanEvent;
    public void TradingMechanism()
    {        
		Debug.Log("TradingMechanism");
		
		if(caravanList.Count != 0) 
		{
			#region <!-- greek trading mechanism.
			
			if(_IsGreekTrading == true)
			{
				if(numberOf_CopperIngots > 0 || numberOf_StoneBlocks > 0)
				{
					int UsedCaravan = Mathf.CeilToInt(this.CheckUsedCaravan(numberOf_CopperIngots, numberOf_StoneBlocks));
					int getGold = this.CheckCollectingGold(numberOf_CopperIngots, pricePerUnitOf_CopperIngots, numberOf_StoneBlocks, pricePerUnitOf_StoneBlocks);
					
                    if (idleCaravanList.Count >= UsedCaravan)
                    {
						caravan_group = new GroupCaravan() {MarketInstance = this};
						
                        for (int i = 0; i < UsedCaravan; i++)
                        {
                            caravan_group.GroupList.Add(idleCaravanList[0]);
                            idleCaravanList.RemoveAt(0);
                        }
						//<!-- Remove resource form storehouse.
						StoreHouse.sumOfCopper -= numberOf_CopperIngots;
						StoreHouse.sumOfStone -= numberOf_StoneBlocks;
						//<!-- Sending caravan.
						caravan_group.TravelingCaravan(3, getGold);
						
                        Debug.Log("Sending " + UsedCaravan + " Cavavan, " + "collect gold = " + getGold);
                    }
                    else
                        _IsGreekTrading = false;
				}
				else
					_IsGreekTrading = false;
			}
			
			#endregion
			
			#region <!-- Persian Trading.
			
			if(_IsPersianTrading == true) 
			{
				if(numberOf_Food > 0 || numberOf_Wood > 0)
				{
					int UsedCaravan = Mathf.CeilToInt(this.CheckUsedCaravan(numberOf_Food, numberOf_Wood));
					int getGold = this.CheckCollectingGold(numberOf_Food, pricePerUnitOf_Food, numberOf_Wood, pricePerUnitOf_Wood);
					
                    if (idleCaravanList.Count >= UsedCaravan)
                    {
						caravan_group = new GroupCaravan() { MarketInstance = this };
						
                        for (int i = 0; i < UsedCaravan; i++)
                        {
                            caravan_group.GroupList.Add(idleCaravanList[0]);
                            idleCaravanList.RemoveAt(0);
                        }
						//<!-- Remove resource form storehouse.
						StoreHouse.sumOfFood -= numberOf_Food;
						StoreHouse.sumOfWood -= numberOf_Wood;
						//<!-- Sending caravan.
						caravan_group.TravelingCaravan(6, getGold);
						
                        Debug.Log("Sending " + UsedCaravan + " Cavavan, " + "collect gold = " + getGold);
                    }
                    else
                        _IsPersianTrading = false;
				}
				else
					_IsPersianTrading = false;
			}
			
			#endregion
		}
    }
	
	// Update is called once per frame
	void Update () {
		
	}	

    Rect new_descriptionGroupRect;
    Rect importGroupRect;
    Rect exportGroupRect;
    Rect selectedGoods1Group_rect;
    Rect selectedGoods2Group_rect;
    private Rect displayGoods_rect = new Rect(5, 35, 120, 30);
    private Rect displaySelectedNumberOfGoods_rect = new Rect(20, 3, 50, 24);
    private Rect selectedLeft_rect = new Rect(0, 7, 16, 16);
    private Rect selectedRight_rect = new Rect(74, 7, 16, 16);
    private Rect displayGoods_rect2 = new Rect(5, 70, 120, 30);
    private Rect displayPrice_rect = new Rect(130, 35, 80, 30);
    private Rect displayPrice_rect2 = new Rect(130, 70, 80, 30);

    protected override void CreateWindow(int windowID)
    {
        base.CreateWindow(windowID);

        GUI.Box(base.notificationBox_rect, base.notificationText, standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height),
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height * 4), false, false);
        {
            building_Skin.box.contentOffset = new Vector2(128, 38);
			
			this.DrawBuildingContent();
            this.DrawGreekTradeUI();
            this.DrawPersiaTradeUI();
        }
        GUI.EndScrollView();
    }

    private void DrawBuildingContent()
    {
        GUI.BeginGroup(base.background_Rect, GUIContent.none, building_Skin.box);
        {
            GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture, ScaleMode.ScaleToFit);
            GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

            #region <!--- description group.

            GUI.BeginGroup(new_descriptionGroupRect, CurrentDescription, building_Skin.textArea);
            {
                //<!-- Current Production rate.
				int nextNumberOfCaravan = caravanList.Count + 1;
                Rect IdleCaravan_rect = new Rect(currentJob_Rect.x, currentJob_Rect.y - currentJob_Rect.height -10, currentJob_Rect.width, currentJob_Rect.height);
                GUI.Label(IdleCaravan_rect, "Idle caravan : " + idleCaravanList.Count, base.job_style);
                GUI.Label(currentJob_Rect, "Current max caravan : " + caravanList.Count, base.job_style);
                GUI.Label(nextJob_Rect, "Next max caravan : " + nextNumberOfCaravan, base.job_style);

                //<!-- Requirements Resource.
                GUI.BeginGroup(update_requireResource_Rect);
                {
                    GUI.Box(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), base.stageManager.gui_Manager.food_icon), standard_Skin.box);
                    GUI.Box(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), base.stageManager.gui_Manager.wood_icon), standard_Skin.box);
                    GUI.Box(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), base.stageManager.gui_Manager.gold_icon), standard_Skin.box);
                    GUI.Box(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), base.stageManager.gui_Manager.employee_icon), standard_Skin.box);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            #endregion

            #region <!--- Upgrade Button mechanichm.

            bool enableUpgrade = false;
            if (base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level]))
                enableUpgrade = true;

            GUI.enabled = enableUpgrade;
            if (GUI.Button(base.upgrade_Button_Rect, new GUIContent("Upgrade")))
            {
                GameResource.UsedResource(RequireResource[Level]);

                base.currentBuildingStatus = BuildingBeh.BuildingStatus.onUpgradeProcess;
                base.OnUpgradeProcess(this);
                base.CloseGUIWindow();
            }
            GUI.enabled = true;

            #endregion

            #region <!--- Destruction button.

            GUI.enabled = this.CheckingCanDestructionBuilding();
            if (GUI.Button(destruction_Button_Rect, new GUIContent("Destruct")))
            {
                this.currentBuildingStatus = BuildingStatus.OnDestructionProcess;
                this.DestructionBuilding();
                base.CloseGUIWindow();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }
	
    //<<!-- Greek data field.
    private int numberOf_CopperIngots = 0;
    private int numberOf_StoneBlocks = 0;
    private int numberOf_Armor = 0;
    private int numberOf_Weapon = 0;
    public static bool _IsGreekTrading = false;
    private void DrawGreekTradeUI()
    {
        GUI.BeginGroup(new Rect(0, 1 * base.background_Rect.height, background_Rect.width, base.background_Rect.height), GUIContent.none, building_Skin.box);
        {
            GUI.DrawTexture(base.imgIcon_Rect, GreekIcon_Texture);
            GUI.Label(base.levelLable_Rect, "Greek", base.status_style);
			
            #region <!-- Trade button.
			
            Rect trade_button_rect = new Rect(base.levelLable_Rect.x, base.levelLable_Rect.y + base.levelLable_Rect.height + 10, base.levelLable_Rect.width, base.levelLable_Rect.height);
            Rect stop_button_rect = new Rect(base.levelLable_Rect.x, trade_button_rect.y + trade_button_rect.height + 2, base.levelLable_Rect.width, base.levelLable_Rect.height);

            GUI.enabled = !_IsGreekTrading;
            if (GUI.Button(trade_button_rect, "Trade"))
            {
                if (numberOf_CopperIngots < StoreHouse.sumOfCopper && numberOf_StoneBlocks < StoreHouse.sumOfStone)
                {
                    _IsGreekTrading = true;
                    TradingMechanism();
                }
            }
            GUI.enabled = true;
            GUI.enabled = _IsGreekTrading;
            if (GUI.Button(stop_button_rect, "Cancel"))
            {
                _IsGreekTrading = false;
                //this.SendingCaravanEvent -= caravan_group.HandleMarketSendingCaravanEvent;
            }
            GUI.enabled = true;
			
			#endregion
			
            //<!-- description group rect.
            GUI.BeginGroup(new_descriptionGroupRect, MarketBeh.CurrentGreekDescription, building_Skin.textArea);
            { 
                //<<!-- Import  group.
                GUI.BeginGroup(importGroupRect, GUIContent.none, standard_Skin.textArea);
                {
                    GUI.Label(new Rect(0, 0, 100, 24), "Purchase");
					
                    #region <!--- Copper ingots.
					
                    GUI.Box(displayGoods_rect, new GUIContent("Copper ingots", stageManager.gui_Manager.copper_icon), goods_Label_style); 
                    GUI.Box(displayPrice_rect, new GUIContent(pricePerUnitOf_CopperIngots.ToString(), stageManager.gui_Manager.gold_icon), goods_Label_style); 					
					GUI.BeginGroup(selectedGoods1Group_rect);
					{
	                    GUI.Box(displaySelectedNumberOfGoods_rect, numberOf_CopperIngots.ToString(), GUI.skin.textField);
						// <!--- Selected number of goods.
	                    if (GUI.Button(selectedLeft_rect, GUIContent.none, base.stageManager.gui_Manager.left_button_Style)) {
	                        if (numberOf_CopperIngots > 0)
	                            numberOf_CopperIngots -= 8;
	                    }
	                    else if (GUI.Button(selectedRight_rect, GUIContent.none, base.stageManager.gui_Manager.right_button_Style)) {
	                        if (numberOf_CopperIngots < carryAbility)
	                            numberOf_CopperIngots += 8;
	                    }
					}
					GUI.EndGroup();
					
                    #endregion		
					
                    #region <!--- Stone blocks.
					
                    GUI.Box(displayGoods_rect2, new GUIContent("Stone blocks", stageManager.gui_Manager.stone_icon), goods_Label_style); 
                    GUI.Box(displayPrice_rect2, new GUIContent(pricePerUnitOf_StoneBlocks.ToString(), stageManager.gui_Manager.gold_icon), goods_Label_style); 
					GUI.BeginGroup(selectedGoods2Group_rect);
					{
	                    GUI.Box(displaySelectedNumberOfGoods_rect, numberOf_StoneBlocks.ToString(), GUI.skin.textField);
	                    if (GUI.Button(selectedLeft_rect, GUIContent.none, base.stageManager.gui_Manager.left_button_Style)) {
	                        if (numberOf_StoneBlocks > 0)
								numberOf_StoneBlocks -= 8;
	                    }
	                    else if (GUI.Button(selectedRight_rect, GUIContent.none, base.stageManager.gui_Manager.right_button_Style)) {
	                        if (numberOf_StoneBlocks < carryAbility)
	                            numberOf_StoneBlocks += 8;
	                    }
					}
					GUI.EndGroup();
					
                    #endregion
                }
                GUI.EndGroup();
                //<<!-- Export group.
                GUI.BeginGroup(exportGroupRect, GUIContent.none, standard_Skin.textArea);
                {
                    GUI.Label(new Rect(0, 0, 100, 24), "Sell");
					
                    #region <!--- "Armor".
					
                    GUI.Box(displayGoods_rect, new GUIContent("Armor", stageManager.gui_Manager.armor_icon), goods_Label_style);
                    GUI.Box(displayPrice_rect, new GUIContent(pricePerUnitOf_Armor.ToString(), stageManager.gui_Manager.gold_icon), goods_Label_style); 					
                    GUI.BeginGroup(selectedGoods1Group_rect);
                    {
                        GUI.Box(displaySelectedNumberOfGoods_rect, numberOf_Armor.ToString(), GUI.skin.textField);
                        // <!--- Selected number of goods.
                        if (GUI.Button(selectedLeft_rect, GUIContent.none, base.stageManager.gui_Manager.left_button_Style))
                        {
                            if (numberOf_Armor > 0)
                                numberOf_Armor -= 8;
                        }
                        else if (GUI.Button(selectedRight_rect, GUIContent.none, base.stageManager.gui_Manager.right_button_Style))
                        {
                            if (numberOf_Armor < 64)
                                numberOf_Armor += 8;
                        }
                    }
                    GUI.EndGroup();
					
                    #endregion		
					
                    #region <!--- "Weapon".
					
                    GUI.Box(displayGoods_rect2, new GUIContent("Weapon", base.stageManager.gui_Manager.weapon_icon), goods_Label_style);
					GUI.Box(displayPrice_rect2, new GUIContent(pricePerUnitOf_Weapon.ToString(), base.stageManager.gui_Manager.gold_icon), goods_Label_style);
                    GUI.BeginGroup(selectedGoods2Group_rect);
                    {
                        GUI.Box(displaySelectedNumberOfGoods_rect, numberOf_Weapon.ToString(), GUI.skin.textField);
                        if (GUI.Button(selectedLeft_rect, GUIContent.none, base.stageManager.gui_Manager.left_button_Style))
                        {
                            if (numberOf_Weapon > 0)
                                numberOf_Weapon -= 8;
                        }
                        else if (GUI.Button(selectedRight_rect, GUIContent.none, base.stageManager.gui_Manager.right_button_Style))
                        {
                            if (numberOf_Weapon < 64)
                                numberOf_Weapon += 8;
                        }
                    }
                    GUI.EndGroup();
					
                    #endregion
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    //<!--- Persian data field.
	private int numberOf_Food = 0;
	private int numberOf_Wood = 0;
    private int numberOfMeats = 0;
    private int numberOfOliveOil = 0;
    public static bool _IsPersianTrading = false;
    private void DrawPersiaTradeUI()
    {
        GUI.BeginGroup(new Rect(0, 2 * base.background_Rect.height, background_Rect.width, base.background_Rect.height), GUIContent.none, building_Skin.box);
        {
            GUI.DrawTexture(base.imgIcon_Rect, PersianIcon_Texture);
            GUI.Label(base.levelLable_Rect, "Persian", base.status_style);
            
			#region <!-- Trade button.
			
            Rect trade_button_rect = new Rect(base.levelLable_Rect.x, base.levelLable_Rect.y + base.levelLable_Rect.height + 10, base.levelLable_Rect.width, base.levelLable_Rect.height);
            Rect stop_button_rect = new Rect(base.levelLable_Rect.x, trade_button_rect.y + trade_button_rect.height + 2, base.levelLable_Rect.width, base.levelLable_Rect.height);

            GUI.enabled = !_IsPersianTrading;
            if (GUI.Button(trade_button_rect, "Trade"))
            {
                _IsPersianTrading = true;
                TradingMechanism();
            }
            GUI.enabled = true;
            GUI.enabled = _IsPersianTrading;
            if (GUI.Button(stop_button_rect, "Cancel"))
            {
                _IsPersianTrading = false;
            }
            GUI.enabled = true;
			
			#endregion
			
            //<!-- description group rect.
            GUI.BeginGroup(new_descriptionGroupRect, MarketBeh.PersiaDescribe, building_Skin.textArea);
            {
                //<!-- Import  group.
                GUI.BeginGroup(importGroupRect, GUIContent.none, standard_Skin.textArea);
                {
                    GUI.Label(new Rect(0, 0, 100, 24), "Purchase");

                    #region <!--- Food.

                    GUI.Box(displayGoods_rect, new GUIContent("Food", stageManager.gui_Manager.food_icon), goods_Label_style);
                    GUI.Box(displayPrice_rect, new GUIContent(pricePerUnitOf_Food.ToString(), stageManager.gui_Manager.gold_icon), goods_Label_style);
                    GUI.BeginGroup(selectedGoods1Group_rect);
                    {
                        GUI.Box(displaySelectedNumberOfGoods_rect, numberOf_Food.ToString(), GUI.skin.textField);
                        if (GUI.Button(selectedLeft_rect, GUIContent.none, base.stageManager.gui_Manager.left_button_Style))
                        {
                            if (numberOf_Food > 0)
                                numberOf_Food -= 8;
                        }
                        else if (GUI.Button(selectedRight_rect, GUIContent.none, base.stageManager.gui_Manager.right_button_Style))
                        {
                            if (numberOf_Food < carryAbility)
                                numberOf_Food += 8;
                        }
                    }
                    GUI.EndGroup();

                    #endregion					
                    #region <!--- Wood.

                    GUI.Box(displayGoods_rect2, new GUIContent("Wood", stageManager.gui_Manager.wood_icon), goods_Label_style);
                    GUI.Box(displayPrice_rect2, new GUIContent(pricePerUnitOf_Wood.ToString(), stageManager.gui_Manager.gold_icon), goods_Label_style);
                    GUI.BeginGroup(selectedGoods2Group_rect);
                    {
                        GUI.Box(displaySelectedNumberOfGoods_rect, numberOf_Wood.ToString(), GUI.skin.textField);
                        if (GUI.Button(selectedLeft_rect, GUIContent.none, base.stageManager.gui_Manager.left_button_Style))
                        {
                            if (numberOf_Wood > 0)
                                numberOf_Wood -= 8;
                        }
                        else if (GUI.Button(selectedRight_rect, GUIContent.none, base.stageManager.gui_Manager.right_button_Style))
                        {
                            if (numberOf_Wood < carryAbility)
                                numberOf_Wood += 8;
                        }
                    }
                    GUI.EndGroup();

                    #endregion
                }
                GUI.EndGroup();
                //<!-- Export group.
                GUI.BeginGroup(exportGroupRect, GUIContent.none, standard_Skin.textArea);
                {
                    GUI.Label(new Rect(0, 0, 100, 24), "Sell");

                    #region <!--- "Meats".

                    GUI.Box(displayGoods_rect, new GUIContent("Meats"), standard_Skin.textField);
                    GUI.Box(displayPrice_rect, new GUIContent(numberOfMeats.ToString(), base.stageManager.gui_Manager.gold_icon), goods_Label_style);
                    GUI.BeginGroup(selectedGoods1Group_rect);
                    {
                        GUI.Box(displaySelectedNumberOfGoods_rect, numberOfMeats.ToString(), GUI.skin.textField);
                        // <!--- Selected number of goods.
                        if (GUI.Button(selectedLeft_rect, GUIContent.none, base.stageManager.gui_Manager.left_button_Style))
                        {
                            if (numberOfMeats > 0)
                                numberOfMeats -= 8;
                        }
                        else if (GUI.Button(selectedRight_rect, GUIContent.none, base.stageManager.gui_Manager.right_button_Style))
                        {
                            if (numberOfMeats < 64)
                                numberOfMeats += 8;
                        }
                    }
                    GUI.EndGroup();

                    #endregion					
                    #region <!--- "".

                    GUI.Box(displayGoods_rect2, new GUIContent(""), standard_Skin.textField);
                    GUI.Box(displayPrice_rect2, new GUIContent(numberOfOliveOil.ToString(), base.stageManager.gui_Manager.gold_icon), goods_Label_style);
                    GUI.BeginGroup(selectedGoods2Group_rect);
                    {
                        GUI.Box(displaySelectedNumberOfGoods_rect, numberOf_Weapon.ToString(), GUI.skin.textField);
                        if (GUI.Button(selectedLeft_rect, GUIContent.none, base.stageManager.gui_Manager.left_button_Style))
                        {
                            if (numberOf_Weapon > 0)
                                numberOf_Weapon -= 8;
                        }
                        else if (GUI.Button(selectedRight_rect, GUIContent.none, base.stageManager.gui_Manager.right_button_Style))
                        {
                            if (numberOf_Weapon < 64)
                                numberOf_Weapon += 8;
                        }
                    }
                    GUI.EndGroup();

                    #endregion
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }
}
