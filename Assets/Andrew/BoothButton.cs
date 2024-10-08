using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BoothButton : MonoBehaviour
{
    private Ingredient ingredientToDisplay;

    public int numberOfIngredients;

    [SerializeField] private Image ingredientImage;

    [SerializeField] private TextMeshProUGUI ingredientText;

    [SerializeField] private TextMeshProUGUI numOfIngredientsText;

    [SerializeField] private Button button;
    [SerializeField] private ConfirmBuyPanel confirmBuyPanel;

    private void Awake()
    {
        ingredientImage ??= GetComponentInChildren<Image>();
    }
    public void SetUpButton(Ingredient ingredientData, int numOfIngredients)
    {
        ingredientToDisplay = ingredientData;
        ingredientImage.sprite = ingredientData.ingrendientSprite;
        ingredientText.text = ingredientData.ingredientType.ToString();
        numberOfIngredients = numOfIngredients;
        numOfIngredientsText.text = numberOfIngredients.ToString();
    }

    public void AttemptToPurchase()
    {
        confirmBuyPanel.InitNewItemToBuy(ingredientToDisplay, numberOfIngredients, this);
        confirmBuyPanel.gameObject.SetActive(true);
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
