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

    [SerializeField] private TextMeshProUGUI numOfItems;

    [SerializeField] private int numOfItemsToBuy;

    [SerializeField] private Ingredient storedIngredient;

    [SerializeField] private MarketBoothButton storedButton;

    private void ClearPanel()
    {
        storedIngredient = null;
        numOfItems.text = null;
        imageItemToBuy.sprite = null;
        ingredientToBuyName.text = null;
        buySlider.maxValue = 0;
        buySlider.value = 0;
    }
    public void InitNewItemToBuy(Ingredient ingredient, int numberOfItems, MarketBoothButton button)
    {
        ClearPanel();
        storedIngredient = ingredient;
        numOfItems.text = numberOfItems.ToString();
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
    }
    public void Buy()
    {
        //check if player has enough money
        PlayerStats.playerStatsInstance.AddToPlayerInventory(numOfItemsToBuy, storedIngredient);
        gameObject.SetActive(false);
        storedButton.UpdateButton(numOfItemsToBuy);
    }
    public void Close()
    {
        ClearPanel();
        gameObject.SetActive(false);
    }
}
