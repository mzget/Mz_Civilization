using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour {

    public GUISkin mainBuildingSkin;
    public GUISkin mainInterface;
    ///<summary>
    /// Texture. 
    ///</summary>
    public Texture2D mapTex;
    public enum _clickedName { building = 0, none };
//    private _clickedName clickState = _clickedName.none;

    private OTFilledSprite background;
	public static List<Vector2> buildingArea_Pos = new List<Vector2>(12) {
		new	Vector2(0, 180), new Vector2(138, 100), new Vector2(138, -100),
		new Vector2(0, -180), new Vector2(-138, -100), new Vector2(-138, 100),
		new Vector2(300, 180), new Vector2(300, 0), new Vector2(300, -180),
		new Vector2(-300, 180), new Vector2(-300, 0), new Vector2(-300, -180),
	};
	public static List<BuildingArea> buildingArea_Obj = new List<BuildingArea>(12);
    //<!-- Private Data Fields.

//    private bool _Clicked = false;
//    private bool _preBuild = false;
    private Vector2 scrollPosition = Vector2.zero;
    private Rect mainGUIRect = new Rect(Main.GAMEWIDTH / 2 - 300, Main.GAMEHEIGHT - 100, 600, 100);
    private Rect windowRect = new Rect(Main.GAMEWIDTH / 2 - 300, Main.GAMEHEIGHT / 2 - 150, 600, 320);
    private Rect imgRect = new Rect(30, 80, 100, 100);
    private Rect contentRect = new Rect(160, 40, 400, 200);
    private Rect buttonRect = new Rect(460, 200, 100, 30);
	
    int amountFarmInstance = 0;
	int amountSawmillInstance = 0;
	int amountMillStoneInstance = 0;
	int amountSmelterInstance = 0;
   
	
	void Awake() {
		this.CreateObjectsPool();
	}
	
	// Use this for initialization
	void Start () {	
		this.GenerateBackground();		
		this.CreateBuildingArea();
		this.LoadingDataStore();
	}
	
	private void CreateObjectsPool() {
		OT.PreFabricate("Building_Area", 16);
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
        background.materialReference = "white";
        // Set the size to match the screen resolution.
        background.size = new Vector2(5120, 1536);
        // Set the fill image size to 50 x 50 pixels
        background.fillSize = new Vector2(128, 128);

        background.name = "Background";
    }
	void CreateBuildingArea() 
	{		
		if(buildingArea_Obj.Count < buildingArea_Obj.Capacity) {			
			for(int i = 0; i < buildingArea_Obj.Capacity; i++) {
				GameObject Temp_obj = OT.CreateObject("Building_Area");
				buildingArea_Obj.Add(Temp_obj.GetComponent<BuildingArea>());
				buildingArea_Obj[i].transform.position = buildingArea_Pos[i];
				buildingArea_Obj[i].IndexOfArea = i;
			}
		}
	}
	void LoadingDataStore() 
	{
        amountFarmInstance = PlayerPrefs.GetInt("amount_farm_instance");
		amountSawmillInstance = PlayerPrefs.GetInt("amount_sawmill_instance");
		amountMillStoneInstance = PlayerPrefs.GetInt("amount_millStone_instance");
		amountSmelterInstance = PlayerPrefs.GetInt("amount_smelter_instance");
		if(amountFarmInstance != 0) {
	        for (int i = 0; i < amountFarmInstance; i++)
	        {
				GameObject farm_instance = Instantiate(Resources.Load("Buildings/Economy/Farm", typeof(GameObject))) as GameObject;
				Farm newFarm = farm_instance.GetComponent<Farm>();	
				
	            int load_Level = PlayerPrefs.GetInt("farm_Level_" + i);
				int loadPos = PlayerPrefs.GetInt("farm_Position_" + i);
				Buildings.FarmInstance.Add(newFarm);
				Buildings.FarmInstance[i].currentBuildingStatus = Buildings.BuildingStatus.none;				
                Buildings.FarmInstance[i].level = load_Level;
				Buildings.FarmInstance[i].transform.position = StageManager.buildingArea_Pos[loadPos];
				
				StageManager.buildingArea_Obj[loadPos].gameObject.SetActive(false);
	        }
		}
		
		Debug.Log("Loading...");
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Mz_SmartDeviceInput.IOS_GUITouch();
        }
    }

    void OnApplicationQuit() {
        Mz_SaveData.Save();
    }
}
