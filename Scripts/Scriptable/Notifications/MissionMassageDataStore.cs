﻿using System.Collections.Generic;


public class MissionMassageDataStore
{

    public MissionMassageDataStore() { }

    public void OnDestroy() { }


    public const string NULL_MISSION_MESSAGE = "No mission";

    #region <@-- 1. Create Sawmill.

    public const string TOPIC_CREATE_SAWMILL = "Mission 1 : Sawmill";
    public const string CREATE_SAWMILL_DESCRIPTION = "Create 1 Sawmill, Wood is important used to build almost all structures.";

    #endregion

	#region <@-- 2. Create Farm.

	public const string TOPIC_CREATE_FARM = "Mission 2 : Farm";
    public const string CREATE_FARM_DESCRIPTION = "Create 1 Farm and gather food, Food for your population will production by Farm.";

	#endregion

    #region <@-- 3. Create House.

    public const string TOPIC_CREATE_HOUSE = "Mission 3 : House";
    public const string CREATE_HOUSE_DESCRIPTION = "Create 2 House, Max population of your village depend on number and level of House";

    #endregion

	#region <@-- 4. Create Storehouse.

	public const string TOPIC_CREATE_STOREHOUSE = "Mission 4 : Storehouse";
	public const string CREATE_STOREHOUSE_DESCRIPTION = "Create 1 Storehouse";

	#endregion

	#region <@-- 5. Create Market.

	public const string TOPIC_CREATE_MARKET = "Mission 5 : Market";
	public const string CREATE_MARKET_DESCRIPTION = "Create 1 Market";

	#endregion

	#region <@-- 6. Upgrade Town center to lv.3

	public const string TOPIC_UPGRADE_TOWNCENTER = "Mission 6 : Upgrade TownCenter";
	public const string UPGRADE_TOWNCENTER_DESCRIPTION = "Upgrade TownCenter to Level 3. You can expand more building area.";

	#endregion

	#region <@-- 7. Expand more building area.

	public const string LV7_TOPIC = "Mission 7 : Expand village";
	public const string LV7_DESCRIPTION = "Buy once building area to expand your village.";

	#endregion
}

