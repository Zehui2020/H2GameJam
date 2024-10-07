using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dish")]
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
    public List<Ingredient.IngredientType> ingredients = new();
    public int dishCost;
}