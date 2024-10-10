using System.Collections.Generic;
using UnityEngine;

public class MarketBooth : MonoBehaviour
{
    [SerializeField] private Animator boothAnimator;
    [SerializeField] protected ConfirmBuyPanel confirmBuyPanel;
    [SerializeField] protected RectTransform buttonParent;

    public virtual void InitBooth()
    {

    }

    public void ExitShop()
    {
        boothAnimator.SetBool("isShopping", false);
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.Walk;
    }

    public void EnterShop()
    {
        InitBooth();
        boothAnimator.SetBool("isShopping", true);
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.InMenu;
    }
}