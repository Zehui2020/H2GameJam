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

    [SerializeField] private Animator fadeOpaque;
    

    //[SerializeField] private MarketBooth marketBooth

    private void Awake()
    {
        //ingredientsToSet = Resources.LoadAll<Ingredient>("Assets/ScriptableObjects/Ingredients").ToList();
        boothCanvas.alpha = 0;
        boothCanvas.interactable = false;
        boothCanvas.blocksRaycasts = false;
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
        //StartCoroutine(FadeOut());
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        fadeOpaque.Play("FadeToBlack");
        yield return new WaitForSeconds(1);
        boothCanvas.alpha = 0;
        boothCanvas.interactable = false;
        boothCanvas.blocksRaycasts = false;
        fadeOpaque.Play("FadeToClear");
        yield return new WaitForSeconds(1);
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.Walk;
    }
    public void EnableShop()
    {
        //StartCoroutine(FadeIn());
        boothCanvas.alpha = 1;
        boothCanvas.interactable = true;
        boothCanvas.blocksRaycasts = true;
    }

}
