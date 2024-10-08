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
    public int currenctMoney;

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

    public void AddMoney(int moneyToGain)
    {
        currenctMoney += moneyToGain;
    }

    public void RemoveMoney(int moneyToLose)
    {
        currenctMoney -= moneyToLose;
    }
}
