using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplianceData")]
public class ApplianceData : ScriptableObject
{
    public enum ApplianceType
    {
        Pot,
        Grill,
        DeepFryer,
        Wok,
        Steamer,
        Pan
    }

    public Dish cookedDish;
    public Sprite cookedSprite;
    public Sprite burntSprite;

    public List<Ingredient.IngredientType> allowedIngredients = new();
    public ApplianceType type;
    public float cookSpeed;
    public float cookDuration;
    public float burnDuration;
    public float burnGracePeriod;

    public int maxIngredients;
}