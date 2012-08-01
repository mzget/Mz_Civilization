using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public GUISkin mainBuildingSkin;
    public GUISkin mainInterface;
    ///<summary>
    /// Texture. 
    ///</summary>
    public Texture2D mapTex;
    public enum _clickedName { building = 0, none };
    private _clickedName clickState = _clickedName.none;

    private OTFilledSprite background;

    public const float GameWidth = 800f;
    public const float GameHeight = 500f; 
    public static float worldTime;

    /// <summary>
    /// Private Data Fields.
    /// </summary>
    private bool _Clicked = false;
    private bool _preBuild = false;
    private Vector2 scrollPosition = Vector2.zero;
    private Rect mainGUIRect = new Rect(Main.GameWidth / 2 - 300, Main.GameHeight - 100, 600, 100);
    private Rect windowRect = new Rect(Main.GameWidth / 2 - 300, Main.GameHeight / 2 - 150, 600, 320);
    private Rect imgRect = new Rect(30, 80, 100, 100);
    private Rect contentRect = new Rect(160, 40, 400, 200);
    private Rect buttonRect = new Rect(460, 200, 100, 30);
    
    
	// Use this for initialization
	void Start () {
		GenerateBackground();
	}
	
    void GenerateBackground()
    {
        // To create the background lets create a filled sprite object
        background = OT.CreateObject(OTObjectType.FilledSprite).GetComponent<OTFilledSprite>();
        // Set the image to our wyrmtale tile
        background.image = mapTex;
        // But this all the way back so all other objects will be located in front.
        background.depth = 10;
        // Set material reference to 'custom' green material - check OT material references
//        background.materialReference = "white";
        // Set the size to match the screen resolution.
        background.size = new Vector2(5120, 1536);
        // Set the fill image size to 50 x 50 pixels
        background.fillSize = new Vector2(128, 128);

        background.name = "Background";
    }
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Mz_SmartDeviceInput.IOS_GUITouch();
        }
    }
}
