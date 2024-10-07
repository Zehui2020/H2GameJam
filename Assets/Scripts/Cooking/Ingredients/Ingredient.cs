using UnityEngine;

public class Ingredient : ScriptableObject
{
    public enum IngredientType
    {
        Grains,
        Spices,
        Meats,
        Seafood,
        Dairy
    }

    public IngredientType ingredientType;
    public Sprite ingrendientSprite;
    public int ingredientCost;
}