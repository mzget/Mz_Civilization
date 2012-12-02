using UnityEngine;
using System.Collections;

public class AICities : ScriptableObject {

    public Texture2D symbols;
    public UnitDataStore.Tribes tribe;

    public int[] AmountOfUnits = new int[3];

    void OnEnable() { 
		Debug.Log(this.name + " :: AICities.OnEnable");
    }

    internal void TraceUnitData() {
        foreach (var item in AmountOfUnits) {
            Debug.Log(name + " : " + tribe + " : " + item);
        }
    }

    void OnDestroy() { }
}
