using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MarketBooth : MonoBehaviour
{
    [SerializeField] private CanvasGroup boothCanvas;

    [SerializeField] private List<Ingredient> ingredientsToSet;

    [SerializeField] private List<MarketBoothButton> marketBoothButtons;

    [SerializeField] private GameObject marketBoothBtnHolder;

    private void Awake()
    {
        boothCanvas.alpha = 0;
        boothCanvas.interactable = false;
        boothCanvas.blocksRaycasts = false;
        SetUpButtons();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Contact");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Contact");
            boothCanvas.alpha = 1;
            boothCanvas.interactable = true;
            boothCanvas.blocksRaycasts = true;
        }
    }

    private void SetUpButtons()
    {
        marketBoothButtons.AddRange(marketBoothBtnHolder.GetComponentsInChildren<MarketBoothButton>());
        foreach (var button in marketBoothButtons)
        {
            button.SetUpButton(ingredientsToSet[0], 2);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boothCanvas.alpha = 0;
            boothCanvas.interactable = false;
            boothCanvas.blocksRaycasts = false;
        }
    }

    public void GetItem()
    {

    }
}
