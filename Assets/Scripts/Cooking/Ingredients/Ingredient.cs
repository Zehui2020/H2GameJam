using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public enum IngredientType
    {
        // Meats
        Pork,
        Chicken,
        Lamb,
        // Grains
        Rice,
        Dough,
        // Spices
        ChineseSpices,
        IndianSpices,
        // Sauces
        SataySauce,
        CurrySauce,
        ChilliSauce,
        // Dairy
        Egg,
        // Seafood
        Crab
    }

    public IngredientType ingredientType;
    public Sprite ingrendientSprite;
    public int ingredientCost;
}