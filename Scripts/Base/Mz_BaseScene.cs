using UnityEngine;
using System;
using System.Collections;

public class Mz_BaseScene : MonoBehaviour {

    public class ResourcePathName {
        public const string PathOfGUI_PREFABS = "GUI_objects/";
    }

    public enum ScenesInstance { 
        LoadingScreen = 0,
        Startup = 1,
        MainMenu,
        Town,
    }
    public static ScenesInstance SceneInstance;
    protected bool _hasQuitCommand = false;
    /// Detect Touch and Input
    public Touch touch;
    public Vector3 mousePos;
    public Vector3 originalPos;
    public Vector3 currentPos;
	private Vector3[] mainCameraPos = new Vector3[] { new Vector3(0,0,-10), new Vector3(2.66f,0,-10) };
	private Vector3 currentCameraPos = new Vector3(0, 0, -10);
    public bool _isDragMove = false;

    /// <summary>
    /// Audio data.
    /// </summary>
    protected static bool ToggleAudioActive = true;
    public AudioEffectManager audioEffect;
    public AudioDescribeManager audioDescribe;
    public GameObject audioBackground_Obj;

    protected void InitializeAudio()
    {
        Debug.Log("Scene :: InitializeAudio");

        //<!-- Setup All Audio Objects.
        if (audioEffect == null)
        {
            audioEffect = GameObject.FindGameObjectWithTag("AudioEffect").GetComponent<AudioEffectManager>();

            if (audioEffect)
            {
                audioEffect.alternativeEffect_source = audioEffect.transform.GetComponentInChildren<AudioSource>();

                audioEffect.audio.mute = !ToggleAudioActive;
                audioEffect.alternativeEffect_source.audio.mute = !ToggleAudioActive;
            }
        }

        if (audioDescribe == null)
        {
            try
            {
                audioDescribe = GameObject.FindGameObjectWithTag("AudioDescribe").GetComponent<AudioDescribeManager>();
                audioDescribe.audio.mute = !ToggleAudioActive;
            }
            catch {
                audioDescribe = null;
            }
        }

        if (audioBackground_Obj == null)
        {
            audioBackground_Obj = GameObject.FindGameObjectWithTag("AudioBackground");
            audioBackground_Obj.audio.mute = !ToggleAudioActive;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Mz_SmartDeviceInput.IOS_INPUT();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
        {
            //Application.Quit();
            _hasQuitCommand = true;
        }
    }

	#region <!-- HasChangeTimeScale event.

	public static event EventHandler HasChangeTimeScale_Event;
	private void OnChangeTimeScale (EventArgs e) {
		if (HasChangeTimeScale_Event != null) 
            HasChangeTimeScale_Event (this, e);
	}
	protected void UpdateTimeScale(int delta) {
		Time.timeScale = delta;
		OnChangeTimeScale(EventArgs.Empty);
	}

	#endregion

    protected void ImplementTouchPostion ()
	{	
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            if(Input.touchCount > 0) {				
            	touch = Input.GetTouch(0);
				
	            if(touch.phase == TouchPhase.Began) {			
					originalPos = touch.position;
					currentPos = touch.position;
	            }

	            if(touch.phase == TouchPhase.Moved) {
					currentPos = touch.position;
                    this.MovingCameraTransform();   					
	            }
				
	            if(touch.phase == TouchPhase.Ended) {
                    /*
					float distance = Vector2.Distance (currentPos, originalPos);
					float vector = currentPos.x - originalPos.x;
//					float speed = Time.deltaTime * (distance / touch.deltaTime);
					if (vector < 0) {
						if(distance > 200)
							currentCameraPos = mainCameraPos[1];
					}
					else if (vector > 0) {
						if(distance > 200)
							currentCameraPos = mainCameraPos[0];
					}
						
					iTween.MoveTo (Camera.main.gameObject, iTween.Hash("position", currentCameraPos, "time", 0.5f, "easetype", iTween.EaseType.linear));
					*/
					currentPos = Vector2.zero;
					originalPos = Vector2.zero;
	            }
            }
        }
		else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
			mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				
			if(Input.GetMouseButtonDown(0)) {
				originalPos = mousePos;
                currentPos = mousePos;
			}
				
			if(Input.GetMouseButton(0)) {
				currentPos = mousePos;
                _isDragMove = true;
				this.MovingCameraTransform();

                Debug.Log("originalPos == " + originalPos + " :: " + "currentPos == " + currentPos);
			}

            if (Input.GetMouseButtonUp(0)) {
                _isDragMove = false;
                originalPos = Vector3.zero;
                currentPos = Vector3.zero;
            }
		}
    }
	protected virtual void MovingCameraTransform ()
	{

	}

    public virtual void OnInput(string nameInput)
    {
        //    	Debug.Log("OnInput :: " + nameInput);
    }

    public virtual void OnPointerOverName(string nameInput)
    {
        //    	Debug.Log("OnPointerOverName :: " + nameInput);
    }

    void OnApplicationQuit()
    {
        Mz_SaveData.Save();

#if UNITY_STANDALONE_WIN
        Application.CancelQuit();

        if(!Application.isLoadingLevel) {
            Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.MainMenu.ToString();
            Application.LoadLevelAsync(Mz_BaseScene.SceneNames.LoadingScene.ToString());
        }
#endif

#if UNITY_IPHONE || UNITY_ANDROID
        //<-- to do asking for quit game.
#endif
    }
	

    protected virtual void OnGUI() {
		GUI.depth = 0;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));

        if(_hasQuitCommand) {
            GUI.BeginGroup(new Rect(Main.GAMEWIDTH / 2 - 150, Main.GAMEHEIGHT / 2 - 100, 300, 200), "Do you want to quit ?", GUI.skin.window);
            {
                if (GUI.Button(new Rect(40, 120, 100, 32), "Yes"))
                    Application.Quit();
                else if (GUI.Button(new Rect(160, 120, 100, 32), "No"))
                    _hasQuitCommand = false;
            }
            GUI.EndGroup();
        }
    }


    public string output = "";
    public string stack = "";
    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }
    void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }
    public void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
    }
}
