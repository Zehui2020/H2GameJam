using System.Collections.Generic;
using UnityEngine;
using static Ingredient;
/// <summary>
/// Script by Andrew
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStatsInstance;

    public List<GenerationData> allGenerations = new();

    [System.Serializable]
    public class IngredientCount
    {
        public IngredientType ingredient;
        public int ingredientCount;

        public IngredientCount(Ingredient.IngredientType ingredient, int ingredientCount)
        {
            this.ingredient = ingredient;
            this.ingredientCount = ingredientCount;
        }
    }
    [SerializeField] private List<IngredientCount> ingredientCountList;

    [System.Serializable]
    public class ApplianceLevel
    {
        public ApplianceData.ApplianceType applianceType;
        public int applianceLevel;

        public ApplianceLevel(ApplianceData.ApplianceType appliance, int applianceLevel)
        {
            this.applianceType = appliance;
            this.applianceLevel = applianceLevel;
        }
    }
    [SerializeField] private List<ApplianceLevel> applianceLevelList;

    [Header("Current Generation")]
    public GenerationData currentGeneration;
    public int generationIndex = 0;

    [Header("Money")]
    public int currentMoney;

    [Header("Reputation")]
    public int currentReputation;

    [Header("Day Counter")]
    public List<string> cookingScenes = new();
    public int dayCounter;
    public int daysPerGeneration;

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
        currentReputation = 0;
        dayCounter = 1;

        for (int i = 0; i < (int)Ingredient.IngredientType.TotalIngredients; i++)
            ingredientCountList.Add(new IngredientCount((Ingredient.IngredientType)i, 20));

        for (int i = 0; i < (int)ApplianceData.ApplianceType.TotalAppliances; i++)
            applianceLevelList.Add(new ApplianceLevel((ApplianceData.ApplianceType)i, 0));

        // Set to -1 for tutorial!!!
        foreach (ApplianceLevel appliance in applianceLevelList)
        {
            if (appliance.applianceType == ApplianceData.ApplianceType.Pot)
            {
                appliance.applianceLevel = -1;
                break;
            }
        }
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
        for (int i = 0; i < ingredientCountList.Count; i++)
        {
            if (ingredient.ingredientType == ingredientCountList[i].ingredient)
            {
                ingredientCountList[i].ingredientCount += numOfItems;
                return;
            }
        }

        Debug.LogError("FAILED TO ADD INGRDIENT");
    }

    public int GetIngredientCount(IngredientType ingredient)
    {
        foreach (IngredientCount ingredientCount in ingredientCountList)
        {
            if (ingredientCount.ingredient == ingredient)
                return ingredientCount.ingredientCount;
        }

        Debug.LogError("FAILED TO GET INGRDIENT");

        return -1;
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

            foreach (IngredientCount ingredient in ingredientCountList)
            {
                if (i == ingredient.ingredient && total >= ingredient.ingredientCount)
                    return false;
            }
        }

        return true;
    }

    public int GetApplianceLevel(ApplianceData applianceData)
    {
        foreach (ApplianceLevel appliance in applianceLevelList)
        {
            if (appliance.applianceType == applianceData.type)
                return appliance.applianceLevel;
        }

        Debug.LogError("FAILED TO GET APPLIANCE LEVEL");

        return 0;
    }

    public void UpgradeAppliances(ApplianceData applianceData)
    {
        for (int i = 0; i < applianceLevelList.Count; i++)
        {
            if (applianceData.type == applianceLevelList[i].applianceType)
            {
                applianceLevelList[i].applianceLevel += 1;
                return;
            }
        }

        Debug.LogError("FAILED TO UPGRADE APPLIANCE LEVEL");
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
        return currentGeneration.dishesToCook;
    }

    public void AddRep(int repToGain)
    {
        currentReputation += repToGain;
    }

    public void LoseRep(int repToLose)
    {
        currentReputation -= repToLose;
    }

    public void GoToCook()
    {
        SceneLoader.Instance.LoadScene(cookingScenes[generationIndex]);
    }

    public void GoToShop()
    {
        dayCounter++;

        if (dayCounter >= daysPerGeneration)
        {
            dayCounter = 0;
            generationIndex++;

            if (generationIndex >= allGenerations.Count)
            {
                SceneLoader.Instance.LoadScene("EndingScene");
                return;
            }

            currentGeneration = allGenerations[generationIndex];
            SceneLoader.Instance.LoadScene("MarketPlace");
        }
    }
}