using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient")]
public class Ingredient : ShopItemData
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
        Crab,
        Fish,
        TotalIngredients
    }

    [Header("Appliance Stats")]
    public Dish dishOnPlate;
    public IngredientType ingredientType;
    public Sprite ingrendientSprite;
}