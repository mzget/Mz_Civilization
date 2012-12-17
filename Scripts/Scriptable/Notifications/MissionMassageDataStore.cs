using System.Collections.Generic;


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
}

