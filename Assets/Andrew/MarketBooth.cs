using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MarketBooth : MonoBehaviour
{
    [SerializeField] private Animator boothAnimator;
    [SerializeField] protected ConfirmBuyPanel confirmBuyPanel;
    [SerializeField] protected RectTransform buttonParent;

    [SerializeField] private Sprite shopkeeperSprite;
    [SerializeField] private Image shopIcon;

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
        shopIcon.sprite = shopkeeperSprite;
        InitBooth();
        boothAnimator.SetBool("isShopping", true);
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.InMenu;
    }

    public void OpenPannel()
    {
        shopIcon.sprite = shopkeeperSprite;
        boothAnimator.SetBool("isOpenPannel", true);
    }

    public void ClosePannel()
    {
        boothAnimator.SetBool("isOpenPannel", false);
    }
}