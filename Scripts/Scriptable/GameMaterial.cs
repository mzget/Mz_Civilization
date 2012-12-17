using UnityEngine;
using System.Collections;

public class GameMaterialData {
    public string name = string.Empty;
    public Texture2D materialIcon = null;
    public int materialNumber;
	
	public GameMaterialData() {
        Debug.Log("Starting :: GameMaterial");
	}
}
