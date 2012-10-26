using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FacebookManager : MonoBehaviour 
{
	public GUISkin facebook_Skin;
    private string user_id; //Facebook id
    private string user_name; //User name
    private Dictionary<string, string> friends; //All the users friends key = id, value = name
	
	
	private MainMenu manager;
	
	
	void Start() {
		manager = this.gameObject.GetComponent<MainMenu>();
	}
	

    public void GetCurrentUserComplete(string fbdata) //Called by js when the userinfo was retrieved. fbdata looks like 1234,name
    {
        string[] parts = fbdata.Split(',');
        user_id = parts[0];
        user_name = parts[1];
		
		manager.username = this.user_name;
    }
	
    public void GetFriendsComplete(string fbdata) //Called by js when the friends are retrieved. fbdata looks like 1234,name;5678,name;..
    {
        friends = new Dictionary<string, string>();

        string[] parts = fbdata.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries); //we've seperated each user, now get their id and name

        foreach (string user in parts)
        {
            string[] userInfo = user.Split(','); //Split each user on ',' first should be id then name.

            friends.Add(userInfo[0], userInfo[1]);
        }
    }
	
	public void TestExternalComunication(string data) {
		user_name = data;
	}

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GAMEWIDTH, Screen.height / Main.GAMEHEIGHT, 1));
		
		if(manager.NewGameGUIState == MainMenu.NewGameUIState.showTextField) {
            if (GUI.Button(new Rect(Main.GAMEWIDTH / 2 - 150, 32, 300, 50), new GUIContent("", "FacebookConnect"), facebook_Skin.customStyles[0]))
            {
	            Application.ExternalCall("GetLoginStatus");
			}
		}
//        GUI.Box(new Rect(Main.GameWidth/2 - 100, 10, 200, 24), user_name); //Display name

        if (friends != null) //If we retrieved our friends already
        {
            float h = 34;
            foreach (var friend in friends)
            {
                GUI.Label(new Rect(200, h, 200, 24), friend.Value);
                h += 24;
            }
        }
    }
}
