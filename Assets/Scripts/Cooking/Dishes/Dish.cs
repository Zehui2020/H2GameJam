using System.Collections.Generic;
using UnityEngine;

public class Dish : ScriptableObject
{
    public enum DishType
    {
        BakKutTeh,
        Satay,
        RotiPrata,
        KatsuDon,
        ChilliCrab,
        Biryani,
        NasiLemak
    }

    public DishType dishType;
    public List<Ingredient> ingredients = new();
    public int dishCost;
}