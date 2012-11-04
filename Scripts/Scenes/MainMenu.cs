using UnityEngine;
using System.Collections;


public class MainMenu : Mz_BaseScene
{
	// Set Build Version to Unlock All Item has locked with score.
	public enum BuildVersion : int { debugMode = 0, releaseMode = 1, };
	public BuildVersion buildVersion;

    public GUISkin newgame_Skin;

    public enum SceneState { none = 0, showOption, showNewGame, showLoadGame, };
    private SceneState sceneState;
    public enum NewGameUIState { none = 0, showTextField, showDuplicateName, showSaveGameSlot, };
    public NewGameUIState newgameUIState;

    public string username = string.Empty;
    private bool _isNullUsernameNotification = false;
    private bool _isDuplicateUsername = false;
    private bool _isFullSaveGameSlot;
    private string player_1;
    private string player_2;
    private string player_3;
    

    Rect gameWindow_rect = new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 150, 400, 300);
    Rect usernameTextfield_rect;
    Rect doneButton_rect;
    Rect mainMenuGroup_rect = new Rect(Screen.width - 300, 0, 300, Main.GAMEHEIGHT);


    void Awake() {
        Mz_GUIManager.CalculateViewportScreen();

        usernameTextfield_rect = new Rect(50, 120, 300, 64);
        doneButton_rect = new Rect(gameWindow_rect.width / 2 - 50, 200, 100, 32);

        mainMenuGroup_rect.width = mainMenuGroup_rect.width * Mz_GUIManager.Extend_heightScale;
        mainMenuGroup_rect.x = Screen.width - mainMenuGroup_rect.width;
    }

    // Use this for initialization
    void Start() 
	{
        base.InitializeAudio();
        //if(PlayerPrefs.HasKey(Mz_SaveData.usernameKey)) {
        //    username = PlayerPrefs.GetString(Mz_SaveData.usernameKey);				
        //    guiState = GUIState.showSaveGame; 
        //}
        //else {			
        //    guiState = GUIState.showNewGame;			
        //    if (newgameUIState != NewGameUIState.showTextField) {
        //        newgameUIState = NewGameUIState.showTextField;
        //    }
        //}
    }

    public bool _showSkinLayout;
    protected override void OnGUI()
    {
        player_1 = PlayerPrefs.GetString(1 + ":" + "username");
        player_2 = PlayerPrefs.GetString(2 + ":" + "username");
        player_3 = PlayerPrefs.GetString(3 + ":" + "username");

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));
        
        //<!--- Draw menu outside viewport.
        if (sceneState == SceneState.none)
        {
            _isDuplicateUsername = false;
            _isNullUsernameNotification = false;
            _isFullSaveGameSlot = false;
            username = ""; 
                
            this.DrawMainMenu();
        }

        GUI.BeginGroup(Mz_GUIManager.viewPort_rect);
        {
            if (_showSkinLayout)
                GUI.Box(new Rect(0, 0, Mz_GUIManager.viewPort_rect.width, Mz_GUIManager.viewPort_rect.height), "Skin layout", GUI.skin.box);

            if (sceneState == SceneState.showNewGame)
            {
                this.DrawNewGameTextField();
            }
            else if (sceneState == SceneState.showLoadGame)
            {
                // Call ShowSaveGameSlot Method.
                this.DrawSaveGameSlot(_isFullSaveGameSlot);
            }

            #region Show Notification When Username have a problem.

            string notificationText = "";
            string dublicateNoticeText = "";

            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                notificationText = "Please Fill Your Username. \n กรุณาใส่ชื่อผู้เล่น";
                dublicateNoticeText = "This name already exists. \n ซื่อนี้มีอยู่แล้ว";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                notificationText = "Please Fill Your Username.";
                dublicateNoticeText = "This name already exists.";
            }

            Rect notification_Rect = new Rect(Mz_GUIManager.viewPort_rect.width / 2 - 200, 0, 400, 64);
            if (_isNullUsernameNotification)
                GUI.Box(notification_Rect, notificationText);
            if (_isDuplicateUsername)
                GUI.Box(notification_Rect, dublicateNoticeText);

            #endregion
        }
        GUI.EndGroup();
		
		
		base.OnGUI();
    }

    private void DrawMainMenu()
    {
        GUI.BeginGroup(mainMenuGroup_rect, "Main Menu", GUI.skin.window);
        {
            if (GUI.Button(new Rect(20, 40, 120, 50), "New Player")) {
                sceneState = SceneState.showNewGame;
            }
            else if (GUI.Button(new Rect(20, 140, 120, 50), "Load game")) {
                sceneState = SceneState.showLoadGame;
            }
            else if (GUI.Button(new Rect(20, 240, 120, 50), "Multiplayer")) { 
                
            }
            else if (GUI.Button(new Rect(20, 340, 120, 50), "About")) { 
            
            }
        }
        GUI.EndGroup();
    }

    private void DrawNewGameTextField()
    {
        float newGameGroup_width = 500 * Mz_GUIManager.Extend_heightScale, newGameGroupHeight = 400f;
        Rect newGameGroup_rect = new Rect(Mz_GUIManager.viewPort_rect.width / 2 - newGameGroup_width/2, Mz_GUIManager.viewPort_rect.height / 2 - newGameGroupHeight / 2, newGameGroup_width, newGameGroupHeight);
        Rect usernameTextfield_rect = new Rect((newGameGroup_rect.width / 2) - ((250 * Mz_GUIManager.Extend_heightScale) / 2), newGameGroup_rect.height / 2 - 50, 250 * Mz_GUIManager.Extend_heightScale, 50);
        Rect startButton_rect = new Rect(50 * Mz_GUIManager.Extend_heightScale, 300, 150 * Mz_GUIManager.Extend_heightScale, 40);
        Rect cancelButton_rect = new Rect(300 * Mz_GUIManager.Extend_heightScale, 300, 150 * Mz_GUIManager.Extend_heightScale, 40);
		
        GUI.BeginGroup(newGameGroup_rect, "New Player", GUI.skin.window);
        {
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
            {
                audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                this.CheckUserNameFormInput();
            }

            if (GUI.Button(startButton_rect, "Start")) {
                this.CheckUserNameFormInput();
            }
            else if (GUI.Button(cancelButton_rect, "Cancel")) {
                sceneState = SceneState.none;
            }

            //<!-- "Please Insert Username !".
            GUI.SetNextControlName("Username");
            username = GUI.TextField(usernameTextfield_rect, username, 13, GUI.skin.textArea);

            if (GUI.GetNameOfFocusedControl() == string.Empty || GUI.GetNameOfFocusedControl() == "")
            {
                GUI.FocusControl("Username");
            }
        } 
        GUI.EndGroup();
    }
    private void CheckUserNameFormInput()
    {
        username.Trim('\n');

        if (username == "" || username == string.Empty || username == null)
        {
            Debug.LogWarning("Username == null");
            _isNullUsernameNotification = true;
            _isDuplicateUsername = false;
        }
        else if (username == player_1 || username == player_2 || username == player_3)
        {
            Debug.LogWarning("Duplicate Username");
            _isDuplicateUsername = true;
            _isNullUsernameNotification = false;
            username = string.Empty;
        }
        else
        {
            _isDuplicateUsername = false;
            _isNullUsernameNotification = false;

            this.EnterUsername();
        }
    }
    //<!-- Enter Username from User. 
    void EnterUsername()
    {
        Debug.Log("EnterUsername");

        //<!-- Autosave Mechanicism. When have empty game slot.  
        if (player_1 == string.Empty)
        {
			Mz_SaveData.SaveSlot = 1;
            this.SaveNewPlayer();
        }
        else if (player_2 == string.Empty)
        {
			Mz_SaveData.SaveSlot = 2;
            this.SaveNewPlayer();
        }
        else if (player_3 == string.Empty)
        {
			Mz_SaveData.SaveSlot = 3;
            this.SaveNewPlayer();
        }
        else
        {
			Mz_SaveData.SaveSlot = 0;
            _isFullSaveGameSlot = true;
			sceneState = SceneState.showLoadGame;

            Debug.LogWarning("<!-- Full Slot Call Showsavegameslot method.");
        }
    }

    //<!-- Show save game slot. If slot is full.
    void DrawSaveGameSlot(bool _toSaveGame)
    {
        float group_width = 500 * Mz_GUIManager.Extend_heightScale, group_height = 380;
        float slot_width = 300 * Mz_GUIManager.Extend_heightScale;
        Rect slot_1Rect = new Rect(group_width/2 - (slot_width/2), 140, slot_width, 48);
        Rect slot_2Rect = new Rect(group_width / 2 - (slot_width / 2), 205, slot_width, 48);
        Rect slot_3Rect = new Rect(group_width / 2 - (slot_width / 2), 270, slot_width, 48);
        Rect textbox_header_rect = new Rect((group_width / 2) - ((200 * Mz_GUIManager.Extend_heightScale) /2), 50, 200 * Mz_GUIManager.Extend_heightScale, 30);

        if (_toSaveGame)
        {
            //<!-- Full save game slot. Show notice message.
            string message = string.Empty;
            //message = "เลือกช่องที่ต้องการ เพื่อลบข้อมูลเก่า และทับด้วยข้อมูลใหม่";
            message = "Select Data Slot To Replace New Data";

            GUI.Box(new Rect(Mz_GUIManager.viewPort_rect.width / 2 - 200, 0, 400, 64), message);
        }

        GUI.BeginGroup(new Rect((Mz_GUIManager.viewPort_rect.width / 2) - group_width / 2, (Main.GAMEHEIGHT / 2) - group_height / 2, group_width, group_height), "Load game", GUI.skin.window);
        {
            if (GUI.Button(new Rect(group_width - (50 * Mz_GUIManager.Extend_heightScale), 0, 50 * Mz_GUIManager.Extend_heightScale, 50), "X"))
            {
                sceneState = SceneState.none;
            }

            if (_toSaveGame)
            {
                #region <!-- To Save game.

                // Display To Save Username.
                GUI.Box(textbox_header_rect, username);
                /// Choose SaveGame Slot for replace new data.
                if (GUI.Button(slot_1Rect, new GUIContent(player_1, "button")))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

					Mz_SaveData.SaveSlot = 1;
                    SaveNewPlayer();
                }
                else if (GUI.Button(slot_2Rect, new GUIContent(player_2, "button")))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

					Mz_SaveData.SaveSlot = 2;
                    SaveNewPlayer();
                }
                else if (GUI.Button(slot_3Rect, new GUIContent(player_3, "button")))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

					Mz_SaveData.SaveSlot = 3;
                    SaveNewPlayer();
                }

                #endregion
            }
            else {
                #region <!-- To Load Game.

                string headerText = "";
                //headerText = "เลือกช่องที่ต้องการใส่ข้อมูลได้เลยครับ";
                headerText = "Select Data Slot";
                // Header.
                GUI.Box(textbox_header_rect, headerText);
                /// Choose SaveGame Slot for Load Save Data.
                string slot_1 = string.Empty;
                string slot_2 = string.Empty;
                string slot_3 = string.Empty;

                if (player_1 == string.Empty) slot_1 = "Empty";
                else slot_1 = player_1;
                if (player_2 == string.Empty) slot_2 = "Empty";
                else slot_2 = player_2;
                if (player_3 == string.Empty) slot_3 = "Empty";
                else slot_3 = player_3;

                #region <!-- GUI data slot button.

                if (GUI.Button(slot_1Rect, new GUIContent(slot_1, "button")))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    if (player_1 != string.Empty)
                    {
						Mz_SaveData.SaveSlot = 1;
                        this.LoadDataToSaveStorage();
                        this.LoadSceneTarget();
                    }
                }
                else if (GUI.Button(slot_2Rect, new GUIContent(slot_2, "button")))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    if (player_2 != string.Empty)
                    {
						Mz_SaveData.SaveSlot = 2;
                        this.LoadDataToSaveStorage();
                        this.LoadSceneTarget();
                    }
                }
                else if (GUI.Button(slot_3Rect, new GUIContent(slot_3, "button")))
                {
                    audioEffect.PlayOnecWithOutStop(audioEffect.buttonDown_Clip);

                    if (player_3 != string.Empty)
                    {
						Mz_SaveData.SaveSlot = 3;
                        this.LoadDataToSaveStorage();
                        this.LoadSceneTarget();
                    }
                }

                #endregion

                #endregion
            }
        }
        GUI.EndGroup();
    }

    void SaveNewPlayer()
    {
		PlayerPrefs.SetString(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_username, username);
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumoffood, 800);
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofwood, 800);
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofstone, 800);
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofgold, 3000);

		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.TownCenter_level, 1);		
		
		//<!--- House instance data. --->>
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.numberOfHouse_Instance, 0);
        //<!-- Academy instance data.
        PlayerPrefsX.SetBool(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_AcademyInstance, false);

        #region <!-- Resource section.
        
        //<!-- Farm Data.
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.amount_farm_instance, 0);
		//<!-- Sawmill Data.
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.amount_sawmill_instance, 0);
		//<!-- MillStone Data.
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.amount_millstone_instance, 0);	
		//<!-- Smelter Data.
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.amount_smelter_instance, 0);

        #endregion

        //<!-- Save Storehouse data.
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.numberOfStorehouseInstance, 0);
		//<!-- Set default value of "MarketInstance".
		PlayerPrefsX.SetBool(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_MarketInstance, false);
		
		//<!-- Save Barracks data.
		PlayerPrefs.SetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.numberOf_BarracksInstancs, 0);

        this.LoadDataToSaveStorage();

        this.LoadSceneTarget();
    }
    private void LoadSceneTarget()
    {
        if (Application.isLoadingLevel == false)
        {
            Mz_LoadingScreen.TargetSceneName = Mz_BaseScene.ScenesInstance.Town.ToString();
            Application.LoadLevelAsync(Mz_BaseScene.ScenesInstance.LoadingScreen.ToString());
        }
    }

    private void LoadDataToSaveStorage()
    {
		Mz_SaveData.Username = PlayerPrefs.GetString(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_username);
		StoreHouse.sumOfFood = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumoffood);
		StoreHouse.sumOfWood = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofwood);
		StoreHouse.sumOfGold = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofgold);
		StoreHouse.sumOfStone = PlayerPrefs.GetInt(Mz_SaveData.SaveSlot + ":" + Mz_SaveData.KEY_sumofstone);

        Debug.Log("Load storage data to static variable complete.");

        if(!Application.isLoadingLevel) {
            Mz_LoadingScreen.TargetSceneName = Mz_BaseScene.ScenesInstance.Town.ToString();
            Application.LoadLevel(Mz_BaseScene.ScenesInstance.LoadingScreen.ToString());
        }
    }
}