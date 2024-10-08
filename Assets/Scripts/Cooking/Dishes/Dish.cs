using System.Collections.Generic;
using System.Linq;
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
        CurryBowl,
        RiceBowl,
        TotalDishes,
        EmptyDish
    }

    [System.Serializable]
    public struct DishCombinations
    {
        public List<Ingredient.IngredientType> ingredients;
        public List<Sprite> requirementSprites;
    }

    public DishType dishType;
    public List<DishCombinations> dishCombinations = new();
    public Sprite dishSprite;
    public int dishCost;

    public int GetDishCombinationIndex(Ingredient ingredient)
    {
        for (int i = 0; i < dishCombinations.Count; i++)
        {
            if (dishCombinations[i].ingredients.Contains(ingredient.ingredientType))
                return i;
        }

        return -1;
    }
}