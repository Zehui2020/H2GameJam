using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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
    public float currentMoney;

    [Header("Reputation")]
    public int currentReputation;

    [Header("Day Counter")]
    public List<string> cookingScenes = new();
    public int dayCounter;
    public int daysPerGeneration;

    [Header("Dialogue")]
    public List<DialogueData> allDialogueDatas;
    public List<Sprite> playerDialogueSprites;

    [Header("Animations")]
    public List<RuntimeAnimatorController> playerAnimators;

    private Queue<Vector2> popupQueue = new Queue<Vector2>();
    private Coroutine popupRoutine;

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
        dayCounter = 0;

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
    public void AddMoney(float moneyToGain, Transform position)
    {
        currentMoney += moneyToGain;

        if (position == null)
            return;

        ShowPopup("+" + moneyToGain.ToString(), position, NotificationPopup.PopupType.MoneyIncrease);
    }
    public void RemoveMoney(float moneyToLose, Transform position)
    {
        currentMoney -= moneyToLose;

        if (position == null)
            return;

        ShowPopup("-" + moneyToLose.ToString(), position, NotificationPopup.PopupType.MoneyDecrease);
    }

    public List<Dish> GetDishesOfThisGeneration()
    {
        return currentGeneration.dishesToCook;
    }

    public void AddRep(int repToGain, Transform position)
    {
        currentReputation += repToGain;
        ShowPopup("+" + repToGain.ToString(), position, NotificationPopup.PopupType.ReputationIncrease);
    }

    public void LoseRep(int repToLose, Transform position)
    {
        currentReputation -= repToLose;
        ShowPopup("-" + repToLose.ToString(), position, NotificationPopup.PopupType.ReputationDecrease);
    }

    public void ShowPopup(string notification, Transform position, NotificationPopup.PopupType notificationType)
    {
        popupQueue.Enqueue(position.position);
        if (popupRoutine == null)
            popupRoutine = StartCoroutine(ShowPopupRoutine(notification, notificationType));
    }
    private IEnumerator ShowPopupRoutine(string notification, NotificationPopup.PopupType notificationType)
    {
        while (popupQueue.Count > 0)
        {
            Vector2 spawnPos = popupQueue.Dequeue();

            NotificationPopup notificationPopup = ObjectPool.Instance.GetPooledObject("NotificationPopup", true) as NotificationPopup;
            notificationPopup.SetupPopup(notification, spawnPos, notificationType);

            yield return new WaitForSeconds(0.5f);

            yield return null;
        }

        popupRoutine = null;
    }

    public Sprite GetPlayerSprite()
    {
        return playerDialogueSprites[generationIndex];
    }

    public void GoToCook()
    {
        SceneLoader.Instance.LoadScene(cookingScenes[generationIndex]);
    }

    public void GoToShop()
    {
        dayCounter++;

        //check if bankrupt
        if (PlayerStats.playerStatsInstance.GetPlayerMoney() <= 0)
        {
            //go to bankrupt scene
            SceneLoader.Instance.LoadScene("BankruptEndingScene");
            return;
        }

        if (dayCounter >= daysPerGeneration)
        {
            dayCounter = 1;
            generationIndex++;

            if (generationIndex >= allGenerations.Count)
            {
                if (PlayerStats.playerStatsInstance.currentReputation > 0)
                {
                    SceneLoader.Instance.LoadScene("GoodEndingScene");
                }
                else
                {
                    SceneLoader.Instance.LoadScene("BadEndingScene");
                }
                
                return;
            }

            currentGeneration = allGenerations[generationIndex];
            SceneLoader.Instance.LoadScene("MarketPlace");
        }
    }

    public DialogueData GetDialogueData(BaseNPC.NPCType npcType)
    {
        foreach (DialogueData dialogueData in allDialogueDatas)
        {
            if (dialogueData.npcType == npcType &&
                dialogueData.generation == currentGeneration.generation &&
                dialogueData.dayNumber == dayCounter)
                return dialogueData;
        }

        return null;
    }

    public float GetPlayerMoney()
    {
        return Mathf.Round(currentMoney * 100f) / 100f;
    }

    public RuntimeAnimatorController GetPlayerRuntimeController()
    {
        return playerAnimators[generationIndex];
    }
}