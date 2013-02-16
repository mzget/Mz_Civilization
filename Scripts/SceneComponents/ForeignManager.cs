using UnityEngine;
using System.Collections;

public class ForeignManager : MonoBehaviour
{
	public enum ForeignTabStatus
	{
		None = 0, DrawActivity = 1,
	};
	public ForeignTabStatus currentForeignTabStatus;

	private TaskManager taskManager;
	private Rect citiesSymbol_rect = new Rect(24 * Mz_OnGUIManager.Extend_heightScale, 24, 100 * Mz_OnGUIManager.Extend_heightScale, 100);
	private Rect citiesTagName_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 130, 120 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect sendButton_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 170, 120 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect selectTroopBox_rect = new Rect(150 * Mz_OnGUIManager.Extend_heightScale, 40, 545 * Mz_OnGUIManager.Extend_heightScale, 450);
	private Rect drawUnit_00_rect = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 10, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
	private Rect selectUnitBoxRect_00 = new Rect(70 * Mz_OnGUIManager.Extend_heightScale, 24, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect maxUnitButtonRect_00 = new Rect(130 * Mz_OnGUIManager.Extend_heightScale, 20, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
	private Rect drawUnitRect_01 = new Rect(240 * Mz_OnGUIManager.Extend_heightScale, 10, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
	private Rect selectUnitBoxRect_01 = new Rect(300 * Mz_OnGUIManager.Extend_heightScale, 24, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect maxUnitButtonRect_01 = new Rect(360 * Mz_OnGUIManager.Extend_heightScale, 20, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
	private Rect drawUnitRect_10 = new Rect(10 * Mz_OnGUIManager.Extend_heightScale, 80, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
	private Rect selectUnitBoxRect_10 = new Rect(70 * Mz_OnGUIManager.Extend_heightScale, 94, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect maxUnitButtonRect_10 = new Rect(130 * Mz_OnGUIManager.Extend_heightScale, 90, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
	private Rect drawUnitRect_11 = new Rect(240 * Mz_OnGUIManager.Extend_heightScale, 80, 60 * Mz_OnGUIManager.Extend_heightScale, 60);
	private Rect selectUnitBoxRect_11 = new Rect(300 * Mz_OnGUIManager.Extend_heightScale, 94, 60 * Mz_OnGUIManager.Extend_heightScale, 32);
	private Rect maxUnitButtonRect_11 = new Rect(360 * Mz_OnGUIManager.Extend_heightScale, 90, 60 * Mz_OnGUIManager.Extend_heightScale, 40);
	private string numberOFUnit_00 = string.Empty; 
	private string numberOFUnit_01 = string.Empty;
	private string numberOFUnit_02 = string.Empty;


    void Awake() {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        taskManager = gameController.GetComponent<TaskManager>();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()	{
	
	}

	void OnGUI() {		
        //GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.FixedGameHeight, 1));
		
        //if(currentForeignTabStatus == ForeignManager.ForeignTabStatus.DrawActivity) {
        //    taskManager.standardWindow_rect = GUI.Window(0, taskManager.standardWindow_rect, DrawActivityWindow, new GUIContent("Select troops"), taskManager.foreignActivityStyle);
        //}		
        //if(taskManager.currentRightSidebarState != TaskManager.RightSidebarState.show_ForeignTab && currentForeignTabStatus != ForeignManager.ForeignTabStatus.None) {
        //    currentForeignTabStatus = ForeignManager.ForeignTabStatus.None;
        //    TaskManager.IsShowInteruptGUI = false;
        //}
	}
	
	private void DrawActivityWindow(int id)
	{
		//<!-- Exit Button.
		if (GUI.Button(taskManager.exitButton_Rect, new GUIContent(string.Empty, "Close Button"), taskManager.taskbarUI_Skin.customStyles[6])) {
			CloseGUIWindow();
		}

		GUI.BeginGroup(selectTroopBox_rect, "Pillage", taskManager.taskbarUI_Skin.box); {
			GUI.DrawTexture(drawUnit_00_rect, taskManager.spearmanUnitIcon);
			numberOFUnit_00 = GUI.TextField(selectUnitBoxRect_00, numberOFUnit_00, 3, taskManager.taskbarUI_Skin.textField);
			if (GUI.Button(maxUnitButtonRect_00, BarracksBeh.AmountOfSpearman.ToString())) {
				numberOFUnit_00 = BarracksBeh.AmountOfSpearman.ToString();
			}
			
			GUI.DrawTexture(drawUnitRect_01, taskManager.hypaspistUnitIcon);
			GUI.TextField(selectUnitBoxRect_01, "0", 3, taskManager.taskbarUI_Skin.textField);
			if (GUI.Button(maxUnitButtonRect_01, BarracksBeh.AmountOfHapaspist.ToString())) {
				numberOFUnit_01 = BarracksBeh.AmountOfHapaspist.ToString();
			}
			
			GUI.DrawTexture(drawUnitRect_10, taskManager.hopliteUnitIcon);
			GUI.TextField(selectUnitBoxRect_10, "0", 3, taskManager.taskbarUI_Skin.textField);
			if (GUI.Button(maxUnitButtonRect_10, BarracksBeh.AmountOfHoplite.ToString())) {
				numberOFUnit_02 = BarracksBeh.AmountOfHoplite.ToString();
			}
			
//			GUI.DrawTexture(drawUnitRect_11, taskManager.ToxotesUnitIcon);
//			GUI.TextField(selectUnitBoxRect_11, "0", 3, taskManager.taskbarUI_Skin.textField);
//			GUI.Button(maxUnitButtonRect_11, "Max");
		}
		GUI.EndGroup();
		
		/// Draw cities symbol.
		GUI.DrawTexture(citiesSymbol_rect, CapitalCity.list_AICity[0].symbols);
		GUI.Box(citiesTagName_rect, CapitalCity.list_AICity[0].name, taskManager.taskbarUI_Skin.box);
		
		if (GUI.Button(sendButton_rect, "Send", taskManager.taskbarUI_Skin.button)) {
            this.SendTroopMechanism();
		}
	}

    private void SendTroopMechanism()
    {
        try {
            int selectedSpearman = numberOFUnit_00 != string.Empty ? int.Parse(numberOFUnit_00) : 0;
            int selectedHapaspist = numberOFUnit_01 != string.Empty ? int.Parse(numberOFUnit_01) : 0;
            int selectedHoplite = numberOFUnit_02 != string.Empty ? int.Parse(numberOFUnit_02) : 0;

            GroupOFUnitBeh groupOfUnitBehs = new GroupOFUnitBeh();
            groupOfUnitBehs.unitBehs.Add(new UnitBeh()
            {
                name = UnitDatabase.GreekUnitDatabase.Spearman_Unit.NAME,
                ability = UnitDatabase.GreekUnitDatabase.Spearman_Unit.Ability,
            });
            groupOfUnitBehs.members.Add(selectedSpearman);
            groupOfUnitBehs.capacities.Add(UnitDatabase.GreekUnitDatabase.Spearman_Unit.Ability.capacity * selectedSpearman);

            groupOfUnitBehs.unitBehs.Add(new UnitBeh()
            {
                name = UnitDatabase.GreekUnitDatabase.Hapaspist_Unit.NAME,
                ability = UnitDatabase.GreekUnitDatabase.Hapaspist_Unit.Ability,
            });
            groupOfUnitBehs.capacities.Add(UnitDatabase.GreekUnitDatabase.Hapaspist_Unit.Ability.capacity * selectedHapaspist);
            groupOfUnitBehs.members.Add(selectedHapaspist);

            groupOfUnitBehs.unitBehs.Add(new UnitBeh()
            {
                name = UnitDatabase.GreekUnitDatabase.Hoplite_Unit.NAME,
                ability = UnitDatabase.GreekUnitDatabase.Hoplite_Unit.Ability,
            });
            groupOfUnitBehs.members.Add(selectedHoplite);
            groupOfUnitBehs.capacities.Add(UnitDatabase.GreekUnitDatabase.Hoplite_Unit.Ability.capacity * selectedHoplite);

            if (selectedSpearman + selectedHapaspist + selectedHoplite > 0)
            {
                TroopsActivity newTroopsActivity = new TroopsActivity();
                newTroopsActivity.currentTroopsStatus = TroopsActivity.TroopsStatus.Pillage;
                newTroopsActivity.targetCity = CapitalCity.list_AICity[0];
                newTroopsActivity.timeToTravel = System.TimeSpan.FromSeconds(CapitalCity.list_AICity[0].distance);
                newTroopsActivity.startTime = System.DateTime.UtcNow;
                newTroopsActivity.groupOfUnitBeh = groupOfUnitBehs;
                newTroopsActivity.attackBonus = WarfareSystem.AttackBonus;
                newTroopsActivity.totalAttackScore = this.CalculationTotalAttackScore(groupOfUnitBehs);
                foreach (int capa in groupOfUnitBehs.capacities)
                {
                    newTroopsActivity.totalCapacity += capa;
                }

                taskManager.displayTroopsActivity.MilitaryActivityList.Add(newTroopsActivity);

                BarracksBeh.AmountOfSpearman -= selectedSpearman;
                BarracksBeh.AmountOfHapaspist -= selectedHapaspist;
                BarracksBeh.AmountOfHoplite -= selectedHoplite;

                Debug.Log("displayTroopsActivity.MilitaryActivityList.Count : " + taskManager.displayTroopsActivity.MilitaryActivityList.Count);

                CloseGUIWindow();
            }
        } catch {
            Debug.LogWarning("Cannot send your troops !");
        } finally {
			numberOFUnit_00 = string.Empty;
			numberOFUnit_01 = string.Empty;
			numberOFUnit_02 = string.Empty;
        }
    }

    private int CalculationTotalAttackScore(GroupOFUnitBeh groupOfUnitBehs)
    {
        int totalATKScore = 0;
        int[] arr_temp_attackScore = new int[groupOfUnitBehs.unitBehs.Count];

        for (int i = 0; i < groupOfUnitBehs.unitBehs.Count; i++)
        {
            arr_temp_attackScore[i] = groupOfUnitBehs.unitBehs[i].ability.attack * groupOfUnitBehs.members[i];
            float tempCalc = arr_temp_attackScore[i] + (arr_temp_attackScore[i] * (WarfareSystem.AttackBonus / 100f));
            totalATKScore += (int)tempCalc;
        }

        Debug.Log("totalATKScore : " + totalATKScore);

        return totalATKScore;
    }

	private void CloseGUIWindow()
	{
		currentForeignTabStatus = ForeignManager.ForeignTabStatus.None;
		TaskManager.IsShowInteruptGUI = false;
		taskManager.MoveInLeftSidebar();
	}
}

