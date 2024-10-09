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
        ChickenRice,
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
        public List<Dish> sideDishes;
    }

    public bool doesCombinationIndexMatter;
    public Utensil.UtensilType utensil;
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