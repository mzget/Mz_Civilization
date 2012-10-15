using UnityEngine;
using System.Collections;

public class JoystickManager : MonoBehaviour {
	
	private GuiRatioFixer guiratio;
    public Joystick joystick;
	
	
	void Awake() {
		this.gameObject.AddComponent<GuiRatioFixer>();
	}

	// Use this for initialization
	void Start () {
		guiratio = this.GetComponent<GuiRatioFixer>();
		
		this.gameObject.AddComponent<Joystick>();
		joystick = this.gameObject.GetComponent<Joystick>();
	}
}
