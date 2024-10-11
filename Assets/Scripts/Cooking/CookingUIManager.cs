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

    public void ShowFinishAlert()
    {
        finishedAnimator.SetTrigger("finish");
    }

    public void SetTimerSlider(float currentValue, float maxValue)
    {
        timerSlider.value = currentValue;
        timerSlider.maxValue = maxValue;
    }

    public void SetReputationSlider(float currentValue, float maxValue)
    {
        reputationSlider.value = currentValue;
        timerSlider.maxValue = maxValue;
    }
}