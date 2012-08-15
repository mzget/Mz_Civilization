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
    
	void Awake() 
	{
        amountFarmInstance = PlayerPrefs.GetInt("amountFarmInstance");
		if(amountFarmInstance != 0) {
	        for (int i = 0; i < amountFarmInstance; i++)
	        {
				GameObject farm_instance = Instantiate(Resources.Load("Buildings/Economy/Farm", typeof(GameObject))) as GameObject;
				Farm newFarm = farm_instance.GetComponent<Farm>();
				newFarm.buildingStatus = Buildings.BuildingStatus.none;
				
				Buildings.FarmInstance.Add(newFarm);
	            int temp_Level = PlayerPrefs.GetInt("farm_Level_" + i);
                Buildings.FarmInstance[i].level = temp_Level;

//                string temp_Position = PlayerPrefs.GetString("farm_Position_" + i);
//                Buildings.FarmInstance[i].buildingPosition_Data = temp_Position;
//				string[] arr_Pos = temp_Position.Split('|');
//              	newFarm.transform.position = new Vector3(float.Parse(arr_Pos[0]), float.Parse(arr_Pos[1]), float.Parse(arr_Pos[2]));
				int loadPos = PlayerPrefs.GetInt("farm_Position_" + i);
				newFarm.transform.position = StageManager.buildingArea_Pos[loadPos];
	        }
		}
		
		Debug.Log("Loading...");
	}
	
	// Use this for initialization
	void Start () {
		this.CreateObjectsPool();
		
		this.GenerateBackground();		
		this.CreateBuildingArea();
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
