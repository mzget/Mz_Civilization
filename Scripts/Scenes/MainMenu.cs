using UnityEngine;
using System.Collections;


public class MainMenu : Mz_BaseScene
{
	// Set Build Version to Unlock All Item has locked with score.
	public enum BuildVersion : int { debugMode = 0, releaseMode = 1, };
	public BuildVersion buildVersion;

    public GUISkin newgame_Skin;

    public enum GUIState { none = 0, showOption, showNewGame, showSaveGame, };
    private GUIState guiState;
    public enum NewGameUIState { none = 0, showTextField, showDuplicateName, showSaveGameSlot, };
    private NewGameUIState newgameUIState;
    public NewGameUIState NewGameGUIState { get { return newgameUIState; } }
	
	//<!-- Audio Manage.	
	public static bool ToggleAudioActive = true;
    private string hoverName = string.Empty;            //<!-- name of gui tooltip.
    private AudioEffectManager audioEffect;
//    private AudioDescribeManager audioDescribe;
    private GameObject audioBackground;

    private bool _showNotification = false;
    private bool _duplicateName = false;
    public string username = string.Empty;
    

    Rect gameWindow_rect = new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 150, 400, 300);
    public Rect usernameTextfield_rect;
    public Rect doneButton_rect;


    void Awake() {
        usernameTextfield_rect = new Rect(50, 120, 300, 64);
        doneButton_rect = new Rect(gameWindow_rect.width / 2 - 50, 200, 100, 32); 

        if(Screen.height != Main.GAMEHEIGHT) {
            //gameWindow_rect = MzCalculateScaleRectGUI.CalculateScale_MiddleCenter_GUIRect(gameWindow_rect);
            //usernameTextfield_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(usernameTextfield_rect);
            //doneButton_rect = MzCalculateScaleRectGUI.CalculateScale_TopLeft_GUIRect(doneButton_rect);
        }
    }

    // Use this for initialization
    void Start() 
	{
		if(PlayerPrefs.HasKey(Mz_SaveData.usernameKey)) {
            username = PlayerPrefs.GetString(Mz_SaveData.usernameKey);				
			guiState = GUIState.showSaveGame; 
		}
		else {			
			guiState = GUIState.showNewGame;			
            if (newgameUIState != NewGameUIState.showTextField) {
                newgameUIState = NewGameUIState.showTextField;
			}
		}
		
        //<!-- Setup All Audio Objects.
        audioEffect = GameObject.FindGameObjectWithTag("AudioEffect").GetComponent<AudioEffectManager>();
        //audioDescribe = GameObject.FindGameObjectWithTag("AudioDescribe").GetComponent<AudioDescribeManager>();
        //audioBackground = GameObject.FindGameObjectWithTag("AudioObject");

        audioEffect.audio.mute = !ToggleAudioActive;
        //audioDescribe.audio.mute = !ToggleAudioActive;
        //audioBackground.audio.mute = !ToggleAudioActive;
    }

    // Update is called once per frame
    void Update() {
		
    }

    void OnGUI() 
    {
        //GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.height / Main.GAMEHEIGHT, Screen.height / Main.GAMEHEIGHT, 1));

        if (guiState == GUIState.none)
        {
            //<!-- New Game.
            if (GUI.Button(new Rect(Main.GAMEWIDTH - 385, 275, 180, 80), new GUIContent("New", "Button"), newgame_Skin.button))
            {
                audioEffect.PlaySoundClickButton();

                if (guiState != GUIState.showNewGame) 
                    guiState = GUIState.showNewGame;

                if (newgameUIState != NewGameUIState.showTextField)
                    newgameUIState = NewGameUIState.showTextField;
            }
            //<!-- Load Game. -->
            else if (GUI.Button(new Rect(Main.GAMEWIDTH - 195, 260, 180, 80), new GUIContent("Load", "Button"), newgame_Skin.button))
            {
                audioEffect.PlaySoundClickButton();

                if (guiState != GUIState.showSaveGame)
                    guiState = GUIState.showSaveGame;
            }
            //<!-- Exit.
            else if (GUI.Button(new Rect(Main.GAMEWIDTH - 195, 360, 180, 80), new GUIContent("Exit", "Button"), newgame_Skin.button))
            {
                audioEffect.PlaySoundClickButton();

                Application.Quit();
            }
            //<!-- Option"
            else if (GUI.Button(new Rect(Main.GAMEWIDTH - 380, 400, 180, 118), new GUIContent("Options", "Button"), newgame_Skin.button))
            {
                audioEffect.PlaySoundClickButton();

                if (guiState != GUIState.showOption)
                    guiState = GUIState.showOption;
            }
        }
        else if (guiState == GUIState.showNewGame)
            ShowNewGame();
        else if (guiState == GUIState.showOption)
            return;
        else if (guiState == GUIState.showSaveGame)
            this.ShowSaveGameSlot();
		
		#region Show Notification When Username have a problem.

		string notificationText = "";
		string dublicateNoticeText = "";

#if UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
			notificationText = "Please Fill Your Username. \n ¡ÃØ³ÒãÊèª×èÍŒÙéàÅè¹ãËÁè";
			dublicateNoticeText = "This name already exists. \n ª×èÍ¹Õé¶Ù¡ãªéä»áÅéÇ";
#endif

#if UNITY_IPHONE || UNITY_ANDROID
			notificationText = "Please Fill Your Username.";
			dublicateNoticeText = "This name already exists.";
#endif

        if (_showNotification)
            GUI.Box(new Rect(Main.GAMEWIDTH / 2 - 256, 0, 512, 50), notificationText);
        if (_duplicateName)
            GUI.Box(new Rect(Main.GAMEWIDTH / 2 - 256, 0, 512, 50), dublicateNoticeText);

		#endregion
    }

    void ShowNewGame()
    {
        if (newgameUIState == NewGameUIState.showTextField)
        {
            //<!-- "Please Insert Username !".
            GUI.BeginGroup(gameWindow_rect, new GUIContent(string.Empty, "NewGameWindow"), newgame_Skin.window);
            {        
		        //<!-- Enter or Return KeyDown.
		        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return) {
                    audioEffect.PlaySoundClickButton();
                    StartCoroutine(CheckUserNameFormInput());		
		        }
				
                //<!-- "Done button"
                if (GUI.Button(doneButton_rect, new GUIContent("Done")))
                {
                    audioEffect.PlaySoundClickButton();
                    StartCoroutine(CheckUserNameFormInput());
                }

                // Enter Username.
                GUI.SetNextControlName("Username");
                username = GUI.TextField(usernameTextfield_rect, username, 13, newgame_Skin.textField);
                if (GUI.GetNameOfFocusedControl() == "")
                {
                    GUI.FocusControl("Username");
                }
            }
            GUI.EndGroup();
        }
        else if (newgameUIState == NewGameUIState.showSaveGameSlot)
        {
            // Call ShowSaveGameSlot Method.
            ShowSaveGameSlot();
        }
    }

    IEnumerator CheckUserNameFormInput() 
	{	       
		yield return new WaitForFixedUpdate();
		username.Trim('\n');
	    if (username != string.Empty && username.Length >= 3) {
            _duplicateName = false;
            _showNotification = false;
			
			SaveNewPlayer();
	    }
    }
	
	/// <summary>
	/// Shows the save game slot. If slot is full.
	/// </summary>
    void ShowSaveGameSlot()
    {
        GUI.BeginGroup(gameWindow_rect, GUIContent.none, newgame_Skin.window);
        {
			if(GUI.Button(new Rect(136,118,128,32), username)) {
                this.LoadDataToSaveStorage();
			}
			else if(GUI.Button(new Rect(136,170,128,32), "Delete Account.")) {
				username = "";
				PlayerPrefs.DeleteAll();
				
				if(guiState != GUIState.showNewGame)
					guiState = GUIState.showNewGame;
				if(newgameUIState != NewGameUIState.showTextField)
					newgameUIState = NewGameUIState.showTextField;
			}
        }
        GUI.EndGroup();
    }

    void SaveNewPlayer()
    {
        PlayerPrefs.SetString(Mz_SaveData.usernameKey, username);
        PlayerPrefs.SetInt(username + ":" + Mz_SaveData.sumoffood, 500);
        PlayerPrefs.SetInt(username + ":" + Mz_SaveData.sumofwood, 500);
        PlayerPrefs.SetInt(username + ":" + Mz_SaveData.sumofstone, 500);
        PlayerPrefs.SetInt(username + ":" + Mz_SaveData.sumofgold, 2000);
		
		PlayerPrefs.SetInt(username + ":" + Mz_SaveData.TownCenter_level, 1);

        this.LoadDataToSaveStorage();
    }

    private void LoadDataToSaveStorage() 
    {
        StorageManage.Username = PlayerPrefs.GetString(Mz_SaveData.usernameKey);
        StoreHouse.sumOfFood = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.sumoffood);
        StoreHouse.sumOfWood = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.sumofwood);
        StoreHouse.sumOfGold = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.sumofgold);
        StoreHouse.sumOfStone = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.sumofstone);
        HouseBeh.SumOfEmployee = PlayerPrefs.GetInt(StorageManage.Username + ":" + Mz_SaveData.sumOfEmployee);

        if(!Application.isLoadingLevel) {
            Mz_LoadingScreen.SceneName = Mz_BaseScene.ScenesInstance.Town.ToString();
            Application.LoadLevel(Mz_BaseScene.ScenesInstance.LoadingScreen.ToString());
        }
    }
}