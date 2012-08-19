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
	
    int amount_Farm_Instance = 0;
	int amount_Sawmill_Instance = 0;
	int amount_MillStone_Instance = 0;
	int amount_Smelter_Instance = 0;
   
	
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
    void GenerateBackground() {
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
	void CreateBuildingArea() {		
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
        amount_Farm_Instance = PlayerPrefs.GetInt(Mz_SaveData.amount_farm_instance);
		amount_Sawmill_Instance = PlayerPrefs.GetInt(Mz_SaveData.amount_sawmill_instance);
		amount_MillStone_Instance = PlayerPrefs.GetInt(Mz_SaveData.amount_millstone_instance);
		amount_Smelter_Instance = PlayerPrefs.GetInt(Mz_SaveData.amount_smelter_instance);
		
		if(amount_Farm_Instance != 0) {
	        for (int i = 0; i < amount_Farm_Instance; i++)
	        {
				GameObject new_farm = Instantiate(Resources.Load("Buildings/Economy/Farm", typeof(GameObject))) as GameObject;
				Farm farm = new_farm.GetComponent<Farm>();	
				
	            int Level = PlayerPrefs.GetInt(Mz_SaveData.farm_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_SaveData.farm_position_ + i);
				
				farm.currentBuildingStatus = Buildings.BuildingStatus.none;				
                farm.level = Level;				
				farm.transform.position = StageManager.buildingArea_Pos[Position];	
				farm.IndexOfPosition = Position;
				
				Buildings.Farm_Instance.Add(farm);	
				StageManager.buildingArea_Obj[Position].gameObject.SetActive(false);
	        }
		}
		
		if(amount_Sawmill_Instance != 0) {
	        for (int i = 0; i < amount_Sawmill_Instance; i++)
	        {
				GameObject new_Sawmill = Instantiate(Resources.Load("Buildings/Economy/Sawmill", typeof(GameObject))) as GameObject;
				Sawmill sawmill = new_Sawmill.GetComponent<Sawmill>();	
				
	            int Level = PlayerPrefs.GetInt(Mz_SaveData.sawmill_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_SaveData.sawmill_position_ + i);
                
				sawmill.currentBuildingStatus = Buildings.BuildingStatus.none;				
                sawmill.level = Level;
				sawmill.transform.position = StageManager.buildingArea_Pos[Position];
				sawmill.IndexOfPosition = Position;
				
				Buildings.Sawmill_Instance.Add(sawmill);
				StageManager.buildingArea_Obj[Position].gameObject.SetActive(false);
	        }
		}
		
		if(amount_MillStone_Instance != 0) {
	        for (int i = 0; i < amount_MillStone_Instance; i++)
	        {
				GameObject new_millstone = Instantiate(Resources.Load("Buildings/Economy/MillStone", typeof(GameObject))) as GameObject;
				MillStone millstone = new_millstone.GetComponent<MillStone>();	
				
	            int Level = PlayerPrefs.GetInt(Mz_SaveData.millstone_level_ + i);
				int Position = PlayerPrefs.GetInt(Mz_SaveData.millstone_position_ + i);
                
				millstone.currentBuildingStatus = Buildings.BuildingStatus.none;				
                millstone.level = Level;
				millstone.transform.position = StageManager.buildingArea_Pos[Position];
				millstone.IndexOfPosition = Position;
				
				Buildings.MillStoneInstance.Add(millstone);
				StageManager.buildingArea_Obj[Position].gameObject.SetActive(false);
	        }
        }
        
        if (amount_Smelter_Instance != 0)
        {
            for (int i = 0; i < amount_Smelter_Instance; i++)
            {
                GameObject new_smelter = Instantiate(Resources.Load("Buildings/Economy/Smelter", typeof(GameObject))) as GameObject;
                Smelter smelter = new_smelter.GetComponent<Smelter>();

                int Level = PlayerPrefs.GetInt(Mz_SaveData.smelter_level_ + i);
                int Position = PlayerPrefs.GetInt(Mz_SaveData.smelter_position_ + i);

                smelter.currentBuildingStatus = Buildings.BuildingStatus.none;
                smelter.level = Level;
                smelter.transform.position = StageManager.buildingArea_Pos[Position];
				smelter.IndexOfPosition = Position;

                Buildings.SmelterInstance.Add(smelter);
                StageManager.buildingArea_Obj[Position].gameObject.SetActive(false);
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
