using UnityEngine;
using System;
using System.Collections;

public class UnitDatabase {

    public class GreekUnitDatabase
    {
        //<!--- Spearman.
        public class Spearman_Unit
        {
            public const string NAME = "Spearman";
            private const string TH_Describe = "Spearman เป็นหน่วยที่มีพรสวรรค์และมีความชำนาญ ในการเอาชนะกองทหารม้า";
            private const string EN_Describe = "The most basic military unit, the Spearman excels at overcoming cavalry.";
            public static string Get_Spearman_describe
            {
                get
                {
                    string temp_data = "";
                    if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                        temp_data = EN_Describe;
                    else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                        temp_data = TH_Describe;
                    return temp_data;
                }
            }
            public static GameMaterialDatabase SpearmanResource = new GameMaterialDatabase() { Food = 40, Armor = 1, Weapon = 1, Gold = 15 };
            public static TimeSpan SpearmanTraining_timer = new TimeSpan(0, 1, 0);
            public static UnitAbility Ability = new UnitAbility() { attack = 20, defense = 40, travelSpeed = 1, capacity = 20 };
        }

        //<!--- Hapaspist.
        public class Hapaspist_Unit
        {
            public const string NAME = "Hapaspist";
            private const string TH_Describe = "Hypaspist เป็นนักรบถือดาบสองมือ พวกเขาคือองค์รักษ์คุ้มกันจักรพรรดิด้วยชีวิต มีความชำนาญในการต่อต้านกองทหารราบอื่นๆ";
            private const string EN_Describe = "Wielding dual swords, the Hypaspist cut through other infantry units with ease.";
            public static string Get_Hypaspist_describe
            {
                get
                {
                    string temp_data = "";
                    if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                        temp_data = EN_Describe;
                    else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                        temp_data = TH_Describe;
                    return temp_data;
                }
            }
            public static UnitAbility Ability = new UnitAbility() { attack = 50, defense = 10, travelSpeed = 5, capacity = 50 };
        }

        //<!--- Hoplite.
        public class Hoplite_Unit
        {
            public const string NAME = "Hoplite";
            private const string TH_Hoplite_Describe = "Hoplite เป็นพลเกราะหนัก ต่อต้านความเสียหายจากหน่วยยิงธนู ขณะที่มีประสิทธิภาพในการต่อสู้กับกองทหารม้า";
            private const string En_Hoplite_Describe = "Heavily armored, the Hoplite resists damage from archer unit while effectively battling cavalry units.";
            public static string Get_Hoplite_describe
            {
                get
                {
                    string temp_data = "";
                    if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                        temp_data = En_Hoplite_Describe;
                    else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                        temp_data = TH_Hoplite_Describe;
                    return temp_data;
                }
            }
            public static UnitAbility Ability = new UnitAbility() { attack = 10, defense = 60, travelSpeed = 1, capacity = 10 };
        }
    }
	
}
