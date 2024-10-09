using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplianceData")]
public class ApplianceData : ShopItemData
{
    public enum ApplianceType
    {
        Pot,
        Grill,
        HotPlate,
        Wok,
        Steamer,
        Pan,
        TotalAppliances
    }

    [Header("Appliance Stats")]
    public Dish cookedDish;

    public Sprite sprite;
    public Sprite cookedSprite;
    public Sprite burntSprite;

    public string applianceName;
    [TextArea(3, 10)] public string applianceDescription;

    public List<Ingredient.IngredientType> allowedIngredients = new();
    public ApplianceType type;
    public float cookSpeed;
    public float cookDuration;
    public float burnDuration;
    public float burnGracePeriod;

    public int maxIngredients;
}