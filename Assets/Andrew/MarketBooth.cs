using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class MarketBooth : MonoBehaviour
{
    [SerializeField] private CanvasGroup boothCanvas;

    [SerializeField] private List<Ingredient> ingredientsToSet = new();

    [SerializeField] private List<BoothButton> marketBoothButtons = new();

    [SerializeField] private GameObject marketBoothBtnHolder;

    private void Awake()
    {
        //ingredientsToSet = Resources.LoadAll<Ingredient>("Assets/ScriptableObjects/Ingredients").ToList();
        ExitShop();
        SetUpButtons();
    }

    private void SetUpButtons()
    {
        marketBoothButtons.AddRange(marketBoothBtnHolder.GetComponentsInChildren<BoothButton>());
        for (int i = 0; i < ingredientsToSet.Count; i++)
        {
            marketBoothButtons[i].SetUpButton(ingredientsToSet[i], 2);
        }
    }


    public void ExitShop()
    {
        boothCanvas.alpha = 0;
        boothCanvas.interactable = false;
        boothCanvas.blocksRaycasts = false;
    }

    public void EnableShop()
    {
        boothCanvas.alpha = 1;
        boothCanvas.interactable = true;
        boothCanvas.blocksRaycasts = true;
    }

    public void GetItem()
    {

    }
}
