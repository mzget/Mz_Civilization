using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game resource. food, wood, stone, gold, employee,
/// </summary>
public class GameMaterialDatabase
{
    public int Food { get; set; }
    public int Wood { get; set; }
    public int Stone { get; set; }
    public int Copper { get; set; }
    public int Iron { get; set; }
    public int Armor { get; set; }
    public int Weapon { get; set; }
    public int Employee { get; set; }
    public int Gold { get; set; }

    public const int rectWidth = 100;
    public static Rect First_Rect = new Rect(0, 1, rectWidth, 38);
    public static Rect Second_Rect = new Rect(rectWidth * 1, 1, rectWidth, 40);
    public static Rect Third_Rect = new Rect(rectWidth * 2, 1, rectWidth, 40);
    public static Rect Fourth_Rect = new Rect(rectWidth * 3, 1, rectWidth, 40);
    public static Rect Fifth_Rect = new Rect(rectWidth * 4, 1, rectWidth, 40);

    public GameMaterialDatabase() { }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="GameMaterialDatabase"/> class.
	/// </summary>
	/// <param name='food'>
	/// Food.
	/// </param>
	/// <param name='wood'>
	/// Wood.
	/// </param>
	/// <param name='stone'>
	/// Stone.
	/// </param>
	/// <param name='gold'>
	/// Gold.
	/// </param>
	/// <param name='employee'>
	/// Employee.
	/// </param>
    public GameMaterialDatabase(int food, int wood, int stone, int gold, int employee)
    {
        this.Food = food;
        this.Wood = wood;
        this.Stone = stone;
        this.Gold = gold;
        this.Employee = employee;
    }
	
	/// <summary>
	/// Useds the resource.
	/// </summary>
	/// <param name='usedResource'>
	/// Used resource.
	/// </param>
    public static void UsedResource(GameMaterialDatabase usedResource)
    {
        StoreHouse.Remove_sumOfFood(usedResource.Food);
        StoreHouse.Remove_sumOfWood(usedResource.Wood);
        StoreHouse.Remove_sumOfStone(usedResource.Stone);
        StoreHouse.Remove_sumOfCopper(usedResource.Copper);
        StoreHouse.sumOfGold -= usedResource.Gold;
		
		StoreHouse.sumOfArmor -= usedResource.Armor;
		StoreHouse.sumOfWeapon -= usedResource.Weapon;
        //<!--- Population.
        HouseBeh.SumOfEmployee += usedResource.Employee;
        HouseBeh.SumOfUnemployed = HouseBeh.SumOfPopulation - HouseBeh.SumOfEmployee;
    }
}
