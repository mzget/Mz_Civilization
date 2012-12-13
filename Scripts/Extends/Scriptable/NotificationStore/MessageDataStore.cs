using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageDataStore
{
	public List<QuestBeh> list_questBeh = new List<QuestBeh>();

	public MessageDataStore () {

	}

	public void OnDestroy() {

	}
	
	#region <@-- New player greeting message.
	
	public const string NEW_PLAYER_GREETING_MESSAGE_TOPIC = "Welcome to Empire Civilization";
	public string newPlayerGreetingMessage {
		get {
			if(Main.CurrentAppLanguage == Main.AppLanguage.Thai) 
				return TH_newPlayerGreetingMessage;
			else
				return EN_newPlayerGreetingMessage;
		}
	}
	const string TH_newPlayerGreetingMessage = "";
	const string EN_newPlayerGreetingMessage = "Now! I can see, You are a leader of the village. I am a village elder and I'm the trainer for you.";
	
	#endregion

    #region <@-- Tutorial Campaign.

    public class Quest_TutorialCampaign
    {
        public const string QUESTNAME_FoodEqualsVillagers = "FoodEqualsVillagers";
        public string QuestObjective
        {
            get
            {
                if (Main.CurrentAppLanguage == Main.AppLanguage.Thai)
                    return TH_QuestObjective;
                else
                    return EN_QuestObjective;
            }
        }
        const string EN_QuestObjective = "Building farm and gather Food";
        const string TH_QuestObjective = "ÊÃéÒ§¿ÒÃìÁáÅÐÃÇºÃÇÁÍÒËÒÃ";


    }

    #endregion
}

