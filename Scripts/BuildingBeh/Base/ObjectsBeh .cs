using UnityEngine;
using System.Collections;

public class ObjectsBeh : Base_ObjectBeh
{
    protected CapitalCity sceneController;

    public bool _canMovable = false;
    protected bool _isDropObject = false;
    protected bool _isDraggable = false;
    protected bool _canActive = false;
    internal Vector3 originalPosition;

    /// <summary>
    /// destroyObj_Event.
    /// </summary>	
    public event System.EventHandler destroyObj_Event;
    protected void OnDestroyObject_event(System.EventArgs e)
    {
        if (destroyObj_Event != null)
        {
            destroyObj_Event(this, e);
            Debug.Log(destroyObj_Event + ": destroyObj_Event : " + this.name);
        }
    }

    protected virtual void Awake() {
        GameObject scene_obj = GameObject.FindGameObjectWithTag("GameController");
        sceneController = scene_obj.GetComponent<CapitalCity>();
    }
	
	protected virtual void Start() {}

    protected virtual void ImplementDraggableObject()
    {		
        Vector3 screenPoint;

        if (Input.touchCount >= 1)
            screenPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        else
            screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        this.transform.position = new Vector3(screenPoint.x, screenPoint.y, -2.5f);
    }

    public TileArea constructionArea;
    internal virtual bool ShowAreaStatus() {
        bool canCreateBuilding = sceneController.tiles_list[0, 0].CheckedTileStatus(constructionArea);

        return canCreateBuilding;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (_isDraggable) {
            this.ImplementDraggableObject();
        }

        if (sceneController.touch.phase == TouchPhase.Ended || sceneController.touch.phase == TouchPhase.Canceled) {
            if (this._isDraggable)
                _isDropObject = true;
        }
    }

    protected override void OnTouchDrag()
    {
        base.OnTouchDrag();

        if (this._canMovable && base._OnTouchBegin)
			this._isDraggable = true;
    }

    protected override void OnTouchEnded()
    {
        base.OnTouchEnded();

        if (_isDraggable)
            _isDropObject = true;
    }
}