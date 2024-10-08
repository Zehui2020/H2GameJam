using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script by Andrew
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStatsInstance;
    [Header("Money")]
    public int currentMoney;

    [Header("Meat Count")]
    public int porkCount; public int chickenCount; public int lambCount; public int beefCount;

    [Header("Seafood Count")]
    public int crabCount;

    [Header("Dairy Count")]
    public int eggCount;

    [Header("Grain Count")]
    public int flourCount; public int riceCount; public int doughCount;

    [Header("Spices Count")]
    public int chineseSpicesCount; public int indianSpicesCount;

    [Header("Sauces Count")]
    public int chilliSauceCount; public int currySauceCount; public int sataySauceCount;

    [Header("Appliances")]
    public int potLevel = 1; public int grillLevel = 1; public int deepFryerLevel = 1; public int wokLevel = 1; public int steamerLevel = 1; public int panLevel = 1;
    private void Awake()
    {
        if (playerStatsInstance == null)
        {
            playerStatsInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (playerStatsInstance != this)
        {
            Destroy(gameObject);
        }

        SetUp();
    }
    private void SetUp()
    {

    }

    public enum PlayerMarketState
    {
        Walk,
        InMenu,
    }

    public enum ShopMenuInRange
    {
        None = 0,
        Ingredient,
        Appliance,
    }

    public PlayerMarketState playerMarketState;
    public ShopMenuInRange shopMenuInRange = 0;

    public void AddToPlayerInventory(int numOfItems, Ingredient ingredient)
    {
        switch (ingredient.ingredientType)
        {
            case Ingredient.IngredientType.Pork:
                porkCount += numOfItems;
                break;
            case Ingredient.IngredientType.Chicken:
                chickenCount += numOfItems;
                break;
            case Ingredient.IngredientType.Lamb:
                lambCount += numOfItems;
                break;
            case Ingredient.IngredientType.Rice:
                riceCount += numOfItems; 
                break;
            case Ingredient.IngredientType.Dough:
                doughCount += numOfItems; 
                break;
            case Ingredient.IngredientType.ChineseSpices:
                chineseSpicesCount += numOfItems; 
                break;
            case Ingredient.IngredientType.IndianSpices:
                indianSpicesCount += numOfItems; 
                break;
            case Ingredient.IngredientType.ChilliSauce:
                chilliSauceCount += numOfItems;
                break;
            case Ingredient.IngredientType.CurrySauce:
                currySauceCount += numOfItems; 
                break;
            case Ingredient.IngredientType.SataySauce:
                sataySauceCount += numOfItems;
                break;
            case Ingredient.IngredientType.Egg:
                eggCount += numOfItems; 
                break;
            case Ingredient.IngredientType.Crab:
                crabCount += numOfItems; 
                break;
            default:
                Debug.LogError("Ingredient not found, register the ingredient inside the Ingredient script.");
                break;
        }
    }

    public int GetApplianceLevel(ApplianceData applianceData)
    {
        switch (applianceData.type)
        {
            case ApplianceData.ApplianceType.Pot:
                return potLevel;

            case ApplianceData.ApplianceType.Grill:
                return grillLevel;

            case ApplianceData.ApplianceType.DeepFryer:
                return deepFryerLevel;

            case ApplianceData.ApplianceType.Wok:
                return wokLevel;

            case ApplianceData.ApplianceType.Steamer:
                return steamerLevel;

            case ApplianceData.ApplianceType.Pan:
                return panLevel;
     
            default:
                Debug.LogError("Not an appliance passed in");
                return 0; 
        }
    }

    public void UpgradeAppliances(ApplianceData applianceData)
    {
        switch (applianceData.type)
        {
            case ApplianceData.ApplianceType.Pot:
                potLevel += 1;
                break;
            case ApplianceData.ApplianceType.Grill:
                grillLevel += 1;
                break;
            case ApplianceData.ApplianceType.DeepFryer:
                deepFryerLevel += 1;
                break;
            case ApplianceData.ApplianceType.Wok:
                wokLevel += 1;
                break;
            case ApplianceData.ApplianceType.Steamer:
                steamerLevel += 1;
                break;
            case ApplianceData.ApplianceType.Pan:
                panLevel += 1;
                break;
        }
    }

    public void AddMoney(int moneyToGain)
    {
        currentMoney += moneyToGain;
    }

    public void RemoveMoney(int moneyToLose)
    {
        currentMoney -= moneyToLose;
    }
}
