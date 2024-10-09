using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script by Andrew
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStatsInstance;

    [System.Serializable]
    public struct GenerationDishes
    {
        public enum Generation
        {
            Origins,
            Renaissance,
            Morden
        }

        public Generation generation;
        public List<Dish> dishes;
    }

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

    [Header("Generation Dishes")]
    public GenerationDishes.Generation currentGeneration;
    public List<GenerationDishes> generationDishes;

    [Header("Reputation")]
    public int currentReputation;

    [Header("Day Counter")]
    public int dayCounter;

    [Header("Appliances")]
    public int potLevel = 1; public int grillLevel = 1; public int hotPlateLevel = 1; public int wokLevel = 1; public int steamerLevel = 1; public int panLevel = 1;
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
        currentMoney = 500;

        porkCount = 0;
        chickenCount = 0;
        lambCount = 0;
        beefCount = 0;
        crabCount = 0;
        eggCount = 0;
        flourCount = 0;
        riceCount = 0;
        doughCount = 0;
        chineseSpicesCount = 0;
        indianSpicesCount = 0;
        chilliSauceCount = 0;
        currySauceCount = 0;
        sataySauceCount = 0;

        currentReputation = 0;
        dayCounter = 1;

        potLevel = 1;
        grillLevel = 1;
        hotPlateLevel = 1;
        wokLevel = 1;
        steamerLevel = 1;
        panLevel = 1;
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

    public bool CheckEnoughIngredients(Appliance.CookedDish _reqDish)
    {
        //list all ingredients
        List<Ingredient.IngredientType> ingredients = _reqDish.dish.dishCombinations[_reqDish.combinationIndex].ingredients;
        //list side dish ingredients
        foreach (Dish d in _reqDish.dish.dishCombinations[_reqDish.combinationIndex].sideDishes)
        {
            foreach (Ingredient.IngredientType i in d.dishCombinations[0].ingredients)
                ingredients.Add(i);
        }

        //check if have enough
        foreach (Ingredient.IngredientType i in ingredients)
        {
            //get total of that type in list
            int total = 0;

            foreach (Ingredient.IngredientType j in ingredients)
            {
                if (i == j)
                {
                    total += 1;
                }
            }

            //check if not enough
            switch (i)
            {
                case Ingredient.IngredientType.Pork:
                    if (total < porkCount)
                        return false;
                    break;
                case Ingredient.IngredientType.Chicken:
                    if (total < chickenCount)
                        return false;
                    break;
                case Ingredient.IngredientType.Lamb:
                    if (total < lambCount)
                        return false;
                    break;
                case Ingredient.IngredientType.Rice:
                    if (total < riceCount)
                        return false;
                    break;
                case Ingredient.IngredientType.Dough:
                    if (total < doughCount)
                        return false;
                    break;
                case Ingredient.IngredientType.ChineseSpices:
                    if (total < chineseSpicesCount)
                        return false;
                    break;
                case Ingredient.IngredientType.IndianSpices:
                    if (total < indianSpicesCount)
                        return false;
                    break;
                case Ingredient.IngredientType.ChilliSauce:
                    if (total < chilliSauceCount)
                        return false;
                    break;
                case Ingredient.IngredientType.CurrySauce:
                    if (total < currySauceCount)
                        return false;
                    break;
                case Ingredient.IngredientType.SataySauce:
                    if (total < sataySauceCount)
                        return false;
                    break;
                case Ingredient.IngredientType.Egg:
                    if (total < eggCount)
                        return false;
                    break;
                case Ingredient.IngredientType.Crab:
                    if (total < crabCount)
                        return false;
                    break;
                default:
                    Debug.LogError("Ingredient not found, register the ingredient inside the Ingredient script.");
                    break;
            }
        }


        return true;
}

    public int GetApplianceLevel(ApplianceData applianceData)
    {
        switch (applianceData.type)
        {
            case ApplianceData.ApplianceType.Pot:
                return potLevel;

            case ApplianceData.ApplianceType.Grill:
                return grillLevel;

            case ApplianceData.ApplianceType.HotPlate:
                return hotPlateLevel;

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
            case ApplianceData.ApplianceType.HotPlate:
                hotPlateLevel += 1;
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

    public List<Dish> GetDishesOfThisGeneration()
    {
        foreach (GenerationDishes generationDish in generationDishes)
        {
            if (generationDish.generation.Equals(currentGeneration))
                return generationDish.dishes;
        }

        return null;
    }

    public void AddRep(int repToGain)
    {
        currentReputation += repToGain;
    }

    public void LoseRep(int repToLose)
    {
        currentReputation -= repToLose;
    }

    public void IncrementDay()
    {
        dayCounter++;
    }
}