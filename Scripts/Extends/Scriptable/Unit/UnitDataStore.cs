using UnityEngine;
using System;
using System.Collections;

public class UnitDataStore {
    public enum Tribes {
		None = 0,
        Greek = 1,
        Egyptian = 2,
        Persian = 3,
        Celtic = 4,
    };

	public class GreekUnitData {		
        //<!--- Spearman.
        public const string Spearman = "Spearman";
	    public const string TH_Spearman_Describe = "Spearman เป็นหน่วยที่มีพรสวรรค์และมีความชำนาญ ในการเอาชนะกองทหารม้า";
	    public const string EN_Spearman_Describe = "The most basic military unit, the Spearman excels at overcoming cavalry."; 
	    public static string Get_Spearman_describe {
	        get {
	            string temp_data = "";	
	            if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
	                temp_data = EN_Spearman_Describe;
	            else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
	                temp_data = TH_Spearman_Describe;	
	            return temp_data;
	        }
	    }
        public static GameResource SpearmanResource = new GameResource() { Food = 40, Armor = 1, Weapon = 1, Gold = 15 };
        public static TimeSpan SpearmanTraining_timer = new TimeSpan(0, 1, 0);

        //<!--- Hapaspist.
        public const string Hapaspist = "Hapaspist";
        public const string TH_Hypaspist_Describe = "Hypaspist เป็นนักรบถือดาบสองมือ พวกเขาคือองค์รักษ์คุ้มกันจักรพรรดิด้วยชีวิต มีความชำนาญในการต่อต้านกองทหารราบอื่นๆ";
        public const string EN_Hypaspist_Describe = "Wielding dual swords, the Hypaspist cut through other infantry units with ease.";
        public static string Get_Hypaspist_describe {
            get{
                string temp_data = "";
                if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                    temp_data = EN_Hypaspist_Describe;
                else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                    temp_data = TH_Hypaspist_Describe;
                return temp_data;
            }
        }

        //<!--- Hoplite.
        public const string Hoplite = "Hoplite";
        public const string TH_Hoplite_Describe = "Hoplite เป็นพลเกราะหนัก ต่อต้านความเสียหายจากหน่วยยิงธนู ขณะที่มีประสิทธิภาพในการต่อสู้กับกองทหารม้า";
        public const string En_Hoplite_Describe = "Heavily armored, the Hoplite resists damage from archer unit while effectively battling cavalry units.";
        public static string Get_Hoplite_describe {
            get {
                string temp_data = "";
                if (Main.CurrentAppLanguage == Main.AppLanguage.defualt_En)
                    temp_data = En_Hoplite_Describe;
                else if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                    temp_data = TH_Hoplite_Describe;
                return temp_data;
            }
        }
	}
	
}
