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

    [SerializeField] private Button exitButton;

    

    //[SerializeField] private MarketBooth marketBooth

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
        StartCoroutine(FadeOut());
    }

    public void EnableShop()
    {
        StartCoroutine(FadeIn());
    }

    public void GetItem()
    {

    }


    private IEnumerator FadeIn()
    {
        while (boothCanvas.alpha < 1)
        {
            boothCanvas.alpha += 0.5f;
            // Introduce a short delay between increments
            yield return new WaitForSeconds(0.05f); // Adjust the duration as needed
        }
        boothCanvas.alpha = 1;
        boothCanvas.interactable = true;
        boothCanvas.blocksRaycasts = true;
    }

    private IEnumerator FadeOut()
    {
        while (boothCanvas.alpha > 0)
        {
            boothCanvas.alpha -= 0.5f;
            // Introduce a short delay between decrements
            yield return new WaitForSeconds(0.05f); // Adjust the duration as needed
        }
        boothCanvas.alpha = 0;
        boothCanvas.interactable = false;
        boothCanvas.blocksRaycasts = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
