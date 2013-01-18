using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GroupOFUnitBeh
{
    public List<UnitBeh> unitBehs = new List<UnitBeh>();
    public List<int> members = new List<int>();
    internal List<int> capacities = new List<int>();
    internal int totalAttackScore;
    internal int totalDefenseScore;

    public GroupOFUnitBeh() { }
}
