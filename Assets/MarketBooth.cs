using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MarketBooth : MonoBehaviour
{
    [SerializeField] private CanvasGroup boothCanvas;

    [SerializeField] private List<Ingredient> ingredientsToSet;

    private void Awake()
    {
        boothCanvas.alpha = 0;
        boothCanvas.interactable = false;
        boothCanvas.blocksRaycasts = false;
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
