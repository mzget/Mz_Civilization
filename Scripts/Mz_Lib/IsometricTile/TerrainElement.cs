using UnityEngine;
using System.Collections;

public class TerrainElement : ObjectsBeh {
    	
	protected override void Awake ()
	{
		base.Awake ();
		
		GameObject main = GameObject.FindGameObjectWithTag("GameController");
        sceneController = main.GetComponent<CapitalCity>();
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

        if (constructionArea.numSlotWidth == 0 || constructionArea.numSlotHeight == 0)
        {
            Debug.LogError("area slot is equal 0");
            return;
        }
		
        this.transform.position = sceneController.tiles_list[0, 0].GetAreaPosition(constructionArea);
        sceneController.tiles_list[0, 0].SetNoEmptyArea(constructionArea);
        this.originalPosition = this.transform.position;
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}