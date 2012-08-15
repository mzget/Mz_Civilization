using UnityEngine;
using System.Collections;


public class GameResource
{
    public int Food;
    public int Wood;
    public int Gold;
    public int Stone;

    public static Rect RequireResource_Rect = new Rect(10, 100, 400, 32);
    public static Rect Food_Rect = new Rect(0, 1, 100, 30);
    public static Rect Wood_Rect = new Rect(100, 1, 100, 30);
    public static Rect Copper_Rect = new Rect(200, 1, 100, 30);
    public static Rect Stone_Rect = new Rect(300, 1, 100, 30);

    public GameResource(int food, int wood, int gold, int stone)
    {
        this.Food = food;
        this.Wood = wood;
        this.Gold = gold;
        this.Stone = stone;
    }
}
