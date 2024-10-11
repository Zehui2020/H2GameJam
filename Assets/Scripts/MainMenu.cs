using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private string nextScene;
    [SerializeField] private AudioManager audioManager;

    private bool isClicked = false;
    private bool canClick = false;

    private IEnumerator Start()
    {
        audioManager.Play("MainMenuBGM");
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            instruction.text = "Press Anywhere To Begin!";
        else
            instruction.text = "Click Anywhere To Begin!";

        yield return new WaitForSeconds(1f);

        canClick = true;
    }

    private void Update()
    {
        if (isClicked || !canClick)
            return;

        if (Input.GetMouseButtonDown(0) || Input.touches.Count() > 0)
        {
            isClicked = true;
            StartCoroutine(TransitionScene());
        }
    }


    private IEnumerator TransitionScene()
    {
        //audioManager.Stop("MainMenuBGM");
        audioManager.FadeSound(false, "MainMenuBGM", 0.5f, 0);
        //play sound effect when click
        audioManager.Play("Click");
        while (audioManager.CheckIfSoundPlaying("Click"))
        {
            yield return null;
        }
        SceneLoader.Instance.LoadScene(nextScene);

    }
}