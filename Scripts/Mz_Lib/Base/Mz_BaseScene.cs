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
    public bool _hasQuitCommand = false;
    /// Detect Touch and Input
    private Mz_SmartDeviceInput smartDeviceInput;
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
            var obj = GameObject.FindGameObjectWithTag("AudioDescribe");
            if (obj != null)
            {
                audioDescribe = obj.GetComponent<AudioDescribeManager>();
                audioDescribe.audio.mute = !ToggleAudioActive;
            }
        }

        /// <! Manage audio background.
        audioBackground_Obj = GameObject.FindGameObjectWithTag("AudioBackground");
        if (audioBackground_Obj == null)
        {
            audioBackground_Obj = new GameObject("AudioBackground", typeof(AudioSource));
            audioBackground_Obj.tag = "AudioBackground";
            audioBackground_Obj.audio.playOnAwake = true;
            audioBackground_Obj.audio.mute = !ToggleAudioActive;

            DontDestroyOnLoad(audioBackground_Obj);
        }
        else
        {
            audioBackground_Obj.audio.mute = !ToggleAudioActive;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
		if (smartDeviceInput == null) {
			this.gameObject.AddComponent<Mz_SmartDeviceInput>();
			smartDeviceInput = this.gameObject.GetComponent<Mz_SmartDeviceInput>();
			smartDeviceInput.cam = Camera.main;
		}

        if (TaskManager.IsShowInteruptGUI == false)
        {
            if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                smartDeviceInput.ImplementTouchInput();
            //if (Application.isWebPlayer || Application.isEditor)
            //    smartDeviceInput.ImplementMouseInput();
        }

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
/*		else if(Application.platform == RuntimePlatform.WindowsEditor || 
            Application.platform == RuntimePlatform.OSXEditor)
        {
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
		}*/
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
    
    protected virtual void OnGUI()
    {
        GUI.depth = 0;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));

        if (_hasQuitCommand)
        {
			TaskManager.IsShowInteruptGUI = true;
			
            GUI.BeginGroup(new Rect(Screen.width / 2 - (200 * Mz_OnGUIManager.Extend_heightScale), Main.GAMEHEIGHT / 2 - 100, 400 * Mz_OnGUIManager.Extend_heightScale, 200), "Do you want to quit ?", GUI.skin.window);
            {
                if (GUI.Button(new Rect(60 * Mz_OnGUIManager.Extend_heightScale, 155, 100 * Mz_OnGUIManager.Extend_heightScale, 40), "Yes"))
                    Application.Quit();
                else if (GUI.Button(new Rect(240 * Mz_OnGUIManager.Extend_heightScale, 155, 100 * Mz_OnGUIManager.Extend_heightScale, 40), "No")) {
                    _hasQuitCommand = false; 
					TaskManager.IsShowInteruptGUI = false;
				}
            }
            GUI.EndGroup();
        }

        #region <@!-- Draw_LogCallback debuging.

        //if (debugLogCallback_style == null) {
        //    debugLogCallback_style = new GUIStyle(GUI.skin.box);
        //    debugLogCallback_style.fontSize = 12;
        //    debugLogCallback_style.alignment = TextAnchor.MiddleLeft;
        //}

        //GUI.Box(new Rect(0, Main.GAMEHEIGHT - 50, Main.GAMEWIDTH * Mz_OnGUIManager.Extend_heightScale, 50), output, debugLogCallback_style);

        #endregion
    }

    void OnApplicationQuit()
    {
        Mz_SaveData.Save();
    }

    public virtual void OnDispose() { }

    #region <!-- Unity Log Callback.
/*
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
*/
    #endregion
}
