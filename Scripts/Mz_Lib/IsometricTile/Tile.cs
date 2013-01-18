using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileArea {
    public int x;
    public int y;
    public int numSlotWidth;
    public int numSlotDepth;
};

public class Tile : MonoBehaviour {

    public StageManager stageManager;
    public enum TileStatus
    {
        Empty = 0,
        NoEmpty,
    };
    public TileStatus tileState;
	public enum ObjectBeh {
		None,
		ShowStatus,
	};
	internal ObjectBeh objectBeh;
    private tk2dSprite sprite;

    void Awake() {
        sprite = this.GetComponent<tk2dSprite>();
    }

	// Use this for initialization
	void Start () {
        GameObject main = GameObject.FindGameObjectWithTag("GameController");
        stageManager = main.GetComponent<StageManager>();
	}
	
	// Update is called once per frame
	void Update () {
        this.CheckingHasCallStatus();
	}

    private void CheckingHasCallStatus()
    {
        if (_isShowStatus && objectBeh == ObjectBeh.ShowStatus) {
            _isShowStatus = false;
			StartCoroutine(this.WaitForNextFrame());
		}
    }

    private IEnumerator WaitForNextFrame()
    {
        yield return new WaitForFixedUpdate();
        if (_isShowStatus == false && objectBeh == ObjectBeh.ShowStatus)
            this.DeActiveShowStatus();
    }

    private void DeActiveShowStatus()
    {
		objectBeh = ObjectBeh.None;
        if (tileState == TileStatus.Empty)
        {
            this.sprite.color = Color.white;
        }
    }

    private bool _isShowStatus = false;
    public const string FUNC_CheckedTileStatus = "CheckedTileStatus";
    public void CheckedTileStatus(TileArea area) {
        _isShowStatus = true;
		this.objectBeh = ObjectBeh.ShowStatus;
 
        for(int i = 0; i < area.numSlotWidth;i++) {
            for (int j = area.numSlotDepth - 1; j >= 0 ; j--)
			{
                int newX = area.x + i;
                int newY = area.y - j;
                stageManager.tiles_list[newX, newY].sprite.color = Color.green;
                stageManager.tiles_list[newX, newY]._isShowStatus = true;
                stageManager.tiles_list[newX, newY].objectBeh = ObjectBeh.ShowStatus;
			}
        }
    }
}
