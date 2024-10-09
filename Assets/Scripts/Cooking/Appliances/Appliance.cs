using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    [System.Serializable]
    public struct CookedDish
    {
        public Dish dish;
        public int combinationIndex;

        public CookedDish(Dish dish, int index)
        {
            this.dish = dish;
            combinationIndex = index;
        }

        public static bool operator ==(CookedDish a, CookedDish b)
        {
            return a.dish == b.dish && a.combinationIndex == b.combinationIndex;
        }

        public static bool operator !=(CookedDish a, CookedDish b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CookedDish))
                return false;

            CookedDish other = (CookedDish)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            return dish.GetHashCode() ^ combinationIndex.GetHashCode();
        }
    }

    [Header("Appliance Stats")]
    [SerializeField] private ApplianceData applianceData;
    [SerializeField] private List<Ingredient.IngredientType> ingredients;
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
    }

    public virtual bool AddIngredient(Ingredient ingredient)
    {
        if (!CanPutIngredient(ingredient.ingredientType) || ingredients.Count >= applianceData.maxIngredients)
            return false;

        ingredients.Add(ingredient.ingredientType);
        applianceUIManager.AddIngredientUI(ingredient);

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
            cookingRoutine = null;
        }

        if (burnRoutine != null)
        {
            StopCoroutine(burnRoutine);
            burnRoutine = null;
            applianceUIManager.StopBurning();
        }
    }

    public void ServeFood()
    {
        if (!canServe)
            return;

        cookingTimer = 0;
        canServe = false;
        StopCooking();
        ingredients.Clear();
        applianceUIManager.ClearIngredientUI();
        spriteRenderer.sprite = applianceData.sprite;
    }

    public void DumpIngredients()
    {
        ingredients.Clear();
        applianceUIManager.StopBurning();
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
        for (int i = 0; i < applianceData.cookedDish.dishCombinations.Count; i++)
        {
            if (!Utility.AreListsEqualContent(applianceData.cookedDish.dishCombinations[i].ingredients, ingredients) ||
                applianceData.cookedDish.dishCombinations[i].sideDishes.Count != 0)
                continue;

            spriteRenderer.sprite = applianceData.cookedSprite;
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
        spriteRenderer.sprite = applianceData.burntSprite;
        canServe = false;
    }

    private bool CanPutIngredient(Ingredient.IngredientType newIngredient)
    {
        foreach (Ingredient.IngredientType ingredient in applianceData.allowedIngredients)
        {
            if (ingredient.Equals(newIngredient))
                return true;
        }

        return false;
    }
}