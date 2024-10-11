using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingUIManager : MonoBehaviour
{
    [SerializeField] private Animator finishedAnimator;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Slider reputationSlider;
    [SerializeField] private TextMeshProUGUI moneyCounter;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerStats.playerStatsInstance;
    }

    public void ShowFinishAlert()
    {
        finishedAnimator.SetTrigger("finish");
    }

    public void SetTimerSlider(float currentValue, float maxValue)
    {
        timerSlider.value = currentValue;
        timerSlider.maxValue = maxValue;
    }

    public void UpdateReputation()
    {
        reputationSlider.minValue = -100;
        reputationSlider.maxValue = 100;

        reputationSlider.value = playerStats.currentReputation;
    }

    public void UpdateMoney()
    {
        moneyCounter.text = "$" + playerStats.GetPlayerMoney().ToString();
    }
}