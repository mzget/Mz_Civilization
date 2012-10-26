using UnityEngine;
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
    /// <summary>
    /// Detect Touch and Input
    /// </summary>
    public Touch touch;
    /// <summary>
    /// UNITY_EDITOR
    /// </summary>
    public Vector3 mousePos;
    public Vector3 originalPos;
    public Vector3 currentPos;
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
            Application.Quit();
            return;
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log(touch.phase);
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    Debug.Log(touch.phase);
                    this.CheckTouchPostionAndMove();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Debug.Log(touch.phase);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                originalPos = mousePos;
                //Debug.Log("originalPos == " + originalPos);
            }

            if (Input.GetMouseButton(0))
            {
                currentPos = mousePos;
                _isDragMove = true;
                this.CheckTouchPostionAndMove();
                //Debug.Log("currentPos == " + currentPos);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragMove = true;
                originalPos = Vector3.zero;
                currentPos = Vector3.zero;
            }
        }
    }

    protected virtual void CheckTouchPostionAndMove()
    {
        //        Debug.Log("CheckTouchPostionAndMove");
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
        PlayerPrefs.Save();

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
