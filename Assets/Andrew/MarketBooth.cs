using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MarketBooth : MonoBehaviour
{
    [SerializeField] private Animator boothAnimator;
    [SerializeField] protected ConfirmBuyPanel confirmBuyPanel;
    [SerializeField] protected RectTransform buttonParent;

    public UnityEvent OnExitShop;

    public virtual void InitBooth()
    {

    }
    public void ExitShop()
    {
        boothAnimator.SetBool("isShopping", false);
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.Walk;
        AudioManager.Instance.PlayOneShot("ClickSuccess");
        OnExitShop?.Invoke();
    }

    public void EnterShop()
    {
        InitBooth();
        boothAnimator.SetBool("isShopping", true);
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.InMenu;
    }
}