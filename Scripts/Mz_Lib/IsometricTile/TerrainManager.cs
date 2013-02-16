using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour {

    public TerrainElement goldMine;
    public TerrainElement stoneMine;
	public TerrainElement tree;

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
	void Start () 
	{
		CreateArea(tree_areas,"Prototypes/Environtment/Tree",tree_group_obj,0,0,8,8);
		CreateArea(goldMine_areas,"Prototypes/Environtment/GoldMine",goldMine_group_obj,8,0,8,8);
		CreateArea(tree_areas,"Prototypes/Environtment/Tree",tree_group_obj,16,0,8,8);
		CreateArea(goldMine_areas,"Prototypes/Environtment/GoldMine",goldMine_group_obj,24,0,8,8);
		
		CreateArea(goldMine_areas,"Prototypes/Environtment/GoldMine",goldMine_group_obj,0,8,8,8);
		CreateArea(tree_areas,"Prototypes/Environtment/Tree",tree_group_obj,0,16,8,8);
		CreateArea(goldMine_areas,"Prototypes/Environtment/GoldMine",goldMine_group_obj,0,24,8,8);
		
		CreateArea(tree_areas,"Prototypes/Environtment/Tree",tree_group_obj,8,24,8,8);
		CreateArea(goldMine_areas,"Prototypes/Environtment/GoldMine",goldMine_group_obj,16,24,8,8);
		CreateArea(tree_areas,"Prototypes/Environtment/Tree",tree_group_obj,24,24,8,8);
		
		CreateArea(goldMine_areas,"Prototypes/Environtment/GoldMine",goldMine_group_obj,24,16,8,8);
		CreateArea(tree_areas,"Prototypes/Environtment/Tree",tree_group_obj,24,8,8,8);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	void CreateArea(List<TileArea> listarea,string ResourcePath,GameObject objGroup,int pointx,int pointy,int width,int height)
	{
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
				listarea.Add(new TileArea() { x = i+pointx, y = j+pointy, numSlotWidth = 1, numSlotHeight = 1});	
			}
		}
		for (int i = 0; i < listarea.Count; i++) {
			GameObject temp = Instantiate(Resources.Load(ResourcePath, typeof(GameObject))) as GameObject;
			temp.transform.parent = objGroup.transform;
			temp.GetComponent<TerrainElement>().constructionArea = listarea[i];
		}
	}
}
