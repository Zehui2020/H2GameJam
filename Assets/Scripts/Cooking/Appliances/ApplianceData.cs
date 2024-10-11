using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplianceData")]
public class ApplianceData : ShopItemData
{
    [System.Serializable]
    public struct ApplianceAppearance
    {
        public List<Ingredient.IngredientType> ingredientTypes;
        public Sprite applianceSprite;
    }

    public enum ApplianceType
    {
        Pot,
        Grill,
        HotPlate,
        Wok,
        Steamer,
        Pan,
        LargePan,
        TotalAppliances
    }

    [Header("Appliance Stats")]
    public Dish cookedDish;

    public Sprite baseSprite;
    public Sprite burntSprite;

    public string applianceName;

    public List<Ingredient.IngredientType> allowedIngredients = new();

    public bool needToCheckSequence;
    public List<ApplianceAppearance> applianceAppearances = new();

    public ApplianceType type;
    public float cookSpeed;
    public float cookDuration;
    public float burnDuration;
    public float burnGracePeriod;

    public int maxIngredients;
}