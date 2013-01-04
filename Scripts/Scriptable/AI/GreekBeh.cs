using UnityEngine;
using System.Collections;

[System.Serializable]
public class GreekBeh : AICities {

    internal static int Spearman_unit = 500;
    internal static int Hapaspist_unit = 1000;
    internal static int Hoplite_unit = 300;

    public const string NAME = "Greek";


	// Use this for initialization
	public GreekBeh() {
	
	}

    public override int CalculationDefenseScore()
    {
        int defenseScore = 0;

        int spearmanScore = Spearman_unit * UnitDatabase.GreekUnitDatabase.Spearman_Unit.Ability.defense;
        float temp_1 = spearmanScore + (spearmanScore * (defenseBonus / 100f));
        defenseScore += (int)temp_1;

        int hapaspistScore = Hapaspist_unit * UnitDatabase.GreekUnitDatabase.Spearman_Unit.Ability.defense;
        float temp_2 =  hapaspistScore + (hapaspistScore * (defenseBonus / 100f));
        defenseScore += (int)temp_2;

        int hopliteScore = Hoplite_unit * UnitDatabase.GreekUnitDatabase.Hoplite_Unit.Ability.defense;
        float temp_3 = hopliteScore + (hopliteScore * (defenseBonus / 100f));
        defenseScore += (int)temp_3;

        Debug.Log("defenseScore : " + defenseScore);

        return defenseScore;
    }
}
