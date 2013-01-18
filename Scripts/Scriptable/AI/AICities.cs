using UnityEngine;
using System.Collections;

[System.Serializable]
public class AICities {
    public enum Tribes {
        None = 0,
        Greek = 1,
        Egyptian = 2,
        Persian = 3,
        Celtic = 4,
    };

    public string name;
    public Tribes tribe;
    public Texture2D symbols;
    public int distance;            //@-- Distance use to calculation time to travel.
    public int defenseBonus;
    public int attackBonus;

    public AICities() {
		Debug.Log(this.name + " :: AICities.OnEnable");
    }

    void OnDestroy() { }

    internal virtual GroupOFUnitBeh CreateTroopsActivity() { return null; }

    public virtual int CalculationDefenseScore() { return 0; }
}
