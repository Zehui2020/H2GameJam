using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ConfirmBuyPanel : MonoBehaviour
{
    [SerializeField] private Slider buySlider;

    [SerializeField] private TextMeshProUGUI ingredientToBuyName;

    [SerializeField] private TextMeshProUGUI descriptionOfItem;

    [SerializeField] private Image imageItemToBuy;

    [SerializeField] private TextMeshProUGUI numOfItemsBox;

    [SerializeField] private int numOfItemsToBuy;

    [SerializeField] private Ingredient storedIngredient;

    [SerializeField] private BoothButton storedButton;

    [SerializeField] private TextMeshProUGUI itemCostBox;

    [SerializeField] private int itemCosts;

    [SerializeField] private bool isAffordable = false;

    private void ClearPanel()
    {
        storedIngredient = null;
        numOfItemsBox.text = null;
        imageItemToBuy.sprite = null;
        ingredientToBuyName.text = null;
        buySlider.maxValue = 0;
        buySlider.value = 0;
        itemCostBox.text = null;
        itemCosts = 0;
    }
    public void InitNewItemToBuy(Ingredient ingredient, int numberOfItems, BoothButton button)
    {
        ClearPanel();
        storedIngredient = ingredient;
        numOfItemsBox.text = numberOfItems.ToString();
        imageItemToBuy.sprite = ingredient.ingrendientSprite;
        ingredientToBuyName.text = ingredient.ingredientType.ToString();
        buySlider.maxValue = numberOfItems;
        buySlider.value = numberOfItems;
        storedButton = button;
    }
    public void OnBuySliderChanged()
    {
        numOfItemsToBuy = Mathf.RoundToInt(buySlider.value);
        Debug.Log("Selected Amount: " + numOfItemsToBuy);
        itemCosts = numOfItemsToBuy * storedIngredient.ingredientCost;
        Debug.Log($"Cost of items {itemCosts}");
        itemCostBox.text = itemCosts.ToString();

        //check if player has enough money
        if (PlayerStats.playerStatsInstance.currenctMoney >= itemCosts)
        {
            isAffordable = true;
            itemCostBox.text = $"<color=white>${itemCosts}</color>";
        }
        else
        {
            isAffordable = false;
            itemCostBox.text = $"<color=red>${itemCosts}</color>";
        }
    }
    public void Buy()
    {
        if (!isAffordable)
        {
            Debug.Log("Not enough money");
            return;
        }
        PlayerStats.playerStatsInstance.AddToPlayerInventory(numOfItemsToBuy, storedIngredient);
        PlayerStats.playerStatsInstance.RemoveMoney(itemCosts);
        gameObject.SetActive(false);
        storedButton.UpdateButton(numOfItemsToBuy);
    }
    public void Close()
    {
        ClearPanel();
        gameObject.SetActive(false);
    }
}
