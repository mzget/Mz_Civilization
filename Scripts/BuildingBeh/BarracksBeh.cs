using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BarracksBeh : BuildingBeh
{
	//<!-- Static Data.
    public static GameResource[] RequireResource = new GameResource[10] {
        new GameResource() { Food = 300, Wood = 120, Copper = 500, Gold = 250 },
        new GameResource() { Food = 800, Wood = 300, Copper = 1200, Gold = 500, Employee = 5},
        new GameResource() { Food = 1800, Wood = 800, Copper = 2200, Gold = 1000, Employee = 12},
        new GameResource() { Food = 3000, Wood = 1600, Copper =  3400, Gold = 2000, Employee = 22},
        new GameResource(500, 500, 500, 500, 50),
        new GameResource(600, 600, 600, 600, 60),
        new GameResource(700, 700, 700, 700, 70),
        new GameResource(800, 800, 800, 800, 80),
        new GameResource(900, 900, 900, 900, 90),
        new GameResource(1000, 1000, 1000, 1000, 100),
    };
	
    public static string BuildingName = "Barrack";
	public const string RequireDescription = "Require :: Academy level 3.";
    private static string Description_TH = "ในค่ายทหารนี้คุณสามารถเกณฑ์ทหารราบได้ ยิ่งระดับค่ายทหารมากเท่าไร \n " + "กองกำลังก็จะเข็มแข็งมากขึ้น";
    private static string Description_EN = "Trains infantry units. Also researches technologies related to infantry units";
	public static string CurrentDescription {
        get {
            string temp = Description_EN;
            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                temp = Description_EN;
            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                temp = Description_TH;
            return temp;
        }
	}

    public static int AmountOfSpearman = 0;
	public static int AmountOfHapaspist = 0;
	public static int AmountOfHoplite = 0;
    private List<TrainingQueue> list_trainingUnit = new List<TrainingQueue>();
	private DateTime startingCounterTimer;
    public enum BarracksStatus { None = 0, TrainingUnit, };
    public BarracksStatus currentBarracksStatus;

	public GUISkin mainBuildingSkin;
    private string spearman_describe;
    private string hypaspist_describe;
    private string hoplite_describe;	

    private Rect troopsIcon_Rect;
    Rect training_group_rect;

    float height = 210f;
    Rect new_background_Rect;
    Rect new_descriptionGroupRect;
    Rect soldierDescriptionRect;
    Rect requireResource_rect;
    Rect createButton_rect;
    private Rect amountLabel_rect;
    private Rect amountTextbox_rect;
	Rect maxAmountLabel_rect;
    public string numberOfSpearman_input;


    protected override void Awake()
    {
        base.Awake();
        base.sprite = this.gameObject.GetComponent<OTSprite>();

        this.name = BuildingName;
        base.buildingType = BuildingType.general;
        base.buildingTimeData = new BuildingsTimeData(buildingType);
    }
    
	// Use this for initialization
	void Start () {
        this.InitializingData();
		this.InitializeTexturesResource();
		
		base.NotEnoughResource_Notification_event += HandleBaseNotEnoughResourceNotification_event;
    }
	
	protected override void InitializeTexturesResource ()
	{
		base.InitializeTexturesResource ();

        buildingIcon_Texture = Resources.Load(BuildingIcons_TextureResourcePath + "Barracks", typeof(Texture2D)) as Texture2D;
	}
    protected override void InitializingData()
    {
        base.InitializingData();

        troopsIcon_Rect = new Rect(base.imgIcon_Rect.x, base.imgIcon_Rect.y + 16, base.imgIcon_Rect.width, base.imgIcon_Rect.height);

        spearman_describe = UnitDataStore.GreekUnitData.Get_Spearman_describe;
        hypaspist_describe = UnitDataStore.GreekUnitData.Get_Hypaspist_describe;
        hoplite_describe = UnitDataStore.GreekUnitData.Get_Hoplite_describe;

        new_background_Rect = new Rect(base.building_background_Rect.x, base.building_background_Rect.y, base.building_background_Rect.width - 16, base.building_background_Rect.height);
        new_descriptionGroupRect = new Rect(base.descriptionGroup_Rect.x, base.descriptionGroup_Rect.y, base.descriptionGroup_Rect.width - 16, base.descriptionGroup_Rect.height);
        soldierDescriptionRect = new Rect(new_descriptionGroupRect.x, 10, new_descriptionGroupRect.width, 190);
        requireResource_rect = new Rect(5, soldierDescriptionRect.height - 45, soldierDescriptionRect.width - 10, 40);
        amountLabel_rect = new Rect(5, soldierDescriptionRect.height - 90, 100, 40);
        amountTextbox_rect = new Rect(amountLabel_rect.x + amountLabel_rect.width, amountLabel_rect.y, 50, 40);
		maxAmountLabel_rect = new Rect(amountTextbox_rect.x + amountTextbox_rect.width, amountTextbox_rect.y, 50, 40);
        createButton_rect = new Rect(30, 150, 100, 35);
		training_group_rect = new Rect(5,80,base.currentJob_Rect.width - 10, 200);
    }

    public override void InitializingBuildingBeh(BuildingBeh.BuildingStatus p_buildingState, int p_indexPosition, int p_level)
    {
        base.InitializingBuildingBeh(p_buildingState, p_indexPosition, p_level);

        BuildingBeh.Barrack_Instances.Add(this);

        this.CalculateNumberOfEmployed(p_level);
    }
	protected override void CalculateNumberOfEmployed (int p_level)
	{
//		base.CalculateNumberOfEmployed (p_level);
		int sumOfEmployed = 0;
		for (int i = 0; i < p_level; i++) {
			sumOfEmployed += RequireResource[i].Employee;
		}
		
		HouseBeh.SumOfEmployee += sumOfEmployed;
	}

    #region <!-- Building Processing.

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
    }

    #endregion

    protected override void ClearStorageData()
    {
        base.ClearStorageData();
		
		base.NotEnoughResource_Notification_event -= HandleBaseNotEnoughResourceNotification_event;
        BuildingBeh.Barrack_Instances.Remove(this);
    }

    /// <summary>
    /// Handle not enough resource for upgrade building.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	void HandleBaseNotEnoughResourceNotification_event (object sender, BuildingBeh.NoEnoughResourceNotificationArg e)
	{
		base.notificationText = e.notification_args;
	}

    private void TrainingUnitMechanism()
    {
        Debug.Log("TrainingUnitMechanism()");

        if (list_trainingUnit.Count > 0) {
            currentBarracksStatus = BarracksStatus.TrainingUnit;
        }
		else
			currentBarracksStatus = BarracksStatus.None;

        Debug.Log("currentBarracksStatus == " + currentBarracksStatus);
    }

    protected override void Update()
    {
        base.Update();

        if (currentBarracksStatus == BarracksStatus.TrainingUnit) {
			TimeSpan elapsedTime = DateTime.UtcNow - list_trainingUnit[0].startTime;
			TimeSpan remainTime = list_trainingUnit[0].ToTalQueueTime - elapsedTime;
            
			list_trainingUnit[0].RemainingTime = remainTime;

			TimeSpan counter = DateTime.UtcNow - startingCounterTimer;
            if (counter.TotalSeconds >= list_trainingUnit[0].Unit.TimeTraining.TotalSeconds) {
				startingCounterTimer = DateTime.UtcNow;

                AmountOfSpearman += 1;
				list_trainingUnit[0].Number -= 1;
            }
			
			if(list_trainingUnit[0].Number == 0) { 
				list_trainingUnit.RemoveAt(0); 
				this.TrainingUnitMechanism();
			}
        }
    }
	
	protected override void OnTouchDown ()
	{
		base.OnTouchDown ();
		
        sceneController.taskManager.currentRightSideState = TaskManager.RightSideState.show_military;
	}

	protected override void CreateWindow (int windowID)
	{
        base.CreateWindow(windowID);
		
        GUI.Box(base.notificationBox_rect, base.notificationText, standard_Skin.box);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 80, base.windowRect.width, base.background_Rect.height), 
            scrollPosition, new Rect(0, 0, base.background_Rect.width, base.background_Rect.height * 3));
        {
            this.DrawMainBuildingContent();         
            this.DrawGroupOfSpearman();
            this.DrawGroupOfHypasist();
            this.DrawGroupOfHoplist();
            this.DrawGroupOfToxotes();
		}
		GUI.EndScrollView ();   // End the scroll view that we began above.
	}

    private void DrawMainBuildingContent()
    {     
        GUI.BeginGroup(new_background_Rect, new GUIContent("", "content background"), building_Skin.box);
        {
            //<!-- building icon.
            GUI.DrawTexture(base.imgIcon_Rect, buildingIcon_Texture);
            //<!-- building level.
            GUI.Label(base.levelLable_Rect, "Level " + this.Level, base.status_style);

            #region <!--- Contents Group.

            GUI.BeginGroup(new_descriptionGroupRect, CurrentDescription, building_Skin.textArea);
			{
                if (base.Level < RequireResource.Length)
                {
                    if (list_trainingUnit.Count > 0) {
						if(list_trainingUnit[0].RemainingTime.Ticks > 0) {
							GUI.BeginGroup(training_group_rect, GUIContent.none, GUI.skin.textArea);
							{
								string queueTime = new DateTime(list_trainingUnit[0].RemainingTime.Ticks).ToString("HH:mm:ss");
		                        GUI.Label(new Rect(2, 2, training_group_rect.width - 4, 40), "Training : " + list_trainingUnit[0].Unit.name + "\t\t\t Units : " +
							          list_trainingUnit[0].Number + "\t\t\t Time : " + queueTime, base.job_style);
		
		                        //GUI.Label(base.nextJob_Rect, "Next Max Capacity : ", base.job_style);
							}
	                        GUI.EndGroup();
						}
                    }
                    else {
						GUI.Box(training_group_rect, "No training.", base.job_style);
                    }

                    //<!-- Requirements Resource.
                    GUI.BeginGroup(update_requireResource_Rect);
                    {
                        GUI.Box(GameResource.First_Rect, new GUIContent(RequireResource[Level].Food.ToString(), base.sceneController.taskManager.food_icon), standard_Skin.box);
                        GUI.Box(GameResource.Second_Rect, new GUIContent(RequireResource[Level].Wood.ToString(), base.sceneController.taskManager.wood_icon), standard_Skin.box);
                        GUI.Box(GameResource.Third_Rect, new GUIContent(RequireResource[Level].Copper.ToString(), base.sceneController.taskManager.copper_icon), standard_Skin.box);
                        GUI.Box(GameResource.Fourth_Rect, new GUIContent(RequireResource[Level].Gold.ToString(), base.sceneController.taskManager.gold_icon), standard_Skin.box);
                        GUI.Box(GameResource.Fifth_Rect, new GUIContent(RequireResource[Level].Employee.ToString(), base.sceneController.taskManager.employee_icon), standard_Skin.box);
                    }
                    GUI.EndGroup();
                }
                else
                {
                    //GUI.Label(currentProduction_Rect, "Current Max Capacity : " + this.currentMaxCapacity, base.job_style);
                    GUI.Label(nextJob_Rect, "Max upgrade building.", base.job_style);
                }
			}
            GUI.EndGroup();

            #endregion

            #region <!--- Upgrade Button mechanichm.

            bool enableUpgrade = false;
            if (base.Level < RequireResource.Length && base.CheckingCanUpgradeLevel() && CheckingEnoughUpgradeResource(RequireResource[Level]))
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
                base.currentBuildingStatus = BuildingBeh.BuildingStatus.OnDestructionProcess;
                this.DestructionBuilding();
                base.CloseGUIWindow();
            }
            GUI.enabled = true;

            #endregion
        }
        GUI.EndGroup();
    }

    private void DrawGroupOfSpearman()
    {
        GUI.BeginGroup(new Rect(0, 2 * height, background_Rect.width, height), new GUIContent("", "Background"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Spearman", "สเปียร์แมน"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, sceneController.taskManager.spearmanUnitIcon);      //<!-- Spearman Images.

            GUI.BeginGroup(soldierDescriptionRect, new GUIContent(spearman_describe, "content"), mainBuildingSkin.textArea);
            {
                GUI.Label(amountLabel_rect, "Amount", standard_Skin.box);
                numberOfSpearman_input = GUI.TextField(amountTextbox_rect, numberOfSpearman_input, 3, standard_Skin.textField);
                ///<!-- Max button use to get max unit user can create.
				if(GUI.Button(maxAmountLabel_rect, "Max", standard_Skin.button)) {
                    numberOfSpearman_input = CalculationCanCreateSpearman().ToString();
				}

                GUI.BeginGroup(requireResource_rect, GUIContent.none, standard_Skin.box);
                {
                    GUI.Box(GameResource.First_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Food.ToString(), sceneController.taskManager.food_icon), standard_Skin.box);
                    GUI.Box(GameResource.Second_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Armor.ToString(), sceneController.taskManager.armor_icon), standard_Skin.box);
                    GUI.Box(GameResource.Third_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Weapon.ToString(), sceneController.taskManager.weapon_icon), standard_Skin.box);
                    GUI.Box(GameResource.Fourth_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanResource.Gold.ToString(), sceneController.taskManager.gold_icon), standard_Skin.box);
                    GUI.Box(GameResource.Fifth_Rect, new GUIContent(UnitDataStore.GreekUnitData.SpearmanTraining_timer.ToString()), standard_Skin.box);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            if (GUI.Button(createButton_rect, "Train"))
            {
				this.TrainSpearmanUnit();
            }
        }
        GUI.EndGroup();
    }
	private void TrainSpearmanUnit ()
	{
		try {
			int amount = int.Parse(numberOfSpearman_input);
			
			if (amount > 0 && amount <= CalculationCanCreateSpearman())
			{
				TimeSpan timePerUnit = UnitDataStore.GreekUnitData.SpearmanTraining_timer;
				TimeSpan total = TimeSpan.FromSeconds(timePerUnit.TotalSeconds * amount);
				
				TrainingQueue queue = new TrainingQueue() {
					Unit = new UnitBeh() { name = "Spearman", TimeTraining = timePerUnit, }, 
					Number = amount,
					RemainingTime = total,
					ToTalQueueTime = total,
					startTime = DateTime.UtcNow,
				};
				list_trainingUnit.Add(queue);
				this.TrainingUnitMechanism();
				startingCounterTimer = DateTime.UtcNow;
				
				GameResource temp =  UnitDataStore.GreekUnitData.SpearmanResource;
				GameResource.UsedResource(new GameResource() { 
					Food = temp.Food * amount, 
					Armor = temp.Armor * amount, 
					Weapon = temp.Weapon * amount,
					Gold = temp.Gold * amount,
				});
			}
			else
				Debug.Log("Invalid input or input value more than limit :: numberOfSpearman == " + amount);
		}
		catch{
			
		}
		finally {
			numberOfSpearman_input = string.Empty;
		}
	}

    private void DrawGroupOfHypasist()
    {
        GUI.BeginGroup(new Rect(0, 3 * height, background_Rect.width, height), new GUIContent("", "Background"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Hypaspist", "พลดาบสองมือ"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, sceneController.taskManager.hypaspistUnitIcon);    //<!-- Hypaspist Images.
            GUI.Box(soldierDescriptionRect, new GUIContent(hypaspist_describe, "content"), mainBuildingSkin.textArea);
            if (GUI.Button(createButton_rect, "Create"))
            {

            }
        }
        GUI.EndGroup();
    }
    private void DrawGroupOfHoplist()
    {
        GUI.BeginGroup(new Rect(0, 4 * height, background_Rect.width, height), new GUIContent("", "Backgroud"), mainBuildingSkin.box);
        {
            GUI.Label(base.tagName_Rect, new GUIContent("Hoplite", "พลหุ้มเกราะ"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, sceneController.taskManager.hopliteUnitIcon);      //<!-- Hoplite Images.
            GUI.Box(soldierDescriptionRect, new GUIContent(hoplite_describe, "content"), mainBuildingSkin.textArea);
            if (GUI.Button(createButton_rect, "Create"))
            {

            }
        }
        GUI.EndGroup();
    }
    private void DrawGroupOfToxotes()
    {
        GUI.BeginGroup(new Rect(0, 5 * height, background_Rect.width, height), new GUIContent("", "background"), mainBuildingSkin.box);
        {
            GUI.Label(tagName_Rect, new GUIContent("Toxotes", "พลธนู"), standard_Skin.label);
            GUI.DrawTexture(this.troopsIcon_Rect, sceneController.taskManager.ToxotesUnitIcon);    //<!-- Toxotes images icons.
            GUI.Box(soldierDescriptionRect, new GUIContent("Toxotes เป็นหน่วยจู่โจมด้วยธนู \n พวกเขามีความชำนาญในการโจมตีจากระยะไกล", "content"), mainBuildingSkin.textArea);
            if (GUI.Button(createButton_rect, "Create"))
            {

            }
        }
        GUI.EndGroup();
    }

    private int CalculationCanCreateSpearman()
    {
        int maximumCanCreate = 0;
        if (StoreHouse.SumOfFood >= UnitDataStore.GreekUnitData.SpearmanResource.Food &&
            StoreHouse.sumOfArmor >= UnitDataStore.GreekUnitData.SpearmanResource.Armor &&
            StoreHouse.sumOfWeapon >= UnitDataStore.GreekUnitData.SpearmanResource.Weapon &&
            StoreHouse.sumOfGold >= UnitDataStore.GreekUnitData.SpearmanResource.Gold)
        {
            List<int> spearmanRequireResource = new List<int>()
            { 
                UnitDataStore.GreekUnitData.SpearmanResource.Food, 
                UnitDataStore.GreekUnitData.SpearmanResource.Armor, 
                UnitDataStore.GreekUnitData.SpearmanResource.Weapon, 
                UnitDataStore.GreekUnitData.SpearmanResource.Gold,
            };
            spearmanRequireResource.Sort();
            int minimumRequireResource = spearmanRequireResource[0];
            if (minimumRequireResource == UnitDataStore.GreekUnitData.SpearmanResource.Food)
            {
                maximumCanCreate = StoreHouse.SumOfFood / minimumRequireResource;
                return maximumCanCreate;
            }
            else if (minimumRequireResource == UnitDataStore.GreekUnitData.SpearmanResource.Armor)
            {
                maximumCanCreate = StoreHouse.sumOfArmor / minimumRequireResource;
                return maximumCanCreate;
            }
            else if (minimumRequireResource == UnitDataStore.GreekUnitData.SpearmanResource.Weapon)
            {
                maximumCanCreate = StoreHouse.sumOfWeapon / minimumRequireResource;
                return maximumCanCreate;
            }
            else if (minimumRequireResource == UnitDataStore.GreekUnitData.SpearmanResource.Gold)
            {
                maximumCanCreate = StoreHouse.sumOfGold / minimumRequireResource;
                return maximumCanCreate;
            }
            else return 0;
        }
        else return 0;
    }
}
