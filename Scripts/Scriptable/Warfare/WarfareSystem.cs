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
		if(troopsActivity.targetCity == CapitalCity.list_AICity[0]) {
			switch (troopsActivity.currentTroopsStatus) {
				case TroopsActivity.TroopsStatus.Pillage :
                    GroupOFUnitBeh groupOfUnits = CapitalCity.list_AICity[0].CreateTroopsActivity();
					WarfareSystem.CostOfPillageProcessing(ref troopsActivity, ref groupOfUnits);
				break;
				default:
				break;
			}
		}
    }

    internal static void CostOfPillageProcessing(ref TroopsActivity troopActivity, ref GroupOFUnitBeh groupOfUnits) {
		float resultRatio = troopActivity.totalAttackScore / groupOfUnits.totalDefenseScore;

		Debug.Log("CostOfPillageProcessing : " + resultRatio);

        if(resultRatio >= 1) {
			if(resultRatio == 1) {
				// attacker lose, and get damage 60 percent.
				// defender win, and get damage 30 percent.
				troopActivity.battleResult = TroopsActivity.ResultOfBattle.Lose;
			}
			else if(resultRatio > 1 && resultRatio < 1.5f) {
				// attacker win, and get damage 40 percent.
				// defender lose, and get damage 50 percent.
			}
			else if(resultRatio >= 1.5f && resultRatio < 2f) {
				// attacker win, and get damage 25 percent.
				// defender lose, and get damage 60 percent.
			}
			else if(resultRatio >= 2 && resultRatio < 3) {
				// attacker win, and get damage 10 percent.
				// defender lose, and get damage 75 percent.
			}
			else if(resultRatio >= 3 && resultRatio < 5) {
				// Attacker win, and get damage 5% .
				// Defender lose, and get damage 90% .
			}
			else if(resultRatio >= 5) {
				// Attacker win, and get damage 1% .
				// Defender lose, and get damage 100% .
			}
		}
        else if (resultRatio < 1) {
			if(resultRatio >= 0.5f) {
				// Attacker lose, get damage 75% .
				// Defender win, get damage 10% .
			}
			else if(resultRatio >= 0.35f && resultRatio < 0.5f) {
				// Attacker lose, and get damage 90% .
				// Defender win, get damage 5% .
			}
			else if(resultRatio < 0.35f) {
				// Attacker lose, and get damage 100% .
				// Defender win, and get damage 1% .
			}
        }
    }
}
