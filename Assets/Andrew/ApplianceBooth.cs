using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplianceBooth : MonoBehaviour
{
    [SerializeField] private CanvasGroup boothCanvas;

    [SerializeField] private List<Appliance> applianceToSet = new();

    [SerializeField] private List<BoothButton> applianceBoothButtons = new();

    [SerializeField] private GameObject marketBoothBtnHolder;

    private void Awake()
    {
        //ingredientsToSet = Resources.LoadAll<Ingredient>("Assets/ScriptableObjects/Ingredients").ToList();
        ExitShop();
        SetUpButtons();
    }

    private void SetUpButtons()
    {
        applianceBoothButtons.AddRange(marketBoothBtnHolder.GetComponentsInChildren<BoothButton>());
        for (int i = 0; i < applianceToSet.Count; i++)
        {
            //applianceBoothButtons[i].SetUpButton(ingredientsToSet[i], 2);
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
