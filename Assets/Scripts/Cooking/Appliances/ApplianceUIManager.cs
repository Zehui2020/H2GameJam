using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplianceUIManager : MonoBehaviour
{
    [SerializeField] private Animator cookingSliderAnimator;
    [SerializeField] private Slider cookingSlider;

    [SerializeField] private RectTransform ingredientUIParent;

    [SerializeField] private IngredientUI ingredientUIPrefab;
    [SerializeField] private IngredientUI addIngredientUI;
    private List<IngredientUI> ingredientUIs = new();

    [SerializeField] private Animator burnWarningAnimator;
    [SerializeField] private Animator cookedNotification;

    public void SetCookingSlider(float currentValue, float maxValue)
    {
        cookingSlider.value = currentValue;
        cookingSlider.maxValue = maxValue;
    }

    public void ClearIngredientUI()
    {
        foreach (IngredientUI ingredientUI in ingredientUIs)
            ingredientUI.RemoveUI();

        ingredientUIs.Clear();
        addIngredientUI.gameObject.SetActive(true);
    }

    public void AddIngredientUI(Ingredient ingredient)
    {
        IngredientUI ingredientUI = Instantiate(ingredientUIPrefab);
        ingredientUI.InitIngredientUI(ingredient);
        ingredientUI.transform.SetParent(ingredientUIParent);
        ingredientUI.transform.localScale = Vector3.one;
        ingredientUI.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ingredientUIParent);

        ingredientUIs.Add(ingredientUI);
        addIngredientUI.transform.SetAsLastSibling();
    }

    public void HideAddIngredientUI()
    {
        addIngredientUI.gameObject.SetActive(false);
    }

    public void SetBurnWarningSpeed(float speedModifier)
    {
        burnWarningAnimator.SetBool("isBurning", true);
        burnWarningAnimator.speed = 1 + speedModifier;
    }

    public void StopBurning()
    {
        burnWarningAnimator.SetBool("isBurning", false);
    }

    public void SetCookingSliderActive(bool active)
    {
        cookingSliderAnimator.SetBool("isCooking", active);
    }

    public void OnFoodCooked()
    {
        cookedNotification.SetTrigger("show");
    }
}