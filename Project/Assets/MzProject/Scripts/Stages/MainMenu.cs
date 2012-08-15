using UnityEngine;
using System.Collections;


public class MainMenu : MonoBehaviour
{
	// Set Build Version to Unlock All Item has locked with score.
	public enum BuildVersion : int { debugMode = 0, releaseMode = 1, };
	public BuildVersion buildVersion;
	public static BuildVersion Version;
    public enum AppLanguage { defualt_En = 0, Thai, };
    public static AppLanguage CurrentAppLanguage = AppLanguage.defualt_En;

    public GUISkin mainMenuSkin;
    public GUISkin newgame_Skin;
    public GUISkin loadGameMenuSkin;
	public GUISkin optionMenuSkin;
	
    public enum GUIState { none = 0, showOption, showNewGame, showSaveGame, };
    private GUIState guiState;

    public enum NewGameUIState { none = 0, showTextField, showDuplicateName, showSaveGameSlot, };
	[System.NonSerialized]
    public NewGameUIState newgameUIState; 
	
	//<!-- Audio Manage.	
	public static bool ToggleAudioActive = true;
    private string hoverName = string.Empty;            /// name of gui tooltip.
    private AudioEffectManager audioEffect;
//    private AudioDescribeManager audioDescribe;
    private GameObject audioBackground;

    private bool _showNotification = false;
    private bool _duplicateName = false;
    private bool _hitEnter = false;
    private static string username = "";  
	public static string Username { get {return username;} set {username = value;}}



    // Use this for initialization
    void Start() 
	{
		if(PlayerPrefs.HasKey("username")) {
			username = PlayerPrefs.GetString("username");
				
			if(username == "") {
				guiState = GUIState.showNewGame;
				
	            if (newgameUIState != NewGameUIState.showTextField) {
	                newgameUIState = NewGameUIState.showTextField;
				}
			}
			else
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
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Main.FixedWidthRatio, Main.FixedHeightRatio, 1));

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
            this.ShowSaveGameSlot(false);
		
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

//        mainMenuSkin.textField.fontSize = 16;
//        mainMenuSkin.textField.normal.textColor = Color.red;

        if (_showNotification)
            GUI.Box(new Rect(Main.GAMEWIDTH / 2 - 256, 0, 512, 50), notificationText, mainMenuSkin.textField);
        if (_duplicateName)
            GUI.Box(new Rect(Main.GAMEWIDTH / 2 - 256, 0, 512, 50), dublicateNoticeText, mainMenuSkin.textField);

		#endregion
    }

    void ShowNewGame()
    {
        Rect newGameRect = new Rect(Main.GAMEWIDTH / 2 - 250, Main.GAMEHEIGHT / 2 - 175, 500, 350);

        if (newgameUIState == NewGameUIState.showTextField)
        {
            //<!-- "Please Insert Username !".
            GUI.BeginGroup(newGameRect, new GUIContent(string.Empty, "NewGameWindow"), newgame_Skin.window);
            {
                /// Enter or Return KeyDown.
                if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && _hitEnter == false)
                {
                    _hitEnter = true;

                    audioEffect.PlaySoundClickButton();
                }
                //<!-- "Start"
                else if (GUI.Button(new Rect(192, 240, 128, 32), new GUIContent("Done", "Button"))) {
                    audioEffect.PlaySoundClickButton();
					CheckUserNameFormInput();
                }

                // Enter Username.
                GUI.SetNextControlName("Username");
                username = GUI.TextField(new Rect(106, 128, 300, 128), username, 13, newgame_Skin.textField);

                if (GUI.GetNameOfFocusedControl() == string.Empty || GUI.GetNameOfFocusedControl() == "")
                {
                    GUI.FocusControl("Username");
                }
            }
            GUI.EndGroup();
        }
        else if (newgameUIState == NewGameUIState.showSaveGameSlot)
        {
            // Call ShowSaveGameSlot Method.
            ShowSaveGameSlot(true);
        }
    }

    void CheckUserNameFormInput() 
	{	
        username.Trim('\n');
	    if (username != "" && username != null) {
            _duplicateName = false;
            _showNotification = false;
		    Debug.Log("Enter");
			
			SaveNewPlayer();
	    }
    }
	
	/// <summary>
	/// Shows the save game slot. If slot is full.
	/// </summary>
	/// <param name='toSaveGame'>
	/// To save game.
	/// </param>
    void ShowSaveGameSlot(bool toSaveGame)
    {
        GUI.BeginGroup(new Rect((Main.GAMEWIDTH  / 2) - 256, (Main.GAMEHEIGHT / 2) - 200, 512, 400), GUIContent.none, newgame_Skin.window);
        {
			if(GUI.Button(new Rect(192,175,128,32), username)) {
                this.LoadNewDataToSaveStorage();
			}
			else if(GUI.Button(new Rect(192,224,128,32), "Delete Account.")) {
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
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetInt("sumoffood", 500);
		PlayerPrefs.SetInt("sumofwood", 500);
		PlayerPrefs.SetInt("sumofgold", 500);
		PlayerPrefs.SetInt("sumofstone", 500);
        PlayerPrefs.SetInt("farm_Level", 0);
        PlayerPrefs.SetInt("sawmill_Level", 0);
		PlayerPrefs.SetInt("millstone_Level", 0);
		PlayerPrefs.SetInt("smelter_Level", 0);

        this.LoadNewDataToSaveStorage();
    }

    private void LoadNewDataToSaveStorage() 
    {
        StorageManage.Username = PlayerPrefs.GetString("username");
        StoreHouse.sumOfFood = PlayerPrefs.GetInt("sumoffood");
        StoreHouse.sumOfWood = PlayerPrefs.GetInt("sumofwood");
        StoreHouse.sumOfGold = PlayerPrefs.GetInt("sumofgold");
        StoreHouse.sumOfStone = PlayerPrefs.GetInt("sumofstone");
		
//        Buildings.FarmInstance.level = PlayerPrefs.GetInt("farm_Level");
//        Buildings.SawmillInstance.level = PlayerPrefs.GetInt("sawmill_Level");
//		Buildings.MillStoneInstance.level = PlayerPrefs.GetInt("millstone_Level");
//		Buildings.SmelterInstance.level = PlayerPrefs.GetInt("smelter_Level");
        
        if(!Application.isLoadingLevel) 
		{
            Mz_LoadingScreen.SceneName = Scenes.ScenesInstance.Town.ToString();
            Application.LoadLevel(Scenes.ScenesInstance.LoadingScreen.ToString());
        }
    }
}