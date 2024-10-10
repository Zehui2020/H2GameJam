using DesignPatterns.ObjectPool;
using System.Collections.Generic;
using UnityEngine.UI;

public class IngredientBooth : MarketBooth
{
    public override void InitBooth()
    {
        if (buttonParent.childCount != 0)
            return;

        base.InitBooth();

        List<Ingredient> ingredients = PlayerStats.playerStatsInstance.currentGeneration.ingredientsToSell;
        foreach (Ingredient ingredient in ingredients)
        {
            BoothButton button = ObjectPool.Instance.GetPooledObject("BoothButton", true) as BoothButton;
            button.SetUpButton(ingredient);
            button.OnSelectButton += (item) => { confirmBuyPanel.ShowPannel(button, item); };
            button.transform.SetParent(buttonParent);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonParent);
    }
}   