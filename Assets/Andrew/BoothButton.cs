using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DesignPatterns.ObjectPool;

public class BoothButton : PooledObject
{
    public ShopItemData itemToStore;

    [SerializeField] private RectTransform buttonRect;
    [SerializeField] private Image imageIcon;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Animator buttonAnimator;

    public int purchasesLeft = 0;

    public event System.Action<ShopItemData> OnSelectButton;

    public void SetUpButton(ShopItemData itemData)
    {
        itemToStore = itemData;

        if (itemData is Ingredient)
        {
            Ingredient ingredient = (Ingredient)itemToStore;
            imageIcon.sprite = ingredient.ingrendientSprite;
            cost.text = "$" + ingredient.GetCost().ToString();
        }
        else if (itemData is ApplianceData)
        {
            ApplianceData appliance = (ApplianceData)itemToStore;
            imageIcon.sprite = appliance.baseSprite;
            cost.text = "$" + appliance.GetCost().ToString();
        }

        purchasesLeft = itemData.maximumPurchases;

        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRect);
    }

    public void SelectButton()
    {
        if (purchasesLeft <= 0)
        {
            AudioManager.Instance.PlayOneShot("ClickFail");
            return;
        }

        //play sound button
        AudioManager.Instance.PlayOneShot("ClickSuccess");
        OnSelectButton?.Invoke(itemToStore);
    }

    public void UpdateButton(int itemsToRemove)
    {
        purchasesLeft -= itemsToRemove;

        if (purchasesLeft <= 0)
            buttonAnimator.SetTrigger("soldOut");
    }

    private void OnDisable()
    {
        OnSelectButton = null;
    }
}