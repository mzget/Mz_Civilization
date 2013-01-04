using UnityEngine;
using System.Collections;

public class WarfareSystem {

    // Todo list.

    public static float AttackBonus = 0;
    public static float DefenseBonus = 0;

	// Use this for initialization
    public WarfareSystem() { }

    internal void WarfareProcessing(TroopsActivity troopsActivity)
    {
		if(troopsActivity.targetCity == StageManager.list_AICity[0]) {
			switch (troopsActivity.currentTroopsStatus) {
				case TroopsActivity.TroopsStatus.Pillage :
					int defenseScore = StageManager.list_AICity[0].CalculationDefenseScore();
					WarfareSystem.CostOfPillageProcessing(ref troopsActivity, defenseScore);
				break;
				default:
				break;
			}
		}
    }

    internal static void CostOfPillageProcessing(ref TroopsActivity troopActivity, int defenseScore) {
        if(troopActivity.totalAttackScore >= defenseScore) {
            troopActivity.battleResult = TroopsActivity.ResultOfBattle.Win;
		}
        else if (troopActivity.totalAttackScore < defenseScore) {
            troopActivity.battleResult = TroopsActivity.ResultOfBattle.Lose;
        }
    }
}
