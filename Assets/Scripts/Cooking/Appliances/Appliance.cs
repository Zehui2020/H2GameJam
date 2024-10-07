using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    [SerializeField] private ApplianceData applianceData;

    private List<Ingredient.IngredientType> ingredients;
    private float cookingTimer;

    private Coroutine cookingRoutine;

    private bool isFinishedCooking = false;

    public void AddIngredient(Ingredient.IngredientType ingredient)
    {
        if (!CanPutIngredient(ingredient))
            return;

        ingredients.Add(ingredient);

        if (cookingRoutine == null)
            cookingRoutine = StartCoroutine(StartCooking());
    }

    public void TakeFood()
    {
        if (!isFinishedCooking)
            return;

        isFinishedCooking = false;
    }

    public void DumpIngredients()
    {
        ingredients.Clear();
    }

    private IEnumerator StartCooking()
    {
        while (true)
        {
            cookingTimer += Time.deltaTime;

            if (cookingTimer >= applianceData.cookDuration && cookingTimer < applianceData.burnDuration)
                isFinishedCooking = true;
            if (cookingTimer >= applianceData.burnDuration)
                yield return null;

            yield return null;
        }
    }

    private bool CanPutIngredient(Ingredient.IngredientType ingredient)
    {
        foreach (Dish dish in applianceData.allowedDishes)
        {
            if (dish.ingredients.Contains(ingredient))
                return true;
        }

        return false;
    }
}