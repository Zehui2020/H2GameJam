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
        NasiLemak,
        TotalDishes
    }

    [System.Serializable]
    public struct DishCombinations
    {
        public List<Ingredient.IngredientType> ingredients;
    }

    public DishType dishType;
    public List<DishCombinations> dishCombinations = new();
    public Sprite dishSprite;
    public int dishCost;
}