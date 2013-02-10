using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsometricEngine : MonoBehaviour {
    GameObject buildingArea_group;
	public GameObject tile_prefab;
    public CapitalCity sceneController;
	
	public const int x = 16;
	public const int y = 16;
    private float tile_width = 64f;
    private float tile_height = 37f;
	
	
	public float map_width
	{
		get {return tile_width * x;}
	}
	public float map_height
	{
		get {return tile_height * y;}
	}
    
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

				int j_order = y - j;
				tile.gameObject.name = i.ToString() + " : " + j_order.ToString();

				int z_pos = i + j_order;
				tile.transform.position = new Vector3(a, b, z_pos);
				tile.transform.parent = buildingArea_group.transform;
                sceneController.tiles_list[i, j_order] = tile.GetComponent<Tile>();
            }
        }

        yield return 0;
    }
	
	// Update is called once per frame
	void Update () {

	}
}
