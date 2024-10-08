using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ConfirmBuyAppliancePanel : ConfirmBuyPanel
{
    [SerializeField] private TextMeshProUGUI currentLevelAppliance;
    [SerializeField] private TextMeshProUGUI nextLevelAppliance;
    [SerializeField] private ApplianceData storedAppliance;

    public void InitNewItemToBuy(ApplianceData appliance, int numberOfItems, BoothButton button)
    {
        ClearPanel();
        InitPanel(numberOfItems);
        storedAppliance = appliance;
        storedButton = button;
        imageItemToBuy.sprite = appliance.sprite;
        itemToBuyName.text = appliance.applianceName;
        descriptionOfItem.text = appliance.applianceDescription;

        currentLevelAppliance.text = "Lvl. " + PlayerStats.playerStatsInstance.GetApplianceLevel(appliance).ToString();
        nextLevelAppliance.text = "Lvl. " +   (PlayerStats.playerStatsInstance.GetApplianceLevel(appliance) + 1).ToString();
        UpdateAffordability(0);
    }

    public override void Buy()
    {
        base.Buy();
        if (!isAffordable)
        {
            Debug.Log("Not enough money");
            return;
        }

        PlayerStats.playerStatsInstance.UpgradeAppliances(storedAppliance);
        storedButton.UpdateButton(numOfItemsToBuy);
        Close();
    }
}

