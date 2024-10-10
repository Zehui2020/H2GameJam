using DesignPatterns.ObjectPool;
using System.Collections.Generic;
using UnityEngine.UI;

public class ApplianceBooth : MarketBooth
{
    public override void InitBooth()
    {
        if (buttonParent.childCount != 0)
            return;

        base.InitBooth();

        List<ApplianceData> appliances = PlayerStats.playerStatsInstance.currentGeneration.applaincesToUpgrade;
        foreach (ApplianceData appliance in appliances)
        {
            BoothButton button = ObjectPool.Instance.GetPooledObject("BoothButton", true) as BoothButton;
            button.SetUpButton(appliance);
            button.OnSelectButton += (item) => { confirmBuyPanel.ShowPannel(button, item); };
            button.transform.SetParent(buttonParent);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonParent);
    }
}