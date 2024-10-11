using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : MonoBehaviour, IAbleToAddIngredient
{
    [System.Serializable]
    public class CookedDish
    {
        public Dish dish;
        public int combinationIndex;

        public CookedDish(Dish dish, int index)
        {
            this.dish = dish;
            combinationIndex = index;
        }
    }

    [Header("Appliance Stats")]
    public ApplianceData applianceData;
    [SerializeField] protected List<Ingredient.IngredientType> ingredients;
    private ApplianceUIManager applianceUIManager;
    private SpriteRenderer spriteRenderer;

    private float cookingTimer = 0;
    public StatModifier cookingSpeedModifier;

    private Coroutine cookingRoutine;
    private Coroutine burnRoutine;
    protected bool canServe = false;

    protected CookedDish cookedDish;

    private void Start()
    {
        InitAppliance();
    }

    public virtual void InitAppliance()
    {
        applianceUIManager = GetComponent<ApplianceUIManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (PlayerStats.playerStatsInstance.GetApplianceLevel(applianceData))
        {
            case 1:
                cookingSpeedModifier.AddModifier(0.15f);
                break;

            case 2:
                cookingSpeedModifier.AddModifier(0.25f);
                break;

            case 3:
                cookingSpeedModifier.AddModifier(0.40f);
                break;
        }
    }

    public virtual bool AddIngredient(Ingredient ingredient)
    {
        if (!CanPutIngredient(ingredient) || ingredients.Count >= applianceData.maxIngredients)
            return false;

        ingredients.Add(ingredient.ingredientType);
        applianceUIManager.AddIngredientUI(ingredient);
        SetSprite();

        if (burnRoutine != null)
        {
            StopCoroutine(burnRoutine);
            burnRoutine = null;
            applianceUIManager.StopBurning();

            cookingTimer -= applianceData.cookDuration * 0.35f;
            if (cookingTimer <= 0)
                cookingTimer = 0;
        }

        if (cookingRoutine == null)
            cookingRoutine = StartCoroutine(StartCooking());
        else
        {
            cookingTimer -= applianceData.cookDuration * 0.35f;
            if (cookingTimer <= 0)
                cookingTimer = 0;
        }

        if (ingredients.Count >= applianceData.maxIngredients)
            applianceUIManager.HideAddIngredientUI();

        return true;
    }

    public void StopCooking()
    {
        if (cookingRoutine != null)
        {
            StopCoroutine(cookingRoutine);

            switch (applianceData.type)
            {
                case ApplianceData.ApplianceType.Pot:
                    AudioManager.Instance.Stop("PotBoil");
                    break;
                case ApplianceData.ApplianceType.Grill:
                    AudioManager.Instance.Stop("Grill");
                    break;
                case ApplianceData.ApplianceType.HotPlate:
                    AudioManager.Instance.Stop("HotPlate");
                    break;
                case ApplianceData.ApplianceType.Wok:
                    AudioManager.Instance.Stop("Wok");
                    break;
                case ApplianceData.ApplianceType.Steamer:
                    AudioManager.Instance.Stop("Steamer");
                    break;
                case ApplianceData.ApplianceType.Pan:
                    AudioManager.Instance.Stop("PanSizzle");
                    break;
                case ApplianceData.ApplianceType.LargePan:
                    AudioManager.Instance.Stop("PanSizzle");
                    break;
            }

            cookingRoutine = null;
        }

        if (burnRoutine != null)
        {
            StopCoroutine(burnRoutine);
            burnRoutine = null;
            applianceUIManager.StopBurning();
        }
    }

    public void ResumeCooking()
    {
        cookingRoutine = StartCoroutine(StartCooking());
    }

    public void ServeFood()
    {
        if (!canServe)
            return;

        DumpIngredients();
    }

    public CookedDish GetCookedDish(Utensil utensil)
    {
        if (!canServe)
            return null;

        if (utensil.utensilType != cookedDish.dish.utensil)
        {
            Debug.Log("INCORRECT UTENSIL!");
            return null;
        }

        ServeFood();
        return cookedDish;
    }

    public void DumpIngredients()
    {
        cookingTimer = 0;
        canServe = false;
        StopCooking();
        ingredients.Clear();
        applianceUIManager.ClearIngredientUI();
        applianceUIManager.SetCookingSliderActive(false);
        spriteRenderer.sprite = applianceData.baseSprite;
    }

    private IEnumerator StartCooking()
    {

        while (true)
        {
            cookingTimer += Time.deltaTime * (applianceData.cookSpeed + cookingSpeedModifier.GetTotalModifier());
            applianceUIManager.SetCookingSlider(cookingTimer, applianceData.cookDuration);
            applianceUIManager.SetCookingSliderActive(true);

            if (cookingTimer >= applianceData.cookDuration)
            {
                applianceUIManager.SetCookingSliderActive(false);
                CookFood();
                burnRoutine = StartCoroutine(BurnRoutine());
                cookingRoutine = null;
                cookingTimer = applianceData.cookDuration;
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator BurnRoutine()
    {
        yield return new WaitForSeconds(applianceData.burnGracePeriod);

        float burnTimer = 0;

        while (burnTimer < applianceData.burnDuration)
        {
            burnTimer += Time.deltaTime;
            applianceUIManager.SetBurnWarningSpeed(burnTimer / applianceData.burnDuration);

            if (burnTimer >= applianceData.burnDuration)
            {
                BurnFood();
                yield break;
            }

            yield return null;
        }

        burnRoutine = null;
    }

    public virtual bool CookFood()
    {
        AudioManager.Instance.PlayOneShot("FinishedCooking");
        for (int i = 0; i < applianceData.cookedDish.dishCombinations.Count; i++)
        {
            if (!Utility.AreListsEqualContent(applianceData.cookedDish.dishCombinations[i].ingredients, ingredients))
                continue;

            SetSprite();
            canServe = true;
            applianceUIManager.OnFoodCooked();
            cookedDish = new CookedDish(applianceData.cookedDish, i);
            return true;
        }

        return false;
    }

    private void BurnFood()
    {
        cookingRoutine = null;
        applianceUIManager.StopBurning();
        if (spriteRenderer != null)
            spriteRenderer.sprite = applianceData.burntSprite;
        canServe = false;
    }

    protected void SetSprite()
    {
        foreach (ApplianceData.ApplianceAppearance appearance in applianceData.applianceAppearances)
        {
            if (Utility.AreListsEqualContent(appearance.ingredientTypes, ingredients))
            {
                spriteRenderer.sprite = appearance.applianceSprite;
                return;
            }
        }
    }

    protected bool CanPutIngredient(Ingredient newIngredient)
    {
        if (ingredients.Count >= applianceData.maxIngredients)
        {
            AudioManager.Instance.PlayOneShot("IncorrectOrder");
            PlayerStats.playerStatsInstance.ShowPopup("Max Ingredient!", transform, NotificationPopup.PopupType.TextNotification);
            return false;
        }

        // Check for allowed ingredient type
        foreach (Ingredient.IngredientType ingredient in applianceData.allowedIngredients)
        {
            if (ingredient == newIngredient.ingredientType)
            {
                if (applianceData.needToCheckSequence)
                    break;
                else
                    return true;
            }
        }

        if (!applianceData.needToCheckSequence)
        {
            AudioManager.Instance.PlayOneShot("IncorrectOrder");
            PlayerStats.playerStatsInstance.ShowPopup("Wrong Ingredient!", transform, NotificationPopup.PopupType.TextNotification);
            return false;
        }

        // Check for ingredient sequence
        foreach (Ingredient.IngredientType ingredientType in applianceData.applianceAppearances[ingredients.Count].ingredientTypes)
        {
            if (ingredientType == newIngredient.ingredientType)
                return true;
        }
        AudioManager.Instance.PlayOneShot("IncorrectOrder");
        PlayerStats.playerStatsInstance.ShowPopup("Wrong Sequence!", transform, NotificationPopup.PopupType.TextNotification);
        return false;
    }
}