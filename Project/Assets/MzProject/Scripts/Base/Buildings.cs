using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingsTimeData {
	
	public float[] arrBuildingTimesData = new float[Buildings.MAX_LEVEL];
	
	public BuildingsTimeData(Buildings.BuildingType r_buildingType) 
	{
		if(r_buildingType == Buildings.BuildingType.general) {
//			float[] time_generalType = new float[];
//			time_generalType[0] = 10;
//			
//			buildingTimesData = time_generalType;
		}
		else if(r_buildingType == Buildings.BuildingType.resource) {
			float[] time_resourceType = { 
				30f, 90f, 300f, 600f, 1200f
			};
			
			arrBuildingTimesData = time_resourceType;
		}
	}
};

public class Buildings : MonoBehaviour {
	public const int MAX_LEVEL = 10;
	
	
    public Font showG;
    public GUISkin standard_Skin;
    public GUISkin building_Skin;
    //<!-- Building Icon.
    public Texture2D buildingIcon_Texture;

	protected GameObject processbar_Obj_parent;
    protected OTSprite processBar_Scolling;
    [System.NonSerialized]    public OTSprite sprite;
		
    public int level = 0;
    protected bool _clicked = false;
    protected bool _CanDestruction = true;
	
	public static bool _CanCreateBuilding = false;
    public static List<StoreHouse> storeHouseId = new List<StoreHouse>();
    public static List<Buildings> onBuilding_Obj = new List<Buildings>();
	
	public enum BuildingStatus { none = 0, buildingProcess = 1, buildingComplete, };
	public BuildingStatus buildingStatus;
	public enum BuildingType { general = 0, resource, };
	protected BuildingType buildingType;

    protected BuildingsTimeData buildingTimeData;

    protected Rect windowRect;
    protected Rect exitButton_Rect;
    protected Vector2 scrollPosition = Vector2.zero;

    protected Rect background_Rect;
    protected Rect upgradeButton_Rect;
    protected Rect destructionButton_Rect;
    protected Rect imgContent_Rect = new Rect(40, 60, 100, 100);
    protected Rect buildingIcon_Rect = new Rect(40, 48, 80, 80);
    protected Rect levelLable_Rect = new Rect(25, 132, 120, 32);
    protected Rect discription_Rect;
    protected Rect contentRect = new Rect(170, 20, 590, 160);
    protected Rect warnningMessage_Rect = new Rect(Main.GameWidth / 2 - 150, Main.GameHeight / 2 - 120, 300, 240);


    protected virtual void DestructionBuilding() { 
    
    }
	
	#region Building Process Section.
	public static bool CheckingCanCreateBuilding() {
		if(onBuilding_Obj.Count < 2)
			return true;
		else 
			return false;
	}
	
	protected virtual void OnBuildingProcess(Buildings obj) {
        Debug.Log("Class :: Building" + obj.name + ": OnBuildingProcess()");

        if (onBuilding_Obj.Count < 2)
        {
            onBuilding_Obj.Add(obj);
			
			for(int i = 0; i<onBuilding_Obj.Count; i++)
				onBuilding_Obj[i].CreateProcessBar();
        }
	}
    protected virtual void CreateProcessBar()
    {
        if (processbar_Obj_parent == null)
        {
            processbar_Obj_parent = Instantiate(Resources.Load("Processbar_Group", typeof(GameObject)),
                new Vector3(this.sprite.position.x, this.sprite.position.y + this.sprite.size.y, 0),
                Quaternion.identity) as GameObject;

            OTSprite backgroundSprite = processbar_Obj_parent.GetComponentInChildren<OTSprite>();
            backgroundSprite.size = new Vector2(128, 24);

            if (processBar_Scolling == null)
            {
                var scrolling = Instantiate(Resources.Load("processbar_scroll", typeof(GameObject))) as GameObject;
                scrolling.transform.parent = processbar_Obj_parent.transform;

                processBar_Scolling = scrolling.GetComponent<OTSprite>();
                processBar_Scolling.pivot = OTObject.Pivot.Left;
                processBar_Scolling.position = new Vector2((-backgroundSprite.size.x / 2) + 2, 0);
                processBar_Scolling.size = new Vector2(12, 24);
            }
        }

        Hashtable scaleData = new Hashtable();
        scaleData.Add("from", new Vector2(12, 24));
        scaleData.Add("to", new Vector2(124, 24));
        scaleData.Add("time", buildingTimeData.arrBuildingTimesData[level - 1]);
        scaleData.Add("onupdate", "BuildingProcess");
        scaleData.Add("easetype", iTween.EaseType.linear);
        scaleData.Add("oncomplete", "DestroyBuildingProcess");
        scaleData.Add("oncompleteparams", this);
        scaleData.Add("oncompletetarget", this.gameObject);

        iTween.ValueTo(this.gameObject, scaleData);
    }
	protected virtual void BuildingProcess(Vector2 Rvalue) {
		Debug.Log("Class :: Buildings" + ":: BuildingProcess");
		
		if(this.processBar_Scolling)
			this.processBar_Scolling.size = Rvalue;
	}
	
	protected virtual void DestroyBuildingProcess(Buildings obj) {
        Debug.Log("Class :: Buildings" + obj.name + ": DestroyBuildingProcess");

        onBuilding_Obj.Remove(obj);
	}
	
	#endregion
	
	#region Incloud Mouse Event.
	
    protected void OnMouseOver()
    {
		
    }

    protected void OnMouseDown()
    {
        _clicked = true;
    }

    protected void OnMouseExit()
    {

    }

    #endregion

    protected virtual void CreateWindow(int windowID)
    {
        //<!-- Exit Button.
        if (GUI.Button(exitButton_Rect, new GUIContent(string.Empty, "Close Button"), building_Skin.customStyles[0]))
        {
            _clicked = false;
        }

        if (GUI.Button(upgradeButton_Rect, new GUIContent("Upgrade"), standard_Skin.button))
        {

        }

        if (_CanDestruction)
        {
            if (GUI.Button(destructionButton_Rect, new GUIContent("Destruction"), standard_Skin.button))
            {
                //GUI.Box()
            }
        }
    }

    protected virtual IEnumerator BuildingTimer()
    {
        yield return 0;
    }

    protected void OnGUI()
    {
        windowRect = new Rect(Main.GameWidth / 2 - 350, Main.GameHeight / 2 - 200, 700, 400);
        exitButton_Rect = new Rect(windowRect.width - 34, 2, 32, 32);
        upgradeButton_Rect = new Rect (windowRect.width - 140, 32, 100, 32);
        destructionButton_Rect = new Rect(windowRect.width - 250, 32, 100, 32);
        background_Rect = new Rect(0, 0, windowRect.width - 12, 320);
        discription_Rect = new Rect(150, 48, 530, background_Rect.height - 96);

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GameWidth, Screen.height / Main.GameHeight, 1));

        standard_Skin.window.font = showG; 

        building_Skin.box.wordWrap = true;
        building_Skin.box.fontSize = 16;
        building_Skin.box.fontStyle = FontStyle.Normal;
        building_Skin.box.contentOffset = new Vector2(128, 38);

        if (_clicked) {
            windowRect = GUI.Window(0, windowRect, CreateWindow, new GUIContent(name, "GUI window"), standard_Skin.window);
        }
    }
}
