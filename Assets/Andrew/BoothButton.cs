using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BoothButton : MonoBehaviour
{
    private ScriptableObject itemToStore;
    public int numberOfIngredients;

    [SerializeField] private Image ingredientImage;
    [SerializeField] private TextMeshProUGUI ingredientText;
    [SerializeField] private TextMeshProUGUI numOfIngredientsText;
    [SerializeField] private Button button;
    [SerializeField] private ConfirmBuyPanel confirmBuyPanel;

    [SerializeField] private MarketBooth marketBooth;

    [SerializeField] private Animator fadeTranslucent;

    private void Awake()
    {
        ingredientImage ??= GetComponentInChildren<Image>();
    }
    public void SetUpButton(ScriptableObject itemData, int numOfIngredients)
    {
        itemToStore = itemData;
        if (itemData is Ingredient)
        {
            Ingredient ingredient = (Ingredient)itemToStore;
            ingredientImage.sprite = ingredient.ingrendientSprite;
            ingredientText.text = ingredient.ingredientType.ToString();
        }
        else if (itemData is ApplianceData)
        {
            ApplianceData appliance = (ApplianceData)itemToStore;
            //ingredientImage.sprite = appliance.sp;
            ingredientText.text = appliance.ingredientType.ToString();
        }
        numberOfIngredients = numOfIngredients;
        numOfIngredientsText.text = numberOfIngredients.ToString();
    }

    public void AttemptToPurchase()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        confirmBuyPanel.InitNewItemToBuy(ingredientToDisplay, numberOfIngredients, this);
        confirmBuyPanel.gameObject.SetActive(true);
        fadeTranslucent.gameObject.GetComponent<Image>().raycastTarget = true;
        fadeTranslucent.Play("FadeToTranslucent");
        yield return new WaitForSeconds(1);
    }
    public void UpdateButton(int itemsToRemove)
    {
        numberOfIngredients -= itemsToRemove;

        if (numberOfIngredients <= 0)
        {
            button.interactable = false;
        }
        numOfIngredientsText.text = numberOfIngredients.ToString();

    }

}
