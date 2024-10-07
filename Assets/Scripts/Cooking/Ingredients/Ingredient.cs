using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public enum IngredientType
    {
        Grains,
        Spices,
        Meats,
        Seafood,
        Dairy,
        Sauces
    }

    public IngredientType ingredientType;
    public Sprite ingrendientSprite;
    public int ingredientCost;
}