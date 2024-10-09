using System.Collections.Generic;
using UnityEngine;

public class Grill : Appliance
{
    [SerializeField] private List<Transform> foodSpawnPos;
    [SerializeField] private DishPickup dishPickup;
    private List<DishPickup> dishPickups;

    [SerializeField] private Dish satayDish;

    public override void InitAppliance()
    {
        base.InitAppliance();
    }

    public override bool AddIngredient(Ingredient ingredient)
    {
        if (!base.AddIngredient(ingredient))
            return false;

        foreach (Transform spawnPos in foodSpawnPos)
        {
            if (spawnPos.childCount > 0)
                continue;

            DishPickup newDishPickup = Instantiate(dishPickup, spawnPos);
            newDishPickup.InitPickup(new CookedDish(satayDish, satayDish.GetDishCombinationIndex(ingredient)));
            dishPickups.Add(newDishPickup);

            return true;
        }

        return true;
    }

    public override bool CookFood()
    {
        if (!base.CookFood())
            return false;

        foreach (DishPickup dishPickup in dishPickups)
            dishPickup.SetCanDrag(true);

        return true;
    }
}