using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ConfirmBuyPanel : MonoBehaviour
{
    protected ShopItemData itemToBuy;

    [SerializeField] protected Image imageItemToBuy;
    [SerializeField] protected TextMeshProUGUI itemName;
    [SerializeField] protected TextMeshProUGUI itemDescription;
    [SerializeField] protected TextMeshProUGUI itemCost;
    [SerializeField] protected Animator pannelAnimator;

    [SerializeField] protected Slider sliderAmount;
    [SerializeField] private TextMeshProUGUI purchaseAmountCount;
    [SerializeField] private TextMeshProUGUI maxPurchaseAmountCount;
    [SerializeField] protected Button purchaseButton;

    [SerializeField] private List<RectTransform> rebuildRects;

    private BoothButton boothButton;
    protected int numOfItemsToBuy;
    protected int itemCosts;
    protected bool isAffordable = false;

    public virtual void ShowPannel(BoothButton boothButton, ShopItemData itemToBuy)
    {
        this.itemToBuy = itemToBuy;
        this.boothButton = boothButton;
        purchaseButton.interactable = true;

        imageItemToBuy.sprite = itemToBuy.itemSprite;
        itemName.text = itemToBuy.shopItemName;
        itemDescription.text = itemToBuy.shopDescription;

        if (sliderAmount != null)
            itemCost.text = PlayerStats.playerStatsInstance.currentMoney + "/" + itemToBuy.GetCost() * sliderAmount.value;
        else
            itemCost.text = PlayerStats.playerStatsInstance.currentMoney + "/" + itemToBuy.GetCost();

        int maxToBuy = itemToBuy.maximumPurchases;
        if (itemToBuy.GetCost() != 0)
            maxToBuy = Mathf.FloorToInt(PlayerStats.playerStatsInstance.currentMoney / itemToBuy.GetCost());

        int maxPurchasable = Mathf.FloorToInt(boothButton.purchasesLeft * itemToBuy.GetCost());

        if (maxToBuy <= 0)
        {
            maxToBuy = 1;
            purchaseButton.interactable = false;
        }
        else if (maxToBuy > maxPurchasable)
            maxToBuy = boothButton.purchasesLeft;

        if (maxToBuy > itemToBuy.maximumPurchases)
            maxToBuy = itemToBuy.maximumPurchases;

        if (sliderAmount != null)
        {
            sliderAmount.minValue = 1;
            sliderAmount.value = 1;
            sliderAmount.maxValue = maxToBuy;

            purchaseAmountCount.text = sliderAmount.value.ToString();
            maxPurchaseAmountCount.text = sliderAmount.maxValue.ToString();
        }

        pannelAnimator.SetBool("isShowing", true);

        foreach (RectTransform rect in rebuildRects)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    public void Close()
    {
        pannelAnimator.SetBool("isShowing", false);
    }

    public void Buy()
    {
        if (itemToBuy is Ingredient ingredient)
        {
            PlayerStats.playerStatsInstance.AddToPlayerInventory(Mathf.CeilToInt(sliderAmount.value), ingredient);
            PlayerStats.playerStatsInstance.currentMoney -= itemToBuy.GetCost() * (int)sliderAmount.value;
            boothButton.UpdateButton((int)sliderAmount.value);
        }
        else if (itemToBuy is ApplianceData appliance)
        {
            PlayerStats.playerStatsInstance.UpgradeAppliances(appliance);
            PlayerStats.playerStatsInstance.currentMoney -= itemToBuy.GetCost();
            boothButton.UpdateButton(1);
        }

        Close();
    }

    public void AddItemToBuy()
    {
        sliderAmount.value += 1;
    }

    public void RemoveItemToBuy()
    {
        sliderAmount.value -= 1;
    }

    public void OnSliderValueChanged()
    {
        purchaseAmountCount.text = sliderAmount.value.ToString();

        int totalCost = itemToBuy.GetCost() * (int)sliderAmount.value;
        itemCost.text = PlayerStats.playerStatsInstance.currentMoney + "/" + itemToBuy.GetCost() * sliderAmount.value;

        if (PlayerStats.playerStatsInstance.currentMoney < totalCost)
            purchaseButton.interactable = false;
        else
            purchaseButton.interactable = true;
    }
}