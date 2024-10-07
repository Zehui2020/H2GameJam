using DesignPatterns.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : PooledObject
{
    [SerializeField] private Image ingredientIcon;

    public void InitIngredientUI(Ingredient ingredient)
    {
        ingredientIcon.sprite = ingredient.ingrendientSprite;
    }

    public void RemoveUI()
    {
        Release();
        gameObject.SetActive(false);
    }
}