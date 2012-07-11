using UnityEngine;
using System.Collections;

public class ObjectName : MonoBehaviour {
    
    public string name = string.Empty;
    public GameObject nameObj;
    private GameObject nameIns;
    
    
	// Use this for initialization
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            nameIns = Instantiate(nameObj) as GameObject;
            nameIns.gameObject.active = false;
            nameIns.GetComponent<TextMesh>().text = name;
            nameIns.transform.position = this.transform.position + new Vector3(0, 80, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region All Mouse Event.

    void OnMouseOver() { 
        nameIns.gameObject.active = true;
    }

    void OnMouseDown() {
        //_Clicked = true;
    }

    void OnMouseExit() { 
        nameIns.gameObject.active = false;
    }

    #endregion
    
    void DestroyObjectName(){
        Destroy(nameIns.gameObject);
    }
}
