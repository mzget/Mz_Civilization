using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TileArea {
    public int x;
    public int y;
    public int numSlotWidth;
    public int numSlotHeight;
};

public class Tile : MonoBehaviour {

    public CapitalCity stageManager;
	
    public enum TileStatus { Empty = 0, NoEmpty, };
    public TileStatus tileState;
	
	public enum TileAbility { None, ShowStatus,	};
	internal TileAbility tileAbility;
	
    private tk2dSprite sprite;

    void Awake() {
        sprite = this.GetComponent<tk2dSprite>();
		
        GameObject main = GameObject.FindGameObjectWithTag("GameController");
        stageManager = main.GetComponent<CapitalCity>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {			
		this.CheckingHasCallStatus();
	}

    private void CheckingHasCallStatus()
    {
        if (_isShowStatus && tileAbility == TileAbility.ShowStatus) {
            _isShowStatus = false;
			StartCoroutine(this.WaitForNextFrame());
		}
    }

    private IEnumerator WaitForNextFrame()
    {
        yield return new WaitForFixedUpdate();

        if (_isShowStatus == false && tileAbility == TileAbility.ShowStatus)
            this.DeActiveShowStatus();
    }

    private void DeActiveShowStatus()
    {
		tileAbility = TileAbility.None;
        this.sprite.color = Color.white;
    }

    private bool _isShowStatus = false;
    internal bool CheckedTileStatus(TileArea area) {
		if(area.x + area.numSlotWidth > IsometricEngine.x || area.y < 0)
			return false;
		
        bool _canCreateBuilding = true;
        _isShowStatus = true;
		this.tileAbility = TileAbility.ShowStatus;
 
        for(int i = 0; i < area.numSlotWidth;i++) {
            for (int j = 0; j < area.numSlotHeight ; j++)
			{
                int newX = area.x + i;
                int newY = area.y + j;
                stageManager.tiles_list[newX, newY]._isShowStatus = true;
                stageManager.tiles_list[newX, newY].tileAbility = TileAbility.ShowStatus;

                if (stageManager.tiles_list[newX, newY].tileState == TileStatus.Empty) {
                    stageManager.tiles_list[newX, newY].sprite.color = Color.green;
                }
                else if (stageManager.tiles_list[newX, newY].tileState == TileStatus.NoEmpty) {
                    stageManager.tiles_list[newX, newY].sprite.color = Color.red;
                    _canCreateBuilding = false;
                }
			}
        }

        return _canCreateBuilding;
    }

    public Vector3 GetAreaPosition(TileArea area) {
        List<Vector3> positions = new List<Vector3>();
        Vector3 areaPositions = Vector3.zero;
        for (int i = 0; i < area.numSlotWidth; i++)
        {
            for (int j = 0 ; j < area.numSlotHeight; j++)
            {
                int newX = area.x + i;
                int newY = area.y + j;
                positions.Add(stageManager.tiles_list[newX, newY].transform.position);
            }
        }

        foreach (Vector3 item in positions)
            areaPositions += item;
		
		areaPositions = new Vector3(areaPositions.x / positions.Count, areaPositions.y / positions.Count, ((areaPositions.z / positions.Count) - (area.numSlotHeight + 0.5f)));

        return areaPositions;
    }

    internal void SetNoEmptyArea(TileArea area) {
        for (int i = 0; i < area.numSlotWidth; i++) {
            for (int j = 0; j < area.numSlotHeight; j++) {
                int newX = area.x + i;
                int newY = area.y + j;				
				
                stageManager.tiles_list[newX, newY].tileState = TileStatus.NoEmpty;
            }
        }
    }

    internal void SetEmptyArea(TileArea area) {
        for (int i = 0; i < area.numSlotWidth; i++) {
            for (int j = 0; j < area.numSlotHeight; j++) {
                int newX = area.x + i;
                int newY = area.y + j;
				
                stageManager.tiles_list[newX, newY].tileState = TileStatus.Empty;
            }
        }
    }
}
