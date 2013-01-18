using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsometricEngine : MonoBehaviour {
    GameObject buildingArea_group;
	public GameObject tile_prefab;
    public StageManager sceneController;
	
	private int x = 16;
	private int y = 16;
    private float tile_width = 64;
    private float tile_height = 33.5f;


	// Use this for initialization
    void Start()
    {
        buildingArea_group = new GameObject("buildingArea_group");
    }

    internal IEnumerator CreateTilemap()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = y; j >= 0; j--)
            {  // Changed loop condition here.
                float a = (j * tile_width / 2) + (i * tile_width / 2);
                float b = (i * tile_height / 2) - (j * tile_height / 2);
                GameObject tile = Instantiate(tile_prefab) as GameObject;
                tile.transform.position = new Vector3(a, b, i);
                tile.transform.parent = buildingArea_group.transform;
				
				tile.gameObject.name = i.ToString() + " : " + j.ToString();

                sceneController.tiles_list[i, j] = tile.GetComponent<Tile>();
            }
        }

        yield return 0;
    }
	
	// Update is called once per frame
	void Update () {

	}
}
