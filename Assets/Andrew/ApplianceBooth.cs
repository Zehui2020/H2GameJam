using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplianceBooth : MonoBehaviour
{
    [SerializeField] private CanvasGroup boothCanvas;
    [SerializeField] private List<ApplianceData> applianceToSet = new();
    [SerializeField] private List<BoothButton> applianceBoothButtons = new();
    [SerializeField] private GameObject marketBoothBtnHolder;
    [SerializeField] private Animator fadeOpaque;

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
        applianceBoothButtons.AddRange(marketBoothBtnHolder.GetComponentsInChildren<BoothButton>());
        for (int i = 0; i < applianceToSet.Count; i++)
        {
            applianceBoothButtons[i].SetUpButton(applianceToSet[i], 0);
        }
    }
    public void ExitShop()
    {
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
