using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour {

    public TerrainElement goldMine;
    public TerrainElement stoneMine;
	public TerrainElement tree;
//	public TerrainElement forest;
//	public TerrainElement bush;
//	public TerrainElement flower;

    public Dictionary<string, TerrainElement> element_dict = new Dictionary<string, TerrainElement>();
	public List<TileArea> goldMine_areas = new List<TileArea>();
    public List<TileArea> stoneMine_areas = new List<TileArea>();
	public List<TileArea> tree_areas = new List<TileArea>();
	
	GameObject goldMine_group_obj;
	GameObject stoneMine_group_obj;
	GameObject tree_group_obj;
	
	
	void Awake() {
		goldMine_group_obj = new GameObject("goldMine_group_obj");
		stoneMine_group_obj = new GameObject("stoneMine_group_obj");
		tree_group_obj = new GameObject("tree_group_obj");
	}
	
	// Use this for initialization
	void Start () {
        element_dict.Add("GoldMine", goldMine);
        element_dict.Add("StoneMine", stoneMine);
		element_dict.Add("Tree", tree);

		goldMine_areas.Add(new TileArea() {x = 5, y = 5, numSlotWidth = 1, numSlotHeight = 1});
		goldMine_areas.Add(new TileArea() { x = 5, y = 6, numSlotWidth = 1, numSlotHeight = 1 });
		goldMine_areas.Add(new TileArea() { x = 6, y = 6, numSlotWidth = 1, numSlotHeight = 1 });

        stoneMine_areas.Add(new TileArea() { x = 6, y = 8, numSlotWidth = 1, numSlotHeight = 1 });
		/*
		tree_areas.Add(new TileArea() { x = 15, y = 0, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 1, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 2, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 3, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 4, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 5, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 6, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 7, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 8, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 9, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 10, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 11, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 12, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 13, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 14, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 15, y = 15, numSlotWidth = 1, numSlotHeight = 1});
		tree_areas.Add(new TileArea() { x = 7, y = 7, numSlotWidth = 1, numSlotHeight = 1});
		*/
		// <@-- Top width.
		for (int i = 0; i < 16; i++) {
			//tree_areas.Add(new TileArea() { x = i, y = 16, numSlotWidth = 1, numSlotHeight = 1});
		}
		// <@-- Down Width.
		for (int i = 0; i < 16; i++) {
			//tree_areas.Add(new TileArea() { x = i, y = 0, numSlotWidth = 1, numSlotHeight = 1});
		}		
		// <@-- Left Height.
		for (int i = 1; i < 16; i++) {
			//tree_areas.Add(new TileArea() { x = 0, y = i, numSlotWidth = 1, numSlotHeight = 1});
		}

        this.CreateElement();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void CreateElement() {
		for (int i = 0; i < goldMine_areas.Count; i++) {
            goldMine.constructionArea = goldMine_areas[i];
			GameObject temp = Instantiate(goldMine.gameObject) as GameObject;
			
			temp.transform.parent = goldMine_group_obj.transform;
		}
        for (int i = 0; i < stoneMine_areas.Count; i++)
        {
            stoneMine.constructionArea = stoneMine_areas[i];
            GameObject temp = Instantiate(stoneMine.gameObject) as GameObject;
			
			temp.transform.parent = stoneMine_group_obj.transform;
        }
		for (int i = 0; i < tree_areas.Count; i++) {
			tree.constructionArea = tree_areas[i];
			GameObject temp = Instantiate(tree.gameObject) as GameObject;
			
			temp.transform.parent = tree_group_obj.transform;
		}
    }
}
