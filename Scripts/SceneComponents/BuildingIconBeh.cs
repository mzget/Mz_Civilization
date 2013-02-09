using UnityEngine;
using System.Collections;

public class BuildingIconBeh : ObjectsBeh {
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		_canMovable = true;
	}
	
	protected override void OnTouchDown ()
	{
		base.OnTouchDown ();
		
		TaskManager.IsShowInteruptGUI = true;
	}
	
	protected override void OnTouchOver ()
	{
		base.OnTouchOver ();
		
		this.gameObject.transform.localScale = Vector3.one * 0.8f;
	}
	
	protected override void ImplementDraggableObject ()
	{
		base.ImplementDraggableObject ();
		
		TaskManager.IsShowInteruptGUI = true;
		
		if(this.gameObject.tag != "Building") {
			this.gameObject.tag = "Building";
			this.gameObject.layer = LayerMask.NameToLayer("Default");
			
			this.transform.parent = null;
			this.gameObject.transform.localScale *= 0.8f;
			sceneController.taskManager.MoveRightSideBarGUI();
		}
		 
		
		Ray cursorRay;
        RaycastHit hit;
        cursorRay = new Ray(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Vector3.forward);
        if (Physics.Raycast(cursorRay, out hit, 100f))
        {
			TileArea temp_originalArea = constructionArea;
//			Debug.Log(constructionArea.x + ":" + constructionArea.y + ":" + constructionArea.numSlotWidth + ":" + constructionArea.numSlotDepth);

            if (hit.collider.tag == "BuildingArea")
            {
                string[] slotId = hit.collider.name.Split(':');
                TileArea newarea = new TileArea() { 
					x = int.Parse(slotId[0]), y = int.Parse(slotId[1]), 
					numSlotWidth = constructionArea.numSlotWidth,
                    numSlotHeight = constructionArea.numSlotHeight,
				};
                bool canCreateBuilding = sceneController.tiles_list[0, 0].CheckedTileStatus(newarea);

                if (this._isDropObject)
				{
                    if(canCreateBuilding) {
                        this.transform.position = sceneController.tiles_list[0, 0].GetAreaPosition(newarea);
                        this.originalPosition = this.transform.position;
						constructionArea = newarea;

						sceneController.CreateBuildingOnBuildingArea(this.gameObject.name, originalPosition, constructionArea);
						Destroy(this.gameObject);
						sceneController.taskManager.CreateBuildingIconInRightSidebar(this.gameObject.name);
                    }
                    else {
                        this.transform.position = this.originalPosition;
						constructionArea = temp_originalArea;

						Destroy(this.gameObject);
						sceneController.taskManager.CreateBuildingIconInRightSidebar(this.gameObject.name);
                    }
					
                    this._isDropObject = false;
                    base._isDraggable = false;
					TaskManager.IsShowInteruptGUI = false;
                }
            }
            else if(hit.collider.tag == "Building" || hit.collider.tag == "TerrainElement") {
                print("Tag == " + hit.collider.tag + " : Name == " + hit.collider.name);
				
                ObjectsBeh hit_obj = hit.collider.GetComponent<ObjectsBeh>();
                hit_obj.ShowAreaStatus();

                if(_isDropObject) {
                    Debug.LogWarning("Building and Terrain element cannot construction");
					
                    this.transform.position = this.originalPosition;
					constructionArea = temp_originalArea;
					Destroy(this.gameObject);
					sceneController.taskManager.CreateBuildingIconInRightSidebar(this.name);

                    this._isDropObject = false;
                    base._isDraggable = false;
					TaskManager.IsShowInteruptGUI = false;
                }
            }
        }
        else
        {
            print("Out of ray direction");
            if (this._isDropObject) {
				this.transform.position = originalPosition;
                this._isDropObject = false;
                base._isDraggable = false;
				TaskManager.IsShowInteruptGUI = false;
				
				Destroy(this.gameObject);
				sceneController.taskManager.CreateBuildingIconInRightSidebar(this.name);
            }
        }

        Debug.DrawRay(cursorRay.origin, Vector3.forward * 100f, Color.red);
	}
}
