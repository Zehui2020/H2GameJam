using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class BookCanvas : MonoBehaviour
{
    [SerializeField] private List<Book> tutorialListOfBooks;
    [SerializeField] private List<Book> originsListOfBooks;
    [SerializeField] private List<Book> renaissanceListOfBooks;
    [SerializeField] private List<Book> modernListOfBooks;

    [SerializeField] private List<Book> listOfBooks;

    [SerializeField] private Animator fade;

    [SerializeField] private Image bookCanvas;

    [SerializeField] private int bookIndex = 0;

    [SerializeField] private bool isClosing = false;

    [SerializeField] private int dayCheck;
    private void Awake()
    {
        if (PlayerStats.playerStatsInstance.currentGeneration.generation == GenerationData.Generation.Tutorial)
        {
            listOfBooks = originsListOfBooks;
        }
        else if (PlayerStats.playerStatsInstance.dayCounter == 0)
        {
            switch (PlayerStats.playerStatsInstance.currentGeneration.generation)
            {
                case GenerationData.Generation.Origins:
                    listOfBooks = originsListOfBooks;
                    break;
                case GenerationData.Generation.Renaissance:
                    listOfBooks = renaissanceListOfBooks;
                    break;
                case GenerationData.Generation.Morden:
                    listOfBooks = modernListOfBooks;
                    break;
            }
        }
        fade ??= GetComponentInChildren<Animator>();

        bookCanvas.sprite = listOfBooks[bookIndex].bookSprite;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touches.Count() > 0)
        {
            //StartCoroutine(TransitionScene());
            GetNextBook();
        }
    }
    public void GetNextBook()
    {
        if (listOfBooks.Count < bookIndex + 1)
        {
            if (!isClosing) StartCoroutine(CloseBook());
            return;
        }
        bookCanvas.sprite = listOfBooks[bookIndex].bookSprite;
    }

    public IEnumerator CloseBook()
    {
        isClosing = true;
        fade.Play("FadeFromTranslucentToClear");
        yield return new WaitForSeconds(1);

        fade.GetComponent<Image>().raycastTarget = false;
    }


}
