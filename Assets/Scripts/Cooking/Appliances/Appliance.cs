using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    [Header("Appliance Stats")]
    [SerializeField] private ApplianceData applianceData;
    [SerializeField] private List<Ingredient.IngredientType> ingredients;
    private ApplianceUIManager applianceUIManager;
    private SpriteRenderer spriteRenderer;

    private float cookingTimer = 0;
    private Coroutine cookingRoutine;
    private Coroutine burnRoutine;
    private bool canServe = false;

    private void Start()
    {
        applianceUIManager = GetComponent<ApplianceUIManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (!CanPutIngredient(ingredient.ingredientType) || ingredients.Count >= applianceData.maxIngredients)
            return;

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
            cookingTimer += Time.deltaTime * applianceData.cookSpeed;
            applianceUIManager.SetCookingSlider(cookingTimer, applianceData.cookDuration);

            if (cookingTimer >= applianceData.cookDuration)
            {
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

    private void CookFood()
    {
        foreach (Dish.DishCombinations combinations in applianceData.cookedDish.dishCombinations)
        {
            if (!Utility.AreListsEqualContent(combinations.ingredients, ingredients))
                continue;

            spriteRenderer.sprite = applianceData.cookedSprite;
            canServe = true;

            break;
        }
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