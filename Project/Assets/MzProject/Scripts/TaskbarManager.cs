using UnityEngine;
using System.Collections;

public class TaskbarManager : MonoBehaviour {

    public const string PathOfMainGUIResource = "Textures/MainGUI/";

    public static bool IsShowInteruptGUI = false;

    public GUISkin taskbarUI_Skin;
    public GUIStyle left_button_Style;
    public GUIStyle right_button_Style;
	
    public Texture2D food_icon;
    public Texture2D wood_icon;
    public Texture2D stone_icon;
    public Texture2D gold_icon;
    public Texture2D employee_icon;

    protected Rect groupHeader_Rect = new Rect(0, 0, Main.GAMEWIDTH, 50);
    protected Rect groupResourec_Rect;
    protected Rect resource_Rect = new Rect(0, 1, 100, 48);
    protected Rect username_Rect;


	// Use this for initialization
	IEnumerator Start () {
        StartCoroutine(InitializeMainGUI());

        yield return 0;
    }

    IEnumerator InitializeMainGUI() {
        left_button_Style.normal.background = Resources.Load(PathOfMainGUIResource + "Back_up", typeof(Texture2D)) as Texture2D;
        left_button_Style.active.background = Resources.Load(PathOfMainGUIResource + "Back_down", typeof(Texture2D)) as Texture2D;
        right_button_Style.normal.background = Resources.Load(PathOfMainGUIResource + "Next_up", typeof(Texture2D)) as Texture2D;
        right_button_Style.active.background = Resources.Load(PathOfMainGUIResource + "Next_down", typeof(Texture2D)) as Texture2D;

        food_icon = taskbarUI_Skin.customStyles[0].normal.background;
        wood_icon = taskbarUI_Skin.customStyles[1].normal.background;
        stone_icon = taskbarUI_Skin.customStyles[2].normal.background;
        gold_icon = taskbarUI_Skin.customStyles[3].normal.background;
        employee_icon = Resources.Load(PathOfMainGUIResource + "Population", typeof(Texture2D)) as Texture2D;

        yield return 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        groupResourec_Rect = new Rect(groupHeader_Rect.width / 2 - 200, 1, 400, 48);
        username_Rect = new Rect(1, 1, 100, 24);
        Rect population_Rect = new Rect(1, 25, 100, 24);

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GAMEWIDTH, Screen.height / Main.GAMEHEIGHT, 1));

        if (GUI.Button(username_Rect, new GUIContent(MainMenu.Username), taskbarUI_Skin.button))
        {

        }
        GUI.Box(population_Rect, new GUIContent(HouseBeh.SumOfPopulation.ToString()), GUI.skin.textField);

        GUI.BeginGroup(groupHeader_Rect, new GUIContent(), GUIStyle.none);
        {
            GUI.BeginGroup(groupResourec_Rect, GUIContent.none, GUIStyle.none);
            {
                if (GUI.Button(resource_Rect, 
					new GUIContent(StoreHouse.sumOfFood + "/" + StoreHouse.SumOfCapacity, taskbarUI_Skin.customStyles[0].normal.background), taskbarUI_Skin.button))
                {

                }
                else if (GUI.Button(new Rect((resource_Rect.width * 1) + resource_Rect.x, resource_Rect.y, resource_Rect.width, resource_Rect.height),
                    new GUIContent(StoreHouse.sumOfWood + "/" + StoreHouse.SumOfCapacity, taskbarUI_Skin.customStyles[1].normal.background), taskbarUI_Skin.button))
                {

                }
                else if (GUI.Button(new Rect((resource_Rect.width * 2) + resource_Rect.x, resource_Rect.y, resource_Rect.width, resource_Rect.height),
                    new GUIContent(StoreHouse.sumOfGold + "/" + StoreHouse.SumOfCapacity, taskbarUI_Skin.customStyles[2].normal.background), taskbarUI_Skin.button))
                {

                }
                else if (GUI.Button(new Rect((resource_Rect.width * 3) + resource_Rect.x, resource_Rect.y, resource_Rect.width, resource_Rect.height),
                    new GUIContent(StoreHouse.sumOfStone + "/" + StoreHouse.SumOfCapacity, taskbarUI_Skin.customStyles[3].normal.background), taskbarUI_Skin.button))
                {

                }
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }
}
