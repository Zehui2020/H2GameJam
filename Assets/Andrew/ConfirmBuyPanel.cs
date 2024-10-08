using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmBuyPanel : MonoBehaviour
{
    [SerializeField] protected Image imageItemToBuy;
    [SerializeField] protected TextMeshProUGUI numOfItemsBox;
    [SerializeField] protected TextMeshProUGUI itemCostBox;
    [SerializeField] protected Animator fadeTranslucent;
    [SerializeField] protected TextMeshProUGUI itemToBuyName;
    [SerializeField] protected TextMeshProUGUI descriptionOfItem;

    protected int numOfItemsToBuy;
    protected int itemCosts;
    protected bool isAffordable = false;

    // Shared initialization for the panel
    protected virtual void InitPanel(int numberOfItems)
    {
        numOfItemsBox.text = numberOfItems.ToString();
    }

    // Handle fade out and close the panel
    public void Close()
    {
        StartCoroutine(FadeOutAndClear());
    }

    private IEnumerator FadeOutAndClear()
    {
        fadeTranslucent.Play("FadeFromTranslucentToClear");
        yield return new WaitForSeconds(1);
        ClearPanel();
        gameObject.SetActive(false);
        fadeTranslucent.gameObject.GetComponent<Image>().raycastTarget = false;
    }

    // Method to clear UI elements
    protected virtual void ClearPanel()
    {
        numOfItemsBox.text = "";
        itemCostBox.text = "";
        itemCosts = 0;
    }

    // To be implemented in derived classes for specific purchasing logic
    public virtual void Buy() { }

    // Check if the player has enough money and update UI accordingly
    protected void UpdateAffordability(int cost)
    {
        if (PlayerStats.playerStatsInstance.currenctMoney >= cost)
        {
            isAffordable = true;
            itemCostBox.text = $"<color=white>${cost}</color>";
        }
        else
        {
            isAffordable = false;
            itemCostBox.text = $"<color=red>${cost}</color>";
        }
    }
}

