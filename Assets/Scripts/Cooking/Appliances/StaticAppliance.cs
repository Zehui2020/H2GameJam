using System.Collections.Generic;
using UnityEngine;

public class StaticAppliance : Appliance
{
    [SerializeField] private List<Transform> foodSpawnPos;
    [SerializeField] private CookingIngredient cookingIngredient;
    private List<CookingIngredient> cookingIngredients = new();

    [SerializeField] private Dish cookedIngredientDish;

    public override void InitAppliance()
    {
        base.InitAppliance();
    }

    public override bool AddIngredient(Ingredient ingredient)
    {
        if (!CanPutIngredient(ingredient))
            return false;

        foreach (Transform spawnPos in foodSpawnPos)
        {
            if (spawnPos.childCount > 0)
                continue;

            CookingIngredient newDishPickup = Instantiate(cookingIngredient, spawnPos);
            newDishPickup.InitCookingIngredient(this, new CookedDish(cookedIngredientDish, cookedIngredientDish.GetDishCombinationIndex(ingredient)));
            cookingIngredients.Add(newDishPickup);

            return true;
        }

        return true;
    }
}