using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConfirmBuyIngredientPanel : ConfirmBuyPanel
{
    [SerializeField] private Slider buySlider;
    [SerializeField] private TextMeshProUGUI ingredientToBuyName;
    [SerializeField] private TextMeshProUGUI descriptionOfItem;
    [SerializeField] private Image imageItemToBuy;
    [SerializeField] private Ingredient storedIngredient;
    [SerializeField] private BoothButton storedButton;

    protected override void InitPanel(int numberOfItems)
    {
        base.InitPanel(numberOfItems);
        buySlider.maxValue = numberOfItems;
        buySlider.value = numberOfItems;
    }
    public void InitNewItemToBuy(Ingredient ingredient, int numberOfItems, BoothButton button)
    {
        ClearPanel();
        InitPanel(numberOfItems);
        storedIngredient = ingredient;
        storedButton = button;
        imageItemToBuy.sprite = ingredient.ingrendientSprite;
        ingredientToBuyName.text = ingredient.ingredientType.ToString();
        descriptionOfItem.text = ingredient.ingredientDescription;
        OnBuySliderChanged();
    }
    public void OnBuySliderChanged()
    {
        if (storedIngredient == null)
        {
            Debug.LogWarning("Stored ingredient is null. Slider change event ignored.");
            return;
        }
        numOfItemsToBuy = Mathf.RoundToInt(buySlider.value);
        itemCosts = numOfItemsToBuy * storedIngredient.ingredientCost;
        UpdateAffordability(itemCosts);
    }

    public override void Buy()
    {
        if (!isAffordable)
        {
            Debug.Log("Not enough money");
            return;
        }

        PlayerStats.playerStatsInstance.AddToPlayerInventory(numOfItemsToBuy, storedIngredient);
        PlayerStats.playerStatsInstance.RemoveMoney(itemCosts);
        storedButton.UpdateButton(numOfItemsToBuy);
        Close();
    }

    protected override void ClearPanel()
    {
        base.ClearPanel();
        buySlider.value = 0;
        buySlider.maxValue = 0;
        // Clear ingredient-specific UI elements
        storedIngredient = null;
        ingredientToBuyName.text = "";
        descriptionOfItem.text = "";
        imageItemToBuy.sprite = null;
    }
}
