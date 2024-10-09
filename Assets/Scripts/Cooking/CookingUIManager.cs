using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingUIManager : MonoBehaviour
{
    [SerializeField] private Animator finishedAnimator;
    [SerializeField] private Slider timerSlider;

    public void ShowFinishAlert()
    {
        finishedAnimator.SetTrigger("finish");
    }

    public void SetTimerSlider(float currentValue, float maxValue)
    {
        timerSlider.value = currentValue;
        timerSlider.maxValue = maxValue;
    }
}