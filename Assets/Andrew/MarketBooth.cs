using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class MarketBooth : MonoBehaviour
{
    [SerializeField] private CanvasGroup boothCanvas;

    [SerializeField] private List<Ingredient> ingredientsToSet = new();

    [SerializeField] private List<MarketBoothButton> marketBoothButtons = new();

    [SerializeField] private GameObject marketBoothBtnHolder;

    private void Awake()
    {
        //ingredientsToSet = Resources.LoadAll<Ingredient>("Assets/ScriptableObjects/Ingredients").ToList();
        boothCanvas.alpha = 0;
        boothCanvas.interactable = false;
        boothCanvas.blocksRaycasts = false;
        SetUpButtons();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Contact");
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Contact");
            boothCanvas.alpha = 1;
            boothCanvas.interactable = true;
            boothCanvas.blocksRaycasts = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
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
        for (int i = 0; i < ingredientsToSet.Count; i++)
        {
            marketBoothButtons[i].SetUpButton(ingredientsToSet[i], 2);
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
