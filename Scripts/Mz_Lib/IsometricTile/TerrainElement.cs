using UnityEngine;
using System.Collections;

public class TerrainElement : TilebaseObjBeh {
    	
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
		
        this.transform.position = Tile.GetAreaPosition(constructionArea);
        Tile.SetNoEmptyArea(constructionArea);
        this.originalPosition = this.transform.position;
    }

    // Enables the behaviour when it is visible
    void OnBecameVisible()
    {
        enabled = true;
    }

    // Disables the behaviour when it is invisible
    void OnBecameInvisible()
    {
        enabled = false;
    }
}