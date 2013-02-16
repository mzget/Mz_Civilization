using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainElement : ObjectsBeh {

	// Use this for initialization
	public static List<TerrainElement> elementId = new List<TerrainElement>();
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
		
		TerrainElement.elementId.Add(this);
		this.gameObject.name = TerrainElement.elementId.Count.ToString();
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}