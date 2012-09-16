using UnityEngine;
using System.Collections;


public class GameResource
{
    public int Food;
    public int Wood;
    public int Stone;
    public int Gold;
    public int Employee;

    public static Rect RequireResource_Rect = new Rect(10, 100, 500, 32);
    public static Rect Food_Rect = new Rect(0, 1, 100, 32);
    public static Rect Wood_Rect = new Rect(100, 1, 100, 32);
    public static Rect Stone_Rect = new Rect(200, 1, 100, 32);
    public static Rect Gold_Rect = new Rect(300, 1, 100, 32);
    public static Rect Employee_Rect = new Rect(400, 1, 100, 32);

    public GameResource(int food, int wood, int stone, int gold, int employee)
    {
        this.Food = food;
        this.Wood = wood;
        this.Stone = stone;
        this.Gold = gold;
        this.Employee = employee;
    }
}
