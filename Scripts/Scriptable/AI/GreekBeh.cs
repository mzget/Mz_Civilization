using UnityEngine;
using System.Collections;

[System.Serializable]
public class GreekBeh : AICities {

    internal static int Spearman_unit = 10;
    internal static int Hapaspist_unit = 10;
    internal static int Hoplite_unit = 10;

    public const string NAME = "Greek";


	// Use this for initialization
	public GreekBeh() {
	
	}

    internal override GroupOFUnitBeh CreateTroopsActivity()
    {
//        base.CreateTroopsActivity();

        GroupOFUnitBeh groupOfUnits = new GroupOFUnitBeh();
        groupOfUnits.unitBehs.Add(new UnitBeh() { name = UnitDatabase.GreekUnitDatabase.Spearman_Unit.NAME, ability = UnitDatabase.GreekUnitDatabase.Spearman_Unit.Ability, });
        groupOfUnits.unitBehs.Add(new UnitBeh() { name = UnitDatabase.GreekUnitDatabase.Hapaspist_Unit.NAME, ability = UnitDatabase.GreekUnitDatabase.Hapaspist_Unit.Ability });
        groupOfUnits.unitBehs.Add(new UnitBeh() { name = UnitDatabase.GreekUnitDatabase.Hoplite_Unit.NAME, ability = UnitDatabase.GreekUnitDatabase.Hoplite_Unit.Ability });
        groupOfUnits.members.Add(Spearman_unit);
        groupOfUnits.members.Add(Hapaspist_unit);
        groupOfUnits.members.Add(Hoplite_unit);
		groupOfUnits.totalDefenseScore = this.CalculationDefenseScore();

        return groupOfUnits;
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
