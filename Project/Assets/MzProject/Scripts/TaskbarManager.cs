using UnityEngine;
using System.Collections;

public class TaskbarManager : MonoBehaviour {

    public GUISkin standard_Skin;
    public GUISkin taskbarUI_Skin;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    protected Rect groupHeader_Rect = new Rect(Main.GameWidth / 2 - 300, 0, 600, 100);
    protected Rect groupResourec_Rect;
    protected Rect resource_Rect = new Rect(0, 1, 100, 40);
    protected Rect username_Rect;
    protected Rect settingGroup_Rect;
	protected Rect setting_Rect;
	
	public enum TaskbarUIState { normal = 0, showSetting, };
	private TaskbarUIState taskbarUIState;

    void OnGUI()
    {
        groupResourec_Rect = new Rect(groupHeader_Rect.width / 2 - 200, 1, 400, 48);
        username_Rect = new Rect(1, 1, 120, 32);
        settingGroup_Rect = new Rect(Main.GameWidth - 128, 0, 128, 34);
		setting_Rect = new Rect(Main.GameWidth - 35, 1, 32, 32);

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GameWidth, Screen.height / Main.GameHeight, 1));

        if (taskbarUIState == TaskbarUIState.showSetting)
        {
            GUI.BeginGroup(settingGroup_Rect, GUIContent.none, standard_Skin.box);
            {
                if (GUI.Button(new Rect(2, 4, 32, 25), new GUIContent("", "Toggle Screen"), taskbarUI_Skin.customStyles[5])) {
                    Screen.fullScreen = !Screen.fullScreen;
                }
            }
            GUI.EndGroup();
        }

		if(GUI.Button(setting_Rect, GUIContent.none, taskbarUI_Skin.customStyles[4])) {
            if (taskbarUIState == TaskbarUIState.showSetting)
                taskbarUIState = TaskbarUIState.normal;
            else if (taskbarUIState == TaskbarUIState.normal)
                taskbarUIState = TaskbarUIState.showSetting;
		}

        if (GUI.Button(username_Rect, new GUIContent(Manager.Username), taskbarUI_Skin.button))
        {

        }

        GUI.BeginGroup(groupHeader_Rect, new GUIContent(), GUIStyle.none);
        {
            GUI.BeginGroup(groupResourec_Rect, GUIContent.none, GUIStyle.none);
            {
                if (GUI.Button(resource_Rect, 
					new GUIContent(StoreHouse.sumOfFood + "/" +StoreHouse.SumOfCapacity, taskbarUI_Skin.customStyles[0].normal.background), taskbarUI_Skin.button))
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
